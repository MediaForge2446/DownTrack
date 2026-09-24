using System.Windows;
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
        Loaded += (_, _) =>
        {
            Owner ??= Application.Current.MainWindow;
        };
    }

    public IReadOnlyList<MediaDownloadSpec> SelectedSpecs { get; private set; } = [];

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
}
