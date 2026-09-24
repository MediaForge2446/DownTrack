using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Views;

namespace DownTrack.Infrastructure.Windows;

public sealed class WpfMediaDialogService(IMediaResolver resolver) : IMediaDialogService
{
    public IReadOnlyList<MediaDownloadSpec> Show(string currentFolder)
    {
        var dialog = new AddMediaWindow(resolver, currentFolder);
        return dialog.ShowDialog() == true
            ? dialog.SelectedSpecs
            : [];
    }
}
