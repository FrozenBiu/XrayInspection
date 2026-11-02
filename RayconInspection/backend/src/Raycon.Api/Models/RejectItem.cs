using System;

namespace Raycon.Api.Models;

public class RejectItem
{
    public string Id { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public float ConveyingSpeed { get; set; }
    public float SourceVoltage { get; set; }
    public float SourceCurrent { get; set; }
    public bool IsContaminant { get; set; }
    public bool HasErrors { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string FileSizeBytes { get; set; } = string.Empty;
}

