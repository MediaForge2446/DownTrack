using System.Windows.Forms;
using DownTrack.Application.Services;

namespace DownTrack.Infrastructure.Windows;

public sealed class WindowsFolderPicker : IFolderPicker
{
    public string? PickFolder(string? initialPath = null)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Choose a DownTrack library root folder",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = true
        };

        if (!string.IsNullOrWhiteSpace(initialPath) && Directory.Exists(initialPath))
            dialog.SelectedPath = initialPath;

        return dialog.ShowDialog() == DialogResult.OK
            ? dialog.SelectedPath
            : null;
    }
}
