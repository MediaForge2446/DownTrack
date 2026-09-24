namespace DownTrack.Core.Models;

public sealed class RootFolder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    public override string ToString() => Name;
}
