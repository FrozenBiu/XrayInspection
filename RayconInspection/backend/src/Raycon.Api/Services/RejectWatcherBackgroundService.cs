using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR;
using Raycon.Api.Hubs;
using Raycon.Api.Models;

namespace Raycon.Api.Services;

public class RejectWatcherBackgroundService : BackgroundService
{
    private readonly ILogger<RejectWatcherBackgroundService> _logger;
    private readonly IConfiguration _config;
    private readonly XriConverterService _converter;
    private readonly IHubContext<InspectionHub> _hubContext;
    private FileSystemWatcher? _watcher;
    private string _currentWatchDir = string.Empty;

    public string CurrentWatchDirectory => _currentWatchDir;
    public string CurrentMachineId { get; private set; } = "RAYCON_EX2_01";
    public string CurrentMachineName { get; private set; } = "Máy Soi Tia X Raycon EX2 - Dây Chuyền 1";

    public RejectWatcherBackgroundService(
        ILogger<RejectWatcherBackgroundService> logger,
        IConfiguration config,
        XriConverterService converter,
        IHubContext<InspectionHub> hubContext)
    {
        _logger = logger;
        _config = config;
        _converter = converter;
        _hubContext = hubContext;

        _currentWatchDir = _config["RayconInspection:WatchDirectory"] 
            ?? @"E:\VisualStudioCode\MyProjects\RayconEX2\Source\GUI_XRS\Archive\Image\Reject";
        CurrentMachineId = _config["RayconInspection:MachineId"] ?? CurrentMachineId;
        CurrentMachineName = _config["RayconInspection:MachineName"] ?? CurrentMachineName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Raycon Inspection Watcher starting. Target directory: {Dir}", _currentWatchDir);

        if (!Directory.Exists(_currentWatchDir))
        {
            _logger.LogWarning("Watch directory does not exist: {Dir}. Creating if local...", _currentWatchDir);
            try { Directory.CreateDirectory(_currentWatchDir); } catch { }
        }

        // Start scan and watcher setup asynchronously so Kestrel port 5000 opens immediately
        _ = Task.Run(async () =>
        {
            await ScanDirectoryAsync(_currentWatchDir, stoppingToken);
            SetupWatcher();
            try
            {
                await _hubContext.Clients.All.SendAsync("ScanCompleted", stoppingToken);
            }
            catch { }
        }, stoppingToken);

        // Keep service alive until shutdown
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void SetupWatcher()
    {
        try
        {
            _watcher?.Dispose();
            _watcher = null;

            if (Directory.Exists(_currentWatchDir))
            {
                _watcher = new FileSystemWatcher(_currentWatchDir, "*.xri")
                {
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                _watcher.Created += async (s, e) =>
                {
                    _logger.LogInformation("New reject file detected: {Name}", e.Name);
                    await Task.Delay(200);
                    var item = _converter.ProcessXriFile(e.FullPath);
                    if (item != null)
                    {
                        _logger.LogInformation("Broadcasting NewReject event to clients: {Id}", item.Id);
                        await _hubContext.Clients.All.SendAsync("NewReject", item);
                    }
                };

                _logger.LogInformation("FileSystemWatcher successfully attached to: {Dir}", _currentWatchDir);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize FileSystemWatcher on {Dir}", _currentWatchDir);
        }
    }

    public async Task ScanDirectoryAsync(string dirPath, CancellationToken token = default)
    {
        await Task.Run(() =>
        {
            try
            {
                if (Directory.Exists(dirPath))
                {
                    var files = Directory.GetFiles(dirPath, "*.xri")
                        .OrderByDescending(f => File.GetCreationTime(f))
                        .Take(50)
                        .ToList();

                    _logger.LogInformation("Processing {Count} reject images from {Dir}...", files.Count, dirPath);
                    foreach (var file in files)
                    {
                        if (token.IsCancellationRequested) break;
                        _converter.ProcessXriFile(file);
                    }
                    _logger.LogInformation("Scan complete. Loaded {Count} items.", _converter.GetAllRejects().Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning directory {Dir}", dirPath);
            }
        }, token);
    }

    public async Task<UpdateConfigResponse> UpdateConfigAsync(ConfigModel newConfig)
    {
        if (string.IsNullOrWhiteSpace(newConfig.WatchDirectory))
        {
            return new UpdateConfigResponse { Success = false, Message = "Đường dẫn thư mục không được để trống!" };
        }

        var normalizedPath = newConfig.WatchDirectory.Trim();

        // Check accessibility
        if (!Directory.Exists(normalizedPath))
        {
            return new UpdateConfigResponse 
            { 
                Success = false, 
                Message = $"Không thể truy cập thư mục: \"{normalizedPath}\". Vui lòng kiểm tra lại đường dẫn mạng hoặc quyền truy cập." 
            };
        }

        try
        {
            _currentWatchDir = normalizedPath;
            if (!string.IsNullOrWhiteSpace(newConfig.MachineId)) CurrentMachineId = newConfig.MachineId.Trim();
            if (!string.IsNullOrWhiteSpace(newConfig.MachineName)) CurrentMachineName = newConfig.MachineName.Trim();

            // Reset Watcher
            SetupWatcher();

            // Clear old images & scan new directory
            _converter.ClearItems();
            await ScanDirectoryAsync(_currentWatchDir);

            // Persist to appsettings.json
            await PersistConfigAsync();

            // Notify UI via SignalR that config changed
            await _hubContext.Clients.All.SendAsync("ConfigUpdated", new
            {
                machineId = CurrentMachineId,
                machineName = CurrentMachineName,
                watchDirectory = _currentWatchDir,
                totalLoaded = _converter.GetAllRejects().Count
            });

            return new UpdateConfigResponse
            {
                Success = true,
                Message = "Cập nhật đường dẫn thành công! Đã kết nối và nạp danh sách ảnh.",
                TotalLoaded = _converter.GetAllRejects().Count,
                WatchDirectory = _currentWatchDir
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating configuration");
            return new UpdateConfigResponse { Success = false, Message = $"Lỗi: {ex.Message}" };
        }
    }

    private async Task PersistConfigAsync()
    {
        try
        {
            var appSettingsPaths = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "appsettings.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")
            };

            foreach (var path in appSettingsPaths)
            {
                if (File.Exists(path))
                {
                    var jsonText = await File.ReadAllTextAsync(path);
                    var node = JsonNode.Parse(jsonText);
                    if (node != null)
                    {
                        var rayconSec = node["RayconInspection"];
                        if (rayconSec == null)
                        {
                            node["RayconInspection"] = new JsonObject();
                            rayconSec = node["RayconInspection"];
                        }

                        if (rayconSec != null)
                        {
                            rayconSec["WatchDirectory"] = _currentWatchDir;
                            rayconSec["MachineId"] = CurrentMachineId;
                            rayconSec["MachineName"] = CurrentMachineName;
                        }

                        await File.WriteAllTextAsync(path, node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                        _logger.LogInformation("Saved new configuration to {Path}", path);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not persist configuration to appsettings.json");
        }
    }

    public override void Dispose()
    {
        _watcher?.Dispose();
        base.Dispose();
    }
}
