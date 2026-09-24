using System.Windows;
using System.Windows.Input;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.ViewModels;

namespace DownTrack.Views;

public partial class AddMediaWindow : Window
{
    private readonly AddMediaViewModel _viewModel;

    public AddMediaWindow(IMediaResolver resolver, string currentFolder)
    {
        InitializeComponent();

        _viewModel = new AddMediaViewModel(resolver, currentFolder);
        _viewModel.Accepted += OnAccepted;
        DataContext = _viewModel;

        Loaded += OnLoaded;
    }

    public IReadOnlyList<MediaDownloadSpec> SelectedSpecs { get; private set; } = [];

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Owner ??= Application.Current.MainWindow;

        if (Owner is null)
            return;

        var availableWidth = Math.Max(MinWidth, Owner.ActualWidth - 72);
        var availableHeight = Math.Max(MinHeight, Owner.ActualHeight - 96);

        Width = Math.Min(1250, availableWidth);
        Height = Math.Min(820, availableHeight);
    }

    private void OnAccepted(IReadOnlyList<MediaDownloadSpec> specs)
    {
        SelectedSpecs = specs;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Accept();

        if (SelectedSpecs.Count > 0)
            DialogResult = true;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            ToggleMaximize();
            return;
        }

        if (e.LeftButton == MouseButtonState.Pressed &&
            WindowState != WindowState.Maximized)
        {
            DragMove();
        }
    }

    private void ToggleMaximize() =>
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
}