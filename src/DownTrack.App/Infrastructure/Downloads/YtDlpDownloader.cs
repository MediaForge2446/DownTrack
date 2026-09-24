using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Tools;

namespace DownTrack.Infrastructure.Downloads;

public sealed class YtDlpDownloader(
    ToolLocator locator,
    IProcessRunner processRunner)
{
    public async Task DownloadAsync(
        MediaDownloadSpec spec,
        string targetPath,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath)
            ?? throw new InvalidOperationException("The destination folder is invalid."));

        var executable = locator.GetYtDlpPath();
        var toolsDirectory = locator.GetToolsDirectory();
        var denoPath = locator.GetDenoPath();
        var args = BuildArguments(spec, targetPath, toolsDirectory, denoPath);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromMinutes(30));

        ProcessResult? result = null;
        Exception? lastError = null;

        for (var attempt = 1; attempt <= 2; attempt++)
        {
            try
            {
                result = await processRunner.RunAsync(
                    executable,
                    args,
                    Path.GetDirectoryName(targetPath),
                    timeoutCts.Token);

                if (result.ExitCode == 0)
                    break;

                var detail = string.IsNullOrWhiteSpace(result.StandardError)
                    ? result.StandardOutput
                    : result.StandardError;

                lastError = new InvalidOperationException(
                    string.IsNullOrWhiteSpace(detail)
                        ? $"yt-dlp exited with code {result.ExitCode}."
                        : detail.Trim());

                if (attempt < 2)
                    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException("The media download timed out after 30 minutes.");
            }
        }

        if (result is null || result.ExitCode != 0)
            throw lastError ?? new InvalidOperationException("The media download failed.");

        if (!File.Exists(targetPath))
            throw new InvalidOperationException(
                $"yt-dlp reported success, but the expected file was not created: {targetPath}");

        var length = new FileInfo(targetPath).Length;
        if (length <= 0)
        {
            File.Delete(targetPath);
            throw new InvalidOperationException("The downloaded file is empty.");
        }
    }

    private static string BuildArguments(
        MediaDownloadSpec spec,
        string targetPath,
        string toolsDirectory,
        string denoPath)
    {
        var url = Quote(spec.SourceUrl);
        var output = Quote(targetPath);
        var ffmpegLocation = Quote(toolsDirectory);
        var jsRuntime = Quote("deno:" + denoPath);

        var common =
            $"--no-playlist --newline --no-overwrites --retries 3 --fragment-retries 3 " +
            $"--socket-timeout 20 --ffmpeg-location {ffmpegLocation} " +
            $"--js-runtimes {jsRuntime} --remote-components ejs:npm";

        return spec.Format switch
        {
            MediaFormat.Mp3 =>
                $"{common} -x --audio-format mp3 --audio-quality {((int)spec.AudioQuality)}K -o {output} {url}",

            MediaFormat.M4a =>
                $"{common} -x --audio-format m4a --audio-quality best -o {output} {url}",

            MediaFormat.Wav =>
                $"{common} -x --audio-format wav --audio-quality best -o {output} {url}",

            MediaFormat.Mp4 =>
                $"{common} -f {Quote(GetVideoSelector(spec.VideoQuality))} --merge-output-format mp4 -o {output} {url}",

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

    private static string Quote(string value) =>\n        char.ToString(char.Parse("\"")) + value.Replace(char.Parse("\""), "\"\\\"") + char.ToString(char.Parse("\""));
}