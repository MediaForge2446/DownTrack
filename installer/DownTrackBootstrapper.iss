#define MyAppName "DownTrack"
#define MyAppVersion "0.0.0"
#define MyAppPublisher "MediaForge2446"
#define ManifestUrl "https://github.com/MediaForge2446/DownTrack/releases/download/nightly/latest.ini"

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
en.LanguageCodePage=1252
he.LanguageName=עברית
he.LanguageID=$040D
he.LanguageCodePage=1255
he.RightToLeft=yes
es.LanguageName=Español
es.LanguageID=$0C0A
es.LanguageCodePage=1252
fr.LanguageName=Français
fr.LanguageID=$040C
fr.LanguageCodePage=1252
de.LanguageName=Deutsch
de.LanguageID=$0407
de.LanguageCodePage=1252
it.LanguageName=Italiano
it.LanguageID=$0410
it.LanguageCodePage=1252
pt.LanguageName=Português
pt.LanguageID=$0416
pt.LanguageCodePage=1252
nl.LanguageName=Nederlands
nl.LanguageID=$0413
nl.LanguageCodePage=1252
pl.LanguageName=Polski
pl.LanguageID=$0415
pl.LanguageCodePage=1250
cs.LanguageName=Čeština
cs.LanguageID=$0405
cs.LanguageCodePage=1250
tr.LanguageName=Türkçe
tr.LanguageID=$041F
tr.LanguageCodePage=1254
uk.LanguageName=Українська
uk.LanguageID=$0422
uk.LanguageCodePage=1251
ru.LanguageName=Русский
ru.LanguageID=$0419
ru.LanguageCodePage=1251
ar.LanguageName=العربية
ar.LanguageID=$0401
ar.LanguageCodePage=1256
ar.RightToLeft=yes
el.LanguageName=Ελληνικά
el.LanguageID=$0408
el.LanguageCodePage=1253
ro.LanguageName=Română
ro.LanguageID=$0418
ro.LanguageCodePage=1250
ja.LanguageName=日本語
ja.LanguageID=$0411
ja.LanguageCodePage=932
ko.LanguageName=한국어
ko.LanguageID=$0412
ko.LanguageCodePage=949
zhcn.LanguageName=简体中文
zhcn.LanguageID=$0804
zhcn.LanguageCodePage=936
zhtw.LanguageName=繁體中文
zhtw.LanguageID=$0404
zhtw.LanguageCodePage=950

