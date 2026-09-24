#define MyAppName "DownTrack"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "MediaForge2446"
#define MyAppExeName "DownTrack.exe"
#define ManifestUrl "https://github.com/MediaForge2446/DownTrack/releases/download/nightly/latest.ini"

[Setup]
AppId={{B57A9A4C-0A15-47C6-9F18-2C0EDB2E6E2D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\DownTrack
DefaultGroupName={#MyAppName}
OutputDir=..\artifacts\installer
OutputBaseFilename=DownTrack-Setup
SetupIconFile=..\src\DownTrack.App\Assets\Brand\downtrack.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=lowest
WizardStyle=modern windows11 includetitlebar hidebevels
WizardSizePercent=120,115
WizardImageFile=
WizardSmallImageFile=
WizardBackColor=$F8F7FC
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableReadyPage=yes
Compression=lzma2
SolidCompression=yes
CloseApplications=yes
RestartApplications=no
AllowNoIcons=yes
Uninstallable=yes
VersionInfoDescription=DownTrack Latest Installer
VersionInfoProductName=DownTrack
VersionInfoCompany=MediaForge2446
SetupLogging=yes

[Files]
Source: "{code:GetPayloadUrl}"; DestDir: "{app}"; DestName: "DownTrack-Payload.zip"; ExternalSize: {code:GetPayloadSize}; Hash: "{code:GetPayloadHash}"; Flags: external download extractarchive ignoreversion recursesubdirs createallsubdirs nocompression

[Icons]
Name: "{autodesktop}\DownTrack"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\DownTrack"; Filename: "{app}\{#MyAppExeName}"

[INI]
Filename: "{app}\DownTrack.Install.ini"; Section: "Install"; Key: "Version"; String: "{code:GetLatestVersion}"
Filename: "{app}\DownTrack.Install.ini"; Section: "Install"; Key: "PayloadSha256"; String: "{code:GetPayloadHash}"
Filename: "{app}\DownTrack.Install.ini"; Section: "MediaEngine"; Key: "YtDlpVersion"; String: "{code:GetToolVersion|YtDlpVersion}"
Filename: "{app}\DownTrack.Install.ini"; Section: "MediaEngine"; Key: "DenoVersion"; String: "{code:GetToolVersion|DenoVersion}"
Filename: "{app}\DownTrack.Install.ini"; Section: "MediaEngine"; Key: "FfmpegVersion"; String: "{code:GetToolVersion|FfmpegVersion}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Open DownTrack"; Flags: nowait postinstall skipifsilent; Check: IsInstallValid

[UninstallDelete]
Type: files; Name: "{app}\DownTrack-Payload.zip"
Type: files; Name: "{app}\DownTrack.Install.ini"

[Messages]
WelcomeLabel1=DownTrack
WelcomeLabel2=This tiny installer always checks for the latest verified build, then installs DownTrack and its current media engine.
SelectDirBrowseLabel=Installation folder:
FinishedHeadingLabel=DownTrack is ready
FinishedLabel=The latest verified DownTrack build is installed and ready to open.

[Code]
var
  LatestVersion: String;
  LatestPayloadUrl: String;
  LatestPayloadHash: String;
  LatestPayloadSize: Int64;
  LatestYtDlpVersion: String;
  LatestDenoVersion: String;
  LatestFfmpegVersion: String;
  ManifestLoaded: Boolean;
  InstallIsValid: Boolean;

function IsValidSha256(const Value: String): Boolean;
var
  I: Integer;
  C: Char;
begin
  Result := Length(Value) = 64;
  if not Result then
    Exit;

  for I := 1 to Length(Value) do
  begin
    C := Value[I];
    if not (((C >= '0') and (C <= '9')) or
            ((C >= 'a') and (C <= 'f')) or
            ((C >= 'A') and (C <= 'F'))) then
    begin
      Result := False;
      Exit;
    end;
  end;
end;

function OnManifestDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  Result := True;
  if WizardForm <> nil then
  begin
    if ProgressMax > 0 then
      WizardForm.StatusLabel.Caption :=
        Format('Checking latest release… %d%%', [(Progress * 100) div ProgressMax])
    else
      WizardForm.StatusLabel.Caption := 'Checking latest release…';
    WizardForm.Update;
  end;
end;

function LoadLatestManifest: Boolean;
var
  ManifestPath: String;
  SizeText: String;
begin
  Result := False;

  if ManifestLoaded then
  begin
    Result := True;
    Exit;
  end;

  try
    ManifestPath := ExpandConstant('{tmp}\downtrack-latest.ini');
    DeleteFile(ManifestPath);

    DownloadTemporaryFile(
      '{#ManifestUrl}',
      'downtrack-latest.ini',
      '',
      @OnManifestDownloadProgress);

    if not FileExists(ManifestPath) then
      RaiseException('The latest release manifest was not downloaded.');

    LatestVersion := Trim(GetIniString('release', 'Version', '', ManifestPath));
    LatestPayloadUrl := Trim(GetIniString('release', 'PayloadUrl', '', ManifestPath));
    LatestPayloadHash := Trim(GetIniString('release', 'PayloadSha256', '', ManifestPath));
    SizeText := Trim(GetIniString('release', 'PayloadSize', '', ManifestPath));
    LatestYtDlpVersion := Trim(GetIniString('release', 'YtDlpVersion', 'unknown', ManifestPath));
    LatestDenoVersion := Trim(GetIniString('release', 'DenoVersion', 'unknown', ManifestPath));
    LatestFfmpegVersion := Trim(GetIniString('release', 'FfmpegVersion', 'current', ManifestPath));

    if LatestVersion = '' then
      RaiseException('The latest release manifest has no version.');

    if LatestPayloadUrl = '' then
      RaiseException('The latest release manifest has no payload URL.');

    if Pos(
         'https://github.com/mediaforge2446/downtrack/releases/download/',
         LowerCase(LatestPayloadUrl)) <> 1 then
      RaiseException('The latest payload URL is not a trusted DownTrack release URL.');

    if not IsValidSha256(LatestPayloadHash) then
      RaiseException('The latest payload manifest contains an invalid SHA-256 value.');

    LatestPayloadSize := StrToInt64Def(SizeText, 0);
    if LatestPayloadSize <= 0 then
      RaiseException('The latest payload manifest contains an invalid size.');

    ManifestLoaded := True;
    Result := True;
  except
    Log(GetExceptionMessage);
    ManifestLoaded := False;
    Result := False;
  end;
end;

function GetPayloadUrl(Param: String): String;
begin
  if not ManifestLoaded then
    LoadLatestManifest;
  Result := LatestPayloadUrl;
end;

function GetPayloadHash(Param: String): String;
begin
  if not ManifestLoaded then
    LoadLatestManifest;
  Result := LatestPayloadHash;
end;

function GetPayloadSize(Param: String): String;
begin
  if not ManifestLoaded then
    LoadLatestManifest;
  Result := IntToStr(LatestPayloadSize);
end;

function GetLatestVersion(Param: String): String;
begin
  if not ManifestLoaded then
    LoadLatestManifest;
  Result := LatestVersion;
end;

function GetToolVersion(Param: String): String;
begin
  if not ManifestLoaded then
    LoadLatestManifest;

  if CompareText(Param, 'YtDlpVersion') = 0 then
    Result := LatestYtDlpVersion
  else if CompareText(Param, 'DenoVersion') = 0 then
    Result := LatestDenoVersion
  else if CompareText(Param, 'FfmpegVersion') = 0 then
    Result := LatestFfmpegVersion
  else
    Result := 'unknown';
end;

function IsInstallValid: Boolean;
begin
  Result := InstallIsValid;
end;

function VerifyInstalledFiles: Boolean;
var
  AppDir: String;
begin
  AppDir := ExpandConstant('{app}');
  Result :=
    FileExists(AppDir + '\DownTrack.exe') and
    FileExists(AppDir + '\Tools\yt-dlp.exe') and
    FileExists(AppDir + '\Tools\ffmpeg.exe') and
    FileExists(AppDir + '\Tools\ffprobe.exe') and
    FileExists(AppDir + '\Tools\deno.exe');
end;

procedure InitializeWizard;
var
  InstalledIni: String;
  InstalledVersion: String;
begin
  InstallIsValid := False;

  WizardForm.WelcomeLabel1.Caption := 'DownTrack';
  WizardForm.WelcomeLabel1.Font.Size := 24;
  WizardForm.WelcomeLabel1.Font.Style := [fsBold];
  WizardForm.WelcomeLabel2.Caption :=
    'A lightweight installer that always checks for the latest verified build, including the current media engine.';
  WizardForm.WelcomeLabel2.Font.Size := 10;

  WizardForm.NextButton.Caption := 'Get latest version';
  WizardForm.CancelButton.Caption := 'Close';
  WizardForm.BackButton.Visible := False;

  InstalledIni := ExpandConstant('{localappdata}\Programs\DownTrack\DownTrack.Install.ini');
  InstalledVersion := GetIniString('Install', 'Version', '', InstalledIni);

  if LoadLatestManifest then
  begin
    if InstalledVersion <> '' then
      WizardForm.StatusLabel.Caption :=
        Format('Installed: %s  •  Latest: %s', [InstalledVersion, LatestVersion])
    else
      WizardForm.StatusLabel.Caption :=
        Format('Latest verified build: %s', [LatestVersion]);

    WizardForm.WelcomeLabel2.Caption :=
      Format(
        'Latest verified build %s is ready. The installer will also bring the current media engine with it.',
        [LatestVersion]);
  end
  else
  begin
    WizardForm.StatusLabel.Caption := 'Latest version could not be checked yet.';
  end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;

  if CurPageID = wpWelcome then
  begin
    if not LoadLatestManifest then
    begin
      MsgBox(
        'DownTrack could not retrieve the latest release information.' + #13#10 + #13#10 +
        'Please check your internet connection and try again.',
        mbCriticalError,
        MB_OK);
      Result := False;
      Exit;
    end;

    WizardForm.NextButton.Caption := 'Download & install';
    WizardForm.StatusLabel.Caption :=
      Format('Latest verified build: %s', [LatestVersion]);
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    InstallIsValid := VerifyInstalledFiles;

    if InstallIsValid then
    begin
      WizardForm.StatusLabel.Caption :=
        Format('DownTrack %s is installed and ready.', [LatestVersion]);
    end
    else
    begin
      WizardForm.StatusLabel.Caption := 'Installation verification failed.';
      MsgBox(
        'DownTrack was downloaded, but the installed files could not be verified.' + #13#10 + #13#10 +
        'The application was not launched. Please try the installer again.',
        mbCriticalError,
        MB_OK);
    end;
  end;
end;
