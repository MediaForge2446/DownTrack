using System.Text.Json;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Tools;

namespace DownTrack.Infrastructure.Downloads;

public sealed class YouTubeMetadataResolver(
    ToolLocator locator,
    IProcessRunner processRunner,
    IAppToolManager toolManager) : IMediaResolver
{
    public async Task<IReadOnlyList<MediaDownloadSpec>> ResolveAsync(
        string url,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var trimmedUrl = url.Trim();

        if (!Uri.TryCreate(trimmedUrl, UriKind.Absolute, out var uri) ||
            !IsSupportedYouTubeHost(uri.Host))
        {
            throw new ArgumentException("Please paste a valid YouTube video or playlist URL.");
        }

        await toolManager.EnsureReadyAsync(progress, cancellationToken);

        var executable = locator.GetYtDlpPath();
        var deno = locator.GetDenoPath();
        var args =
            $"--flat-playlist --dump-single-json --skip-download --no-warnings " +
            $"--retries 3 --socket-timeout 20 " +
            $"--js-runtimes {Quote("deno:" + deno)} --remote-components ejs:npm {Quote(trimmedUrl)}";

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromMinutes(3));

        ProcessResult result;
        try
        {
            result = await processRunner.RunAsync(
                executable,
                args,
                cancellationToken: timeoutCts.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException("Media analysis timed out after 3 minutes.");
        }

        if (result.ExitCode != 0)
        {
            var detail = string.IsNullOrWhiteSpace(result.StandardError)
                ? result.StandardOutput
                : result.StandardError;

            throw new InvalidOperationException(
                string.IsNullOrWhiteSpace(detail)
                    ? $"Could not analyze the media (exit code {result.ExitCode})."
                    : detail.Trim());
        }

        var json = ExtractJson(result.StandardOutput);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (root.TryGetProperty("entries", out var entries) && entries.ValueKind == JsonValueKind.Array)
        {
            return entries
                .EnumerateArray()
                .Where(x => x.ValueKind == JsonValueKind.Object)
                .Select(ToSpec)
                .Where(x => !string.IsNullOrWhiteSpace(x.SourceUrl))
                .ToList();
        }

        return [ToSpec(root)];
    }

    private static bool IsSupportedYouTubeHost(string host) =>
        host.Equals("youtube.com", StringComparison.OrdinalIgnoreCase) ||
        host.EndsWith(".youtube.com", StringComparison.OrdinalIgnoreCase) ||
        host.Equals("youtu.be", StringComparison.OrdinalIgnoreCase);

    private static MediaDownloadSpec ToSpec(JsonElement item)
    {
        var id = GetString(item, "id");
        var sourceUrl = GetString(item, "webpage_url");

        if (string.IsNullOrWhiteSpace(sourceUrl) && !string.IsNullOrWhiteSpace(id))
            sourceUrl = $"https://www.youtube.com/watch?v={id}";

        return new MediaDownloadSpec
        {
            SourceUrl = sourceUrl ?? string.Empty,
            Title = GetString(item, "title") ?? id ?? "YouTube media",
            ThumbnailUrl = GetString(item, "thumbnail")
        };
    }

    private static string ExtractJson(string stdout)
    {
        var trimmed = stdout.Trim();
        var first = trimmed.IndexOf('{');
        var last = trimmed.LastIndexOf('}');

        if (first >= 0 && last > first)
            return trimmed[first..(last + 1)];

        throw new InvalidOperationException("The media resolver did not return metadata.");
    }

    private static string? GetString(JsonElement item, string property) =>
        item.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static string Quote(string value) =>
        "\"" + value.Replace("\"", "\\\"") + "\"";
}
