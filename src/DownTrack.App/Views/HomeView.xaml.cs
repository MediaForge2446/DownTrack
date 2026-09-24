using System.Windows;
using System.Windows.Controls;
using DownTrack.Core.Models;

namespace DownTrack.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void Root_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.HomeViewModel viewModel &&
            sender is Button button &&
            button.Tag is RootFolder root)
        {
            viewModel.Open(root);
        }
    }
}
