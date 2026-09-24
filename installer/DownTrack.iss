#define MyAppName "DownTrack"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "MediaForge2446"
#define MyAppExeName "DownTrack.exe"

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "he"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"
Name: "it"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "pt"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "nl"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "pl"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "cs"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "tr"; MessagesFile: "compiler:Languages\Turkish.isl"
Name: "uk"; MessagesFile: "compiler:Languages\Ukrainian.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "ar"; MessagesFile: "compiler:Languages\Arabic.isl"
Name: "el"; MessagesFile: "compiler:Languages\Greek.isl"
Name: "ro"; MessagesFile: "compiler:Languages\Romanian.isl"
Name: "ja"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "ko"; MessagesFile: "compiler:Languages\Korean.isl"
Name: "zhcn"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"
Name: "zhtw"; MessagesFile: "compiler:Languages\ChineseTraditional.isl"

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
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=lowest
SetupIconFile=..\src\DownTrack.App\Assets\Brand\downtrack.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
DisableProgramGroupPage=yes

[Files]
Source: "..\artifacts\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autodesktop}\DownTrack"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\DownTrack"; Filename: "{app}\{#MyAppExeName}"

[INI]
Filename: "{app}\DownTrack.Install.ini"; Section: "Install"; Key: "Version"; String: "{#MyAppVersion}"
Filename: "{app}\DownTrack.Install.ini"; Section: "Install"; Key: "InstalledUtc"; String: "{code:GetInstallUtc}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch DownTrack"; Flags: nowait postinstall skipifsilent

[Code]
function GetInstallUtc(Param: String): String;
begin
  Result := GetDateTimeString('yyyy-mm-dd hh:nn:ss', '-', ':');
end;
