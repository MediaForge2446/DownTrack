#define MyAppName "DownTrack"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "MediaForge2446"
#define MyAppExeName "DownTrack.exe"

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "he"; MessagesFile: "compiler:Default.isl"
Name: "es"; MessagesFile: "compiler:Default.isl"
Name: "fr"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Default.isl"
Name: "it"; MessagesFile: "compiler:Default.isl"
Name: "pt"; MessagesFile: "compiler:Default.isl"
Name: "nl"; MessagesFile: "compiler:Default.isl"
Name: "pl"; MessagesFile: "compiler:Default.isl"
Name: "cs"; MessagesFile: "compiler:Default.isl"
Name: "tr"; MessagesFile: "compiler:Default.isl"
Name: "uk"; MessagesFile: "compiler:Default.isl"
Name: "ru"; MessagesFile: "compiler:Default.isl"
Name: "ar"; MessagesFile: "compiler:Default.isl"
Name: "el"; MessagesFile: "compiler:Default.isl"
Name: "ro"; MessagesFile: "compiler:Default.isl"
Name: "ja"; MessagesFile: "compiler:Default.isl"
Name: "ko"; MessagesFile: "compiler:Default.isl"
Name: "zhcn"; MessagesFile: "compiler:Default.isl"
Name: "zhtw"; MessagesFile: "compiler:Default.isl"

[LangOptions]
en.LanguageName=English
en.LanguageID=$0409
he.LanguageName=עברית
he.LanguageID=$040D
he.RightToLeft=yes
es.LanguageName=Español
es.LanguageID=$0C0A
fr.LanguageName=Français
fr.LanguageID=$040C
de.LanguageName=Deutsch
de.LanguageID=$0407
it.LanguageName=Italiano
it.LanguageID=$0410
pt.LanguageName=Português
pt.LanguageID=$0416
nl.LanguageName=Nederlands
nl.LanguageID=$0413
pl.LanguageName=Polski
pl.LanguageID=$0415
cs.LanguageName=Čeština
cs.LanguageID=$0405
tr.LanguageName=Türkçe
tr.LanguageID=$041F
uk.LanguageName=Українська
uk.LanguageID=$0422
ru.LanguageName=Русский
ru.LanguageID=$0419
ar.LanguageName=العربية
ar.LanguageID=$0401
ar.RightToLeft=yes
el.LanguageName=Ελληνικά
el.LanguageID=$0408
ro.LanguageName=Română
ro.LanguageID=$0418
ja.LanguageName=日本語
ja.LanguageID=$0411
ko.LanguageName=한국어
ko.LanguageID=$0412
zhcn.LanguageName=简体中文
zhcn.LanguageID=$0804
zhtw.LanguageName=繁體中文
zhtw.LanguageID=$0404

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
