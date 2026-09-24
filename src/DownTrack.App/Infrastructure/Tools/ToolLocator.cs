using DownTrack.Infrastructure;

namespace DownTrack.Infrastructure.Tools;

public sealed class ToolLocator
{
    public bool HasBundledMediaEngine =>
        File.Exists(AppPaths.BundledYtDlpPath) &&
        File.Exists(AppPaths.BundledFfmpegPath) &&
        File.Exists(AppPaths.BundledFfprobePath) &&
        File.Exists(AppPaths.BundledDenoPath);

    private bool HasUserMediaEngine =>
        File.Exists(AppPaths.YtDlpPath) &&
        File.Exists(Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe")) &&
        File.Exists(Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe")) &&
        File.Exists(AppPaths.DenoPath);

    public string GetYtDlpPath()
    {
        if (HasUserMediaEngine)
            return AppPaths.YtDlpPath;

        if (HasBundledMediaEngine)
            return AppPaths.BundledYtDlpPath;

        throw new FileNotFoundException(
            "yt-dlp is not installed.",
            AppPaths.YtDlpPath);
    }

    public string GetToolsDirectory()
    {
        if (HasUserMediaEngine)
            return AppPaths.ToolsDirectory;

        if (HasBundledMediaEngine)
            return AppPaths.BundledToolsDirectory;

        throw new FileNotFoundException(
            "FFmpeg is not installed correctly.",
            AppPaths.ToolsDirectory);
    }

    public string GetDenoPath()
    {
        if (HasUserMediaEngine)
            return AppPaths.DenoPath;

        if (HasBundledMediaEngine)
            return AppPaths.BundledDenoPath;

        throw new FileNotFoundException(
            "Deno is not installed correctly.",
            AppPaths.DenoPath);
    }
}