using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace DownTrack.Infrastructure.Localization;

public sealed record SupportedLanguage(
    string Code,
    string NativeName,
    string EnglishName,
    bool RightToLeft = false);

public sealed class LocalizationService : System.ComponentModel.INotifyPropertyChanged
{
    private const string AutoCode = "auto";

    private static readonly IReadOnlyList<SupportedLanguage> _languages =
    [
        new("auto", "Automatic (Windows)", "Automatic (Windows)"),
        new("en", "English", "English"),
        new("he", "עברית", "Hebrew", true),
        new("es", "Español", "Spanish"),
        new("fr", "Français", "French"),
        new("de", "Deutsch", "German"),
        new("it", "Italiano", "Italian"),
        new("pt", "Português", "Portuguese"),
        new("nl", "Nederlands", "Dutch"),
        new("pl", "Polski", "Polish"),
        new("cs", "Čeština", "Czech"),
        new("tr", "Türkçe", "Turkish"),
        new("uk", "Українська", "Ukrainian"),
        new("ru", "Русский", "Russian"),
        new("ar", "العربية", "Arabic", true),
        new("el", "Ελληνικά", "Greek"),
        new("ro", "Română", "Romanian"),
        new("ja", "日本語", "Japanese"),
        new("ko", "한국어", "Korean"),
        new("zh-Hans", "简体中文", "Chinese (Simplified)"),
        new("zh-Hant", "繁體中文", "Chinese (Traditional)")
    ];

    private static readonly Dictionary<string, Dictionary<string, string>> _translations =
        BuildTranslations();

