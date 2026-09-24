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
he.LanguageName=עברית
he.LanguageID=$040D
he.RightToLeft=yes
ar.LanguageName=العربية
ar.LanguageID=$0401
ar.RightToLeft=yes
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
AppId={{6A9A7C5F-47D7-4F6B-93E6-7BC1A0C6CF16}
AppName=DownTrack Setup
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={tmp}\DownTrackSetup
OutputDir=..\artifacts\installer
OutputBaseFilename=DownTrack-WebSetup
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=lowest
Uninstallable=no
CreateAppDir=no
DisableProgramGroupPage=yes
DisableDirPage=yes
DisableWelcomePage=yes
DisableReadyPage=yes
DisableFinishedPage=yes
DisableStartupPrompt=yes
ShowLanguageDialog=no
LanguageDetectionMethod=uilanguage
WizardStyle=modern
Compression=lzma2
SolidCompression=yes
VersionInfoDescription=DownTrack lightweight web installer
VersionInfoProductName=DownTrack
VersionInfoCompany=MediaForge2446

[CustomMessages]
en.Title=DownTrack
en.Tagline=Your media. Your library. Always up to date.
en.Checking=Checking for the latest verified version…
en.Latest=Latest version: %1
en.Downloading=Downloading the latest DownTrack…
en.Verifying=Verifying downloaded files…
en.Installing=Installing DownTrack…
en.Complete=DownTrack is ready.
en.UpToDate=You already have the latest version.
en.Start=Install latest version
en.Open=Open DownTrack
en.Retry=Try again
en.Close=Close
en.Error=We couldn't complete the installation.
en.Progress=Preparing your installation…

he.Title=DownTrack
he.Tagline=המדיה שלך. הספרייה שלך. תמיד מעודכנת.
he.Checking=בודק את הגרסה המאומתת האחרונה…
he.Latest=הגרסה האחרונה: %1
he.Downloading=מוריד את DownTrack העדכני…
he.Verifying=מאמת את הקבצים שהורדו…
he.Installing=מתקין את DownTrack…
he.Complete=DownTrack מוכן.
he.UpToDate=הגרסה העדכנית כבר מותקנת.
he.Start=התקן את הגרסה האחרונה
he.Open=פתח את DownTrack
he.Retry=נסה שוב
he.Close=סגור
he.Error=לא ניתן היה להשלים את ההתקנה.
he.Progress=מכין את ההתקנה שלך…

es.Title=DownTrack
es.Tagline=Tu contenido. Tu biblioteca. Siempre actualizado.
es.Checking=Comprobando la última versión verificada…
es.Latest=Última versión: %1
es.Downloading=Descargando el DownTrack más reciente…
es.Verifying=Verificando los archivos descargados…
es.Installing=Instalando DownTrack…
es.Complete=DownTrack está listo.
es.UpToDate=Ya tienes la versión más reciente.
es.Start=Instalar la última versión
es.Open=Abrir DownTrack
es.Retry=Intentar de nuevo
es.Close=Cerrar
es.Error=No se pudo completar la instalación.
es.Progress=Preparando la instalación…

fr.Title=DownTrack
fr.Tagline=Votre contenu. Votre bibliothèque. Toujours à jour.
fr.Checking=Recherche de la dernière version vérifiée…
fr.Latest=Dernière version : %1
fr.Downloading=Téléchargement de la dernière version de DownTrack…
fr.Verifying=Vérification des fichiers téléchargés…
fr.Installing=Installation de DownTrack…
fr.Complete=DownTrack est prêt.
fr.UpToDate=Vous utilisez déjà la dernière version.
fr.Start=Installer la dernière version
fr.Open=Ouvrir DownTrack
fr.Retry=Réessayer
fr.Close=Fermer
fr.Error=L’installation n’a pas pu être terminée.
fr.Progress=Préparation de l’installation…

de.Title=DownTrack
de.Tagline=Deine Medien. Deine Bibliothek. Immer aktuell.
de.Checking=Suche nach der neuesten geprüften Version…
de.Latest=Neueste Version: %1
de.Downloading=Neueste DownTrack-Version wird heruntergeladen…
de.Verifying=Heruntergeladene Dateien werden überprüft…
de.Installing=DownTrack wird installiert…
de.Complete=DownTrack ist bereit.
de.UpToDate=Die neueste Version ist bereits installiert.
de.Start=Neueste Version installieren
de.Open=DownTrack öffnen
de.Retry=Erneut versuchen
de.Close=Schließen
de.Error=Die Installation konnte nicht abgeschlossen werden.
de.Progress=Installation wird vorbereitet…

it.Title=DownTrack
it.Tagline=I tuoi contenuti. La tua libreria. Sempre aggiornata.
it.Checking=Verifica dell'ultima versione verificata…
it.Latest=Ultima versione: %1
it.Downloading=Download dell'ultima versione di DownTrack…
it.Verifying=Verifica dei file scaricati…
it.Installing=Installazione di DownTrack…
it.Complete=DownTrack è pronto.
it.UpToDate=Hai già l'ultima versione.
it.Start=Installa l'ultima versione
it.Open=Apri DownTrack
it.Retry=Riprova
it.Close=Chiudi
it.Error=Impossibile completare l'installazione.
it.Progress=Preparazione dell'installazione…

pt.Title=DownTrack
pt.Tagline=Sua mídia. Sua biblioteca. Sempre atualizada.
pt.Checking=Verificando a versão verificada mais recente…
pt.Latest=Versão mais recente: %1
pt.Downloading=Baixando a versão mais recente do DownTrack…
pt.Verifying=Verificando os arquivos baixados…
pt.Installing=Instalando o DownTrack…
pt.Complete=O DownTrack está pronto.
pt.UpToDate=Você já tem a versão mais recente.
pt.Start=Instalar a versão mais recente
pt.Open=Abrir o DownTrack
pt.Retry=Tentar novamente
pt.Close=Fechar
pt.Error=Não foi possível concluir a instalação.
pt.Progress=Preparando sua instalação…

nl.Title=DownTrack
nl.Tagline=Jouw media. Jouw bibliotheek. Altijd actueel.
nl.Checking=Laatste geverifieerde versie controleren…
nl.Latest=Nieuwste versie: %1
nl.Downloading=De nieuwste DownTrack downloaden…
nl.Verifying=Gedownloade bestanden verifiëren…
nl.Installing=DownTrack installeren…
nl.Complete=DownTrack is klaar.
nl.UpToDate=Je hebt al de nieuwste versie.
nl.Start=Nieuwste versie installeren
nl.Open=DownTrack openen
nl.Retry=Opnieuw proberen
nl.Close=Sluiten
nl.Error=De installatie kon niet worden voltooid.
nl.Progress=Installatie voorbereiden…

pl.Title=DownTrack
pl.Tagline=Twoje media. Twoja biblioteka. Zawsze aktualne.
pl.Checking=Sprawdzanie najnowszej zweryfikowanej wersji…
pl.Latest=Najnowsza wersja: %1
pl.Downloading=Pobieranie najnowszego DownTrack…
pl.Verifying=Weryfikowanie pobranych plików…
pl.Installing=Instalowanie DownTrack…
pl.Complete=DownTrack jest gotowy.
pl.UpToDate=Masz już najnowszą wersję.
pl.Start=Zainstaluj najnowszą wersję
pl.Open=Otwórz DownTrack
pl.Retry=Spróbuj ponownie
pl.Close=Zamknij
pl.Error=Nie udało się ukończyć instalacji.
pl.Progress=Przygotowywanie instalacji…

cs.Title=DownTrack
cs.Tagline=Vaše média. Vaše knihovna. Vždy aktuální.
cs.Checking=Kontrola nejnovější ověřené verze…
cs.Latest=Nejnovější verze: %1
cs.Downloading=Stahování nejnovějšího DownTrack…
cs.Verifying=Ověřování stažených souborů…
cs.Installing=Instalace DownTrack…
cs.Complete=DownTrack je připraven.
cs.UpToDate=Nejnovější verzi už máte.
cs.Start=Instalovat nejnovější verzi
cs.Open=Otevřít DownTrack
cs.Retry=Zkusit znovu
cs.Close=Zavřít
cs.Error=Instalaci se nepodařilo dokončit.
cs.Progress=Příprava instalace…

tr.Title=DownTrack
tr.Tagline=Medyanız. Kitaplığınız. Her zaman güncel.
tr.Checking=En yeni doğrulanmış sürüm kontrol ediliyor…
tr.Latest=En yeni sürüm: %1
tr.Downloading=En yeni DownTrack indiriliyor…
tr.Verifying=İndirilen dosyalar doğrulanıyor…
tr.Installing=DownTrack yükleniyor…
tr.Complete=DownTrack hazır.
tr.UpToDate=En yeni sürüme zaten sahipsiniz.
tr.Start=En yeni sürümü yükle
tr.Open=DownTrack'i aç
tr.Retry=Tekrar dene
tr.Close=Kapat
tr.Error=Yükleme tamamlanamadı.
tr.Progress=Yükleme hazırlanıyor…

uk.Title=DownTrack
uk.Tagline=Ваші медіа. Ваша бібліотека. Завжди актуальні.
uk.Checking=Перевірка останньої підтвердженої версії…
uk.Latest=Остання версія: %1
uk.Downloading=Завантаження найновішого DownTrack…
uk.Verifying=Перевірка завантажених файлів…
uk.Installing=Встановлення DownTrack…
uk.Complete=DownTrack готовий.
uk.UpToDate=У вас уже остання версія.
uk.Start=Встановити останню версію
uk.Open=Відкрити DownTrack
uk.Retry=Спробувати ще раз
uk.Close=Закрити
uk.Error=Не вдалося завершити встановлення.
uk.Progress=Підготовка встановлення…

ru.Title=DownTrack
ru.Tagline=Ваши медиа. Ваша библиотека. Всегда актуальны.
ru.Checking=Проверка последней проверенной версии…
ru.Latest=Последняя версия: %1
ru.Downloading=Загрузка последней версии DownTrack…
ru.Verifying=Проверка загруженных файлов…
ru.Installing=Установка DownTrack…
ru.Complete=DownTrack готов.
ru.UpToDate=У вас уже установлена последняя версия.
ru.Start=Установить последнюю версию
ru.Open=Открыть DownTrack
ru.Retry=Повторить
ru.Close=Закрыть
ru.Error=Не удалось завершить установку.
ru.Progress=Подготовка установки…

ar.Title=DownTrack
ar.Tagline=وسائطك. مكتبتك. محدثة دائمًا.
ar.Checking=جارٍ التحقق من أحدث إصدار موثوق…
ar.Latest=أحدث إصدار: %1
ar.Downloading=جارٍ تنزيل أحدث إصدار من DownTrack…
ar.Verifying=جارٍ التحقق من الملفات التي تم تنزيلها…
ar.Installing=جارٍ تثبيت DownTrack…
ar.Complete=DownTrack جاهز.
ar.UpToDate=لديك بالفعل أحدث إصدار.
ar.Start=تثبيت أحدث إصدار
ar.Open=فتح DownTrack
ar.Retry=حاول مرة أخرى
ar.Close=إغلاق
ar.Error=تعذر إكمال التثبيت.
ar.Progress=جارٍ تجهيز التثبيت…

el.Title=DownTrack
el.Tagline=Τα πολυμέσα σας. Η βιβλιοθήκη σας. Πάντα ενημερωμένα.
el.Checking=Έλεγχος της πιο πρόσφατης επαληθευμένης έκδοσης…
el.Latest=Τελευταία έκδοση: %1
el.Downloading=Λήψη της πιο πρόσφατης έκδοσης του DownTrack…
el.Verifying=Επαλήθευση των ληφθέντων αρχείων…
el.Installing=Εγκατάσταση του DownTrack…
el.Complete=Το DownTrack είναι έτοιμο.
el.UpToDate=Έχετε ήδη την πιο πρόσφατη έκδοση.
el.Start=Εγκατάσταση τελευταίας έκδοσης
el.Open=Άνοιγμα DownTrack
el.Retry=Δοκιμή ξανά
el.Close=Κλείσιμο
el.Error=Δεν ήταν δυνατή η ολοκλήρωση της εγκατάστασης.
el.Progress=Προετοιμασία εγκατάστασης…

ro.Title=DownTrack
ro.Tagline=Media ta. Biblioteca ta. Mereu actualizate.
ro.Checking=Se verifică cea mai recentă versiune validată…
ro.Latest=Cea mai recentă versiune: %1
ro.Downloading=Se descarcă cea mai recentă versiune DownTrack…
ro.Verifying=Se verifică fișierele descărcate…
ro.Installing=Se instalează DownTrack…
ro.Complete=DownTrack este gata.
ro.UpToDate=Aveți deja cea mai recentă versiune.
ro.Start=Instalează cea mai recentă versiune
ro.Open=Deschide DownTrack
ro.Retry=Încearcă din nou
ro.Close=Închide
ro.Error=Instalarea nu a putut fi finalizată.
ro.Progress=Se pregătește instalarea…

ja.Title=DownTrack
ja.Tagline=あなたのメディア。あなたのライブラリ。いつでも最新。
ja.Checking=最新の検証済みバージョンを確認しています…
ja.Latest=最新バージョン: %1
ja.Downloading=最新のDownTrackをダウンロードしています…
ja.Verifying=ダウンロードしたファイルを検証しています…
ja.Installing=DownTrackをインストールしています…
ja.Complete=DownTrackの準備ができました。
ja.UpToDate=すでに最新バージョンです。
ja.Start=最新バージョンをインストール
ja.Open=DownTrackを開く
ja.Retry=もう一度試す
ja.Close=閉じる
ja.Error=インストールを完了できませんでした。
ja.Progress=インストールを準備しています…

ko.Title=DownTrack
ko.Tagline=내 미디어. 내 라이브러리. 항상 최신 상태.
ko.Checking=최신 검증 버전을 확인하는 중…
ko.Latest=최신 버전: %1
ko.Downloading=최신 DownTrack을 다운로드하는 중…
ko.Verifying=다운로드한 파일을 확인하는 중…
ko.Installing=DownTrack을 설치하는 중…
ko.Complete=DownTrack을 사용할 준비가 되었습니다.
ko.UpToDate=이미 최신 버전이 설치되어 있습니다.
ko.Start=최신 버전 설치
ko.Open=DownTrack 열기
ko.Retry=다시 시도
ko.Close=닫기
ko.Error=설치를 완료하지 못했습니다.
ko.Progress=설치를 준비하는 중…

zhcn.Title=DownTrack
zhcn.Tagline=你的媒体。你的媒体库。始终保持最新。
zhcn.Checking=正在检查最新的已验证版本…
zhcn.Latest=最新版本：%1
zhcn.Downloading=正在下载最新的 DownTrack…
zhcn.Verifying=正在验证下载的文件…
zhcn.Installing=正在安装 DownTrack…
zhcn.Complete=DownTrack 已准备就绪。
zhcn.UpToDate=你已经拥有最新版本。
zhcn.Start=安装最新版本
zhcn.Open=打开 DownTrack
zhcn.Retry=重试
zhcn.Close=关闭
zhcn.Error=无法完成安装。
zhcn.Progress=正在准备安装…

zhtw.Title=DownTrack
zhtw.Tagline=你的媒體。你的資料庫。永遠保持最新。
zhtw.Checking=正在檢查最新的已驗證版本…
zhtw.Latest=最新版本：%1
zhtw.Downloading=正在下載最新的 DownTrack…
zhtw.Verifying=正在驗證下載的檔案…
zhtw.Installing=正在安裝 DownTrack…
zhtw.Complete=DownTrack 已準備就緒。
zhtw.UpToDate=你已經擁有最新版本。
zhtw.Start=安裝最新版本
zhtw.Open=開啟 DownTrack
zhtw.Retry=重試
zhtw.Close=關閉
zhtw.Error=無法完成安裝。
zhtw.Progress=正在準備安裝…

[Code]
var
  SetupForm: TSetupForm;
  HeaderPanel: TPanel;
  AccentBar: TPanel;
  LogoText: TNewStaticText;
  TaglineText: TNewStaticText;
  StatusText: TNewStaticText;
  VersionText: TNewStaticText;
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

function P(const S: String): String;
begin
  Result := S;
end;

function DownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
var
  Percent: Integer;
begin
  Result := True;
  if ProgressMax > 0 then
  begin
    Percent := (Progress * 100) div ProgressMax;
    ProgressBar.Position := Percent;
    StatusText.Caption := CustomMessage('Downloading');
  end
  else
    StatusText.Caption := CustomMessage('Downloading');

  SetupForm.Update;
end;

function LoadLatestManifest: Boolean;
var
  ManifestPath: String;
begin
  Result := False;

  if ManifestLoaded then
  begin
    Result := True;
    Exit;
  end;

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

    LatestVersion := Trim(GetIniString('release', 'Version', '', ManifestPath));
    PayloadUrl := Trim(GetIniString('release', 'PayloadUrl', '', ManifestPath));
    PayloadHash := Trim(GetIniString('release', 'PayloadSha256', '', ManifestPath));
    PayloadSize := StrToInt64Def(
      Trim(GetIniString('release', 'PayloadSize', '0', ManifestPath)),
      0);

    if LatestVersion = '' then
      RaiseException('Latest release has no version.');

    if SetupUrl = '' then
      RaiseException('Latest release has no setup URL.');

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
  IniPath := ExpandConstant('{localappdata}\Programs\DownTrack\DownTrack.Install.ini');
  Result := Trim(GetIniString('Install', 'Version', '', IniPath));
end;

procedure SetInstallState(const TitleText, Status: String);
begin
  LogoText.Caption := TitleText;
  StatusText.Caption := Status;
  SetupForm.Update;
end;

procedure OpenDownTrack(Sender: TObject);
var
  ResultCode: Integer;
begin
  if FileExists(
    ExpandConstant('{localappdata}\Programs\DownTrack\DownTrack.exe')) then
  begin
    Exec(
      ExpandConstant('{localappdata}\Programs\DownTrack\DownTrack.exe'),
      '',
      '',
      SW_SHOWNORMAL,
      ewNoWait,
      ResultCode);
  end;

  SetupForm.Close;
end;

procedure ShowCompleted;
begin
  ProgressBar.Position := 100;
  SetInstallState(CustomMessage('Title'), CustomMessage('Complete'));
  VersionText.Caption := FmtMessage(CustomMessage('Latest'), [LatestVersion]);
  PrimaryButton.Caption := CustomMessage('Open');
  PrimaryButton.Enabled := True;
  PrimaryButton.OnClick := @OpenDownTrack;
  SecondaryButton.Visible := True;
  SecondaryButton.Caption := CustomMessage('Close');
end;


procedure CreateInstallShortcuts(const AppPath, InstallPath: String);
begin
  ForceDirectories(ExpandConstant('{userprograms}\DownTrack'));

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
    ExpandConstant('{userprograms}\DownTrack\DownTrack.lnk'),
    'DownTrack',
    AppPath,
    '',
    InstallPath,
    AppPath,
    0,
    SW_SHOWNORMAL);
