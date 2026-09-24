#define MyAppName "DownTrack"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "MediaForge2446"
#define MyAppExeName "DownTrack.exe"

[Setup]
AppId={{B57A9A4C-0A15-47C6-9F18-2C0EDB2E6E2D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\DownTrack
DefaultGroupName={#MyAppName}
OutputDir=..\artifacts\installer
OutputBaseFilename=Setup
Compression=lzma2
SolidCompression=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=lowest
SetupIconFile=..\src\DownTrack.App\Assets\Brand\downtrack.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
DisableProgramGroupPage=yes

[Files]
Source: "..\artifacts\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autodesktop}\DownTrack"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\DownTrack"; Filename: "{app}\{#MyAppExeName}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch DownTrack"; Flags: nowait postinstall skipifsilent
