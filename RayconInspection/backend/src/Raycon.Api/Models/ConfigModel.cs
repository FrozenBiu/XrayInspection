namespace Raycon.Api.Models;

public class ConfigModel
{
    public string MachineId { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string WatchDirectory { get; set; } = string.Empty;
}

public class UpdateConfigResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int TotalLoaded { get; set; }
    public string WatchDirectory { get; set; } = string.Empty;
}