end;

function ExtractPayload(const PayloadPath, InstallPath: String): Boolean;
var
  ScriptPath: String;
  PowerShellPath: String;
  ResultCode: Integer;
  Script: AnsiString;
begin
  Result := False;

  if not ForceDirectories(InstallPath) then
    RaiseException('Could not create the DownTrack installation directory.');

  ScriptPath := ExpandConstant('{tmp}\extract-downtrack.ps1');
  Script :=
    '$ErrorActionPreference = "Stop"' + #13#10 +
    'New-Item -ItemType Directory -Force -Path $args[1] | Out-Null' + #13#10 +
    'Expand-Archive -LiteralPath $args[0] -DestinationPath $args[1] -Force' + #13#10;

  if not SaveStringToFile(ScriptPath, Script, False) then
    RaiseException('Could not prepare the payload extraction script.');

  PowerShellPath := ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe');

  if not Exec(
    PowerShellPath,
    '-NoLogo -NoProfile -NonInteractive -WindowStyle Hidden -ExecutionPolicy Bypass ' +
    '-File "' + ScriptPath + '" "' + PayloadPath + '" "' + InstallPath + '"',
    '',
    SW_HIDE,
    ewWaitUntilTerminated,
    ResultCode) then
    RaiseException('Windows PowerShell could not be started.');

  if ResultCode <> 0 then
    RaiseException(
      Format('Payload extraction returned exit code %d.', [ResultCode]));

  Result := FileExists(InstallPath + '\DownTrack.exe');
