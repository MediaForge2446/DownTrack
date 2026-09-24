using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DownTrack.Core.Models;

namespace DownTrack.Views;

public partial class ExplorerView : UserControl
{
    public ExplorerView()
    {
        InitializeComponent();
    }

    private ExplorerViewModel? ViewModel => DataContext as ExplorerViewModel;

    private void Entry_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ViewModel?.SelectedEntry is { IsDirectory: true } entry)
            ViewModel.NavigateTo(entry);
    }

    private void Folder_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox list && list.SelectedItem is VirtualEntry entry)
            ViewModel?.NavigateTo(entry);
    }

    private void CancelPending_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel is null)
            return;

        if (sender is Button button && button.DataContext is PendingChange change)
            ViewModel.CancelPending(change);
    }
}