    private static readonly string _settingsFile =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DownTrack",
            "language.txt");

    private readonly CultureInfo _windowsUiCulture = CultureInfo.CurrentUICulture;
    private string _selectedCode = AutoCode;
    private string _activeCode = "en";

    private LocalizationService()
    {
    }

    public static LocalizationService Instance { get; } = new();

    public static IReadOnlyList<SupportedLanguage> SupportedLanguages => _languages;

    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

    public string SelectedCode
    {
        get => _selectedCode;
        private set
        {
            if (_selectedCode == value)
                return;

            _selectedCode = value;
            PropertyChanged?.Invoke(
                this,
                new(nameof(SelectedCode)));
        }
    }

    public string ActiveCode
    {
        get => _activeCode;
        private set
        {
            if (_activeCode == value)
                return;

            _activeCode = value;
            PropertyChanged?.Invoke(
                this,
                new(nameof(ActiveCode)));
        }
    }

    public FlowDirection FlowDirection =>
        IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

    public bool IsRightToLeft =>
        _languages.FirstOrDefault(x => x.Code == ActiveCode)?.RightToLeft == true;

    public string this[string key] =>
        _translations.TryGetValue(ActiveCode, out var language) &&
        language.TryGetValue(key, out var translated)
            ? translated
            : _translations["en"].TryGetValue(key, out var fallback)
                ? fallback
                : key;

    public void Initialize()
    {
        var stored = ReadStoredSelection();
        SelectedCode = string.IsNullOrWhiteSpace(stored) ? AutoCode : stored;
        ActiveCode = ResolveLanguageCode(SelectedCode == AutoCode
            ? _windowsUiCulture
            : CreateCultureSafely(SelectedCode));

        ApplyCulture(ActiveCode);
    }

    public void SetLanguage(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            code = AutoCode;

        SelectedCode = code;
        ActiveCode = ResolveLanguageCode(
            code == AutoCode
                ? _windowsUiCulture
                : CreateCultureSafely(code));

        ApplyCulture(ActiveCode);
        SaveStoredSelection(SelectedCode);

        PropertyChanged?.Invoke(
            this,
            new("Item[]"));
        PropertyChanged?.Invoke(
            this,
            new(nameof(FlowDirection)));
        PropertyChanged?.Invoke(
            this,
            new(nameof(IsRightToLeft)));
    }

    public string GetLanguageDisplayName(string code)
    {
        return _languages.FirstOrDefault(x => x.Code == code)?.NativeName
            ?? code;
    }

    public string T(string key, params object[] args)
    {
        var value = this[key];
        return args.Length == 0 ? value : string.Format(value, args);
    }

    private static string ResolveLanguageCode(CultureInfo culture)
    {
        var name = culture.Name;

        if (name.StartsWith("zh-TW", StringComparison.OrdinalIgnoreCase) ||
            name.StartsWith("zh-HK", StringComparison.OrdinalIgnoreCase) ||
            name.StartsWith("zh-MO", StringComparison.OrdinalIgnoreCase) ||
            name.StartsWith("zh-Hant", StringComparison.OrdinalIgnoreCase))
            return "zh-Hant";

        if (name.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            return "zh-Hans";

        var exact = _languages.FirstOrDefault(
            x => x.Code.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (exact is not null)
            return exact.Code;

        var neutral = culture.TwoLetterISOLanguageName;
        var match = _languages.FirstOrDefault(
            x => x.Code.Equals(neutral, StringComparison.OrdinalIgnoreCase));
        return match?.Code ?? "en";
    }

    private static CultureInfo CreateCultureSafely(string code)
    {
        try
        {
            return code switch
            {
                "zh-Hans" => CultureInfo.GetCultureInfo("zh-CN"),
                "zh-Hant" => CultureInfo.GetCultureInfo("zh-TW"),
                "he" => CultureInfo.GetCultureInfo("he-IL"),
                "ar" => CultureInfo.GetCultureInfo("ar-SA"),
                "uk" => CultureInfo.GetCultureInfo("uk-UA"),
                "el" => CultureInfo.GetCultureInfo("el-GR"),
                "ro" => CultureInfo.GetCultureInfo("ro-RO"),
                _ => CultureInfo.GetCultureInfo(code)
            };
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.GetCultureInfo("en-US");
        }
    }

    private static void ApplyCulture(string code)
    {
        var cultureName = code switch
        {
            "zh-Hans" => "zh-CN",
            "zh-Hant" => "zh-TW",
            _ => code == "he" ? "he-IL" : code
        };

        try
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        catch (CultureNotFoundException)
        {
            var english = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentCulture = english;
            CultureInfo.CurrentUICulture = english;
            CultureInfo.DefaultThreadCurrentCulture = english;
            CultureInfo.DefaultThreadCurrentUICulture = english;
        }
    }

    private static string? ReadStoredSelection()
    {
        try
        {
            return File.Exists(_settingsFile)
                ? File.ReadAllText(_settingsFile).Trim()
                : null;
        }
        catch
        {
            return null;
        }
    }

    private static void SaveStoredSelection(string code)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFile)!);
            File.WriteAllText(_settingsFile, code);
        }
        catch
        {
            // Language preference is best-effort; app still runs.
        }
    }

    private static Dictionary<string, Dictionary<string, string>> BuildTranslations()
    {
        var en = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["App.Library"] = "Library",
            ["App.MediaEngine"] = "Media Engine",
            ["App.Settings"] = "Settings",
            ["Common.Cancel"] = "Cancel",
            ["Common.Delete"] = "Delete",
            ["Common.Continue"] = "Continue",
            ["Home.LibraryEyebrow"] = "LIBRARY",
            ["Home.SetupRequired"] = "SETUP REQUIRED",
            ["AddMedia.UrlHint"] = "YouTube video or playlist URL",
            ["AddMedia.OpenError"] = "DownTrack could not open the Add Media window.\n\n{0}",
            ["Media.Untitled"] = "Untitled",
            ["Download.PreparingEngine"] = "Preparing media engine…",
            ["Download.Downloading"] = "Downloading {0}…",
            ["Engine.DownloadingYtDlp"] = "Downloading yt-dlp…",
            ["Engine.DownloadingFfmpeg"] = "Downloading FFmpeg…",
            ["Engine.InstallingRuntime"] = "Installing YouTube runtime…",
            ["Engine.Ready"] = "Media engine ready.",
            ["Engine.VerifyingFfmpeg"] = "Verifying FFmpeg package…",
            ["Error.MissingTargetPath"] = "Missing target path.",
            ["Error.MissingMedia"] = "Download change is missing media information.",
            ["Error.IncompleteRename"] = "Rename change is incomplete.",
            ["Error.TargetExists"] = "Target already exists: {0}",
            ["Error.IncompleteDelete"] = "Delete change is incomplete.",
            ["Error.InvalidDestination"] = "The destination folder is invalid.",
            ["Error.DownloadedFileEmpty"] = "The downloaded file is empty.",
            ["Error.MetadataMissing"] = "The media resolver did not return metadata.",
            ["Engine.HashMissingFile"] = "No SHA-256 checksum was found for {0}.",
            ["Engine.HashFailedFile"] = "Checksum verification failed for {0}.",
            ["Engine.HashMissingFfmpeg"] = "No SHA-256 checksum was found for the FFmpeg package.",
            ["Engine.HashFailedFfmpeg"] = "Checksum verification failed for the FFmpeg package.",
            ["Engine.FfmpegMissingExecutables"] = "The FFmpeg package did not contain the expected executables.",
            ["Engine.HashMissingDeno"] = "No SHA-256 checksum was found for Deno.",
            ["Engine.HashFailedDeno"] = "Checksum verification failed for Deno.",
            ["Engine.DenoMissingExecutable"] = "The Deno package did not contain deno.exe.",
            ["Window.Minimize"] = "Minimize",
            ["Window.Maximize"] = "Maximize / restore",
            ["Window.Close"] = "Close",

            ["Home.Badge"] = "PERSONAL MEDIA WORKSPACE",
            ["Home.Title"] = "Your library, organized your way.",
            ["Home.Subtitle"] = "Keep folders tidy, stage changes safely, and bring YouTube media exactly where you want it.",
            ["Home.AddRootFolder"] = "Add root folder",
            ["Home.SaveNote"] = "Nothing changes on disk until you save.",
            ["Home.RootFolders"] = "ROOT FOLDERS",
            ["Home.Library"] = "LIBRARY",
            ["Home.LibrarySubtitle"] = "{0} folders connected",
            ["Home.Engine"] = "MEDIA ENGINE",
            ["Home.EngineReady"] = "Media engine ready",
            ["Home.Workflow"] = "WORKFLOW",
            ["Home.WorkflowTitle"] = "Stage → review → save",
            ["Home.WorkflowSubtitle"] = "Your library only changes when you commit.",
            ["Home.YourFolders"] = "Your folders",
            ["Home.FoldersSubtitle"] = "Open a folder to work inside the DownTrack Explorer.",
            ["Home.AddFolder"] = "Add folder",
            ["Home.Connected"] = "CONNECTED",
            ["Home.OpenExplorer"] = "Open Explorer",
            ["Home.StartTitle"] = "Start with your first folder",
            ["Home.StartSubtitle"] = "Choose a folder that DownTrack should manage. Everything else starts from there.",
            ["Home.ChooseFolder"] = "Choose folder",
            ["Home.MediaFooter"] = "Media engine ready",
            ["Home.Setup"] = "Setup",
            ["Home.EngineNeedsSetup"] = "Media engine setup required",
            ["Home.SettingUpEngine"] = "Setting up the media engine…",

            ["Explorer.SaveChangesArrow"] = "Save changes  →",
            ["Explorer.SaveChangesCount"] = "Save changes ({0})  →",
            ["Explorer.QueuedOne"] = "1 queued",
            ["Explorer.Queued"] = "{0} queued",
            ["Explorer.NewFolderPrompt"] = "Choose a name for the new folder.",
            ["Explorer.NewFolderDefault"] = "New Folder",
            ["Explorer.RenamePrompt"] = "Enter the new name.",
            ["Explorer.PendingCreate"] = "Pending: create “{0}”.",
            ["Explorer.PendingRename"] = "Pending: rename “{0}”.",
            ["Explorer.DeleteFolderPrompt"] = "Delete folder “{0}” from the staged plan?",
            ["Explorer.DeleteFilePrompt"] = "Delete “{0}” from the staged plan?",
            ["Explorer.ConfirmDelete"] = "Confirm delete",
            ["Explorer.PendingDelete"] = "Pending: delete “{0}”.",
            ["Explorer.MediaAddedOne"] = "Media added to the pending queue.",
            ["Explorer.MediaAddedMany"] = "{0} media items added to the pending queue.",
            ["Explorer.ApplyingChanges"] = "Applying staged changes…",
            ["Explorer.SomeNeedAttention"] = "Some changes need attention.",
            ["Explorer.Cancelled"] = "Cancelled: {0}",

            ["Explorer.Badge"] = "LIBRARY / EXPLORER",
            ["Explorer.Back"] = "Back",
            ["Explorer.Forward"] = "Forward",
            ["Explorer.Home"] = "Home",
            ["Explorer.NewFolder"] = "New folder",
            ["Explorer.Rename"] = "Rename",
            ["Explorer.Delete"] = "Delete",
            ["Explorer.AddMedia"] = "Add media",
            ["Explorer.Staged"] = "Changes are staged safely",
            ["Explorer.Folders"] = "FOLDERS",
            ["Explorer.Name"] = "NAME",
            ["Explorer.Type"] = "TYPE",
            ["Explorer.State"] = "STATE",
            ["Explorer.EmptyTitle"] = "This folder is empty",
            ["Explorer.EmptySubtitle"] = "Create a folder or add media. New items will appear here immediately as staged changes.",
            ["Explorer.PendingTitle"] = "Pending changes",
            ["Explorer.PendingSubtitle"] = "These changes are still virtual.",
            ["Explorer.NonePending"] = "No pending changes",
            ["Explorer.Cancel"] = "Cancel",
            ["Explorer.SaveChanges"] = "Save changes",
            ["Explorer.AllSaved"] = "All changes are saved.",

            ["AddMedia.Title"] = "Add media",
            ["AddMedia.Subtitle"] = "Analyze first. Review every item. Stage only what you want.",
            ["AddMedia.Analyze"] = "Analyze",
            ["AddMedia.SelectAll"] = "Select all",
            ["AddMedia.Mp3All"] = "MP3 to all",
            ["AddMedia.Format"] = "FORMAT",
            ["AddMedia.AudioQuality"] = "AUDIO QUALITY",
            ["AddMedia.VideoQuality"] = "VIDEO QUALITY",
            ["AddMedia.NoMedia"] = "Paste a YouTube link",
            ["AddMedia.NoMediaSubtitle"] = "The selected items will become pending changes. Nothing is downloaded here.",
            ["AddMedia.Preparing"] = "Preparing media…",
            ["AddMedia.PreparingSubtitle"] = "Checking the media engine and analyzing the link.",
            ["AddMedia.PastePrompt"] = "Paste a YouTube video or playlist URL.",
            ["AddMedia.NothingFound"] = "Nothing was found. Check the link and try again.",
            ["AddMedia.OneReady"] = "1 media item ready. Edit the options before adding.",
            ["AddMedia.ManyReady"] = "{0} media items ready. Each row is independent.",
            ["AddMedia.AnalysisCancelled"] = "Analysis cancelled.",
            ["AddMedia.ErrorFallback"] = "We couldn't analyze this link. Please try again.",
            ["AddMedia.Destination"] = "DESTINATION",
            ["AddMedia.Stage"] = "Stage selected",
            ["AddMedia.Cancel"] = "Cancel",

            ["Settings.Title"] = "Settings",
            ["Settings.Language"] = "Language",
            ["Settings.LanguageDescription"] = "Choose the interface language. Automatic follows your Windows display language.",
            ["Settings.Automatic"] = "Automatic (Windows)",
            ["Settings.Apply"] = "Apply",
            ["Settings.Done"] = "Done",
            ["Settings.Current"] = "Current language",
            ["Settings.RestartNotNeeded"] = "Changes apply immediately.",

        };

        var dictionaries = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = en
        };

        AddLanguage(dictionaries, "he", en, new()
        {
            ["App.Library"]="ספרייה",["App.MediaEngine"]="מנוע מדיה",["App.Settings"]="הגדרות",
            ["Home.Badge"]="סביבת המדיה האישית",["Home.Title"]="הספרייה שלך, בדיוק בדרך שלך.",
            ["Home.Subtitle"]="שמור על התיקיות מסודרות, תכנן שינויים בבטחה והכנס מדיה מ־YouTube בדיוק למקום הרצוי.",
            ["Home.AddRootFolder"]="הוסף תיקיית שורש",["Home.SaveNote"]="שום דבר לא משתנה בדיסק עד ששומרים.",
            ["Home.RootFolders"]="תיקיות שורש",["Home.Library"]="ספרייה",["Home.Engine"]="מנוע מדיה",
            ["Home.EngineReady"]="מנוע המדיה מוכן",["Home.Workflow"]="תהליך עבודה",["Home.WorkflowTitle"]="תכנן → בדוק → שמור",
            ["Home.WorkflowSubtitle"]="הספרייה משתנה רק לאחר שמירת השינויים.",["Home.YourFolders"]="התיקיות שלך",
            ["Home.FoldersSubtitle"]="פתח תיקייה ועבוד בתוכה בסייר DownTrack.",["Home.AddFolder"]="הוסף תיקייה",
            ["Home.Connected"]="מחובר",["Home.OpenExplorer"]="פתח סייר",["Home.StartTitle"]="מתחילים מהתיקייה הראשונה",
            ["Home.StartSubtitle"]="בחר תיקייה ש־DownTrack ינהל. מכאן הכול מתחיל.",["Home.ChooseFolder"]="בחר תיקייה",
            ["Home.MediaFooter"]="מנוע המדיה מוכן",["Home.Setup"]="הגדרה",
            ["Explorer.Badge"]="ספרייה / סייר",["Explorer.Back"]="חזרה",["Explorer.Forward"]="קדימה",
            ["Explorer.Home"]="בית",["Explorer.NewFolder"]="תיקייה חדשה",["Explorer.Rename"]="שנה שם",
            ["Explorer.Delete"]="מחק",["Explorer.AddMedia"]="הוסף מדיה",["Explorer.Staged"]="השינויים מוכנים לשמירה",
            ["Explorer.Folders"]="תיקיות",["Explorer.Name"]="שם",["Explorer.Type"]="סוג",["Explorer.State"]="מצב",
            ["Explorer.EmptyTitle"]="התיקייה ריקה",["Explorer.EmptySubtitle"]="צור תיקייה או הוסף מדיה. פריטים חדשים יופיעו כאן מיד כשינויים ממתינים.",
            ["Explorer.PendingTitle"]="שינויים ממתינים",["Explorer.PendingSubtitle"]="השינויים האלה עדיין וירטואליים.",
            ["Explorer.Queued"]="{0} בתור",["Explorer.NonePending"]="אין שינויים ממתינים",["Explorer.Cancel"]="ביטול",
            ["Explorer.SaveChanges"]="שמור שינויים",["Explorer.AllSaved"]="כל השינויים נשמרו.",
            ["AddMedia.Title"]="הוסף מדיה",["AddMedia.Subtitle"]="נתח קודם. בדוק כל פריט. שמור רק את מה שאתה רוצה.",
            ["AddMedia.Analyze"]="נתח",["AddMedia.SelectAll"]="בחר הכול",["AddMedia.Mp3All"]="MP3 לכולם",
            ["AddMedia.Format"]="פורמט",["AddMedia.AudioQuality"]="איכות שמע",["AddMedia.VideoQuality"]="איכות וידאו",
            ["AddMedia.NoMedia"]="הדבק קישור ל־YouTube",["AddMedia.NoMediaSubtitle"]="הפריטים שנבחרו יהפכו לשינויים ממתינים. שום דבר לא יורד כאן.",
            ["AddMedia.Preparing"]="מכין מדיה…",["AddMedia.PreparingSubtitle"]="בודק את מנוע המדיה ומנתח את הקישור.",
            ["AddMedia.Destination"]="יעד",["AddMedia.Stage"]="העבר להמתנה",["AddMedia.Cancel"]="ביטול",
            ["Settings.Title"]="הגדרות",["Settings.Language"]="שפה",
            ["Settings.LanguageDescription"]="בחר את שפת הממשק. אוטומטי עוקב אחר שפת התצוגה של Windows.",
            ["Settings.Automatic"]="אוטומטי (Windows)",["Settings.Apply"]="החל",["Settings.Done"]="סיום",
            ["Settings.Current"]="השפה הנוכחית",["Settings.RestartNotNeeded"]="השינוי חל מיד."
        });

        var genericNames = new Dictionary<string, Dictionary<string,string>>();
        var cultureTranslations = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["es"]=["Biblioteca","Motor multimedia","Configuración","TUS CARPETAS","Agregar carpeta raíz","Nada cambia en el disco hasta que guardes.","Preparar → revisar → guardar","Agregar carpeta","Conectado","Abrir Explorer","Comienza con tu primera carpeta","Elegir carpeta","Cambios pendientes","Guardar cambios","Agregar multimedia","Analizar","Seleccionar todo","Cancelar"],
            ["fr"]=["Bibliothèque","Moteur multimédia","Paramètres","VOS DOSSIERS","Ajouter un dossier racine","Rien ne change sur le disque avant l’enregistrement.","Préparer → vérifier → enregistrer","Ajouter un dossier","Connecté","Ouvrir l’Explorateur","Commencez par votre premier dossier","Choisir un dossier","Modifications en attente","Enregistrer les modifications","Ajouter un média","Analyser","Tout sélectionner","Annuler"],
            ["de"]=["Bibliothek","Medien-Engine","Einstellungen","IHRE ORDNER","Stammordner hinzufügen","Nichts wird auf dem Datenträger geändert, bis Sie speichern.","Vorbereiten → prüfen → speichern","Ordner hinzufügen","Verbunden","Explorer öffnen","Beginnen Sie mit Ihrem ersten Ordner","Ordner auswählen","Ausstehende Änderungen","Änderungen speichern","Medien hinzufügen","Analysieren","Alle auswählen","Abbrechen"],
            ["it"]=["Libreria","Motore multimediale","Impostazioni","LE TUE CARTELLE","Aggiungi cartella principale","Nulla cambia sul disco finché non salvi.","Prepara → controlla → salva","Aggiungi cartella","Connesso","Apri Esplora","Inizia con la tua prima cartella","Scegli cartella","Modifiche in sospeso","Salva modifiche","Aggiungi media","Analizza","Seleziona tutto","Annulla"],
            ["pt"]=["Biblioteca","Mecanismo de mídia","Configurações","SUAS PASTAS","Adicionar pasta raiz","Nada muda no disco até você salvar.","Preparar → revisar → salvar","Adicionar pasta","Conectado","Abrir Explorer","Comece com sua primeira pasta","Escolher pasta","Alterações pendentes","Salvar alterações","Adicionar mídia","Analisar","Selecionar tudo","Cancelar"],
            ["nl"]=["Bibliotheek","Media-engine","Instellingen","JE MAPPEN","Hoofdmap toevoegen","Er verandert niets op schijf totdat je opslaat.","Voorbereiden → controleren → opslaan","Map toevoegen","Verbonden","Explorer openen","Begin met je eerste map","Map kiezen","In afwachting","Wijzigingen opslaan","Media toevoegen","Analyseren","Alles selecteren","Annuleren"],
            ["pl"]=["Biblioteka","Silnik multimediów","Ustawienia","TWOJE FOLDERY","Dodaj folder główny","Nic nie zmieni się na dysku, dopóki nie zapiszesz.","Przygotuj → sprawdź → zapisz","Dodaj folder","Połączono","Otwórz Eksplorator","Zacznij od pierwszego folderu","Wybierz folder","Oczekujące zmiany","Zapisz zmiany","Dodaj multimedia","Analizuj","Zaznacz wszystko","Anuluj"],
            ["cs"]=["Knihovna","Mediální engine","Nastavení","VAŠE SLOŽKY","Přidat kořenovou složku","Na disku se nic nezmění, dokud neuložíte.","Připravit → zkontrolovat → uložit","Přidat složku","Připojeno","Otevřít Průzkumník","Začněte první složkou","Vybrat složku","Čekající změny","Uložit změny","Přidat média","Analyzovat","Vybrat vše","Zrušit"],
            ["tr"]=["Kitaplık","Medya motoru","Ayarlar","KLASÖRLERİNİZ","Kök klasör ekle","Kaydetmeden diskte hiçbir şey değişmez.","Hazırla → incele → kaydet","Klasör ekle","Bağlı","Gezgini aç","İlk klasörünüzle başlayın","Klasör seç","Bekleyen değişiklikler","Değişiklikleri kaydet","Medya ekle","Analiz et","Tümünü seç","İptal"],
            ["uk"]=["Бібліотека","Медіадвигун","Налаштування","ВАШІ ПАПКИ","Додати кореневу папку","На диску нічого не зміниться, доки ви не збережете.","Підготувати → перевірити → зберегти","Додати папку","Підключено","Відкрити Провідник","Почніть із першої папки","Вибрати папку","Очікувані зміни","Зберегти зміни","Додати медіа","Аналізувати","Вибрати все","Скасувати"],
            ["ru"]=["Библиотека","Медиа-движок","Настройки","ВАШИ ПАПКИ","Добавить корневую папку","На диске ничего не изменится, пока вы не сохраните.","Подготовить → проверить → сохранить","Добавить папку","Подключено","Открыть проводник","Начните с первой папки","Выбрать папку","Ожидающие изменения","Сохранить изменения","Добавить медиа","Анализировать","Выбрать всё","Отмена"],
            ["ar"]=["المكتبة","محرك الوسائط","الإعدادات","مجلداتك","إضافة مجلد جذر","لن يتغير شيء على القرص حتى تحفظ.","تجهيز ← مراجعة ← حفظ","إضافة مجلد","متصل","فتح المستكشف","ابدأ بالمجلد الأول","اختيار مجلد","التغييرات المعلقة","حفظ التغييرات","إضافة وسائط","تحليل","تحديد الكل","إلغاء"],
            ["el"]=["Βιβλιοθήκη","Μηχανή πολυμέσων","Ρυθμίσεις","ΟΙ ΦΑΚΕΛΟΙ ΣΑΣ","Προσθήκη βασικού φακέλου","Τίποτα δεν αλλάζει στον δίσκο μέχρι να αποθηκεύσετε.","Προετοιμασία → έλεγχος → αποθήκευση","Προσθήκη φακέλου","Συνδεδεμένο","Άνοιγμα Explorer","Ξεκινήστε με τον πρώτο σας φάκελο","Επιλογή φακέλου","Εκκρεμείς αλλαγές","Αποθήκευση αλλαγών","Προσθήκη πολυμέσων","Ανάλυση","Επιλογή όλων","Άκυρο"],
            ["ro"]=["Bibliotecă","Motor media","Setări","DOSARELE TALE","Adaugă dosar rădăcină","Nimic nu se schimbă pe disc până nu salvezi.","Pregătește → verifică → salvează","Adaugă folder","Conectat","Deschide Explorer","Începe cu primul tău folder","Alege folderul","Modificări în așteptare","Salvează modificările","Adaugă media","Analizează","Selectează tot","Anulează"],
            ["ja"]=["ライブラリ","メディアエンジン","設定","フォルダー","ルートフォルダーを追加","保存するまでディスク上は変更されません。","準備 → 確認 → 保存","フォルダーを追加","接続済み","Explorerを開く","最初のフォルダーから始める","フォルダーを選択","保留中の変更","変更を保存","メディアを追加","解析","すべて選択","キャンセル"],
            ["ko"]=["라이브러리","미디어 엔진","설정","폴더","루트 폴더 추가","저장하기 전에는 디스크가 변경되지 않습니다.","준비 → 검토 → 저장","폴더 추가","연결됨","탐색기 열기","첫 번째 폴더로 시작하세요","폴더 선택","대기 중인 변경","변경 사항 저장","미디어 추가","분석","모두 선택","취소"],
            ["zh-Hans"]=["媒体库","媒体引擎","设置","你的文件夹","添加根文件夹","保存前不会更改磁盘。","准备 → 审核 → 保存","添加文件夹","已连接","打开资源管理器","从第一个文件夹开始","选择文件夹","待处理更改","保存更改","添加媒体","分析","全选","取消"],
            ["zh-Hant"]=["媒體庫","媒體引擎","設定","你的資料夾","新增根資料夾","儲存前不會變更磁碟。","準備 → 檢查 → 儲存","新增資料夾","已連線","開啟檔案總管","從第一個資料夾開始","選擇資料夾","待處理變更","儲存變更","新增媒體","分析","全選","取消"]
        };

        foreach (var (code, values) in cultureTranslations)
        {
            var d = new Dictionary<string,string>(en, StringComparer.OrdinalIgnoreCase);
            d["App.Library"]=values[0];
            d["App.MediaEngine"]=values[1];
            d["App.Settings"]=values[2];
            d["Home.YourFolders"]=values[3];
            d["Home.AddRootFolder"]=values[4];
            d["Home.SaveNote"]=values[5];
            d["Home.WorkflowTitle"]=values[6];
            d["Home.AddFolder"]=values[7];
            d["Home.Connected"]=values[8];
            d["Home.OpenExplorer"]=values[9];
            d["Home.StartTitle"]=values[10];
            d["Home.ChooseFolder"]=values[11];
            d["Explorer.PendingTitle"]=values[12];
            d["Explorer.SaveChanges"]=values[13];
            d["Explorer.AddMedia"]=values[14];
            d["AddMedia.Analyze"]=values[15];
            d["AddMedia.SelectAll"]=values[16];
            d["AddMedia.Cancel"]=values[17];
            dictionaries[code] = d;
        }

        CompleteTranslationCatalog.Apply(dictionaries);
        return dictionaries;
    }

    private static void AddLanguage(
        Dictionary<string, Dictionary<string, string>> destination,
        string code,
        Dictionary<string, string> fallback,
        Dictionary<string, string> overrides)
    {
        var d = new Dictionary<string, string>(fallback, StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in overrides)
            d[key] = value;
        destination[code] = d;
    }
}