[Setup]
AppId={{6A9A7C5F-47D7-4F6B-93E6-7BC1A0C6CF16}
AppName=DownTrack
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={tmp}\DownTrackBootstrap
OutputDir=..\artifacts\installer
OutputBaseFilename=DownTrack-WebSetup
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=lowest
Uninstallable=no
CreateAppDir=no
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableWelcomePage=no
DisableReadyPage=yes
DisableFinishedPage=yes
DisableStartupPrompt=yes
ShowLanguageDialog=no
LanguageDetectionMethod=uilanguage
WizardStyle=modern
Compression=lzma2
SolidCompression=yes
VersionInfoDescription=DownTrack Web Installer
VersionInfoProductName=DownTrack
VersionInfoCompany=MediaForge2446
SetupIconFile=..\src\DownTrack.App\Assets\Brand\downtrack.ico


[Files]
Source: "DownTrackInstallerTranslations.txt"; Flags: dontcopy

[Code]
function SetForegroundWindow(hWnd: Integer): Boolean;
  external 'SetForegroundWindow@user32.dll stdcall';
function BringWindowToTop(hWnd: Integer): Boolean;
  external 'BringWindowToTop@user32.dll stdcall';

var
  ContentPanel: TPanel;
  Dot1: TPanel;
  Dot2: TPanel;
  Dot3: TPanel;
  HeaderPanel: TPanel;
  BodyPanel: TPanel;
  AccentBar: TPanel;
  FooterPanel: TPanel;
  LogoMark: TPanel;
  LogoMarkText: TNewStaticText;
  LogoText: TNewStaticText;
  TaglineText: TNewStaticText;
  LanguageLabel: TNewStaticText;
  LanguageCombo: TNewComboBox;
  StatusText: TNewStaticText;
  VersionText: TNewStaticText;
  DetailText: TNewStaticText;
  ProgressBar: TNewProgressBar;
  PrimaryButton: TNewButton;
  SecondaryButton: TNewButton;
  LatestVersion: String;
  PayloadUrl: String;
  PayloadHash: String;
  PayloadSize: Int64;
  InstallDirectory: String;
  ManifestLoaded: Boolean;
  Installing: Boolean;
  AutoStartTriggered: Boolean;
  CurrentLanguage: String;
  SelectedLanguageMode: String;
  Translations: TStringList;

procedure InstallLatest(Sender: TObject); forward;

function LookupTranslation(const Id: String): String;
var
  I: Integer;
  Prefix: String;
  Entry: String;
begin
  Result := '';
  if not Assigned(Translations) then
    Exit;

  Prefix := Id + '=';
  for I := 0 to Translations.Count - 1 do
  begin
    Entry := Translations.Strings[I];
    if Pos(Prefix, Entry) = 1 then
    begin
      Result := Copy(
        Entry,
        Length(Prefix) + 1,
        Length(Entry) - Length(Prefix));
      Exit;
    end;
  end;
end;

function T(const Key: String): String;
var
  Value: String;
begin
  Value := LookupTranslation(CurrentLanguage + '.' + Key);

  if Value = '' then
    Value := LookupTranslation('en.' + Key);

  Result := Value;
end;

function LangCodeFromIndex(Index: Integer): String;
begin
  case Index of
    0: Result := 'auto';
    1: Result := 'en';
    2: Result := 'he';
    3: Result := 'es';
    4: Result := 'fr';
    5: Result := 'de';
    6: Result := 'it';
    7: Result := 'pt';
    8: Result := 'nl';
    9: Result := 'pl';
    10: Result := 'cs';
    11: Result := 'tr';
    12: Result := 'uk';
    13: Result := 'ru';
    14: Result := 'ar';
    15: Result := 'el';
    16: Result := 'ro';
    17: Result := 'ja';
    18: Result := 'ko';
    19: Result := 'zhcn';
    20: Result := 'zhtw';
  else
    Result := 'auto';
  end;
end;

function IsRtlLanguage(const Code: String): Boolean;
begin
  Result :=
    SameText(Code, 'he') or
    SameText(Code, 'ar');
end;

function DetectWindowsLanguage: String;
var
  Detected: String;
begin
  Detected := ActiveLanguage;

  if Detected = '' then
    Detected := 'en';

  if
    (not SameText(Detected, 'en')) and
    (not SameText(Detected, 'he')) and
    (not SameText(Detected, 'es')) and
    (not SameText(Detected, 'fr')) and
    (not SameText(Detected, 'de')) and
    (not SameText(Detected, 'it')) and
    (not SameText(Detected, 'pt')) and
    (not SameText(Detected, 'nl')) and
    (not SameText(Detected, 'pl')) and
    (not SameText(Detected, 'cs')) and
    (not SameText(Detected, 'tr')) and
    (not SameText(Detected, 'uk')) and
    (not SameText(Detected, 'ru')) and
    (not SameText(Detected, 'ar')) and
    (not SameText(Detected, 'el')) and
    (not SameText(Detected, 'ro')) and
    (not SameText(Detected, 'ja')) and
    (not SameText(Detected, 'ko')) and
    (not SameText(Detected, 'zhcn')) and
    (not SameText(Detected, 'zhtw')) then
    Detected := 'en';

  Result := Detected;
end;

function GetSavedLanguageMode: String;
var
  FileName: String;
begin
  FileName := ExpandConstant('{localappdata}\DownTrack\installer-language.ini');

  if FileExists(FileName) then
    Result := Trim(GetIniString('Installer', 'Language', 'auto', FileName))
  else
    Result := 'auto';

  if Result = '' then
    Result := 'auto';
end;

procedure SaveLanguageMode(const Value: String);
var
  FileName: String;
begin
  FileName := ExpandConstant('{localappdata}\DownTrack\installer-language.ini');
  ForceDirectories(ExtractFileDir(FileName));
  SetIniString('Installer', 'Language', Value, FileName);
end;

procedure PopulateTranslations;
var
  Lines: TArrayOfString;
  I: Integer;
  TranslationPath: String;
begin
  Translations := TStringList.Create;
  TranslationPath :=
    ExpandConstant('{tmp}\\DownTrackInstallerTranslations.txt');

  try
    ExtractTemporaryFile('DownTrackInstallerTranslations.txt');

    if not LoadStringsFromFile(TranslationPath, Lines) then
      RaiseException('Could not load installer translations.');

    for I := 0 to GetArrayLength(Lines) - 1 do
      if Trim(Lines[I]) <> '' then
        Translations.Add(Lines[I]);
  except
    Log(GetExceptionMessage);
  end;
end;

function SetTimer(
  hWnd: HWND;
  nIDEvent: UINT_PTR;
  uElapse: UINT;
  lpTimerFunc: NativeInt): UINT_PTR;
external 'SetTimer@user32.dll stdcall';

function KillTimer(hWnd: HWND; uIDEvent: UINT_PTR): Boolean;
external 'KillTimer@user32.dll stdcall';

procedure StartupInstallTimer(
  hWnd: HWND;
  Msg: UINT;
  TimerId: UINT_PTR;
  Tick: DWORD);
begin
  KillTimer(hWnd, TimerId);
  InstallLatest(nil);
end;

procedure ApplyLanguageToForm;
var
  Rtl: Boolean;
begin
  if CurrentLanguage = '' then
    CurrentLanguage := 'en';

  Rtl := IsRtlLanguage(CurrentLanguage);

  LogoText.Caption := T('Title');
  TaglineText.Caption := T('Tagline');
  LanguageLabel.Caption := T('Language');
  StatusText.Caption := T('Checking');
  VersionText.Caption := T('Progress');
  DetailText.Caption := '';
  PrimaryButton.Caption := T('Start');
  SecondaryButton.Caption := T('Close');

  LogoText.Alignment := taLeftJustify;
  TaglineText.Alignment := taLeftJustify;
  LanguageLabel.Alignment := taLeftJustify;
  StatusText.Alignment := taCenter;
  VersionText.Alignment := taCenter;
  DetailText.Alignment := taCenter;

  AccentBar.Left := ScaleX(28);
  LogoText.Left := ScaleX(46);
  TaglineText.Left := ScaleX(47);
  LanguageLabel.Left := ScaleX(505);
  LanguageCombo.Left := ScaleX(560);

  if Rtl then
  begin
    LogoText.Alignment := taRightJustify;
    TaglineText.Alignment := taRightJustify;
    LanguageLabel.Alignment := taRightJustify;

    AccentBar.Left := WizardForm.ClientWidth - ScaleX(33);
    LogoText.Left := WizardForm.ClientWidth - ScaleX(266);
    TaglineText.Left := WizardForm.ClientWidth - ScaleX(397);
    LanguageCombo.Left := ScaleX(30);
    LanguageLabel.Left := ScaleX(210);
  end;

  WizardForm.Caption := T('Title');
  WizardForm.Update;
end;

procedure LanguageChanged(Sender: TObject);
var
  Index: Integer;
begin
  Index := LanguageCombo.ItemIndex;
  SelectedLanguageMode := LangCodeFromIndex(Index);

  if SameText(SelectedLanguageMode, 'auto') then
    CurrentLanguage := DetectWindowsLanguage
  else
    CurrentLanguage := SelectedLanguageMode;

  SaveLanguageMode(SelectedLanguageMode);
  ApplyLanguageToForm;
  if not Installing then
    PrimaryButton.Enabled := True;
end;

function GetLatestVersionText: String;
var
  S: String;
begin
  S := T('Latest');
  StringChangeEx(S, '%1', LatestVersion, True);
  Result := S;
end;

function DownloadProgress(
  const Url, FileName: String;
  const Progress, ProgressMax: Int64): Boolean;
var
  DownloadedMb: Int64;
  TotalMb: Int64;
begin
  Result := True;

  if ProgressMax > 0 then
    ProgressBar.Position := (Progress * 100) div ProgressMax;

  DownloadedMb := Progress div 1048576;
  TotalMb := PayloadSize div 1048576;

  DetailText.Caption :=
    IntToStr(DownloadedMb) + ' MB / ' + IntToStr(TotalMb) + ' MB';

  StatusText.Caption := T('Downloading');
  WizardForm.Update;
end;

function LoadLatestManifest: Boolean;
var
  ManifestPath: String;
begin
  Result := False;
  ManifestLoaded := False;

  try
    ManifestPath := ExpandConstant('{tmp}\latest.ini');
    DeleteFile(ManifestPath);

    DownloadTemporaryFile(
      '{#ManifestUrl}',
      'latest.ini',
      '',
      nil);

    if not FileExists(ManifestPath) then
      RaiseException('Manifest was not downloaded.');

    LatestVersion :=
      Trim(GetIniString('release', 'Version', '', ManifestPath));
    PayloadUrl :=
      Trim(GetIniString('release', 'PayloadUrl', '', ManifestPath));
    PayloadHash :=
      LowerCase(Trim(
        GetIniString('release', 'PayloadSha256', '', ManifestPath)));
    PayloadSize :=
      StrToInt64Def(
        Trim(GetIniString('release', 'PayloadSize', '0', ManifestPath)),
        0);

    if LatestVersion = '' then
      RaiseException('Latest release has no version.');

    if Pos(
      'https://github.com/mediaforge2446/downtrack/releases/download/',
      LowerCase(PayloadUrl)) <> 1 then
      RaiseException('Payload URL is not a trusted DownTrack release URL.');

    if Length(PayloadHash) <> 64 then
      RaiseException('Latest payload hash is invalid.');

    if PayloadSize <= 0 then
      RaiseException('Latest payload size is invalid.');

    ManifestLoaded := True;
    Result := True;
  except
    Log(GetExceptionMessage);
    ManifestLoaded := False;
  end;
end;

function GetInstalledVersion: String;
var
  IniPath: String;
begin
  IniPath :=
    ExpandConstant(
      '{localappdata}\Programs\DownTrack\DownTrack.Install.ini');

  Result := Trim(
    GetIniString('Install', 'Version', '', IniPath));
end;

function ExtractPayload(
  const PayloadPath, InstallPath: String): Boolean;
var
  ScriptPath: String;
  PowerShellPath: String;
  ResultCode: Integer;
  Script: String;
begin
  Result := False;

  if not ForceDirectories(InstallPath) then
    RaiseException(
      'Could not create the DownTrack installation directory.');

  ScriptPath :=
    ExpandConstant('{tmp}\extract-downtrack.ps1');

  Script :=
    '$ErrorActionPreference = "Stop"' + #13#10 +
    'New-Item -ItemType Directory -Force -Path $args[1] | Out-Null' + #13#10 +
    'Expand-Archive -LiteralPath $args[0] -DestinationPath $args[1] -Force' + #13#10;

  if not SaveStringToFile(
    ScriptPath,
    Script,
    False) then
    RaiseException(
      'Could not prepare the payload extraction script.');

  PowerShellPath :=
    ExpandConstant(
      '{sys}\WindowsPowerShell\v1.0\powershell.exe');

  if not Exec(
    PowerShellPath,
    '-NoLogo -NoProfile -NonInteractive -WindowStyle Hidden ' +
    '-ExecutionPolicy Bypass -File "' +
    ScriptPath + '" "' + PayloadPath + '" "' +
    InstallPath + '"',
    '',
    SW_HIDE,
    ewWaitUntilTerminated,
    ResultCode) then
    RaiseException(
      'Windows PowerShell could not be started.');

  if ResultCode <> 0 then
    RaiseException(
      'Payload extraction returned exit code ' +
      IntToStr(ResultCode) + '.');

  Result :=
    FileExists(InstallPath + '\DownTrack.exe');
end;

procedure OpenDownTrack(Sender: TObject);
var
  ResultCode: Integer;
begin
  if FileExists(
    ExpandConstant(
      '{localappdata}\Programs\DownTrack\DownTrack.exe')) then
  begin
    Exec(
      ExpandConstant(
        '{localappdata}\Programs\DownTrack\DownTrack.exe'),
      '',
      '',
      SW_SHOWNORMAL,
      ewNoWait,
      ResultCode);
  end;

  WizardForm.Close;
end;

procedure CreateInstallShortcuts(
  const AppPath, InstallPath: String);
begin
  ForceDirectories(
    ExpandConstant('{userprograms}\DownTrack'));

  CreateShellLink(
    ExpandConstant('{autodesktop}\DownTrack.lnk'),
    'DownTrack',
    AppPath,
    '',
    InstallPath,
    AppPath,
    0,
    SW_SHOWNORMAL);

  CreateShellLink(
    ExpandConstant(
      '{userprograms}\DownTrack\DownTrack.lnk'),
    'DownTrack',
    AppPath,
    '',
    InstallPath,
    AppPath,
    0,
    SW_SHOWNORMAL);
end;

procedure ShowCompleted;
begin
  ProgressBar.Position := 100;
  StatusText.Caption := T('Complete');
  VersionText.Caption := GetLatestVersionText;
  DetailText.Caption := T('Progress');
  PrimaryButton.Caption := T('Open');
  PrimaryButton.Enabled := True;
  PrimaryButton.Visible := True;
  PrimaryButton.OnClick := @OpenDownTrack;
  SecondaryButton.Visible := True;
  SecondaryButton.Caption := T('Close');
  WizardForm.Update;
end;

procedure SetInstallState(
  const TitleText, Status: String);
begin
  LogoText.Caption := TitleText;
  StatusText.Caption := Status;
  WizardForm.Update;
end;


procedure InstallLatest(Sender: TObject);
var
  PayloadPath: String;
begin
  if Installing then
    Exit;

  Installing := True;
  PrimaryButton.Enabled := False;
  SecondaryButton.Enabled := False;

  try
    if not LoadLatestManifest then
      RaiseException(T('Error'));

    VersionText.Caption := GetLatestVersionText;

    PayloadPath :=
      ExpandConstant('{tmp}\DownTrack-Payload.zip');
    InstallDirectory :=
      ExpandConstant('{localappdata}\Programs\DownTrack');

    StatusText.Caption := T('Checking');
    DetailText.Caption := GetLatestVersionText;
    ProgressBar.Position := 3;
    WizardForm.Update;

    DownloadTemporaryFile(
      PayloadUrl,
      'DownTrack-Payload.zip',
      PayloadHash,
      @DownloadProgress);

    StatusText.Caption := T('Verifying');
    DetailText.Caption :=
      IntToStr(PayloadSize div 1048576) + ' MB';
    ProgressBar.Position := 70;
    WizardForm.Update;

    if not FileExists(PayloadPath) then
      RaiseException('The downloaded DownTrack payload was not found.');

    if DirExists(InstallDirectory) then
    begin
      if not DelTree(
        InstallDirectory,
        False,
        True,
        True) then
        RaiseException(T('CloseDownTrack'));
    end;

    StatusText.Caption := T('Installing');
    DetailText.Caption := T('Progress');
    ProgressBar.Position := 80;
    WizardForm.Update;

    if not ExtractPayload(
      PayloadPath,
      InstallDirectory) then
      RaiseException(
        'The DownTrack payload did not contain DownTrack.exe.');

    CreateInstallShortcuts(
      InstallDirectory + '\DownTrack.exe',
      InstallDirectory);

    SetIniString(
      'Install',
      'Version',
      LatestVersion,
      InstallDirectory + '\DownTrack.Install.ini');

    SetIniString(
      'Install',
      'InstalledUtc',
      GetDateTimeString(
        'yyyy-mm-dd hh:nn:ss',
        '-',
        ':'),
      InstallDirectory + '\DownTrack.Install.ini');

    ShowCompleted;
  except
    SetInstallState(T('Title'), T('Error'));
    PrimaryButton.Caption := T('Retry');
    PrimaryButton.Enabled := True;
    PrimaryButton.Visible := True;
    PrimaryButton.OnClick := @InstallLatest;
  PrimaryButton.Visible := False;
    SecondaryButton.Caption := T('Close');
    SecondaryButton.Visible := True;
    SecondaryButton.Enabled := True;
    DetailText.Caption := T('CloseDownTrack');
    Log(GetExceptionMessage);
  finally
    Installing := False;
  end;
end;

procedure CloseInstaller(Sender: TObject);
begin
  if Installing then
    Exit;

  WizardForm.Close;
end;

procedure InitializeInstallerUi;
var
  SavedMode: String;
  I: Integer;
begin
  { Compact, calm layout inspired by DownTrack's web design. }
  WizardForm.ClientWidth := ScaleX(760);
  WizardForm.ClientHeight := ScaleY(455);
  WizardForm.Position := poScreenCenter;
  WizardForm.Caption := 'DownTrack';
  WizardForm.Color := $00F6F4FC;
  WizardForm.Font.Name := 'Segoe UI';
  WizardForm.Font.Size := 9;

  { Keep the native window, but remove every stock wizard visual/control. }
  WizardForm.NextButton.Visible := False;
  WizardForm.BackButton.Visible := False;
  WizardForm.CancelButton.Visible := False;
  WizardForm.WizardBitmapImage.Visible := False;
  WizardForm.WizardBitmapImage2.Visible := False;
  WizardForm.WizardSmallBitmapImage.Visible := False;
  WizardForm.WelcomeLabel1.Visible := False;
  WizardForm.WelcomeLabel2.Visible := False;
  WizardForm.BeveledLabel.Visible := False;

  ContentPanel := TPanel.Create(WizardForm);
  ContentPanel.Parent := WizardForm;
  ContentPanel.Left := 0;
  ContentPanel.Top := 0;
  ContentPanel.Width := WizardForm.ClientWidth;
  ContentPanel.Height := WizardForm.ClientHeight;
  ContentPanel.BevelOuter := bvNone;
  ContentPanel.Color := $00F6F4FC;
  ContentPanel.BringToFront;

  HeaderPanel := TPanel.Create(WizardForm);
  HeaderPanel.Parent := ContentPanel;
  HeaderPanel.Align := alTop;
  HeaderPanel.Height := ScaleY(64);
  HeaderPanel.BevelOuter := bvNone;
  HeaderPanel.Color := $00FFFFFF;

  AccentBar := TPanel.Create(WizardForm);
  AccentBar.Parent := HeaderPanel;
  AccentBar.Left := ScaleX(28);
  AccentBar.Top := ScaleY(16);
  AccentBar.Width := ScaleX(4);
  AccentBar.Height := ScaleY(31);
  AccentBar.BevelOuter := bvNone;
  AccentBar.Color := $007A4CFF;

  LogoText := TNewStaticText.Create(WizardForm);
  LogoText.Parent := HeaderPanel;
  LogoText.Left := ScaleX(46);
  LogoText.Top := ScaleY(13);
  LogoText.Width := ScaleX(220);
  LogoText.Height := ScaleY(28);
  LogoText.Font.Size := 17;
  LogoText.Font.Style := [fsBold];

  TaglineText := TNewStaticText.Create(WizardForm);
  TaglineText.Parent := HeaderPanel;
  TaglineText.Left := ScaleX(47);
  TaglineText.Top := ScaleY(39);
  TaglineText.Width := ScaleX(350);
  TaglineText.Height := ScaleY(17);
  TaglineText.Font.Size := 8;
  TaglineText.Font.Color := $00827A93;

  LanguageLabel := TNewStaticText.Create(WizardForm);
  LanguageLabel.Parent := HeaderPanel;
  LanguageLabel.Left := ScaleX(505);
  LanguageLabel.Top := ScaleY(20);
  LanguageLabel.Width := ScaleX(50);
  LanguageLabel.Height := ScaleY(17);
  LanguageLabel.Font.Size := 8;
  LanguageLabel.Font.Color := $00827A93;

  LanguageCombo := TNewComboBox.Create(WizardForm);
  LanguageCombo.Parent := HeaderPanel;
  LanguageCombo.Left := ScaleX(560);
  LanguageCombo.Top := ScaleY(13);
  LanguageCombo.Width := ScaleX(170);
  LanguageCombo.Height := ScaleY(30);
  LanguageCombo.Style := csDropDownList;
  LanguageCombo.DropDownCount := 12;
  LanguageCombo.OnChange := @LanguageChanged;

  LanguageCombo.Items.Add('Automatic (Windows)');
  LanguageCombo.Items.Add('English');
  LanguageCombo.Items.Add('עברית');
  LanguageCombo.Items.Add('Español');
  LanguageCombo.Items.Add('Français');
  LanguageCombo.Items.Add('Deutsch');
  LanguageCombo.Items.Add('Italiano');
  LanguageCombo.Items.Add('Português');
  LanguageCombo.Items.Add('Nederlands');
  LanguageCombo.Items.Add('Polski');
  LanguageCombo.Items.Add('Čeština');
  LanguageCombo.Items.Add('Türkçe');
  LanguageCombo.Items.Add('Українська');
  LanguageCombo.Items.Add('Русский');
  LanguageCombo.Items.Add('العربية');
  LanguageCombo.Items.Add('Ελληνικά');
  LanguageCombo.Items.Add('Română');
  LanguageCombo.Items.Add('日本語');
  LanguageCombo.Items.Add('한국어');
  LanguageCombo.Items.Add('简体中文');
  LanguageCombo.Items.Add('繁體中文');

  BodyPanel := TPanel.Create(WizardForm);
  BodyPanel.Parent := ContentPanel;
  BodyPanel.Left := ScaleX(28);
  BodyPanel.Top := ScaleY(84);
  BodyPanel.Width := ContentPanel.Width - ScaleX(56);
  BodyPanel.Height := ScaleY(252);
  BodyPanel.BevelOuter := bvNone;
  BodyPanel.Color := $00FFFFFF;

  Dot1 := TPanel.Create(WizardForm);
  Dot1.Parent := BodyPanel;
  Dot1.Left := (BodyPanel.Width div 2) - ScaleX(21);
  Dot1.Top := ScaleY(33);
  Dot1.Width := ScaleX(8);
  Dot1.Height := ScaleY(8);
  Dot1.BevelOuter := bvNone;
  Dot1.Color := $007A4CFF;

  Dot2 := TPanel.Create(WizardForm);
  Dot2.Parent := BodyPanel;
  Dot2.Left := (BodyPanel.Width div 2) - ScaleX(4);
  Dot2.Top := ScaleY(33);
  Dot2.Width := ScaleX(8);
  Dot2.Height := ScaleY(8);
  Dot2.BevelOuter := bvNone;
  Dot2.Color := $00FF4F88;

  Dot3 := TPanel.Create(WizardForm);
  Dot3.Parent := BodyPanel;
  Dot3.Left := (BodyPanel.Width div 2) + ScaleX(13);
  Dot3.Top := ScaleY(33);
  Dot3.Width := ScaleX(8);
  Dot3.Height := ScaleY(8);
  Dot3.BevelOuter := bvNone;
  Dot3.Color := $0029C7D9;

  StatusText := TNewStaticText.Create(WizardForm);
  StatusText.Parent := BodyPanel;
  StatusText.Left := ScaleX(30);
  StatusText.Top := ScaleY(75);
  StatusText.Width := BodyPanel.Width - ScaleX(60);
  StatusText.Height := ScaleY(31);
  StatusText.Alignment := taCenter;
  StatusText.Font.Size := 15;
  StatusText.Font.Style := [fsBold];

  VersionText := TNewStaticText.Create(WizardForm);
  VersionText.Parent := BodyPanel;
  VersionText.Left := ScaleX(30);
  VersionText.Top := ScaleY(111);
  VersionText.Width := BodyPanel.Width - ScaleX(60);
  VersionText.Height := ScaleY(22);
  VersionText.Alignment := taCenter;
  VersionText.Font.Size := 10;
  VersionText.Font.Color := $007D7691;

  DetailText := TNewStaticText.Create(WizardForm);
  DetailText.Parent := BodyPanel;
  DetailText.Left := ScaleX(30);
  DetailText.Top := ScaleY(145);
  DetailText.Width := BodyPanel.Width - ScaleX(60);
  DetailText.Height := ScaleY(20);
  DetailText.Alignment := taCenter;
  DetailText.Font.Size := 9;
  DetailText.Font.Color := $007A4CFF;

  ProgressBar := TNewProgressBar.Create(WizardForm);
  ProgressBar.Parent := BodyPanel;
  ProgressBar.Left := ScaleX(48);
  ProgressBar.Top := ScaleY(185);
  ProgressBar.Width := BodyPanel.Width - ScaleX(96);
  ProgressBar.Height := ScaleY(9);
  ProgressBar.Min := 0;
  ProgressBar.Max := 100;
  ProgressBar.Position := 0;

  FooterPanel := TPanel.Create(WizardForm);
  FooterPanel.Parent := ContentPanel;
  FooterPanel.Align := alBottom;
  FooterPanel.Height := ScaleY(82);
  FooterPanel.BevelOuter := bvNone;
  FooterPanel.Color := $00F0EFF7;

  SecondaryButton := TNewButton.Create(WizardForm);
  SecondaryButton.Parent := FooterPanel;
  SecondaryButton.Left := FooterPanel.Width - ScaleX(228);
  SecondaryButton.Top := ScaleY(22);
  SecondaryButton.Width := ScaleX(92);
  SecondaryButton.Height := ScaleY(36);
  SecondaryButton.Caption := T('Close');
  SecondaryButton.OnClick := @CloseInstaller;

  PrimaryButton := TNewButton.Create(WizardForm);
  PrimaryButton.Parent := FooterPanel;
  PrimaryButton.Left := FooterPanel.Width - ScaleX(126);
  PrimaryButton.Top := ScaleY(22);
  PrimaryButton.Width := ScaleX(106);
  PrimaryButton.Height := ScaleY(36);
  PrimaryButton.Font.Style := [fsBold];
  PrimaryButton.Caption := T('Start');
  PrimaryButton.OnClick := @InstallLatest;

  SavedMode := GetSavedLanguageMode;

  if SameText(SavedMode, 'auto') then
    CurrentLanguage := DetectWindowsLanguage
  else
    CurrentLanguage := SavedMode;

  I := 0;
  if not SameText(SavedMode, 'auto') then
  begin
    while (I < 20) and
      not SameText(LangCodeFromIndex(I + 1), SavedMode) do
      I := I + 1;
    I := I + 1;
  end;

  LanguageCombo.ItemIndex := I;
  ApplyLanguageToForm;
end;

procedure InstallerActivated(Sender: TObject);
begin
  if AutoStartTriggered or Installing then
    Exit;

  AutoStartTriggered := True;

  { Make it a normal app window: bring it to the foreground, but never
    make it top-most. }
  BringWindowToTop(WizardForm.Handle);
  SetForegroundWindow(WizardForm.Handle);

  { Paint the initial screen before the first network operation. }
  StatusText.Caption := T('Checking');
  VersionText.Caption := T('Progress');
  DetailText.Caption := T('Progress');
  ProgressBar.Position := 0;
  WizardForm.Update;

  InstallLatest(PrimaryButton);
end;

procedure InitializeWizard;
begin
  PopulateTranslations;
  InitializeInstallerUi;

  AutoStartTriggered := False;

  { Auto-start when the normal window becomes active. This gives Windows
    a chance to paint the UI first, then starts manifest/payload work. }
  WizardForm.OnActivate := @InstallerActivated;

  WizardForm.Update;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  { No network work here. }
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := False;
end;
