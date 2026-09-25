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
DisableWelcomePage=yes
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

[Code]
var
  SetupForm: TSetupForm;
  HeaderPanel: TPanel;
  BodyPanel: TPanel;
  AccentBar: TPanel;
  FooterPanel: TPanel;
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
  CurrentLanguage: String;
  SelectedLanguageMode: String;
  Translations: TStringList;

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
begin
  Translations := TStringList.Create;

  Translations.Add('en.Title=DownTrack');
  Translations.Add('en.Tagline=Your media. Your library. Always up to date.');
  Translations.Add('en.Language=Language');
  Translations.Add('en.Automatic=Automatic (Windows)');
  Translations.Add('en.Checking=Checking the latest version…');
  Translations.Add('en.Latest=Latest version: %1');
  Translations.Add('en.Downloading=Downloading DownTrack…');
  Translations.Add('en.Verifying=Verifying your download…');
  Translations.Add('en.Installing=Installing DownTrack…');
  Translations.Add('en.Complete=DownTrack is ready.');
  Translations.Add('en.UpToDate=You already have the latest version.');
  Translations.Add('en.Start=Install latest version');
  Translations.Add('en.Open=Open DownTrack');
  Translations.Add('en.Retry=Try again');
  Translations.Add('en.Close=Close');
  Translations.Add('en.Error=We couldn’t complete the installation.');
  Translations.Add('en.Progress=This will only take a few moments.');
  Translations.Add('en.CloseDownTrack=Please close DownTrack and try again.');

  Translations.Add('he.Title=DownTrack');
  Translations.Add('he.Tagline=המדיה שלך. הספרייה שלך. תמיד מעודכנת.');
  Translations.Add('he.Language=שפה');
  Translations.Add('he.Automatic=אוטומטי (Windows)');
  Translations.Add('he.Checking=בודק את הגרסה העדכנית ביותר…');
  Translations.Add('he.Latest=הגרסה האחרונה: %1');
  Translations.Add('he.Downloading=מוריד את DownTrack…');
  Translations.Add('he.Verifying=מאמת את ההורדה…');
  Translations.Add('he.Installing=מתקין את DownTrack…');
  Translations.Add('he.Complete=DownTrack מוכן.');
  Translations.Add('he.UpToDate=הגרסה העדכנית ביותר כבר מותקנת.');
  Translations.Add('he.Start=התקן את הגרסה האחרונה');
  Translations.Add('he.Open=פתח את DownTrack');
  Translations.Add('he.Retry=נסה שוב');
  Translations.Add('he.Close=סגור');
  Translations.Add('he.Error=לא ניתן היה להשלים את ההתקנה.');
  Translations.Add('he.Progress=זה ייקח רק כמה רגעים.');
  Translations.Add('he.CloseDownTrack=סגור את DownTrack ונסה שוב.');

  Translations.Add('es.Title=DownTrack');
  Translations.Add('es.Tagline=Tus medios. Tu biblioteca. Siempre actualizada.');
  Translations.Add('es.Language=Idioma');
  Translations.Add('es.Automatic=Automático (Windows)');
  Translations.Add('es.Checking=Comprobando la versión más reciente…');
  Translations.Add('es.Latest=Última versión: %1');
  Translations.Add('es.Downloading=Descargando DownTrack…');
  Translations.Add('es.Verifying=Verificando la descarga…');
  Translations.Add('es.Installing=Instalando DownTrack…');
  Translations.Add('es.Complete=DownTrack está listo.');
  Translations.Add('es.UpToDate=Ya tienes la última versión.');
  Translations.Add('es.Start=Instalar la última versión');
  Translations.Add('es.Open=Abrir DownTrack');
  Translations.Add('es.Retry=Intentar de nuevo');
  Translations.Add('es.Close=Cerrar');
  Translations.Add('es.Error=No se pudo completar la instalación.');
  Translations.Add('es.Progress=Esto solo tardará unos momentos.');
  Translations.Add('es.CloseDownTrack=Cierra DownTrack y vuelve a intentarlo.');

  Translations.Add('fr.Title=DownTrack');
  Translations.Add('fr.Tagline=Vos médias. Votre bibliothèque. Toujours à jour.');
  Translations.Add('fr.Language=Langue');
  Translations.Add('fr.Automatic=Automatique (Windows)');
  Translations.Add('fr.Checking=Vérification de la dernière version…');
  Translations.Add('fr.Latest=Dernière version : %1');
  Translations.Add('fr.Downloading=Téléchargement de DownTrack…');
  Translations.Add('fr.Verifying=Vérification du téléchargement…');
  Translations.Add('fr.Installing=Installation de DownTrack…');
  Translations.Add('fr.Complete=DownTrack est prêt.');
  Translations.Add('fr.UpToDate=Vous avez déjà la dernière version.');
  Translations.Add('fr.Start=Installer la dernière version');
  Translations.Add('fr.Open=Ouvrir DownTrack');
  Translations.Add('fr.Retry=Réessayer');
  Translations.Add('fr.Close=Fermer');
  Translations.Add('fr.Error=Impossible de terminer l’installation.');
  Translations.Add('fr.Progress=Cela ne prendra que quelques instants.');
  Translations.Add('fr.CloseDownTrack=Fermez DownTrack puis réessayez.');

  Translations.Add('de.Title=DownTrack');
  Translations.Add('de.Tagline=Deine Medien. Deine Bibliothek. Immer aktuell.');
  Translations.Add('de.Language=Sprache');
  Translations.Add('de.Automatic=Automatisch (Windows)');
  Translations.Add('de.Checking=Neueste Version wird geprüft…');
  Translations.Add('de.Latest=Neueste Version: %1');
  Translations.Add('de.Downloading=DownTrack wird heruntergeladen…');
  Translations.Add('de.Verifying=Download wird überprüft…');
  Translations.Add('de.Installing=DownTrack wird installiert…');
  Translations.Add('de.Complete=DownTrack ist bereit.');
  Translations.Add('de.UpToDate=Die neueste Version ist bereits installiert.');
  Translations.Add('de.Start=Neueste Version installieren');
  Translations.Add('de.Open=DownTrack öffnen');
  Translations.Add('de.Retry=Erneut versuchen');
  Translations.Add('de.Close=Schließen');
  Translations.Add('de.Error=Die Installation konnte nicht abgeschlossen werden.');
  Translations.Add('de.Progress=Dies dauert nur wenige Augenblicke.');
  Translations.Add('de.CloseDownTrack=Bitte DownTrack schließen und erneut versuchen.');

  Translations.Add('it.Title=DownTrack');
  Translations.Add('it.Tagline=I tuoi contenuti. La tua libreria. Sempre aggiornata.');
  Translations.Add('it.Language=Lingua');
  Translations.Add('it.Automatic=Automatico (Windows)');
  Translations.Add('it.Checking=Controllo dell’ultima versione…');
  Translations.Add('it.Latest=Ultima versione: %1');
  Translations.Add('it.Downloading=Download di DownTrack…');
  Translations.Add('it.Verifying=Verifica del download…');
  Translations.Add('it.Installing=Installazione di DownTrack…');
  Translations.Add('it.Complete=DownTrack è pronto.');
  Translations.Add('it.UpToDate=Hai già l’ultima versione.');
  Translations.Add('it.Start=Installa l’ultima versione');
  Translations.Add('it.Open=Apri DownTrack');
  Translations.Add('it.Retry=Riprova');
  Translations.Add('it.Close=Chiudi');
  Translations.Add('it.Error=Impossibile completare l’installazione.');
  Translations.Add('it.Progress=Ci vorranno solo pochi istanti.');
  Translations.Add('it.CloseDownTrack=Chiudi DownTrack e riprova.');

  Translations.Add('pt.Title=DownTrack');
  Translations.Add('pt.Tagline=Sua mídia. Sua biblioteca. Sempre atualizada.');
  Translations.Add('pt.Language=Idioma');
  Translations.Add('pt.Automatic=Automático (Windows)');
  Translations.Add('pt.Checking=Verificando a versão mais recente…');
  Translations.Add('pt.Latest=Versão mais recente: %1');
  Translations.Add('pt.Downloading=Baixando o DownTrack…');
  Translations.Add('pt.Verifying=Verificando o download…');
  Translations.Add('pt.Installing=Instalando o DownTrack…');
  Translations.Add('pt.Complete=O DownTrack está pronto.');
  Translations.Add('pt.UpToDate=Você já tem a versão mais recente.');
  Translations.Add('pt.Start=Instalar a versão mais recente');
  Translations.Add('pt.Open=Abrir o DownTrack');
  Translations.Add('pt.Retry=Tentar novamente');
  Translations.Add('pt.Close=Fechar');
  Translations.Add('pt.Error=Não foi possível concluir a instalação.');
  Translations.Add('pt.Progress=Isso levará apenas alguns instantes.');
  Translations.Add('pt.CloseDownTrack=Feche o DownTrack e tente novamente.');

  Translations.Add('nl.Title=DownTrack');
  Translations.Add('nl.Tagline=Jouw media. Jouw bibliotheek. Altijd actueel.');
  Translations.Add('nl.Language=Taal');
  Translations.Add('nl.Automatic=Automatisch (Windows)');
  Translations.Add('nl.Checking=Laatste versie controleren…');
  Translations.Add('nl.Latest=Nieuwste versie: %1');
  Translations.Add('nl.Downloading=DownTrack downloaden…');
  Translations.Add('nl.Verifying=Download controleren…');
  Translations.Add('nl.Installing=DownTrack installeren…');
  Translations.Add('nl.Complete=DownTrack is klaar.');
  Translations.Add('nl.UpToDate=Je hebt al de nieuwste versie.');
  Translations.Add('nl.Start=Nieuwste versie installeren');
  Translations.Add('nl.Open=DownTrack openen');
  Translations.Add('nl.Retry=Opnieuw proberen');
  Translations.Add('nl.Close=Sluiten');
  Translations.Add('nl.Error=De installatie kon niet worden voltooid.');
  Translations.Add('nl.Progress=Dit duurt maar een paar momenten.');
  Translations.Add('nl.CloseDownTrack=Sluit DownTrack en probeer het opnieuw.');

  Translations.Add('pl.Title=DownTrack');
  Translations.Add('pl.Tagline=Twoje media. Twoja biblioteka. Zawsze aktualne.');
  Translations.Add('pl.Language=Język');
  Translations.Add('pl.Automatic=Automatycznie (Windows)');
  Translations.Add('pl.Checking=Sprawdzanie najnowszej wersji…');
  Translations.Add('pl.Latest=Najnowsza wersja: %1');
  Translations.Add('pl.Downloading=Pobieranie DownTrack…');
  Translations.Add('pl.Verifying=Weryfikowanie pobranego pliku…');
  Translations.Add('pl.Installing=Instalowanie DownTrack…');
  Translations.Add('pl.Complete=DownTrack jest gotowy.');
  Translations.Add('pl.UpToDate=Masz już najnowszą wersję.');
  Translations.Add('pl.Start=Zainstaluj najnowszą wersję');
  Translations.Add('pl.Open=Otwórz DownTrack');
  Translations.Add('pl.Retry=Spróbuj ponownie');
  Translations.Add('pl.Close=Zamknij');
  Translations.Add('pl.Error=Nie udało się ukończyć instalacji.');
  Translations.Add('pl.Progress=To potrwa tylko chwilę.');
  Translations.Add('pl.CloseDownTrack=Zamknij DownTrack i spróbuj ponownie.');

  Translations.Add('cs.Title=DownTrack');
  Translations.Add('cs.Tagline=Vaše média. Vaše knihovna. Vždy aktuální.');
  Translations.Add('cs.Language=Jazyk');
  Translations.Add('cs.Automatic=Automaticky (Windows)');
  Translations.Add('cs.Checking=Kontrola nejnovější verze…');
  Translations.Add('cs.Latest=Nejnovější verze: %1');
  Translations.Add('cs.Downloading=Stahování DownTrack…');
  Translations.Add('cs.Verifying=Ověřování stažení…');
  Translations.Add('cs.Installing=Instalace DownTrack…');
  Translations.Add('cs.Complete=DownTrack je připraven.');
  Translations.Add('cs.UpToDate=Nejnovější verzi už máte.');
  Translations.Add('cs.Start=Instalovat nejnovější verzi');
  Translations.Add('cs.Open=Otevřít DownTrack');
  Translations.Add('cs.Retry=Zkusit znovu');
  Translations.Add('cs.Close=Zavřít');
  Translations.Add('cs.Error=Instalaci se nepodařilo dokončit.');
  Translations.Add('cs.Progress=Zabere to jen pár okamžiků.');
  Translations.Add('cs.CloseDownTrack=Zavřete DownTrack a zkuste to znovu.');

  Translations.Add('tr.Title=DownTrack');
  Translations.Add('tr.Tagline=Medyanız. Kitaplığınız. Her zaman güncel.');
  Translations.Add('tr.Language=Dil');
  Translations.Add('tr.Automatic=Otomatik (Windows)');
  Translations.Add('tr.Checking=En yeni sürüm kontrol ediliyor…');
  Translations.Add('tr.Latest=En yeni sürüm: %1');
  Translations.Add('tr.Downloading=DownTrack indiriliyor…');
  Translations.Add('tr.Verifying=İndirme doğrulanıyor…');
  Translations.Add('tr.Installing=DownTrack yükleniyor…');
  Translations.Add('tr.Complete=DownTrack hazır.');
  Translations.Add('tr.UpToDate=Zaten en yeni sürüme sahipsiniz.');
  Translations.Add('tr.Start=En yeni sürümü yükle');
  Translations.Add('tr.Open=DownTrack’i aç');
  Translations.Add('tr.Retry=Tekrar dene');
  Translations.Add('tr.Close=Kapat');
  Translations.Add('tr.Error=Yükleme tamamlanamadı.');
  Translations.Add('tr.Progress=Bu yalnızca birkaç dakika sürecek.');
  Translations.Add('tr.CloseDownTrack=DownTrack’i kapatıp tekrar deneyin.');

  Translations.Add('uk.Title=DownTrack');
  Translations.Add('uk.Tagline=Ваші медіа. Ваша бібліотека. Завжди актуальні.');
  Translations.Add('uk.Language=Мова');
  Translations.Add('uk.Automatic=Автоматично (Windows)');
  Translations.Add('uk.Checking=Перевірка останньої версії…');
  Translations.Add('uk.Latest=Остання версія: %1');
  Translations.Add('uk.Downloading=Завантаження DownTrack…');
  Translations.Add('uk.Verifying=Перевірка завантаження…');
  Translations.Add('uk.Installing=Встановлення DownTrack…');
  Translations.Add('uk.Complete=DownTrack готовий.');
  Translations.Add('uk.UpToDate=У вас уже остання версія.');
  Translations.Add('uk.Start=Встановити останню версію');
  Translations.Add('uk.Open=Відкрити DownTrack');
  Translations.Add('uk.Retry=Спробувати ще раз');
  Translations.Add('uk.Close=Закрити');
  Translations.Add('uk.Error=Не вдалося завершити встановлення.');
  Translations.Add('uk.Progress=Це займе лише кілька хвилин.');
  Translations.Add('uk.CloseDownTrack=Закрийте DownTrack і спробуйте ще раз.');

  Translations.Add('ru.Title=DownTrack');
  Translations.Add('ru.Tagline=Ваши медиа. Ваша библиотека. Всегда актуальны.');
  Translations.Add('ru.Language=Язык');
  Translations.Add('ru.Automatic=Автоматически (Windows)');
  Translations.Add('ru.Checking=Проверка последней версии…');
  Translations.Add('ru.Latest=Последняя версия: %1');
  Translations.Add('ru.Downloading=Загрузка DownTrack…');
  Translations.Add('ru.Verifying=Проверка загрузки…');
  Translations.Add('ru.Installing=Установка DownTrack…');
  Translations.Add('ru.Complete=DownTrack готов.');
  Translations.Add('ru.UpToDate=У вас уже установлена последняя версия.');
  Translations.Add('ru.Start=Установить последнюю версию');
  Translations.Add('ru.Open=Открыть DownTrack');
  Translations.Add('ru.Retry=Повторить');
  Translations.Add('ru.Close=Закрыть');
  Translations.Add('ru.Error=Не удалось завершить установку.');
  Translations.Add('ru.Progress=Это займет всего несколько минут.');
  Translations.Add('ru.CloseDownTrack=Закройте DownTrack и попробуйте снова.');

  Translations.Add('ar.Title=DownTrack');
  Translations.Add('ar.Tagline=وسائطك. مكتبتك. محدثة دائمًا.');
  Translations.Add('ar.Language=اللغة');
  Translations.Add('ar.Automatic=تلقائي (Windows)');
  Translations.Add('ar.Checking=جارٍ التحقق من أحدث إصدار…');
  Translations.Add('ar.Latest=أحدث إصدار: %1');
  Translations.Add('ar.Downloading=جارٍ تنزيل DownTrack…');
  Translations.Add('ar.Verifying=جارٍ التحقق من التنزيل…');
  Translations.Add('ar.Installing=جارٍ تثبيت DownTrack…');
  Translations.Add('ar.Complete=DownTrack جاهز.');
  Translations.Add('ar.UpToDate=لديك بالفعل أحدث إصدار.');
  Translations.Add('ar.Start=تثبيت أحدث إصدار');
  Translations.Add('ar.Open=فتح DownTrack');
  Translations.Add('ar.Retry=حاول مرة أخرى');
  Translations.Add('ar.Close=إغلاق');
  Translations.Add('ar.Error=تعذر إكمال التثبيت.');
  Translations.Add('ar.Progress=لن يستغرق ذلك سوى بضع لحظات.');
  Translations.Add('ar.CloseDownTrack=أغلق DownTrack وحاول مرة أخرى.');

  Translations.Add('el.Title=DownTrack');
  Translations.Add('el.Tagline=Τα πολυμέσα σας. Η βιβλιοθήκη σας. Πάντα ενημερωμένα.');
  Translations.Add('el.Language=Γλώσσα');
  Translations.Add('el.Automatic=Αυτόματα (Windows)');
  Translations.Add('el.Checking=Έλεγχος της πιο πρόσφατης έκδοσης…');
  Translations.Add('el.Latest=Τελευταία έκδοση: %1');
  Translations.Add('el.Downloading=Λήψη DownTrack…');
  Translations.Add('el.Verifying=Επαλήθευση λήψης…');
  Translations.Add('el.Installing=Εγκατάσταση DownTrack…');
  Translations.Add('el.Complete=Το DownTrack είναι έτοιμο.');
  Translations.Add('el.UpToDate=Έχετε ήδη την πιο πρόσφατη έκδοση.');
  Translations.Add('el.Start=Εγκατάσταση τελευταίας έκδοσης');
  Translations.Add('el.Open=Άνοιγμα DownTrack');
  Translations.Add('el.Retry=Δοκιμή ξανά');
  Translations.Add('el.Close=Κλείσιμο');
  Translations.Add('el.Error=Δεν ήταν δυνατή η ολοκλήρωση της εγκατάστασης.');
  Translations.Add('el.Progress=Θα χρειαστούν μόνο λίγα λεπτά.');
  Translations.Add('el.CloseDownTrack=Κλείστε το DownTrack και δοκιμάστε ξανά.');

  Translations.Add('ro.Title=DownTrack');
  Translations.Add('ro.Tagline=Media ta. Biblioteca ta. Mereu actualizată.');
  Translations.Add('ro.Language=Limbă');
  Translations.Add('ro.Automatic=Automat (Windows)');
  Translations.Add('ro.Checking=Se verifică cea mai recentă versiune…');
  Translations.Add('ro.Latest=Cea mai recentă versiune: %1');
  Translations.Add('ro.Downloading=Se descarcă DownTrack…');
  Translations.Add('ro.Verifying=Se verifică descărcarea…');
  Translations.Add('ro.Installing=Se instalează DownTrack…');
  Translations.Add('ro.Complete=DownTrack este gata.');
  Translations.Add('ro.UpToDate=Aveți deja cea mai recentă versiune.');
  Translations.Add('ro.Start=Instalează cea mai recentă versiune');
  Translations.Add('ro.Open=Deschide DownTrack');
  Translations.Add('ro.Retry=Încearcă din nou');
  Translations.Add('ro.Close=Închide');
  Translations.Add('ro.Error=Instalarea nu a putut fi finalizată.');
  Translations.Add('ro.Progress=Va dura doar câteva momente.');
  Translations.Add('ro.CloseDownTrack=Închide DownTrack și încearcă din nou.');

  Translations.Add('ja.Title=DownTrack');
  Translations.Add('ja.Tagline=あなたのメディア。あなたのライブラリ。いつでも最新。');
  Translations.Add('ja.Language=言語');
  Translations.Add('ja.Automatic=自動（Windows）');
  Translations.Add('ja.Checking=最新バージョンを確認しています…');
  Translations.Add('ja.Latest=最新バージョン: %1');
  Translations.Add('ja.Downloading=DownTrackをダウンロードしています…');
  Translations.Add('ja.Verifying=ダウンロードを確認しています…');
  Translations.Add('ja.Installing=DownTrackをインストールしています…');
  Translations.Add('ja.Complete=DownTrackの準備ができました。');
  Translations.Add('ja.UpToDate=すでに最新バージョンです。');
  Translations.Add('ja.Start=最新バージョンをインストール');
  Translations.Add('ja.Open=DownTrackを開く');
  Translations.Add('ja.Retry=もう一度試す');
  Translations.Add('ja.Close=閉じる');
  Translations.Add('ja.Error=インストールを完了できませんでした。');
  Translations.Add('ja.Progress=完了まであと少しです。');
  Translations.Add('ja.CloseDownTrack=DownTrackを閉じて、もう一度お試しください。');

  Translations.Add('ko.Title=DownTrack');
  Translations.Add('ko.Tagline=내 미디어. 내 라이브러리. 항상 최신 상태.');
  Translations.Add('ko.Language=언어');
  Translations.Add('ko.Automatic=자동 (Windows)');
  Translations.Add('ko.Checking=최신 버전을 확인하는 중…');
  Translations.Add('ko.Latest=최신 버전: %1');
  Translations.Add('ko.Downloading=DownTrack을 다운로드하는 중…');
  Translations.Add('ko.Verifying=다운로드를 확인하는 중…');
  Translations.Add('ko.Installing=DownTrack을 설치하는 중…');
  Translations.Add('ko.Complete=DownTrack을 사용할 준비가 되었습니다.');
  Translations.Add('ko.UpToDate=이미 최신 버전이 설치되어 있습니다.');
  Translations.Add('ko.Start=최신 버전 설치');
  Translations.Add('ko.Open=DownTrack 열기');
  Translations.Add('ko.Retry=다시 시도');
  Translations.Add('ko.Close=닫기');
  Translations.Add('ko.Error=설치를 완료하지 못했습니다.');
  Translations.Add('ko.Progress=잠시만 기다려 주세요.');
  Translations.Add('ko.CloseDownTrack=DownTrack을 닫고 다시 시도하세요.');

  Translations.Add('zhcn.Title=DownTrack');
  Translations.Add('zhcn.Tagline=你的媒体。你的媒体库。始终保持最新。');
  Translations.Add('zhcn.Language=语言');
  Translations.Add('zhcn.Automatic=自动（Windows）');
  Translations.Add('zhcn.Checking=正在检查最新版本…');
  Translations.Add('zhcn.Latest=最新版本：%1');
  Translations.Add('zhcn.Downloading=正在下载 DownTrack…');
  Translations.Add('zhcn.Verifying=正在验证下载…');
  Translations.Add('zhcn.Installing=正在安装 DownTrack…');
  Translations.Add('zhcn.Complete=DownTrack 已准备就绪。');
  Translations.Add('zhcn.UpToDate=你已经拥有最新版本。');
  Translations.Add('zhcn.Start=安装最新版本');
  Translations.Add('zhcn.Open=打开 DownTrack');
  Translations.Add('zhcn.Retry=重试');
  Translations.Add('zhcn.Close=关闭');
  Translations.Add('zhcn.Error=无法完成安装。');
  Translations.Add('zhcn.Progress=只需等待片刻。');
  Translations.Add('zhcn.CloseDownTrack=请关闭 DownTrack 后重试。');

  Translations.Add('zhtw.Title=DownTrack');
  Translations.Add('zhtw.Tagline=你的媒體。你的資料庫。永遠保持最新。');
  Translations.Add('zhtw.Language=語言');
  Translations.Add('zhtw.Automatic=自動（Windows）');
  Translations.Add('zhtw.Checking=正在檢查最新版本…');
  Translations.Add('zhtw.Latest=最新版本：%1');
  Translations.Add('zhtw.Downloading=正在下載 DownTrack…');
  Translations.Add('zhtw.Verifying=正在驗證下載…');
  Translations.Add('zhtw.Installing=正在安裝 DownTrack…');
  Translations.Add('zhtw.Complete=DownTrack 已準備就緒。');
  Translations.Add('zhtw.UpToDate=你已經擁有最新版本。');
  Translations.Add('zhtw.Start=安裝最新版本');
  Translations.Add('zhtw.Open=開啟 DownTrack');
  Translations.Add('zhtw.Retry=重試');
  Translations.Add('zhtw.Close=關閉');
  Translations.Add('zhtw.Error=無法完成安裝。');
  Translations.Add('zhtw.Progress=只需要稍候片刻。');
  Translations.Add('zhtw.CloseDownTrack=請關閉 DownTrack 後再試一次。');
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
  StatusText.Alignment := taLeftJustify;
  VersionText.Alignment := taLeftJustify;
  DetailText.Alignment := taLeftJustify;

  AccentBar.Left := ScaleX(28);
  LogoText.Left := ScaleX(52);
  TaglineText.Left := ScaleX(53);
  LanguageLabel.Left := ScaleX(520);
  LanguageCombo.Left := ScaleX(585);
  StatusText.Left := ScaleX(52);
  VersionText.Left := ScaleX(52);
  DetailText.Left := ScaleX(52);
  ProgressBar.Left := ScaleX(52);
  PrimaryButton.Left := ScaleX(452);
  SecondaryButton.Left := ScaleX(334);

  if Rtl then
  begin
    LogoText.Alignment := taRightJustify;
    TaglineText.Alignment := taRightJustify;
    LanguageLabel.Alignment := taRightJustify;
    StatusText.Alignment := taRightJustify;
    VersionText.Alignment := taRightJustify;
    DetailText.Alignment := taRightJustify;

    AccentBar.Left := SetupForm.ClientWidth - ScaleX(34);
    LogoText.Left := ScaleX(180);
    TaglineText.Left := ScaleX(120);
    LanguageCombo.Left := ScaleX(52);
    LanguageLabel.Left := ScaleX(210);
    StatusText.Left := ScaleX(120);
    VersionText.Left := ScaleX(120);
    DetailText.Left := ScaleX(120);
    ProgressBar.Left := ScaleX(120);
    PrimaryButton.Left := ScaleX(48);
    SecondaryButton.Left := ScaleX(330);
  end;

  SetupForm.Caption := T('Title');
  SetupForm.Update;
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
  SetupForm.Update;
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

  SetupForm.Close;
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
  PrimaryButton.OnClick := @OpenDownTrack;
  SecondaryButton.Visible := True;
  SecondaryButton.Caption := T('Close');
  SetupForm.Update;
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
    SetupForm.Update;

    DownloadTemporaryFile(
      PayloadUrl,
      'DownTrack-Payload.zip',
      PayloadHash,
      @DownloadProgress);

    StatusText.Caption := T('Verifying');
    DetailText.Caption := PayloadSize div 1048576;
    DetailText.Caption :=
      IntToStr(PayloadSize div 1048576) + ' MB';
    ProgressBar.Position := 70;
    SetupForm.Update;

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
    SetupForm.Update;

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
    PrimaryButton.OnClick := @InstallLatest;
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

  SetupForm.Close;
