namespace DownTrack.Application.Services;

public interface IFolderPicker
{
    string? PickFolder(string? initialPath = null);
}
