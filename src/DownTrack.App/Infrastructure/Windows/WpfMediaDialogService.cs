using System.Windows;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Views;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Windows;

public sealed class WpfMediaDialogService(IMediaResolver resolver) : IMediaDialogService
{
    public IReadOnlyList<MediaDownloadSpec> Show(string currentFolder)
    {
        var owner = System.Windows.Application.Current.MainWindow;

        try
        {
            var dialog = new AddMediaWindow(resolver, currentFolder)
            {
                Owner = owner,
                WindowStartupLocation = owner is null
                    ? WindowStartupLocation.CenterScreen
                    : WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false
            };

            return dialog.ShowDialog() == true
                ? dialog.SelectedSpecs
                : [];
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                owner,
                LocalizationService.Instance.T("AddMedia.OpenError", ex.Message),
                LocalizationService.Instance.T("AddMedia.Title"),
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            return [];
        }
    }
}