end;

procedure SetInstallState(
  const TitleText, Status: String);
begin
  LogoText.Caption := TitleText;
  StatusText.Caption := Status;
  SetupForm.Update;
end;

procedure InitializeInstallerUi;
var
  SavedMode: String;
  I: Integer;
begin
  SetupForm :=
    CreateCustomForm(
      ScaleX(800),
      ScaleY(500),
      False,
      False);

  SetupForm.Caption := 'DownTrack';
  SetupForm.BorderStyle := bsNone;
  SetupForm.Position := poScreenCenter;
  SetupForm.Color := $00F7F8FC;

  HeaderPanel := TPanel.Create(SetupForm);
  HeaderPanel.Parent := SetupForm;
  HeaderPanel.Left := 0;
  HeaderPanel.Top := 0;
  HeaderPanel.Width := SetupForm.ClientWidth;
  HeaderPanel.Height := ScaleY(92);
  HeaderPanel.BevelOuter := bvNone;
  HeaderPanel.Color := $00FFFFFF;

  AccentBar := TPanel.Create(SetupForm);
  AccentBar.Parent := SetupForm;
  AccentBar.Left := ScaleX(28);
  AccentBar.Top := ScaleY(24);
  AccentBar.Width := ScaleX(6);
  AccentBar.Height := ScaleY(44);
  AccentBar.BevelOuter := bvNone;
  AccentBar.Color := $007B61FF;

  LogoText := TNewStaticText.Create(SetupForm);
  LogoText.Parent := SetupForm;
  LogoText.Left := ScaleX(52);
  LogoText.Top := ScaleY(18);
  LogoText.Width := ScaleX(350);
  LogoText.Height := ScaleY(34);
  LogoText.Font.Size := 23;
  LogoText.Font.Style := [fsBold];

  TaglineText := TNewStaticText.Create(SetupForm);
  TaglineText.Parent := SetupForm;
  TaglineText.Left := ScaleX(53);
  TaglineText.Top := ScaleY(53);
  TaglineText.Width := ScaleX(430);
  TaglineText.Height := ScaleY(22);
  TaglineText.Font.Size := 9;
  TaglineText.Font.Color := $0069788A;
  TaglineText.WordWrap := True;

  LanguageLabel := TNewStaticText.Create(SetupForm);
  LanguageLabel.Parent := SetupForm;
  LanguageLabel.Left := ScaleX(520);
  LanguageLabel.Top := ScaleY(22);
  LanguageLabel.Width := ScaleX(58);
  LanguageLabel.Height := ScaleY(20);
  LanguageLabel.Font.Size := 8;
  LanguageLabel.Font.Color := $0069788A;

  LanguageCombo := TNewComboBox.Create(SetupForm);
  LanguageCombo.Parent := SetupForm;
  LanguageCombo.Left := ScaleX(585);
  LanguageCombo.Top := ScaleY(17);
  LanguageCombo.Width := ScaleX(160);
  LanguageCombo.Height := ScaleY(32);
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

  BodyPanel := TPanel.Create(SetupForm);
  BodyPanel.Parent := SetupForm;
  BodyPanel.Left := ScaleX(34);
  BodyPanel.Top := ScaleY(116);
  BodyPanel.Width := SetupForm.ClientWidth - ScaleX(68);
  BodyPanel.Height := ScaleY(250);
  BodyPanel.BevelOuter := bvLowered;
  BodyPanel.Color := $00FFFFFF;

  StatusText := TNewStaticText.Create(SetupForm);
  StatusText.Parent := SetupForm;
  StatusText.Left := ScaleX(52);
  StatusText.Top := ScaleY(142);
  StatusText.Width := ScaleX(690);
  StatusText.Height := ScaleY(34);
  StatusText.Font.Size := 12;
  StatusText.Font.Style := [fsBold];

  VersionText := TNewStaticText.Create(SetupForm);
  VersionText.Parent := SetupForm;
  VersionText.Left := ScaleX(52);
  VersionText.Top := ScaleY(180);
  VersionText.Width := ScaleX(690);
  VersionText.Height := ScaleY(28);
  VersionText.Font.Size := 10;
  VersionText.Font.Color := $0069788A;

  DetailText := TNewStaticText.Create(SetupForm);
  DetailText.Parent := SetupForm;
  DetailText.Left := ScaleX(52);
  DetailText.Top := ScaleY(214);
  DetailText.Width := ScaleX(690);
  DetailText.Height := ScaleY(24);
  DetailText.Font.Size := 9;
  DetailText.Font.Color := $007B61FF;

  ProgressBar := TNewProgressBar.Create(SetupForm);
  ProgressBar.Parent := SetupForm;
  ProgressBar.Left := ScaleX(52);
  ProgressBar.Top := ScaleY(264);
  ProgressBar.Width := ScaleX(690);
  ProgressBar.Height := ScaleY(10);
  ProgressBar.Min := 0;
  ProgressBar.Max := 100;
  ProgressBar.Position := 0;

  FooterPanel := TPanel.Create(SetupForm);
  FooterPanel.Parent := SetupForm;
  FooterPanel.Left := 0;
  FooterPanel.Top := ScaleY(408);
  FooterPanel.Width := SetupForm.ClientWidth;
  FooterPanel.Height := ScaleY(92);
  FooterPanel.BevelOuter := bvNone;
  FooterPanel.Color := $00F0F3F9;

  PrimaryButton := TNewButton.Create(SetupForm);
  PrimaryButton.Parent := SetupForm;
  PrimaryButton.Left := ScaleX(452);
  PrimaryButton.Top := ScaleY(432);
  PrimaryButton.Width := ScaleX(290);
  PrimaryButton.Height := ScaleY(44);
  PrimaryButton.Font.Style := [fsBold];
  PrimaryButton.OnClick := @InstallLatest;

  SecondaryButton := TNewButton.Create(SetupForm);
  SecondaryButton.Parent := SetupForm;
  SecondaryButton.Left := ScaleX(334);
  SecondaryButton.Top := ScaleY(432);
  SecondaryButton.Width := ScaleX(100);
  SecondaryButton.Height := ScaleY(44);
  SecondaryButton.OnClick := @CloseInstaller;

  SavedMode := GetSavedLanguageMode;

  if SameText(SavedMode, 'auto') then
    CurrentLanguage := DetectWindowsLanguage
  else
    CurrentLanguage := SavedMode;

  I := 0;
  if SameText(SavedMode, 'auto') then
    I := 0
  else
  begin
    while (I < 20) and
      not SameText(
        LangCodeFromIndex(I + 1),
        SavedMode) do
      I := I + 1;
    I := I + 1;
  end;

  LanguageCombo.ItemIndex := I;
  ApplyLanguageToForm;
end;

procedure InitializeWizard;
begin
  PopulateTranslations;
  WizardForm.Hide;
  InitializeInstallerUi;

  SetupForm.Show;
  SetupForm.Update;

  if LoadLatestManifest then
  begin
    VersionText.Caption := GetLatestVersionText;

    if (GetInstalledVersion <> '') and
       SameText(
         GetInstalledVersion,
         LatestVersion) then
    begin
      StatusText.Caption := T('UpToDate');
      DetailText.Caption := GetLatestVersionText;
      ProgressBar.Position := 100;
      PrimaryButton.Caption := T('Open');
      PrimaryButton.OnClick := @OpenDownTrack;
    end
    else
    begin
      StatusText.Caption := T('Checking');
      DetailText.Caption := GetLatestVersionText;
    end;
  end
  else
  begin
    StatusText.Caption := T('Error');
    DetailText.Caption := T('Retry');
    PrimaryButton.Caption := T('Retry');
    PrimaryButton.OnClick := @InstallLatest;
  end;

  SetupForm.ShowModal;
  SetupForm.Close;
  WizardForm.Close;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := False;
end;
