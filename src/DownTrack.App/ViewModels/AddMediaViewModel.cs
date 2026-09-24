using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class AddMediaViewModel : ObservableObject
{
    private readonly IMediaResolver _resolver;
    private string _url = string.Empty;
    private string _status = LocalizationService.Instance.T("AddMedia.PastePrompt");
    private bool _isResolving;
    private CancellationTokenSource? _analysisCts;

    public AddMediaViewModel(IMediaResolver resolver, string currentFolder)
    {
        _resolver = resolver;
        CurrentFolder = currentFolder;

        AnalyzeCommand = new AsyncRelayCommand(
            AnalyzeAsync,
            () => !_isResolving && !string.IsNullOrWhiteSpace(Url));

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

        LocalizationService.Instance.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
            {
                if (!IsResolving && Items.Count == 0)
                    Status = LocalizationService.Instance.T("AddMedia.PastePrompt");
            }
        };
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
        Status = LocalizationService.Instance.T("AddMedia.Preparing");
        AnalyzeCommand.RaiseCanExecuteChanged();

        _analysisCts?.Cancel();
        _analysisCts?.Dispose();
        _analysisCts = new CancellationTokenSource();

        try
        {
            var progress = new Progress<string>(message => Status = message);
            var resolved = await _resolver.ResolveAsync(
                Url,
                progress,
                _analysisCts.Token);

            Items.Clear();

            foreach (var spec in resolved)
            {
                if (string.IsNullOrWhiteSpace(spec.SourceUrl))
                    continue;

                Items.Add(new MediaRowViewModel(spec));
            }

            Status = Items.Count switch
            {
                0 => LocalizationService.Instance.T("AddMedia.NothingFound"),
                1 => LocalizationService.Instance.T("AddMedia.OneReady"),
                _ => LocalizationService.Instance.T("AddMedia.ManyReady", Items.Count)
            };
        }
        catch (OperationCanceledException)
        {
            Status = LocalizationService.Instance.T("AddMedia.AnalysisCancelled");
        }
        catch (Exception ex)
        {
            Items.Clear();
            Status = BuildUserSafeError(ex);
        }
        finally
        {
            _analysisCts?.Dispose();
            _analysisCts = null;
            IsResolving = false;
            AnalyzeCommand.RaiseCanExecuteChanged();
        }
    }

    public void CancelAnalysis()
    {
        _analysisCts?.Cancel();
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

    private static string BuildUserSafeError(Exception exception)
    {
        var message = exception.GetBaseException().Message.Trim();

        if (message.Length > 480)
            message = message[..480] + "…";

        return string.IsNullOrWhiteSpace(message)
            ? LocalizationService.Instance.T("AddMedia.ErrorFallback")
            : message;
    }
}