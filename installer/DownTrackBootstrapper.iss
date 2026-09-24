#define MyAppName "DownTrack"
#define MyAppVersion "0.0.1"
#define MyAppPublisher "MediaForge2446"
#define MyAppExeName "DownTrack.exe"
#define ManifestUrl "https://github.com/MediaForge2446/DownTrack/releases/download/nightly/latest.ini"

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "ar"; MessagesFile: "compiler:Languages\Arabic.isl"
Name: "zhcn"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"
Name: "cs"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "da"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "nl"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "fi"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"
Name: "he"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "it"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "ja"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "ko"; MessagesFile: "compiler:Languages\Korean.isl"
Name: "no"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "pl"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "pt"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "tr"; MessagesFile: "compiler:Languages\Turkish.isl"
Name: "uk"; MessagesFile: "compiler:Languages\Ukrainian.isl"

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
LanguageDetectionMethod=uilanguage
ShowLanguageDialog=auto
UsePreviousLanguage=no
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

[CustomMessages]
en.BootstrapIntro=This lightweight installer always checks for the latest verified DownTrack build and current media engine.
en.LatestDetected=Latest verified build: %1
en.CheckingLatest=Checking for the latest verified release...
en.DownloadInstall=Download & install
en.InstallReady=DownTrack %1 is ready to install.
en.InstallVerificationFailed=DownTrack was downloaded, but the installed files could not be verified.
en.NetworkError=DownTrack could not retrieve the latest release information.%n%nPlease check your internet connection and try again.
en.VerificationRetry=Please run the installer again.
ar.VerificationRetry=يرجى تشغيل المثبّت مرة أخرى.
zhcn.VerificationRetry=请重新运行安装程序。
cs.VerificationRetry=Spusťte instalátor znovu.
da.VerificationRetry=Kør installationsprogrammet igen.
nl.VerificationRetry=Voer het installatieprogramma opnieuw uit.
fi.VerificationRetry=Suorita asennusohjelma uudelleen.
fr.VerificationRetry=Relancez le programme d’installation.
de.VerificationRetry=Führen Sie das Installationsprogramm erneut aus.
he.VerificationRetry=הפעל את המתקין שוב.
it.VerificationRetry=Esegui nuovamente il programma di installazione.
ja.VerificationRetry=インストーラーをもう一度実行してください。
ko.VerificationRetry=설치 프로그램을 다시 실행하세요.
no.VerificationRetry=Kjør installasjonsprogrammet på nytt.
pl.VerificationRetry=Uruchom instalator ponownie.
pt.VerificationRetry=Execute o instalador novamente.
ru.VerificationRetry=Запустите установщик ещё раз.
es.VerificationRetry=Vuelve a ejecutar el instalador.
tr.VerificationRetry=Yükleyiciyi tekrar çalıştırın.
uk.VerificationRetry=Запустіть інсталятор ще раз.

ar.BootstrapIntro=يتحقق هذا المثبّت الخفيف دائمًا من أحدث إصدار موثوق من DownTrack ومحرك الوسائط الحالي.
ar.LatestDetected=أحدث إصدار موثوق: %1
ar.CheckingLatest=جارٍ التحقق من أحدث إصدار موثوق...
ar.DownloadInstall=تنزيل وتثبيت
ar.InstallReady=الإصدار %1 من DownTrack جاهز للتثبيت.
ar.InstallVerificationFailed=تم تنزيل DownTrack، ولكن تعذّر التحقق من الملفات المثبّتة.
ar.NetworkError=تعذّر على DownTrack الحصول على معلومات أحدث إصدار.%n%nتحقق من اتصال الإنترنت وحاول مرة أخرى.

zhcn.BootstrapIntro=此轻量安装程序每次都会检查最新的已验证 DownTrack 版本和当前媒体引擎。
zhcn.LatestDetected=最新已验证版本：%1
zhcn.CheckingLatest=正在检查最新的已验证版本...
zhcn.DownloadInstall=下载并安装
zhcn.InstallReady=DownTrack %1 已准备好安装。
zhcn.InstallVerificationFailed=DownTrack 已下载，但无法验证已安装的文件。
zhcn.NetworkError=DownTrack 无法获取最新版本信息。%n%n请检查网络连接后重试。

