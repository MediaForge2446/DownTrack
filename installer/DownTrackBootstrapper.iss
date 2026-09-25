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

function T(const Key: String): String;
var
  Value: String;
begin
  Value := '';
  if Assigned(Translations) then
    Value := Translations.Values[CurrentLanguage + '.' + Key];

  if Value = '' then
    Value := Translations.Values['en.' + Key];

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

  Translations.Values['en.Title'] := 'DownTrack';
  Translations.Values['en.Tagline'] := 'Your media. Your library. Always up to date.';
  Translations.Values['en.Language'] := 'Language';
  Translations.Values['en.Automatic'] := 'Automatic (Windows)';
  Translations.Values['en.Checking'] := 'Checking the latest version…';
  Translations.Values['en.Latest'] := 'Latest version: %1';
  Translations.Values['en.Downloading'] := 'Downloading DownTrack…';
  Translations.Values['en.Verifying'] := 'Verifying your download…';
  Translations.Values['en.Installing'] := 'Installing DownTrack…';
  Translations.Values['en.Complete'] := 'DownTrack is ready.';
  Translations.Values['en.UpToDate'] := 'You already have the latest version.';
  Translations.Values['en.Start'] := 'Install latest version';
  Translations.Values['en.Open'] := 'Open DownTrack';
  Translations.Values['en.Retry'] := 'Try again';
  Translations.Values['en.Close'] := 'Close';
  Translations.Values['en.Error'] := 'We couldn’t complete the installation.';
  Translations.Values['en.Progress'] := 'This will only take a few moments.';
  Translations.Values['en.CloseDownTrack'] := 'Please close DownTrack and try again.';

  Translations.Values['he.Title'] := 'DownTrack';
  Translations.Values['he.Tagline'] := 'המדיה שלך. הספרייה שלך. תמיד מעודכנת.';
  Translations.Values['he.Language'] := 'שפה';
  Translations.Values['he.Automatic'] := 'אוטומטי (Windows)';
  Translations.Values['he.Checking'] := 'בודק את הגרסה העדכנית ביותר…';
  Translations.Values['he.Latest'] := 'הגרסה האחרונה: %1';
  Translations.Values['he.Downloading'] := 'מוריד את DownTrack…';
  Translations.Values['he.Verifying'] := 'מאמת את ההורדה…';
  Translations.Values['he.Installing'] := 'מתקין את DownTrack…';
  Translations.Values['he.Complete'] := 'DownTrack מוכן.';
  Translations.Values['he.UpToDate'] := 'הגרסה העדכנית ביותר כבר מותקנת.';
  Translations.Values['he.Start'] := 'התקן את הגרסה האחרונה';
  Translations.Values['he.Open'] := 'פתח את DownTrack';
  Translations.Values['he.Retry'] := 'נסה שוב';
  Translations.Values['he.Close'] := 'סגור';
  Translations.Values['he.Error'] := 'לא ניתן היה להשלים את ההתקנה.';
  Translations.Values['he.Progress'] := 'זה ייקח רק כמה רגעים.';
  Translations.Values['he.CloseDownTrack'] := 'סגור את DownTrack ונסה שוב.';

  Translations.Values['es.Title'] := 'DownTrack';
  Translations.Values['es.Tagline'] := 'Tus medios. Tu biblioteca. Siempre actualizada.';
  Translations.Values['es.Language'] := 'Idioma';
  Translations.Values['es.Automatic'] := 'Automático (Windows)';
  Translations.Values['es.Checking'] := 'Comprobando la versión más reciente…';
  Translations.Values['es.Latest'] := 'Última versión: %1';
  Translations.Values['es.Downloading'] := 'Descargando DownTrack…';
  Translations.Values['es.Verifying'] := 'Verificando la descarga…';
  Translations.Values['es.Installing'] := 'Instalando DownTrack…';
  Translations.Values['es.Complete'] := 'DownTrack está listo.';
  Translations.Values['es.UpToDate'] := 'Ya tienes la última versión.';
  Translations.Values['es.Start'] := 'Instalar la última versión';
  Translations.Values['es.Open'] := 'Abrir DownTrack';
  Translations.Values['es.Retry'] := 'Intentar de nuevo';
  Translations.Values['es.Close'] := 'Cerrar';
  Translations.Values['es.Error'] := 'No se pudo completar la instalación.';
  Translations.Values['es.Progress'] := 'Esto solo tardará unos momentos.';
  Translations.Values['es.CloseDownTrack'] := 'Cierra DownTrack y vuelve a intentarlo.';

  Translations.Values['fr.Title'] := 'DownTrack';
  Translations.Values['fr.Tagline'] := 'Vos médias. Votre bibliothèque. Toujours à jour.';
  Translations.Values['fr.Language'] := 'Langue';
  Translations.Values['fr.Automatic'] := 'Automatique (Windows)';
  Translations.Values['fr.Checking'] := 'Vérification de la dernière version…';
  Translations.Values['fr.Latest'] := 'Dernière version : %1';
  Translations.Values['fr.Downloading'] := 'Téléchargement de DownTrack…';
  Translations.Values['fr.Verifying'] := 'Vérification du téléchargement…';
  Translations.Values['fr.Installing'] := 'Installation de DownTrack…';
  Translations.Values['fr.Complete'] := 'DownTrack est prêt.';
  Translations.Values['fr.UpToDate'] := 'Vous avez déjà la dernière version.';
  Translations.Values['fr.Start'] := 'Installer la dernière version';
  Translations.Values['fr.Open'] := 'Ouvrir DownTrack';
  Translations.Values['fr.Retry'] := 'Réessayer';
  Translations.Values['fr.Close'] := 'Fermer';
  Translations.Values['fr.Error'] := 'Impossible de terminer l’installation.';
  Translations.Values['fr.Progress'] := 'Cela ne prendra que quelques instants.';
  Translations.Values['fr.CloseDownTrack'] := 'Fermez DownTrack puis réessayez.';

  Translations.Values['de.Title'] := 'DownTrack';
  Translations.Values['de.Tagline'] := 'Deine Medien. Deine Bibliothek. Immer aktuell.';
  Translations.Values['de.Language'] := 'Sprache';
  Translations.Values['de.Automatic'] := 'Automatisch (Windows)';
  Translations.Values['de.Checking'] := 'Neueste Version wird geprüft…';
  Translations.Values['de.Latest'] := 'Neueste Version: %1';
  Translations.Values['de.Downloading'] := 'DownTrack wird heruntergeladen…';
  Translations.Values['de.Verifying'] := 'Download wird überprüft…';
  Translations.Values['de.Installing'] := 'DownTrack wird installiert…';
  Translations.Values['de.Complete'] := 'DownTrack ist bereit.';
  Translations.Values['de.UpToDate'] := 'Die neueste Version ist bereits installiert.';
  Translations.Values['de.Start'] := 'Neueste Version installieren';
  Translations.Values['de.Open'] := 'DownTrack öffnen';
  Translations.Values['de.Retry'] := 'Erneut versuchen';
  Translations.Values['de.Close'] := 'Schließen';
  Translations.Values['de.Error'] := 'Die Installation konnte nicht abgeschlossen werden.';
  Translations.Values['de.Progress'] := 'Dies dauert nur wenige Augenblicke.';
  Translations.Values['de.CloseDownTrack'] := 'Bitte DownTrack schließen und erneut versuchen.';

  Translations.Values['it.Title'] := 'DownTrack';
  Translations.Values['it.Tagline'] := 'I tuoi contenuti. La tua libreria. Sempre aggiornata.';
  Translations.Values['it.Language'] := 'Lingua';
  Translations.Values['it.Automatic'] := 'Automatico (Windows)';
  Translations.Values['it.Checking'] := 'Controllo dell’ultima versione…';
  Translations.Values['it.Latest'] := 'Ultima versione: %1';
  Translations.Values['it.Downloading'] := 'Download di DownTrack…';
  Translations.Values['it.Verifying'] := 'Verifica del download…';
  Translations.Values['it.Installing'] := 'Installazione di DownTrack…';
  Translations.Values['it.Complete'] := 'DownTrack è pronto.';
  Translations.Values['it.UpToDate'] := 'Hai già l’ultima versione.';
  Translations.Values['it.Start'] := 'Installa l’ultima versione';
  Translations.Values['it.Open'] := 'Apri DownTrack';
  Translations.Values['it.Retry'] := 'Riprova';
  Translations.Values['it.Close'] := 'Chiudi';
  Translations.Values['it.Error'] := 'Impossibile completare l’installazione.';
  Translations.Values['it.Progress'] := 'Ci vorranno solo pochi istanti.';
  Translations.Values['it.CloseDownTrack'] := 'Chiudi DownTrack e riprova.';

  Translations.Values['pt.Title'] := 'DownTrack';
  Translations.Values['pt.Tagline'] := 'Sua mídia. Sua biblioteca. Sempre atualizada.';
  Translations.Values['pt.Language'] := 'Idioma';
  Translations.Values['pt.Automatic'] := 'Automático (Windows)';
  Translations.Values['pt.Checking'] := 'Verificando a versão mais recente…';
  Translations.Values['pt.Latest'] := 'Versão mais recente: %1';
  Translations.Values['pt.Downloading'] := 'Baixando o DownTrack…';
  Translations.Values['pt.Verifying'] := 'Verificando o download…';
  Translations.Values['pt.Installing'] := 'Instalando o DownTrack…';
  Translations.Values['pt.Complete'] := 'O DownTrack está pronto.';
  Translations.Values['pt.UpToDate'] := 'Você já tem a versão mais recente.';
  Translations.Values['pt.Start'] := 'Instalar a versão mais recente';
  Translations.Values['pt.Open'] := 'Abrir o DownTrack';
  Translations.Values['pt.Retry'] := 'Tentar novamente';
  Translations.Values['pt.Close'] := 'Fechar';
  Translations.Values['pt.Error'] := 'Não foi possível concluir a instalação.';
  Translations.Values['pt.Progress'] := 'Isso levará apenas alguns instantes.';
  Translations.Values['pt.CloseDownTrack'] := 'Feche o DownTrack e tente novamente.';

  Translations.Values['nl.Title'] := 'DownTrack';
  Translations.Values['nl.Tagline'] := 'Jouw media. Jouw bibliotheek. Altijd actueel.';
  Translations.Values['nl.Language'] := 'Taal';
  Translations.Values['nl.Automatic'] := 'Automatisch (Windows)';
  Translations.Values['nl.Checking'] := 'Laatste versie controleren…';
  Translations.Values['nl.Latest'] := 'Nieuwste versie: %1';
  Translations.Values['nl.Downloading'] := 'DownTrack downloaden…';
  Translations.Values['nl.Verifying'] := 'Download controleren…';
  Translations.Values['nl.Installing'] := 'DownTrack installeren…';
  Translations.Values['nl.Complete'] := 'DownTrack is klaar.';
  Translations.Values['nl.UpToDate'] := 'Je hebt al de nieuwste versie.';
  Translations.Values['nl.Start'] := 'Nieuwste versie installeren';
  Translations.Values['nl.Open'] := 'DownTrack openen';
  Translations.Values['nl.Retry'] := 'Opnieuw proberen';
  Translations.Values['nl.Close'] := 'Sluiten';
  Translations.Values['nl.Error'] := 'De installatie kon niet worden voltooid.';
  Translations.Values['nl.Progress'] := 'Dit duurt maar een paar momenten.';
  Translations.Values['nl.CloseDownTrack'] := 'Sluit DownTrack en probeer het opnieuw.';

  Translations.Values['pl.Title'] := 'DownTrack';
  Translations.Values['pl.Tagline'] := 'Twoje media. Twoja biblioteka. Zawsze aktualne.';
  Translations.Values['pl.Language'] := 'Język';
  Translations.Values['pl.Automatic'] := 'Automatycznie (Windows)';
  Translations.Values['pl.Checking'] := 'Sprawdzanie najnowszej wersji…';
  Translations.Values['pl.Latest'] := 'Najnowsza wersja: %1';
  Translations.Values['pl.Downloading'] := 'Pobieranie DownTrack…';
  Translations.Values['pl.Verifying'] := 'Weryfikowanie pobranego pliku…';
  Translations.Values['pl.Installing'] := 'Instalowanie DownTrack…';
  Translations.Values['pl.Complete'] := 'DownTrack jest gotowy.';
  Translations.Values['pl.UpToDate'] := 'Masz już najnowszą wersję.';
  Translations.Values['pl.Start'] := 'Zainstaluj najnowszą wersję';
  Translations.Values['pl.Open'] := 'Otwórz DownTrack';
  Translations.Values['pl.Retry'] := 'Spróbuj ponownie';
  Translations.Values['pl.Close'] := 'Zamknij';
  Translations.Values['pl.Error'] := 'Nie udało się ukończyć instalacji.';
  Translations.Values['pl.Progress'] := 'To potrwa tylko chwilę.';
  Translations.Values['pl.CloseDownTrack'] := 'Zamknij DownTrack i spróbuj ponownie.';

  Translations.Values['cs.Title'] := 'DownTrack';
  Translations.Values['cs.Tagline'] := 'Vaše média. Vaše knihovna. Vždy aktuální.';
  Translations.Values['cs.Language'] := 'Jazyk';
  Translations.Values['cs.Automatic'] := 'Automaticky (Windows)';
  Translations.Values['cs.Checking'] := 'Kontrola nejnovější verze…';
  Translations.Values['cs.Latest'] := 'Nejnovější verze: %1';
  Translations.Values['cs.Downloading'] := 'Stahování DownTrack…';
  Translations.Values['cs.Verifying'] := 'Ověřování stažení…';
  Translations.Values['cs.Installing'] := 'Instalace DownTrack…';
  Translations.Values['cs.Complete'] := 'DownTrack je připraven.';
  Translations.Values['cs.UpToDate'] := 'Nejnovější verzi už máte.';
  Translations.Values['cs.Start'] := 'Instalovat nejnovější verzi';
  Translations.Values['cs.Open'] := 'Otevřít DownTrack';
  Translations.Values['cs.Retry'] := 'Zkusit znovu';
  Translations.Values['cs.Close'] := 'Zavřít';
  Translations.Values['cs.Error'] := 'Instalaci se nepodařilo dokončit.';
  Translations.Values['cs.Progress'] := 'Zabere to jen pár okamžiků.';
  Translations.Values['cs.CloseDownTrack'] := 'Zavřete DownTrack a zkuste to znovu.';

  Translations.Values['tr.Title'] := 'DownTrack';
  Translations.Values['tr.Tagline'] := 'Medyanız. Kitaplığınız. Her zaman güncel.';
  Translations.Values['tr.Language'] := 'Dil';
  Translations.Values['tr.Automatic'] := 'Otomatik (Windows)';
  Translations.Values['tr.Checking'] := 'En yeni sürüm kontrol ediliyor…';
  Translations.Values['tr.Latest'] := 'En yeni sürüm: %1';
  Translations.Values['tr.Downloading'] := 'DownTrack indiriliyor…';
  Translations.Values['tr.Verifying'] := 'İndirme doğrulanıyor…';
  Translations.Values['tr.Installing'] := 'DownTrack yükleniyor…';
  Translations.Values['tr.Complete'] := 'DownTrack hazır.';
  Translations.Values['tr.UpToDate'] := 'Zaten en yeni sürüme sahipsiniz.';
  Translations.Values['tr.Start'] := 'En yeni sürümü yükle';
  Translations.Values['tr.Open'] := 'DownTrack’i aç';
  Translations.Values['tr.Retry'] := 'Tekrar dene';
  Translations.Values['tr.Close'] := 'Kapat';
  Translations.Values['tr.Error'] := 'Yükleme tamamlanamadı.';
  Translations.Values['tr.Progress'] := 'Bu yalnızca birkaç dakika sürecek.';
  Translations.Values['tr.CloseDownTrack'] := 'DownTrack’i kapatıp tekrar deneyin.';

  Translations.Values['uk.Title'] := 'DownTrack';
  Translations.Values['uk.Tagline'] := 'Ваші медіа. Ваша бібліотека. Завжди актуальні.';
  Translations.Values['uk.Language'] := 'Мова';
  Translations.Values['uk.Automatic'] := 'Автоматично (Windows)';
  Translations.Values['uk.Checking'] := 'Перевірка останньої версії…';
  Translations.Values['uk.Latest'] := 'Остання версія: %1';
  Translations.Values['uk.Downloading'] := 'Завантаження DownTrack…';
  Translations.Values['uk.Verifying'] := 'Перевірка завантаження…';
  Translations.Values['uk.Installing'] := 'Встановлення DownTrack…';
  Translations.Values['uk.Complete'] := 'DownTrack готовий.';
  Translations.Values['uk.UpToDate'] := 'У вас уже остання версія.';
  Translations.Values['uk.Start'] := 'Встановити останню версію';
  Translations.Values['uk.Open'] := 'Відкрити DownTrack';
  Translations.Values['uk.Retry'] := 'Спробувати ще раз';
  Translations.Values['uk.Close'] := 'Закрити';
  Translations.Values['uk.Error'] := 'Не вдалося завершити встановлення.';
  Translations.Values['uk.Progress'] := 'Це займе лише кілька хвилин.';
  Translations.Values['uk.CloseDownTrack'] := 'Закрийте DownTrack і спробуйте ще раз.';

  Translations.Values['ru.Title'] := 'DownTrack';
  Translations.Values['ru.Tagline'] := 'Ваши медиа. Ваша библиотека. Всегда актуальны.';
  Translations.Values['ru.Language'] := 'Язык';
  Translations.Values['ru.Automatic'] := 'Автоматически (Windows)';
  Translations.Values['ru.Checking'] := 'Проверка последней версии…';
  Translations.Values['ru.Latest'] := 'Последняя версия: %1';
  Translations.Values['ru.Downloading'] := 'Загрузка DownTrack…';
  Translations.Values['ru.Verifying'] := 'Проверка загрузки…';
  Translations.Values['ru.Installing'] := 'Установка DownTrack…';
  Translations.Values['ru.Complete'] := 'DownTrack готов.';
  Translations.Values['ru.UpToDate'] := 'У вас уже установлена последняя версия.';
  Translations.Values['ru.Start'] := 'Установить последнюю версию';
  Translations.Values['ru.Open'] := 'Открыть DownTrack';
  Translations.Values['ru.Retry'] := 'Повторить';
  Translations.Values['ru.Close'] := 'Закрыть';
  Translations.Values['ru.Error'] := 'Не удалось завершить установку.';
  Translations.Values['ru.Progress'] := 'Это займет всего несколько минут.';
  Translations.Values['ru.CloseDownTrack'] := 'Закройте DownTrack и попробуйте снова.';

  Translations.Values['ar.Title'] := 'DownTrack';
  Translations.Values['ar.Tagline'] := 'وسائطك. مكتبتك. محدثة دائمًا.';
  Translations.Values['ar.Language'] := 'اللغة';
  Translations.Values['ar.Automatic'] := 'تلقائي (Windows)';
  Translations.Values['ar.Checking'] := 'جارٍ التحقق من أحدث إصدار…';
  Translations.Values['ar.Latest'] := 'أحدث إصدار: %1';
  Translations.Values['ar.Downloading'] := 'جارٍ تنزيل DownTrack…';
  Translations.Values['ar.Verifying'] := 'جارٍ التحقق من التنزيل…';
  Translations.Values['ar.Installing'] := 'جارٍ تثبيت DownTrack…';
  Translations.Values['ar.Complete'] := 'DownTrack جاهز.';
  Translations.Values['ar.UpToDate'] := 'لديك بالفعل أحدث إصدار.';
  Translations.Values['ar.Start'] := 'تثبيت أحدث إصدار';
  Translations.Values['ar.Open'] := 'فتح DownTrack';
  Translations.Values['ar.Retry'] := 'حاول مرة أخرى';
  Translations.Values['ar.Close'] := 'إغلاق';
  Translations.Values['ar.Error'] := 'تعذر إكمال التثبيت.';
  Translations.Values['ar.Progress'] := 'لن يستغرق ذلك سوى بضع لحظات.';
  Translations.Values['ar.CloseDownTrack'] := 'أغلق DownTrack وحاول مرة أخرى.';

  Translations.Values['el.Title'] := 'DownTrack';
  Translations.Values['el.Tagline'] := 'Τα πολυμέσα σας. Η βιβλιοθήκη σας. Πάντα ενημερωμένα.';
  Translations.Values['el.Language'] := 'Γλώσσα';
  Translations.Values['el.Automatic'] := 'Αυτόματα (Windows)';
  Translations.Values['el.Checking'] := 'Έλεγχος της πιο πρόσφατης έκδοσης…';
  Translations.Values['el.Latest'] := 'Τελευταία έκδοση: %1';
  Translations.Values['el.Downloading'] := 'Λήψη DownTrack…';
  Translations.Values['el.Verifying'] := 'Επαλήθευση λήψης…';
  Translations.Values['el.Installing'] := 'Εγκατάσταση DownTrack…';
  Translations.Values['el.Complete'] := 'Το DownTrack είναι έτοιμο.';
  Translations.Values['el.UpToDate'] := 'Έχετε ήδη την πιο πρόσφατη έκδοση.';
  Translations.Values['el.Start'] := 'Εγκατάσταση τελευταίας έκδοσης';
  Translations.Values['el.Open'] := 'Άνοιγμα DownTrack';
  Translations.Values['el.Retry'] := 'Δοκιμή ξανά';
  Translations.Values['el.Close'] := 'Κλείσιμο';
  Translations.Values['el.Error'] := 'Δεν ήταν δυνατή η ολοκλήρωση της εγκατάστασης.';
  Translations.Values['el.Progress'] := 'Θα χρειαστούν μόνο λίγα λεπτά.';
  Translations.Values['el.CloseDownTrack'] := 'Κλείστε το DownTrack και δοκιμάστε ξανά.';

  Translations.Values['ro.Title'] := 'DownTrack';
  Translations.Values['ro.Tagline'] := 'Media ta. Biblioteca ta. Mereu actualizată.';
  Translations.Values['ro.Language'] := 'Limbă';
  Translations.Values['ro.Automatic'] := 'Automat (Windows)';
  Translations.Values['ro.Checking'] := 'Se verifică cea mai recentă versiune…';
  Translations.Values['ro.Latest'] := 'Cea mai recentă versiune: %1';
  Translations.Values['ro.Downloading'] := 'Se descarcă DownTrack…';
  Translations.Values['ro.Verifying'] := 'Se verifică descărcarea…';
  Translations.Values['ro.Installing'] := 'Se instalează DownTrack…';
  Translations.Values['ro.Complete'] := 'DownTrack este gata.';
  Translations.Values['ro.UpToDate'] := 'Aveți deja cea mai recentă versiune.';
  Translations.Values['ro.Start'] := 'Instalează cea mai recentă versiune';
  Translations.Values['ro.Open'] := 'Deschide DownTrack';
  Translations.Values['ro.Retry'] := 'Încearcă din nou';
  Translations.Values['ro.Close'] := 'Închide';
  Translations.Values['ro.Error'] := 'Instalarea nu a putut fi finalizată.';
  Translations.Values['ro.Progress'] := 'Va dura doar câteva momente.';
  Translations.Values['ro.CloseDownTrack'] := 'Închide DownTrack și încearcă din nou.';

  Translations.Values['ja.Title'] := 'DownTrack';
  Translations.Values['ja.Tagline'] := 'あなたのメディア。あなたのライブラリ。いつでも最新。';
  Translations.Values['ja.Language'] := '言語';
  Translations.Values['ja.Automatic'] := '自動（Windows）';
  Translations.Values['ja.Checking'] := '最新バージョンを確認しています…';
  Translations.Values['ja.Latest'] := '最新バージョン: %1';
  Translations.Values['ja.Downloading'] := 'DownTrackをダウンロードしています…';
  Translations.Values['ja.Verifying'] := 'ダウンロードを確認しています…';
  Translations.Values['ja.Installing'] := 'DownTrackをインストールしています…';
  Translations.Values['ja.Complete'] := 'DownTrackの準備ができました。';
  Translations.Values['ja.UpToDate'] := 'すでに最新バージョンです。';
  Translations.Values['ja.Start'] := '最新バージョンをインストール';
  Translations.Values['ja.Open'] := 'DownTrackを開く';
  Translations.Values['ja.Retry'] := 'もう一度試す';
  Translations.Values['ja.Close'] := '閉じる';
  Translations.Values['ja.Error'] := 'インストールを完了できませんでした。';
  Translations.Values['ja.Progress'] := '完了まであと少しです。';
  Translations.Values['ja.CloseDownTrack'] := 'DownTrackを閉じて、もう一度お試しください。';

  Translations.Values['ko.Title'] := 'DownTrack';
  Translations.Values['ko.Tagline'] := '내 미디어. 내 라이브러리. 항상 최신 상태.';
  Translations.Values['ko.Language'] := '언어';
  Translations.Values['ko.Automatic'] := '자동 (Windows)';
  Translations.Values['ko.Checking'] := '최신 버전을 확인하는 중…';
  Translations.Values['ko.Latest'] := '최신 버전: %1';
  Translations.Values['ko.Downloading'] := 'DownTrack을 다운로드하는 중…';
  Translations.Values['ko.Verifying'] := '다운로드를 확인하는 중…';
  Translations.Values['ko.Installing'] := 'DownTrack을 설치하는 중…';
  Translations.Values['ko.Complete'] := 'DownTrack을 사용할 준비가 되었습니다.';
  Translations.Values['ko.UpToDate'] := '이미 최신 버전이 설치되어 있습니다.';
  Translations.Values['ko.Start'] := '최신 버전 설치';
  Translations.Values['ko.Open'] := 'DownTrack 열기';
  Translations.Values['ko.Retry'] := '다시 시도';
  Translations.Values['ko.Close'] := '닫기';
  Translations.Values['ko.Error'] := '설치를 완료하지 못했습니다.';
  Translations.Values['ko.Progress'] := '잠시만 기다려 주세요.';
  Translations.Values['ko.CloseDownTrack'] := 'DownTrack을 닫고 다시 시도하세요.';

  Translations.Values['zhcn.Title'] := 'DownTrack';
  Translations.Values['zhcn.Tagline'] := '你的媒体。你的媒体库。始终保持最新。';
  Translations.Values['zhcn.Language'] := '语言';
  Translations.Values['zhcn.Automatic'] := '自动（Windows）';
  Translations.Values['zhcn.Checking'] := '正在检查最新版本…';
  Translations.Values['zhcn.Latest'] := '最新版本：%1';
  Translations.Values['zhcn.Downloading'] := '正在下载 DownTrack…';
  Translations.Values['zhcn.Verifying'] := '正在验证下载…';
  Translations.Values['zhcn.Installing'] := '正在安装 DownTrack…';
  Translations.Values['zhcn.Complete'] := 'DownTrack 已准备就绪。';
  Translations.Values['zhcn.UpToDate'] := '你已经拥有最新版本。';
  Translations.Values['zhcn.Start'] := '安装最新版本';
  Translations.Values['zhcn.Open'] := '打开 DownTrack';
  Translations.Values['zhcn.Retry'] := '重试';
  Translations.Values['zhcn.Close'] := '关闭';
  Translations.Values['zhcn.Error'] := '无法完成安装。';
  Translations.Values['zhcn.Progress'] := '只需等待片刻。';
  Translations.Values['zhcn.CloseDownTrack'] := '请关闭 DownTrack 后重试。';

  Translations.Values['zhtw.Title'] := 'DownTrack';
  Translations.Values['zhtw.Tagline'] := '你的媒體。你的資料庫。永遠保持最新。';
  Translations.Values['zhtw.Language'] := '語言';
  Translations.Values['zhtw.Automatic'] := '自動（Windows）';
  Translations.Values['zhtw.Checking'] := '正在檢查最新版本…';
  Translations.Values['zhtw.Latest'] := '最新版本：%1';
  Translations.Values['zhtw.Downloading'] := '正在下載 DownTrack…';
  Translations.Values['zhtw.Verifying'] := '正在驗證下載…';
  Translations.Values['zhtw.Installing'] := '正在安裝 DownTrack…';
  Translations.Values['zhtw.Complete'] := 'DownTrack 已準備就緒。';
  Translations.Values['zhtw.UpToDate'] := '你已經擁有最新版本。';
  Translations.Values['zhtw.Start'] := '安裝最新版本';
  Translations.Values['zhtw.Open'] := '開啟 DownTrack';
  Translations.Values['zhtw.Retry'] := '重試';
  Translations.Values['zhtw.Close'] := '關閉';
  Translations.Values['zhtw.Error'] := '無法完成安裝。';
  Translations.Values['zhtw.Progress'] := '只需要稍候片刻。';
  Translations.Values['zhtw.CloseDownTrack'] := '請關閉 DownTrack 後再試一次。';
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
