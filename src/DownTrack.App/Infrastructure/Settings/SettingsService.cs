using System.Text.Json;
using DownTrack.Infrastructure;

namespace DownTrack.Infrastructure.Settings;

public sealed class SettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _path = Path.Combine(
        AppPaths.RootDirectory,
        "settings.json");

    private AppSettings _current = new();

    public static SettingsService Instance { get; } = new();

    public AppSettings Current => _current;

    public async Task InitializeAsync()
    {
        try
        {
            if (!File.Exists(_path))
            {
                await SaveAsync();
                return;
            }

            await using var stream = File.OpenRead(_path);
            _current = await JsonSerializer.DeserializeAsync<AppSettings>(stream, JsonOptions)
                       ?? new AppSettings();
        }
        catch
        {
            _current = new AppSettings();
        }
    }

    public async Task SaveAsync()
    {
        Directory.CreateDirectory(AppPaths.RootDirectory);

        var temp = _path + ".tmp";
        await using (var stream = File.Create(temp))
        {
            await JsonSerializer.SerializeAsync(stream, _current, JsonOptions);
            await stream.FlushAsync();
        }

        File.Move(temp, _path, true);
    }

    public async Task UpdateAsync(Action<AppSettings> update)
    {
        update(_current);
        await SaveAsync();
    }
}