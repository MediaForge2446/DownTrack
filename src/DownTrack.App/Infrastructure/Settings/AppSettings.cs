using DownTrack.Core.Enums;

namespace DownTrack.Infrastructure.Settings;

public enum AppThemeMode
{
    System,
    Light,
    Dark
}

public sealed class AppSettings
{
    public string LanguageCode { get; set; } = "auto";
    public AppThemeMode Theme { get; set; } = AppThemeMode.System;
    public bool AutoUpdateApp { get; set; } = true;
    public bool AutoUpdateTools { get; set; } = true;
    public MediaFormat DefaultFormat { get; set; } = MediaFormat.Mp3;
    public AudioQuality DefaultAudioQuality { get; set; } = AudioQuality.Kbps128;
    public VideoQuality DefaultVideoQuality { get; set; } = VideoQuality.P720;
}