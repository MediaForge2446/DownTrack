using System.Globalization;
using System.Windows.Data;
using DownTrack.Core.Enums;

namespace DownTrack.Infrastructure.Windows;

public sealed class PendingStatusBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            PendingChangeStatus.Error => System.Windows.Application.Current.Resources["ErrorBrush"],
            PendingChangeStatus.Processing => System.Windows.Application.Current.Resources["AccentBrush"],
            _ => System.Windows.Application.Current.Resources["PendingBrush"]
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}
