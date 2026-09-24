using System.Text.Json;
using DownTrack.Core.Models;

namespace DownTrack.Infrastructure.Settings;

public sealed class AppSettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _path;
    private readonly object _gate = new();

    private AppSettingsService()
    {
        _path = Path.Combine(AppPaths.RootDirectory, "settings.json");
        Current = Load();
    }

    public static AppSettingsService Instance { get; } = new();

    public UserSettings Current { get; private set; }

    public event EventHandler? Changed;

    public void Initialize()
    {
        ThemeService.Instance.Apply(Current.Theme);
    }

    public void Update(Action<UserSettings> update)
    {
        lock (_gate)
        {
            update(Current);
            Save();
        }

        Changed?.Invoke(this, EventArgs.Empty);
    }

    private UserSettings Load()
    {
        try
        {
            if (!File.Exists(_path))
                return new UserSettings();

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<UserSettings>(json, JsonOptions)
                   ?? new UserSettings();
        }
        catch
        {
            return new UserSettings();
        }
    }

    private void Save()
    {
        try
        {
            Directory.CreateDirectory(AppPaths.RootDirectory);
            var temp = _path + ".tmp";
            var json = JsonSerializer.Serialize(Current, JsonOptions);
            File.WriteAllText(temp, json);

            if (File.Exists(_path))
                File.Copy(_path, _path + ".bak", true);

            File.Move(temp, _path, true);
        }
        catch
        {
            // Settings are non-critical; the app continues with in-memory values.
        }
    }
}