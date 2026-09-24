using System.Text.Json;
using DownTrack.Core.Models;

namespace DownTrack.Infrastructure.Storage;

public sealed class JsonAppStateStore : IAppStateStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly string _filePath;

    public JsonAppStateStore(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<AppState> LoadAsync(CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        if (!File.Exists(_filePath))
            return new AppState();

        try
        {
            await using var stream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<AppState>(stream, JsonOptions, cancellationToken)
                   ?? new AppState();
        }
        catch (JsonException)
        {
            var backup = _filePath + ".bak";
            if (!File.Exists(backup))
                return new AppState();

            await using var stream = File.OpenRead(backup);
            return await JsonSerializer.DeserializeAsync<AppState>(stream, JsonOptions, cancellationToken)
                   ?? new AppState();
        }
    }

    public async Task SaveAsync(AppState state, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        await _gate.WaitAsync(cancellationToken);
        try
        {
            var tempPath = _filePath + ".tmp";
            var json = JsonSerializer.Serialize(state, JsonOptions);

            await File.WriteAllTextAsync(tempPath, json, cancellationToken);

            if (File.Exists(_filePath))
            {
                File.Copy(_filePath, _filePath + ".bak", overwrite: true);
                File.Delete(_filePath);
            }

            File.Move(tempPath, _filePath);
        }
        finally
        {
            _gate.Release();
        }
    }
}
