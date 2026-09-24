using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Tools;

namespace DownTrack.Infrastructure.Downloads;

public sealed class YtDlpDownloader(ToolLocator locator, IProcessRunner processRunner)
{
    public async Task DownloadAsync(
        MediaDownloadSpec spec,
        string targetPath,
        CancellationToken cancellationToken = default)
    {
        var executable = locator.GetYtDlpPath();
        var args = BuildArguments(spec, targetPath);
        var result = await processRunner.RunAsync(
            executable,
            args,
            Path.GetDirectoryName(targetPath),
            cancellationToken);

        if (result.ExitCode != 0)
        {
            var detail = string.IsNullOrWhiteSpace(result.StandardError)
                ? result.StandardOutput
                : result.StandardError;

            throw new InvalidOperationException(
                string.IsNullOrWhiteSpace(detail)
                    ? $"yt-dlp exited with code {result.ExitCode}."
                    : detail.Trim());
        }
    }

    private static string BuildArguments(MediaDownloadSpec spec, string targetPath)
    {
        var url = Quote(spec.SourceUrl);
        var output = Quote(targetPath);

        return spec.Format switch
        {
            MediaFormat.Mp3 =>
                $"--no-playlist --newline -x --audio-format mp3 --audio-quality {((int)spec.AudioQuality)}K -o {output} {url}",

            MediaFormat.M4a =>
                $"--no-playlist --newline -x --audio-format m4a --audio-quality best -o {output} {url}",

            MediaFormat.Wav =>
                $"--no-playlist --newline -x --audio-format wav --audio-quality best -o {output} {url}",

            MediaFormat.Mp4 =>
                $"--no-playlist --newline -f {Quote(GetVideoSelector(spec.VideoQuality))} --merge-output-format mp4 -o {output} {url}",

            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string GetVideoSelector(VideoQuality quality) =>
        quality switch
        {
            VideoQuality.P480 => "bv*[height<=480]+ba/b[height<=480]/b",
            VideoQuality.P720 => "bv*[height<=720]+ba/b[height<=720]/b",
            VideoQuality.P1080 => "bv*[height<=1080]+ba/b[height<=1080]/b",
            VideoQuality.Source => "bv*+ba/b",
            _ => "bv*[height<=720]+ba/b[height<=720]/b"
        };

    private static string Quote(string value) => $""{value.Replace(""", "\"")}"";
}