end;

procedure CompleteInstallation;
var
  InstallIni: String;
begin
  InstallIni := InstallDirectory + '\DownTrack.Install.ini';
  SetIniString('Install', 'Version', LatestVersion, InstallIni);
  SetIniString('Install', 'InstalledUtc',
    GetDateTimeString('yyyy-mm-dd hh:nn:ss', '-', ':'), InstallIni);

  CreateInstallShortcuts(
    InstallDirectory + '\DownTrack.exe',
    InstallDirectory);

  ShowCompleted;
end;

procedure InstallLatest(Sender: TObject);
var
  PayloadPath: String;
  InstallIni: String;
begin
  if Installing then
    Exit;

  Installing := True;
  PrimaryButton.Enabled := False;

  try
    if not LoadLatestManifest then
    begin
      SetInstallState(CustomMessage('Title'), CustomMessage('Error'));
      PrimaryButton.Caption := CustomMessage('Retry');
      PrimaryButton.Enabled := True;
      PrimaryButton.OnClick := @InstallLatest;
      Exit;
    end;

    VersionText.Caption := FmtMessage(
      CustomMessage('Latest'),
      [LatestVersion]);

    PayloadPath := ExpandConstant('{tmp}\DownTrack-Payload.zip');
    InstallDirectory := ExpandConstant('{localappdata}\Programs\DownTrack');

    SetInstallState(
      CustomMessage('Title'),
      CustomMessage('Checking'));
    ProgressBar.Position := 5;

    DownloadTemporaryFile(
      PayloadUrl,
      'DownTrack-Payload.zip',
      PayloadHash,
      @DownloadProgress);

    SetInstallState(
      CustomMessage('Title'),
      CustomMessage('Verifying'));
    ProgressBar.Position := 65;

    if not FileExists(PayloadPath) then
      RaiseException('The downloaded DownTrack payload was not found.');

    if DirExists(InstallDirectory) then
    begin
      if not DelTree(InstallDirectory, False, True, True) then
        RaiseException(
          'DownTrack is currently in use. Close DownTrack and try again.');
    end;

    SetInstallState(
      CustomMessage('Title'),
      CustomMessage('Installing'));
    ProgressBar.Position := 78;

    if not ExtractPayload(PayloadPath, InstallDirectory) then
      RaiseException(
        'The DownTrack payload did not contain the application executable.');

    InstallIni := InstallDirectory + '\DownTrack.Install.ini';
    SetIniString('Install', 'Version', LatestVersion, InstallIni);

    CompleteInstallation;
  except
    SetInstallState(CustomMessage('Title'), CustomMessage('Error'));
    PrimaryButton.Caption := CustomMessage('Retry');
    PrimaryButton.Enabled := True;
    PrimaryButton.OnClick := @InstallLatest;
    SecondaryButton.Caption := CustomMessage('Close');
    SecondaryButton.Visible := True;
    Log(GetExceptionMessage);
  finally
    Installing := False;
  end;
