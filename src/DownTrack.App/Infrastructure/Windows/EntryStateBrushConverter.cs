using System.Globalization;
using System.Windows.Data;
using DownTrack.Core.Models;

namespace DownTrack.Infrastructure.Windows;

public sealed class EntryStateBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            VirtualEntryState.Pending => System.Windows.Application.Current.Resources["PendingBrush"],
            VirtualEntryState.Error => System.Windows.Application.Current.Resources["ErrorBrush"],
            _ => System.Windows.Application.Current.Resources["TealBrush"]
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}