cs.BootstrapIntro=Tento odlehčený instalátor vždy ověří nejnovější ověřenou verzi DownTrack a aktuální mediální engine.
cs.LatestDetected=Nejnovější ověřená verze: %1
cs.CheckingLatest=Ověřování nejnovější ověřené verze...
cs.DownloadInstall=Stáhnout a nainstalovat
cs.InstallReady=DownTrack %1 je připraven k instalaci.
cs.InstallVerificationFailed=DownTrack byl stažen, ale nainstalované soubory se nepodařilo ověřit.
cs.NetworkError=DownTrack nemohl získat informace o nejnovější verzi.%n%nZkontrolujte připojení k internetu a zkuste to znovu.

da.BootstrapIntro=Dette lette installationsprogram kontrollerer altid den seneste verificerede DownTrack-version og den aktuelle mediemotor.
da.LatestDetected=Seneste verificerede version: %1
da.CheckingLatest=Kontrollerer den seneste verificerede version...
da.DownloadInstall=Download og installer
da.InstallReady=DownTrack %1 er klar til installation.
da.InstallVerificationFailed=DownTrack blev downloadet, men de installerede filer kunne ikke verificeres.
da.NetworkError=DownTrack kunne ikke hente oplysninger om den seneste version.%n%nKontrollér din internetforbindelse, og prøv igen.

nl.BootstrapIntro=Dit lichte installatieprogramma controleert altijd de nieuwste geverifieerde DownTrack-versie en de huidige media-engine.
nl.LatestDetected=Nieuwste geverifieerde versie: %1
nl.CheckingLatest=Nieuwste geverifieerde versie controleren...
nl.DownloadInstall=Downloaden en installeren
nl.InstallReady=DownTrack %1 is klaar om te installeren.
nl.InstallVerificationFailed=DownTrack is gedownload, maar de geïnstalleerde bestanden konden niet worden geverifieerd.
nl.NetworkError=DownTrack kon de nieuwste releasegegevens niet ophalen.%n%nControleer je internetverbinding en probeer het opnieuw.

fi.BootstrapIntro=Tämä kevyt asennusohjelma tarkistaa aina uusimman vahvistetun DownTrack-version ja nykyisen mediamoottorin.
fi.LatestDetected=Uusin vahvistettu versio: %1
fi.CheckingLatest=Tarkistetaan uusinta vahvistettua versiota...
fi.DownloadInstall=Lataa ja asenna
fi.InstallReady=DownTrack %1 on valmis asennettavaksi.
fi.InstallVerificationFailed=DownTrack ladattiin, mutta asennettujen tiedostojen vahvistaminen epäonnistui.
fi.NetworkError=DownTrack ei voinut hakea uusimman julkaisun tietoja.%n%nTarkista internet-yhteytesi ja yritä uudelleen.

fr.BootstrapIntro=Ce programme d’installation léger vérifie toujours la dernière version vérifiée de DownTrack ainsi que le moteur multimédia actuel.
fr.LatestDetected=Dernière version vérifiée : %1
fr.CheckingLatest=Vérification de la dernière version vérifiée...
fr.DownloadInstall=Télécharger et installer
fr.InstallReady=DownTrack %1 est prêt à être installé.
fr.InstallVerificationFailed=DownTrack a été téléchargé, mais les fichiers installés n’ont pas pu être vérifiés.
fr.NetworkError=DownTrack n’a pas pu récupérer les informations de la dernière version.%n%nVérifiez votre connexion Internet et réessayez.

de.BootstrapIntro=Dieses schlanke Installationsprogramm prüft immer die neueste verifizierte DownTrack-Version und die aktuelle Medienengine.
de.LatestDetected=Neueste verifizierte Version: %1
de.CheckingLatest=Neueste verifizierte Version wird geprüft...
de.DownloadInstall=Herunterladen und installieren
de.InstallReady=DownTrack %1 ist zur Installation bereit.
de.InstallVerificationFailed=DownTrack wurde heruntergeladen, aber die installierten Dateien konnten nicht überprüft werden.
de.NetworkError=DownTrack konnte die Informationen zur neuesten Version nicht abrufen.%n%nÜberprüfen Sie Ihre Internetverbindung und versuchen Sie es erneut.

he.BootstrapIntro=מתקין קל זה בודק תמיד את הגרסה המאומתת האחרונה של DownTrack ואת מנוע המדיה העדכני.
he.LatestDetected=הגרסה המאומתת האחרונה: %1
he.CheckingLatest=בודק את הגרסה המאומתת האחרונה...
he.DownloadInstall=הורדה והתקנה
he.InstallReady=DownTrack %1 מוכן להתקנה.
he.InstallVerificationFailed=DownTrack הורד, אך לא ניתן היה לאמת את הקבצים שהותקנו.
he.NetworkError=DownTrack לא הצליח לקבל את פרטי הגרסה האחרונה.%n%nבדוק את חיבור האינטרנט ונסה שוב.

