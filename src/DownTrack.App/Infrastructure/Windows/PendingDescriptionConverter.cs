using System.Globalization;
using System.Windows.Data;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Windows;

public sealed class PendingDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not PendingChange change)
            return string.Empty;

        var sourceName = Name(change.SourcePath);
        var targetName = Name(change.TargetPath);
        var mediaName = string.IsNullOrWhiteSpace(change.Media?.Title)
            ? targetName
            : change.Media!.Title;

        return change.ChangeType switch
        {
            PendingChangeType.CreateFolder =>
                LocalizationService.Instance.T("Pending.CreateFolder", targetName),
            PendingChangeType.Rename =>
                LocalizationService.Instance.T("Pending.Rename", sourceName, targetName),
            PendingChangeType.Delete =>
                LocalizationService.Instance.T("Pending.Delete", sourceName),
            PendingChangeType.Download =>
                LocalizationService.Instance.T("Pending.Download", mediaName),
            _ => LocalizationService.Instance.T("Pending.Generic")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;

    private static string Name(string? path) =>
        string.IsNullOrWhiteSpace(path)
            ? string.Empty
            : Path.GetFileName(path);
}