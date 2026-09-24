using DownTrack.Core.Enums;

namespace DownTrack.Core.Models;

public sealed class MediaDownloadSpec
{
    public string SourceUrl { get; set; } = string.Empty;
    public string Title { get; set; } = "New media";
    public MediaFormat Format { get; set; } = MediaFormat.Mp3;
    public AudioQuality AudioQuality { get; set; } = AudioQuality.Kbps128;
    public VideoQuality VideoQuality { get; set; } = VideoQuality.P720;

    public string Extension => Format switch
    {
        MediaFormat.Mp3 => ".mp3",
        MediaFormat.M4a => ".m4a",
        MediaFormat.Mp4 => ".mp4",
        MediaFormat.Wav => ".wav",
        _ => ".mp3"
    };
}
