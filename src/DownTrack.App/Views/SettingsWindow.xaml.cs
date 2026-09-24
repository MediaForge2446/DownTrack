using System.Windows;
using System.Windows.Input;
using DownTrack.Application.Services;
using DownTrack.ViewModels;

namespace DownTrack.Views;

public partial class SettingsWindow : Window
{
    private readonly SettingsViewModel _viewModel;

    public SettingsWindow(IAppToolManager toolManager)
    {
        InitializeComponent();

        _viewModel = new SettingsViewModel(toolManager);
        DataContext = _viewModel;
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _viewModel.SaveAsync();
            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "DownTrack",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e) =>
        DialogResult = false;

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is System.Windows.Controls.Button)
            return;

        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}