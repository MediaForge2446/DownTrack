using DownTrack.Core.Enums;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Settings;

public static class SettingsValueFormatter
{
    public static string Format(MediaFormat value) =>
        value switch
        {
            MediaFormat.Mp3 => "MP3",
            MediaFormat.M4a => "M4A",
            MediaFormat.Mp4 => "MP4",
            MediaFormat.Wav => "WAV",
            _ => value.ToString()
        };

    public static string Format(AudioQuality value) =>
        $"{(int)value} kbps";

    public static string Format(VideoQuality value) =>
        value == VideoQuality.Source
            ? LocalizationService.Instance.T("Settings.Original")
            : $"{(int)value}p";
}