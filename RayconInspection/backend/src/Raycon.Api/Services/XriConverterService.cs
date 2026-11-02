using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using Raycon.Api.Models;
using XrsImage;

namespace Raycon.Api.Services;

public class XriConverterService
{
    private readonly ILogger<XriConverterService> _logger;
    private readonly StoreImageData _archiver;
    private readonly ConcurrentDictionary<string, RejectItem> _items = new();
    private readonly string _cacheFolder;

    public XriConverterService(ILogger<XriConverterService> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _archiver = new StoreImageData();
        _cacheFolder = Path.Combine(env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot"), "images");
        
        if (!Directory.Exists(_cacheFolder))
        {
            Directory.CreateDirectory(_cacheFolder);
        }
    }

    public IReadOnlyList<RejectItem> GetAllRejects()
    {
        return _items.Values.OrderByDescending(x => x.CreatedAt).ToList();
    }

    public void ClearItems()
    {
        _items.Clear();
    }

    public RejectItem? GetRejectById(string id)
    {
        _items.TryGetValue(id, out var item);
        return item;
    }

    public RejectItem? ProcessXriFile(string xriFilePath)
    {
        try
        {
            if (!File.Exists(xriFilePath)) return null;

            var fileName = Path.GetFileName(xriFilePath);
            var id = Path.GetFileNameWithoutExtension(xriFilePath);

            if (_items.TryGetValue(id, out var existing))
            {
                return existing;
            }

            // Retry file open in case file is still being written by X-ray software
            XRSImage? img = null;
            for (int retry = 0; retry < 5; retry++)
            {
                try
                {
                    img = _archiver.GetImage(xriFilePath);
                    if (img != null) break;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }

            if (img == null)
            {
                _logger.LogWarning("Failed to load XRSImage from {Path}", xriFilePath);
                return null;
            }

            var pngFileName = $"{id}.png";
            var outPngPath = Path.Combine(_cacheFolder, pngFileName);

            if (!File.Exists(outPngPath))
            {
                img.SaveAsCombinedPng(outPngPath);
            }

            // Parse timestamp from filename: yyyy-MM-dd_HH-mm-ss,ff
            DateTime createdAt = DateTime.Now;
            try
            {
                if (DateTime.TryParseExact(id, "yyyy-MM-dd_HH-mm-ss,ff", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                {
                    createdAt = dt;
                }
                else
                {
                    createdAt = File.GetCreationTime(xriFilePath);
                }
            }
            catch
            {
                createdAt = File.GetCreationTime(xriFilePath);
            }

            var item = new RejectItem
            {
                Id = id,
                FileName = fileName,
                CreatedAt = createdAt,
                ProductName = string.IsNullOrWhiteSpace(img.ProductName) ? "Mẫu không tên" : img.ProductName.Trim(),
                Width = img.Width,
                Height = img.Height,
                ConveyingSpeed = img.ConveyingSpeed,
                SourceVoltage = img.SourceVoltage,
                SourceCurrent = img.SourceCurrent,
                IsContaminant = img.IsContaminant(),
                HasErrors = img.HasErrors(),
                ImageUrl = $"/images/{pngFileName}",
                ThumbnailUrl = $"/images/{pngFileName}",
                FileSizeBytes = new FileInfo(xriFilePath).Length.ToString()
            };

            _items[id] = item;
            _logger.LogInformation("Processed Reject: {Id} | Product: {Product} | {W}x{H}", id, item.ProductName, item.Width, item.Height);
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting XRI file {Path}", xriFilePath);
            return null;
        }
    }
}
