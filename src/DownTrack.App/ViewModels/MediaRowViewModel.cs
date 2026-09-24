using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class MediaRowViewModel : ObservableObject
{
    private string _title;
    private bool _selected = true;
    private MediaFormat _format = MediaFormat.Mp3;
    private AudioQuality _audioQuality = AudioQuality.Kbps128;
    private VideoQuality _videoQuality = VideoQuality.P720;

    public MediaRowViewModel(MediaDownloadSpec source)
    {
        SourceUrl = source.SourceUrl;
        ThumbnailUrl = source.ThumbnailUrl;
        _title = source.Title;
        _format = source.Format;
        _audioQuality = source.AudioQuality;
        _videoQuality = source.VideoQuality;
    }

    public string SourceUrl { get; }
    public string? ThumbnailUrl { get; }

    public IReadOnlyList<MediaFormat> Formats { get; } = Enum.GetValues<MediaFormat>();
    public IReadOnlyList<AudioQuality> AudioQualities { get; } = Enum.GetValues<AudioQuality>();
    public IReadOnlyList<VideoQuality> VideoQualities { get; } = Enum.GetValues<VideoQuality>();

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public MediaFormat Format
    {
        get => _format;
        set
        {
            if (SetProperty(ref _format, value))
                OnPropertyChanged(nameof(IsVideo));
        }
    }

    public AudioQuality AudioQuality
    {
        get => _audioQuality;
        set => SetProperty(ref _audioQuality, value);
    }

    public VideoQuality VideoQuality
    {
        get => _videoQuality;
        set => SetProperty(ref _videoQuality, value);
    }

    public bool IsVideo => Format == MediaFormat.Mp4;

    public MediaDownloadSpec ToSpec() => new()
    {
        SourceUrl = SourceUrl,
        Title = string.IsNullOrWhiteSpace(Title) ? LocalizationService.Instance.T("Media.Untitled") : Title.Trim(),
        ThumbnailUrl = ThumbnailUrl,
        Format = Format,
        AudioQuality = AudioQuality,
        VideoQuality = VideoQuality
    };
}
