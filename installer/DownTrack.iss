#define MyAppName "DownTrack"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "MediaForge2446"
#define MyAppExeName "DownTrack.exe"

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"; LanguageName: "English"; LanguageID: $0409
Name: "he"; MessagesFile: "compiler:Default.isl"; LanguageName: "עברית"; LanguageID: $040D
Name: "es"; MessagesFile: "compiler:Default.isl"; LanguageName: "Español"; LanguageID: $0C0A
Name: "fr"; MessagesFile: "compiler:Default.isl"; LanguageName: "Français"; LanguageID: $040C
Name: "de"; MessagesFile: "compiler:Default.isl"; LanguageName: "Deutsch"; LanguageID: $0407
Name: "it"; MessagesFile: "compiler:Default.isl"; LanguageName: "Italiano"; LanguageID: $0410
Name: "pt"; MessagesFile: "compiler:Default.isl"; LanguageName: "Português"; LanguageID: $0416
Name: "nl"; MessagesFile: "compiler:Default.isl"; LanguageName: "Nederlands"; LanguageID: $0413
Name: "pl"; MessagesFile: "compiler:Default.isl"; LanguageName: "Polski"; LanguageID: $0415
Name: "cs"; MessagesFile: "compiler:Default.isl"; LanguageName: "Čeština"; LanguageID: $0405
Name: "tr"; MessagesFile: "compiler:Default.isl"; LanguageName: "Türkçe"; LanguageID: $041F
Name: "uk"; MessagesFile: "compiler:Default.isl"; LanguageName: "Українська"; LanguageID: $0422
Name: "ru"; MessagesFile: "compiler:Default.isl"; LanguageName: "Русский"; LanguageID: $0419
Name: "ar"; MessagesFile: "compiler:Default.isl"; LanguageName: "العربية"; LanguageID: $0401
Name: "el"; MessagesFile: "compiler:Default.isl"; LanguageName: "Ελληνικά"; LanguageID: $0408
Name: "ro"; MessagesFile: "compiler:Default.isl"; LanguageName: "Română"; LanguageID: $0418
Name: "ja"; MessagesFile: "compiler:Default.isl"; LanguageName: "日本語"; LanguageID: $0411
Name: "ko"; MessagesFile: "compiler:Default.isl"; LanguageName: "한국어"; LanguageID: $0412
Name: "zhcn"; MessagesFile: "compiler:Default.isl"; LanguageName: "简体中文"; LanguageID: $0804
Name: "zhtw"; MessagesFile: "compiler:Default.isl"; LanguageName: "繁體中文"; LanguageID: $0404

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
