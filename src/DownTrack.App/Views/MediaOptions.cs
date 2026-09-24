using DownTrack.Core.Enums;

namespace DownTrack.Views;

public static class MediaOptions
{
    public static IReadOnlyList<MediaFormat> Formats { get; } = Enum.GetValues<MediaFormat>();
    public static IReadOnlyList<AudioQuality> AudioQualities { get; } = Enum.GetValues<AudioQuality>();
    public static IReadOnlyList<VideoQuality> VideoQualities { get; } = Enum.GetValues<VideoQuality>();
}