end;

procedure CloseInstaller(Sender: TObject);
begin
  SetupForm.Close;
end;

procedure SetupFormClose(Sender: TObject; var Action: TCloseAction);
begin
  if Installing then
  begin
    Action := caNone;
    Exit;
  end;

  Action := caFree;
  WizardForm.Close;
end;

procedure InitializeWizard;
var
  InstalledVersion: String;
begin
  WizardForm.Hide;

  SetupForm := CreateCustomForm(ScaleX(760), ScaleY(470), True, True);
  SetupForm.Caption := 'DownTrack';
  SetupForm.BorderStyle := bsNone;
  SetupForm.Position := poScreenCenter;
  SetupForm.Color := $00F7F8FC;
  SetupForm.OnClose := @SetupFormClose;

  HeaderPanel := TPanel.Create(SetupForm);
  HeaderPanel.Parent := SetupForm;
  HeaderPanel.Left := 0;
  HeaderPanel.Top := 0;
  HeaderPanel.Width := SetupForm.ClientWidth;
  HeaderPanel.Height := ScaleY(86);
  HeaderPanel.BevelOuter := bvNone;
  HeaderPanel.Color := $00FFFFFF;

  AccentBar := TPanel.Create(SetupForm);
  AccentBar.Parent := SetupForm;
  AccentBar.Left := ScaleX(28);
  AccentBar.Top := ScaleY(22);
  AccentBar.Width := ScaleX(6);
  AccentBar.Height := ScaleY(42);
  AccentBar.BevelOuter := bvNone;
  AccentBar.Color := $007B61FF;

  LogoText := TNewStaticText.Create(SetupForm);
  LogoText.Parent := SetupForm;
  LogoText.Left := ScaleX(50);
  LogoText.Top := ScaleY(18);
  LogoText.Width := ScaleX(300);
  LogoText.Height := ScaleY(30);
  LogoText.Caption := CustomMessage('Title');
  LogoText.Font.Size := 22;
  LogoText.Font.Style := [fsBold];
  TaglineText := TNewStaticText.Create(SetupForm);
  TaglineText.Parent := SetupForm;
  TaglineText.Left := ScaleX(51);
  TaglineText.Top := ScaleY(51);
  TaglineText.Width := ScaleX(620);
  TaglineText.Height := ScaleY(22);
  TaglineText.Caption := CustomMessage('Tagline');
  TaglineText.Font.Size := 9;
  TaglineText.Font.Color := $0069788A;
  StatusText := TNewStaticText.Create(SetupForm);
  StatusText.Parent := SetupForm;
  StatusText.Left := ScaleX(50);
  StatusText.Top := ScaleY(164);
  StatusText.Width := ScaleX(650);
  StatusText.Height := ScaleY(32);
  StatusText.Caption := CustomMessage('Checking');
  StatusText.Font.Size := 11;
  StatusText.Font.Style := [fsBold];
  VersionText := TNewStaticText.Create(SetupForm);
  VersionText.Parent := SetupForm;
  VersionText.Left := ScaleX(50);
  VersionText.Top := ScaleY(202);
  VersionText.Width := ScaleX(650);
  VersionText.Height := ScaleY(26);
  VersionText.Caption := CustomMessage('Progress');
  VersionText.Font.Size := 9;
  VersionText.Font.Color := $0069788A;
  ProgressBar := TNewProgressBar.Create(SetupForm);
  ProgressBar.Parent := SetupForm;
  ProgressBar.Left := ScaleX(50);
  ProgressBar.Top := ScaleY(250);
  ProgressBar.Width := ScaleX(660);
  ProgressBar.Height := ScaleY(8);
  ProgressBar.Min := 0;
  ProgressBar.Max := 100;
  ProgressBar.Position := 0;

  PrimaryButton := TNewButton.Create(SetupForm);
  PrimaryButton.Parent := SetupForm;
  PrimaryButton.Left := ScaleX(450);
  PrimaryButton.Top := ScaleY(352);
  PrimaryButton.Width := ScaleX(260);
  PrimaryButton.Height := ScaleY(44);
  PrimaryButton.Caption := CustomMessage('Start');
  PrimaryButton.Font.Style := [fsBold];
  PrimaryButton.OnClick := @InstallLatest;

  SecondaryButton := TNewButton.Create(SetupForm);
  SecondaryButton.Parent := SetupForm;
  SecondaryButton.Left := ScaleX(330);
  SecondaryButton.Top := ScaleY(352);
  SecondaryButton.Width := ScaleX(108);
  SecondaryButton.Height := ScaleY(44);
  SecondaryButton.Caption := CustomMessage('Close');
  SecondaryButton.OnClick := @CloseInstaller;

  InstalledVersion := GetInstalledVersion;
  if LoadLatestManifest then
  begin
    if (InstalledVersion <> '') and
       SameText(InstalledVersion, LatestVersion) then
    begin
      SetInstallState(CustomMessage('Title'), CustomMessage('UpToDate'));
      VersionText.Caption := FmtMessage(CustomMessage('Latest'), [LatestVersion]);
      PrimaryButton.Caption := CustomMessage('Open');
      PrimaryButton.OnClick := @OpenDownTrack;
      ProgressBar.Position := 100;
    end
    else
    begin
      StatusText.Caption := CustomMessage('Latest');
      VersionText.Caption := FmtMessage(CustomMessage('Latest'), [LatestVersion]);
    end;
  end;

  SetupForm.Show;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := False;
end;
