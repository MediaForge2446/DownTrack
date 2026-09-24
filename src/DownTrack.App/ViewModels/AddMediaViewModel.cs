using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;

namespace DownTrack.ViewModels;

public sealed class AddMediaViewModel : ObservableObject
{
    private readonly IMediaResolver _resolver;
    private string _url = string.Empty;
    private string _status = "Paste a YouTube video or playlist URL.";
    private bool _isResolving;

    public AddMediaViewModel(IMediaResolver resolver, string currentFolder)
    {
        _resolver = resolver;
        CurrentFolder = currentFolder;
        AnalyzeCommand = new AsyncRelayCommand(AnalyzeAsync, () => !_isResolving && !string.IsNullOrWhiteSpace(Url));
        SelectAllCommand = new RelayCommand(_ =>
        {
            foreach (var row in Items)
                row.Selected = true;
        });
        ApplyMp3Command = new RelayCommand(_ =>
        {
            foreach (var row in Items)
                row.Format = MediaFormat.Mp3;
        });
    }

    public string CurrentFolder { get; }
    public IReadOnlyList<MediaFormat> Formats { get; } = Enum.GetValues<MediaFormat>();
    public IReadOnlyList<AudioQuality> AudioQualities { get; } = Enum.GetValues<AudioQuality>();
    public IReadOnlyList<VideoQuality> VideoQualities { get; } = Enum.GetValues<VideoQuality>();
    public ObservableCollection<MediaRowViewModel> Items { get; } = [];

    public string Url
    {
        get => _url;
        set
        {
            if (SetProperty(ref _url, value))
                AnalyzeCommand.RaiseCanExecuteChanged();
        }
    }

    public string Status
    {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public bool IsResolving
    {
        get => _isResolving;
        private set => SetProperty(ref _isResolving, value);
    }

    public AsyncRelayCommand AnalyzeCommand { get; }
    public RelayCommand SelectAllCommand { get; }
    public RelayCommand ApplyMp3Command { get; }

    public event Action<IReadOnlyList<MediaDownloadSpec>>? Accepted;

    private async Task AnalyzeAsync()
    {
        IsResolving = true;
        Status = "Analyzing link…";
        AnalyzeCommand.RaiseCanExecuteChanged();

        try
        {
            var resolved = await _resolver.ResolveAsync(Url);
            Items.Clear();

            foreach (var spec in resolved)
            {
                var row = new MediaRowViewModel(spec);
                row.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Items));
                Items.Add(row);
            }

            Status = Items.Count switch
            {
                0 => "Nothing was found.",
                1 => "1 media item ready. Edit the options before adding.",
                _ => $"{Items.Count} media items ready. Each row is independent."
            };
        }
        catch (Exception ex)
        {
            Items.Clear();
            Status = ex.Message;
        }
        finally
        {
            IsResolving = false;
            AnalyzeCommand.RaiseCanExecuteChanged();
        }
    }

    public void Accept()
    {
        var specs = Items
            .Where(x => x.Selected)
            .Select(x => x.ToSpec())
            .ToList();

        if (specs.Count > 0)
            Accepted?.Invoke(specs);
    }
}
