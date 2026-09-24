using DownTrack.Core.Models;

namespace DownTrack.Application.Services;

public interface IMediaDialogService
{
    IReadOnlyList<MediaDownloadSpec> Show(string currentFolder);
}