it.BootstrapIntro=Questo programma di installazione leggero controlla sempre l'ultima versione verificata di DownTrack e il motore multimediale corrente.
it.LatestDetected=Ultima versione verificata: %1
it.CheckingLatest=Verifica dell'ultima versione verificata...
it.DownloadInstall=Scarica e installa
it.InstallReady=DownTrack %1 è pronto per l'installazione.
it.InstallVerificationFailed=DownTrack è stato scaricato, ma non è stato possibile verificare i file installati.
it.NetworkError=DownTrack non ha potuto recuperare le informazioni sull'ultima versione.%n%nControlla la connessione Internet e riprova.

ja.BootstrapIntro=この軽量インストーラーは、常に最新の検証済み DownTrack バージョンと現在のメディアエンジンを確認します。
ja.LatestDetected=最新の検証済みバージョン: %1
ja.CheckingLatest=最新の検証済みバージョンを確認しています...
ja.DownloadInstall=ダウンロードしてインストール
ja.InstallReady=DownTrack %1 のインストール準備ができました。
ja.InstallVerificationFailed=DownTrack はダウンロードされましたが、インストールされたファイルを確認できませんでした。
ja.NetworkError=DownTrack は最新リリースの情報を取得できませんでした。%n%nインターネット接続を確認して、もう一度お試しください。

ko.BootstrapIntro=이 경량 설치 프로그램은 항상 최신의 검증된 DownTrack 버전과 현재 미디어 엔진을 확인합니다.
ko.LatestDetected=최신 검증 버전: %1
ko.CheckingLatest=최신 검증 버전을 확인하는 중...
ko.DownloadInstall=다운로드 및 설치
ko.InstallReady=DownTrack %1을(를) 설치할 준비가 되었습니다.
ko.InstallVerificationFailed=DownTrack을 다운로드했지만 설치된 파일을 확인할 수 없습니다.
ko.NetworkError=DownTrack에서 최신 릴리스 정보를 가져오지 못했습니다.%n%n인터넷 연결을 확인한 후 다시 시도하세요.

no.BootstrapIntro=Dette lette installasjonsprogrammet kontrollerer alltid den nyeste verifiserte DownTrack-versjonen og den aktuelle mediemotoren.
no.LatestDetected=Nyeste verifiserte versjon: %1
no.CheckingLatest=Kontrollerer nyeste verifiserte versjon...
no.DownloadInstall=Last ned og installer
no.InstallReady=DownTrack %1 er klar til å installeres.
no.InstallVerificationFailed=DownTrack ble lastet ned, men de installerte filene kunne ikke verifiseres.
no.NetworkError=DownTrack kunne ikke hente informasjon om den nyeste versjonen.%n%nKontroller internettforbindelsen og prøv igjen.

pl.BootstrapIntro=Ten lekki instalator zawsze sprawdza najnowszą zweryfikowaną wersję DownTrack i bieżący silnik multimediów.
pl.LatestDetected=Najnowsza zweryfikowana wersja: %1
pl.CheckingLatest=Sprawdzanie najnowszej zweryfikowanej wersji...
pl.DownloadInstall=Pobierz i zainstaluj
pl.InstallReady=DownTrack %1 jest gotowy do instalacji.
pl.InstallVerificationFailed=DownTrack został pobrany, ale nie udało się zweryfikować zainstalowanych plików.
pl.NetworkError=DownTrack nie mógł pobrać informacji o najnowszym wydaniu.%n%nSprawdź połączenie z internetem i spróbuj ponownie.

pt.BootstrapIntro=Este instalador leve verifica sempre a versão mais recente verificada do DownTrack e o mecanismo de mídia atual.
pt.LatestDetected=Versão verificada mais recente: %1
pt.CheckingLatest=Verificando a versão verificada mais recente...
pt.DownloadInstall=Baixar e instalar
pt.InstallReady=O DownTrack %1 está pronto para ser instalado.
pt.InstallVerificationFailed=O DownTrack foi baixado, mas não foi possível verificar os arquivos instalados.
pt.NetworkError=O DownTrack não conseguiu obter as informações da versão mais recente.%n%nVerifique sua conexão com a Internet e tente novamente.

