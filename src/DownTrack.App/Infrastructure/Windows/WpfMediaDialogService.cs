using System.Windows;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Views;

namespace DownTrack.Infrastructure.Windows;

public sealed class WpfMediaDialogService(IMediaResolver resolver) : IMediaDialogService
{
    public IReadOnlyList<MediaDownloadSpec> Show(string currentFolder)
    {
        var owner = System.Windows.Application.Current.MainWindow;
        var dialog = new AddMediaWindow(resolver, currentFolder)
        {
            Owner = owner,
            WindowStartupLocation = owner is null
                ? WindowStartupLocation.CenterScreen
                : WindowStartupLocation.CenterOwner,
            ShowInTaskbar = false
        };

        try
        {
            return dialog.ShowDialog() == true
                ? dialog.SelectedSpecs
                : [];
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                owner,
                $"DownTrack could not open the Add Media window.\n\n{ex.Message}",
                "Add Media",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            return [];
        }
    }
}
