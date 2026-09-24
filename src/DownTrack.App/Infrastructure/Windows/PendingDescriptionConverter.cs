using System.Globalization;
using System.Windows.Data;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Windows;

public sealed class PendingDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is PendingChange change
            ? change.ChangeType switch
            {
                PendingChangeType.CreateFolder =>
                    LocalizationService.Instance.T("Pending.CreateFolder", Path.GetFileName(change.TargetPath) ?? string.Empty),
                PendingChangeType.Rename =>
                    LocalizationService.Instance.T("Pending.Rename", Path.GetFileName(change.SourcePath) ?? string.Empty, Path.GetFileName(change.TargetPath) ?? string.Empty),
                PendingChangeType.Delete =>
                    LocalizationService.Instance.T("Pending.Delete", Path.GetFileName(change.SourcePath) ?? string.Empty),
                PendingChangeType.Download =>
                    LocalizationService.Instance.T("Pending.Download", change.Media?.Title ?? Path.GetFileName(change.TargetPath) ?? string.Empty ?? string.Empty),
                _ => LocalizationService.Instance.T("Pending.Generic")
            }
            : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}