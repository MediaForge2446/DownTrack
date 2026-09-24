using System.Globalization;
using System.Windows.Data;
using DownTrack.Core.Enums;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Windows;

public sealed class MediaOptionLabelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            MediaFormat.Mp3 => "MP3",
            MediaFormat.M4a => "M4A",
            MediaFormat.Mp4 => "MP4",
            MediaFormat.Wav => "WAV",
            AudioQuality.Kbps128 => "128 kbps",
            AudioQuality.Kbps192 => "192 kbps",
            AudioQuality.Kbps320 => "320 kbps",
            VideoQuality.P480 => "480p",
            VideoQuality.P720 => "720p",
            VideoQuality.P1080 => "1080p",
            VideoQuality.Source => LocalizationService.Instance.T("Settings.Original"),
            _ => value?.ToString() ?? string.Empty
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}
