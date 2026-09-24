using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Updates;

public sealed record AppUpdateInfo(
    string Version,
    string SetupUrl,
    string SetupSha256,
    long SetupSize,
    bool IsUpdateAvailable);

public sealed class AppUpdateService
{
    private const string ManifestUrl =
        "https://github.com/MediaForge2446/DownTrack/releases/download/nightly/latest.ini";

    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromMinutes(3)
    };

    public static AppUpdateService Instance { get; } = new();

    public async Task<AppUpdateInfo> CheckAsync(
        CancellationToken cancellationToken = default)
    {
        var ini = await Client.GetStringAsync(ManifestUrl, cancellationToken);

        var version = Read(ini, "Version")
            ?? throw new InvalidOperationException(
                LocalizationService.Instance.T("Updates.InvalidManifest"));

        var url = Read(ini, "SetupUrl")
            ?? throw new InvalidOperationException(
                LocalizationService.Instance.T("Updates.InvalidManifest"));

        var hash = Read(ini, "SetupSha256")
            ?? throw new InvalidOperationException(
                LocalizationService.Instance.T("Updates.InvalidManifest"));

        var size = long.TryParse(Read(ini, "SetupSize"), out var parsed)
            ? parsed
            : 0;

        var current = Assembly.GetEntryAssembly()?.GetName().Version
            ?? new Version(0, 0, 0);

        var available = Version.TryParse(
                NormalizeVersion(version),
                out var latest)
            && latest > current;

        return new AppUpdateInfo(
            version,
            url,
            hash,
            size,
            available);
    }

    public async Task ApplyAsync(
        AppUpdateInfo update,
        CancellationToken cancellationToken = default)
    {
        if (!Uri.TryCreate(update.SetupUrl, UriKind.Absolute, out var uri) ||
            !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ||
            !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                LocalizationService.Instance.T("Updates.InvalidManifest"));
        }

        var temp = Path.Combine(
            Path.GetTempPath(),
            $"DownTrack-update-{Guid.NewGuid():N}.exe");

        try
        {
            await using (var source = await Client.GetStreamAsync(uri, cancellationToken))
            await using (var destination = File.Create(temp))
            {
                await source.CopyToAsync(destination, cancellationToken);
            }

            await using (var stream = File.OpenRead(temp))
            {
                var hash = await SHA256.HashDataAsync(stream, cancellationToken);
                var actual = Convert.ToHexString(hash);

                if (!actual.Equals(
                        update.SetupSha256,
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        LocalizationService.Instance.T("Updates.HashFailed"));
                }
            }

            if (update.SetupSize > 0 &&
                new FileInfo(temp).Length != update.SetupSize)
            {
                throw new InvalidOperationException(
                    LocalizationService.Instance.T("Updates.SizeMismatch"));
            }

            var psi = new ProcessStartInfo
            {
                FileName = temp,
                Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            if (!Process.Start(psi) is null)
                return;

            throw new InvalidOperationException(
                LocalizationService.Instance.T("Updates.StartFailed"));
        }
        finally
        {
            _ = Task.Run(async () =>
            {
                for (var attempt = 0; attempt < 20; attempt++)
                {
                    try
                    {
                        if (File.Exists(temp))
                            File.Delete(temp);
                        return;
                    }
                    catch
                    {
                        await Task.Delay(500);
                    }
                }
            });
        }
    }

    private static string? Read(string ini, string key)
    {
        foreach (var line in ini.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
                return trimmed[(key.Length + 1)..].Trim();
        }

        return null;
    }

    private static string NormalizeVersion(string value) =>
        value.Split('+', 2)[0].Trim();
}