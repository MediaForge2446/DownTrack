using System.Globalization;
using System.Windows.Data;

namespace DownTrack.Infrastructure.Windows;

public sealed class BooleanModeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is true ? "Video" : "Audio";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}
