namespace DownTrack.Core.Models;

public sealed class UserSettings
{
    public string Theme { get; set; } = "System";
    public string Language { get; set; } = "auto";
    public bool AutoUpdate { get; set; } = true;
    public bool AutoUpdateTools { get; set; } = true;
    public string DefaultFormat { get; set; } = "Mp3";
    public int DefaultAudioQuality { get; set; } = 128;
    public int DefaultVideoQuality { get; set; } = 720;
}