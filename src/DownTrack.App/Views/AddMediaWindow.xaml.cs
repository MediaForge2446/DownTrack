using System.Windows;
using System.Windows.Input;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.ViewModels;

namespace DownTrack.Views;

public partial class AddMediaWindow : Window
{
    private readonly AddMediaViewModel _viewModel;
    private bool _isClosing;

    public AddMediaWindow(IMediaResolver resolver, string currentFolder)
    {
        InitializeComponent();

        _viewModel = new AddMediaViewModel(resolver, currentFolder);
        _viewModel.Accepted += OnAccepted;
        DataContext = _viewModel;

        Loaded += OnLoaded;
        Closing += OnClosing;
    }

    public IReadOnlyList<MediaDownloadSpec> SelectedSpecs { get; private set; } = [];

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Owner ??= System.Windows.Application.Current.MainWindow;

        var workArea = SystemParameters.WorkArea;
        var maxWidth = Math.Min(980, Math.Max(MinWidth, workArea.Width * 0.80));
        var maxHeight = Math.Min(700, Math.Max(MinHeight, workArea.Height * 0.82));

        Width = maxWidth;
        Height = maxHeight;
        WindowStartupLocation = Owner is null
            ? WindowStartupLocation.CenterScreen
            : WindowStartupLocation.CenterOwner;

        UrlBox.Focus();
        UrlBox.CaretIndex = UrlBox.Text.Length;
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        _isClosing = true;
        _viewModel.CancelAnalysis();
    }

    private void OnAccepted(IReadOnlyList<MediaDownloadSpec> specs) =>
        SelectedSpecs = specs;

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (_isClosing || _viewModel.IsResolving)
            return;

        _viewModel.Accept();

        if (SelectedSpecs.Count > 0)
            DialogResult = true;
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