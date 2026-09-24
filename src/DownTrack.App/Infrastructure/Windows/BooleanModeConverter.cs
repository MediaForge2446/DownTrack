using System.Globalization;
using System.Windows.Data;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Windows;

public sealed class BooleanModeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is true
            ? LocalizationService.Instance.T("Media.Video")
            : LocalizationService.Instance.T("Media.Audio");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}