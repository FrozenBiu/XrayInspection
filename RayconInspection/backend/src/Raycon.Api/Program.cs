using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Raycon.Api.Hubs;
using Raycon.Api.Models;
using Raycon.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on port 5000 (HTTP)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<XriConverterService>();
builder.Services.AddSingleton<RejectWatcherBackgroundService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<RejectWatcherBackgroundService>());

var app = builder.Build();

app.UseCors("AllowAll");

// Ensure wwwroot/images exists
var webRoot = app.Environment.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
var imagesPath = Path.Combine(webRoot, "images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(); // Serve wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

// SignalR Hub
app.MapHub<InspectionHub>("/hubs/inspection");

// API Endpoints: Rejects
app.MapGet("/api/rejects", (XriConverterService converter) =>
{
    return Results.Ok(converter.GetAllRejects());
});

app.MapGet("/api/rejects/{id}", (string id, XriConverterService converter) =>
{
    var item = converter.GetRejectById(id);
    return item != null ? Results.Ok(item) : Results.NotFound();
});

// API Endpoints: Machine Status & Config
app.MapGet("/api/status", (RejectWatcherBackgroundService watcher, XriConverterService converter) =>
{
    return Results.Ok(new
    {
        machineId = watcher.CurrentMachineId,
        machineName = watcher.CurrentMachineName,
        watchDirectory = watcher.CurrentWatchDirectory,
        totalLoaded = converter.GetAllRejects().Count,
        serverTime = DateTime.Now
    });
});

app.MapGet("/api/config", (RejectWatcherBackgroundService watcher) =>
{
    return Results.Ok(new ConfigModel
    {
        MachineId = watcher.CurrentMachineId,
        MachineName = watcher.CurrentMachineName,
        WatchDirectory = watcher.CurrentWatchDirectory
    });
});

app.MapPost("/api/config", async (ConfigModel config, RejectWatcherBackgroundService watcher) =>
{
    var result = await watcher.UpdateConfigAsync(config);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

app.MapPost("/api/config/test", (ConfigModel config) =>
{
    if (string.IsNullOrWhiteSpace(config.WatchDirectory))
    {
        return Results.BadRequest(new { success = false, message = "Đường dẫn không được để trống" });
    }

    var path = config.WatchDirectory.Trim();
    if (!Directory.Exists(path))
    {
        return Results.Ok(new { success = false, message = $"Thư mục không tồn tại hoặc không thể truy cập: \"{path}\"" });
    }

    try
    {
        var xriFiles = Directory.GetFiles(path, "*.xri");
        return Results.Ok(new 
        { 
            success = true, 
            message = $"Kết nối thành công! Tìm thấy {xriFiles.Length} file .xri trong thư mục.",
            fileCount = xriFiles.Length 
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { success = false, message = $"Lỗi khi đọc thư mục: {ex.Message}" });
    }
});

app.Run();