ru.BootstrapIntro=Этот лёгкий установщик всегда проверяет последнюю проверенную версию DownTrack и текущий медиа-движок.
ru.LatestDetected=Последняя проверенная версия: %1
ru.CheckingLatest=Проверка последней проверенной версии...
ru.DownloadInstall=Скачать и установить
ru.InstallReady=DownTrack %1 готов к установке.
ru.InstallVerificationFailed=DownTrack загружен, но установленные файлы не удалось проверить.
ru.NetworkError=DownTrack не удалось получить информацию о последней версии.%n%nПроверьте подключение к Интернету и повторите попытку.

es.BootstrapIntro=Este instalador ligero comprueba siempre la versión verificada más reciente de DownTrack y el motor multimedia actual.
es.LatestDetected=Última versión verificada: %1
es.CheckingLatest=Comprobando la última versión verificada...
es.DownloadInstall=Descargar e instalar
es.InstallReady=DownTrack %1 está listo para instalarse.
es.InstallVerificationFailed=DownTrack se descargó, pero no se pudieron verificar los archivos instalados.
es.NetworkError=DownTrack no pudo obtener la información de la última versión.%n%nComprueba tu conexión a Internet y vuelve a intentarlo.

tr.BootstrapIntro=Bu hafif yükleyici, her zaman doğrulanmış en yeni DownTrack sürümünü ve güncel medya motorunu kontrol eder.
tr.LatestDetected=En yeni doğrulanmış sürüm: %1
tr.CheckingLatest=En yeni doğrulanmış sürüm denetleniyor...
tr.DownloadInstall=İndir ve yükle
tr.InstallReady=DownTrack %1 yüklenmeye hazır.
tr.InstallVerificationFailed=DownTrack indirildi, ancak yüklenen dosyalar doğrulanamadı.
tr.NetworkError=DownTrack en son sürüm bilgilerini alamadı.%n%nİnternet bağlantınızı kontrol edip tekrar deneyin.

uk.BootstrapIntro=Цей легкий інсталятор завжди перевіряє останню перевірену версію DownTrack і поточний медіадвигун.
uk.LatestDetected=Остання перевірена версія: %1
uk.CheckingLatest=Перевірка останньої перевіреної версії...
uk.DownloadInstall=Завантажити й встановити
uk.InstallReady=DownTrack %1 готовий до встановлення.
uk.InstallVerificationFailed=DownTrack завантажено, але перевірити встановлені файли не вдалося.
uk.NetworkError=DownTrack не вдалося отримати інформацію про останню версію.%n%nПеревірте підключення до Інтернету та повторіть спробу.

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
      RaiseException(CustomMessage('NetworkError'));

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
  WizardForm.WelcomeLabel2.Caption := CustomMessage('BootstrapIntro');
  WizardForm.WelcomeLabel2.Font.Size := 10;

  WizardForm.NextButton.Caption := SetupMessage(msgButtonNext);
  WizardForm.CancelButton.Caption := SetupMessage(msgButtonCancel);
  WizardForm.BackButton.Visible := False;

  InstalledIni := ExpandConstant('{localappdata}\Programs\DownTrack\DownTrack.Install.ini');
  InstalledVersion := GetIniString('Install', 'Version', '', InstalledIni);

  if LoadLatestManifest then
  begin
    if InstalledVersion <> '' then
      WizardForm.StatusLabel.Caption :=
        FmtMessage('%1  •  %2', [InstalledVersion, LatestVersion])
    else
      WizardForm.StatusLabel.Caption :=
        FmtMessage(CustomMessage('LatestDetected'), [LatestVersion]);

    WizardForm.WelcomeLabel2.Caption := FmtMessage(CustomMessage('LatestDetected'), [LatestVersion]);
  end
  else
  begin
    WizardForm.StatusLabel.Caption := CustomMessage('CheckingLatest');
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

    WizardForm.NextButton.Caption := CustomMessage('DownloadInstall');
    WizardForm.StatusLabel.Caption :=
      Format(CustomMessage('LatestDetected'), [LatestVersion]);
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    InstallIsValid := VerifyInstalledFiles;

    if InstallIsValid then
    begin
      WizardForm.StatusLabel.Caption := FmtMessage(CustomMessage('InstallReady'), [LatestVersion]);
    end
    else
    begin
      WizardForm.StatusLabel.Caption := CustomMessage('InstallVerificationFailed');
      MsgBox(
        CustomMessage('InstallVerificationFailed') + #13#10 + #13#10 +
        CustomMessage('VerificationRetry'),
        mbCriticalError,
        MB_OK);
    end;
  end;
end;
