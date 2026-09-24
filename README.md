# DownTrack

DownTrack is a clean-slate premium Windows media workspace: an Explorer-style library with staged filesystem changes and background media downloading.

## Product shape

- No application-wide sidebar.
- Home screen shows configured root folders.
- Root folders open an Explorer command surface.
- Folder creation, rename, delete and downloads are staged as virtual changes.
- Staged items appear immediately in the Explorer with a pending state.
- Physical disk changes happen only after **Save Changes**.
- Pending changes are persisted locally so the virtual workspace can survive restarts.
- Add Media opens a floating modal and targets the folder currently open in Explorer.
- Each media row has independent title, format, audio quality and video quality controls.
- MP3 defaults to 128 kbps.
- MP4 defaults to 720p.
- Media processing runs through hidden background processes.

## Architecture

Core contains domain models and enums.

Application contains commands and service contracts.

Infrastructure contains persistence, staging projection, media engine management, process execution and commit services.

Views and ViewModels contain the WPF presentation layer.

The repository intentionally starts from zero and does not reuse legacy MediaForge source code.

## Build

Requires Windows and the .NET 10 SDK.

~~~powershell
dotnet restore DownTrack.sln
dotnet build DownTrack.sln -c Release -p:Platform=x64
dotnet publish src/DownTrack.App/DownTrack.App.csproj -c Release -r win-x64 --self-contained true -o artifacts/publish -p:Platform=x64
~~~

GitHub Actions creates an Inno Setup installer artifact named Setup.exe.

## Media Engine

The app can install yt-dlp and FFmpeg into:

%LocalAppData%\DownTrack\Tools

The bootstrapper verifies SHA-256 checksums before activating each tool.
