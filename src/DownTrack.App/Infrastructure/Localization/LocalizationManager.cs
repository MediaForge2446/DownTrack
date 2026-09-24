using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using DownTrack.Infrastructure;

namespace DownTrack.Infrastructure.Localization;

public sealed record LanguageOption(
    string Code,
    string NativeName,
    string EnglishName);

public sealed class LocalizationManager : INotifyPropertyChanged
{
    private const string AutoCode = "auto";
    private static readonly Lazy<LocalizationManager> LazyInstance = new(() => new LocalizationManager());

    private readonly Dictionary<string, Dictionary<string, string>> _translations;
    private string _selectedLanguageCode = AutoCode;
    private string _effectiveLanguageCode = "en";

    private LocalizationManager()
    {
        Languages = new ReadOnlyCollection<LanguageOption>(
        [
            new("auto", "Automatic", "Windows language"),
            new("en", "English", "English"),
            new("he", "עברית", "Hebrew"),
            new("ar", "العربية", "Arabic"),
            new("es", "Español", "Spanish"),
            new("fr", "Français", "French"),
            new("de", "Deutsch", "German"),
            new("it", "Italiano", "Italian"),
            new("pt", "Português", "Portuguese"),
            new("nl", "Nederlands", "Dutch"),
            new("pl", "Polski", "Polish"),
            new("tr", "Türkçe", "Turkish"),
            new("ru", "Русский", "Russian"),
            new("uk", "Українська", "Ukrainian"),
            new("ja", "日本語", "Japanese"),
            new("ko", "한국어", "Korean"),
            new("zh-CN", "简体中文", "Chinese Simplified"),
            new("zh-TW", "繁體中文", "Chinese Traditional"),
            new("cs", "Čeština", "Czech"),
            new("da", "Dansk", "Danish"),
            new("sv", "Svenska", "Swedish")
        ]);

        _translations = CreateTranslations();
    }

    public static LocalizationManager Instance => LazyInstance.Value;

    public ReadOnlyCollection<LanguageOption> Languages { get; }

    public string SelectedLanguageCode
    {
        get => _selectedLanguageCode;
        set => ApplyLanguage(value);
    }

    public string EffectiveLanguageCode => _effectiveLanguageCode;

    public bool IsRightToLeft =>
        _effectiveLanguageCode is "he" or "ar";

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Initialize()
    {
        var saved = string.Empty;

        try
        {
            if (File.Exists(AppPaths.LanguageFile))
                saved = File.ReadAllText(AppPaths.LanguageFile).Trim();
        }
        catch
        {
            // Fall back to automatic detection.
        }

        ApplyLanguage(
            string.IsNullOrWhiteSpace(saved)
                ? AutoCode
                : saved,
            persist: false);
    }

    public string this[string key] =>
        _translations.TryGetValue(_effectiveLanguageCode, out var language) &&
        language.TryGetValue(key, out var translated)
            ? translated
            : _translations["en"].GetValueOrDefault(key, key);

    public void SetLanguage(string code) => ApplyLanguage(code);

    private void ApplyLanguage(string? requested, bool persist = true)
    {
        var code = Languages.Any(x =>
            string.Equals(x.Code, requested, StringComparison.OrdinalIgnoreCase))
            ? requested!
            : AutoCode;

        var effective = ResolveEffectiveLanguage(code);

        if (string.Equals(_selectedLanguageCode, code, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(_effectiveLanguageCode, effective, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        _selectedLanguageCode = code;
        _effectiveLanguageCode = effective;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(effective);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(effective);
        }
        catch
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        if (persist && code != AutoCode)
        {
            Directory.CreateDirectory(AppPaths.RootDirectory);
            File.WriteAllText(AppPaths.LanguageFile, code);
        }
        else if (persist && code == AutoCode)
        {
            try
            {
                if (File.Exists(AppPaths.LanguageFile))
                    File.Delete(AppPaths.LanguageFile);
            }
            catch
            {
                // Preference will remain automatic for this session.
            }
        }

        OnPropertyChanged(nameof(SelectedLanguageCode));
        OnPropertyChanged(nameof(EffectiveLanguageCode));
        OnPropertyChanged(nameof(IsRightToLeft));
        OnPropertyChanged("Item[]");
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? LanguageChanged;

    private static string ResolveEffectiveLanguage(string requested)
    {
        if (!string.Equals(requested, AutoCode, StringComparison.OrdinalIgnoreCase))
            return NormalizeSupportedCode(requested);

        var candidates = new[]
        {
            CultureInfo.InstalledUICulture.Name,
            CultureInfo.CurrentUICulture.Name,
            CultureInfo.CurrentCulture.Name
        };

        foreach (var candidate in candidates)
        {
            var normalized = NormalizeSupportedCode(candidate);
            if (SupportedCodes.Contains(normalized, StringComparer.OrdinalIgnoreCase))
                return normalized;
        }

        return "en";
    }

    private static string NormalizeSupportedCode(string code)
    {
        if (code.StartsWith("zh-TW", StringComparison.OrdinalIgnoreCase) ||
            code.StartsWith("zh-HK", StringComparison.OrdinalIgnoreCase) ||
            code.StartsWith("zh-MO", StringComparison.OrdinalIgnoreCase))
            return "zh-TW";

        if (code.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            return "zh-CN";

        var dash = code.IndexOf('-');
        var prefix = dash > 0 ? code[..dash] : code;

        return SupportedCodes.Contains(prefix, StringComparer.OrdinalIgnoreCase)
            ? prefix.ToLowerInvariant()
            : "en";
    }

    private static readonly string[] SupportedCodes =
    [
        "en", "he", "ar", "es", "fr", "de", "it", "pt", "nl", "pl",
        "tr", "ru", "uk", "ja", "ko", "zh-CN", "zh-TW", "cs", "da", "sv"
    ];

    private static Dictionary<string, Dictionary<string, string>> CreateTranslations()
    {
        var en = new Dictionary<string, string>
        {
            ["App.Library"] = "Library",
            ["App.MediaEngine"] = "Media Engine",
            ["App.SetupRequired"] = "SETUP REQUIRED",
            ["App.Minimize"] = "Minimize",
            ["App.Maximize"] = "Maximize / restore",
            ["App.Close"] = "Close",
            ["App.Settings"] = "Settings",
            ["Home.Workspace"] = "PERSONAL MEDIA WORKSPACE",
            ["Home.Title"] = "Your library, organized your way.",
            ["Home.Subtitle"] = "Keep folders tidy, stage changes safely, and bring YouTube media exactly where you want it.",
            ["Home.AddRootFolder"] = "Add root folder",
            ["Home.NoDiskChanges"] = "Nothing changes on disk until you save.",
            ["Home.RootFolders"] = "ROOT FOLDERS",
            ["Home.Library"] = "LIBRARY",
            ["Home.RootFolderLabel"] = "Root folders",
            ["Home.FoldersConnected"] = "folders connected",
            ["Home.Workflow"] = "WORKFLOW",
            ["Home.StageReviewSave"] = "Stage → review → save",
            ["Home.CommitHint"] = "Your library only changes when you commit.",
            ["Home.YourFolders"] = "Your folders",
            ["Home.OpenExplorerHint"] = "Open a folder to work inside the DownTrack Explorer.",
            ["Home.AddFolder"] = "Add folder",
            ["Home.Connected"] = "CONNECTED",
            ["Home.OpenExplorer"] = "Open Explorer",
            ["Home.StartFirstFolder"] = "Start with your first folder",
            ["Home.StartFirstFolderHint"] = "Choose a folder that DownTrack should manage. Everything else starts from there.",
            ["Home.ChooseFolder"] = "Choose folder",
            ["Explorer.Title"] = "LIBRARY / EXPLORER",
            ["Explorer.Folders"] = "FOLDERS",
            ["Explorer.Back"] = "Back",
            ["Explorer.Forward"] = "Forward",
            ["Explorer.Home"] = "Home",
            ["Explorer.NewFolder"] = "New folder",
            ["Explorer.Rename"] = "Rename",
            ["Explorer.Delete"] = "Delete",
            ["Explorer.AddMedia"] = "Add media",
            ["Explorer.StagedSafely"] = "Changes are staged safely",
            ["Explorer.Name"] = "NAME",
            ["Explorer.Type"] = "TYPE",
            ["Explorer.State"] = "STATE",
            ["Explorer.EmptyTitle"] = "This folder is empty",
            ["Explorer.EmptyHint"] = "Create a folder or add media. New items will appear here immediately as staged changes.",
            ["Explorer.PendingChanges"] = "Pending changes",
            ["Explorer.PendingHint"] = "These changes are still virtual.",
            ["Explorer.Queued"] = "queued",
            ["Explorer.NoPending"] = "No pending changes",
            ["Explorer.Cancel"] = "Cancel",
            ["Explorer.SaveChanges"] = "Save changes",
            ["AddMedia.Title"] = "Add media",
            ["AddMedia.Subtitle"] = "Analyze first. Review every item. Stage only what you want.",
            ["AddMedia.Analyze"] = "Analyze",
            ["AddMedia.SelectAll"] = "Select all",
            ["AddMedia.Mp3ToAll"] = "MP3 to all",
            ["AddMedia.Preparing"] = "Preparing media…",
            ["AddMedia.PreparingHint"] = "Checking the media engine and analyzing the link.",
            ["AddMedia.Destination"] = "DESTINATION",
            ["AddMedia.StageSelected"] = "Stage selected",
            ["AddMedia.Cancel"] = "Cancel",
            ["AddMedia.NoMedia"] = "Paste a YouTube link",
            ["AddMedia.NoMediaHint"] = "The selected items will become pending changes. Nothing is downloaded here.",
            ["AddMedia.Format"] = "FORMAT",
            ["AddMedia.AudioQuality"] = "AUDIO QUALITY",
            ["AddMedia.VideoQuality"] = "VIDEO QUALITY",
            ["Settings.Title"] = "Settings",
            ["Settings.Subtitle"] = "Personalize DownTrack without reinstalling it.",
            ["Settings.Language"] = "Language",
            ["Settings.LanguageHint"] = "Choose the interface language. Automatic uses your Windows display language.",
            ["Settings.Automatic"] = "Automatic (Windows)",
            ["Settings.English"] = "English",
            ["Settings.Save"] = "Done",
            ["Settings.Version"] = "DownTrack"
        };

        static Dictionary<string, string> Merge(Dictionary<string, string> baseText, params (string Key, string Value)[] values)
        {
            var result = new Dictionary<string, string>(baseText);
            foreach (var (key, value) in values)
                result[key] = value;
            return result;
        }

        return new(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = en,
            ["he"] = Merge(en,
                ("App.Library","ספרייה"),("App.MediaEngine","מנוע מדיה"),("App.SetupRequired","נדרשת הגדרה"),("App.Minimize","מזער"),("App.Maximize","הגדל / שחזר"),("App.Close","סגור"),("App.Settings","הגדרות"),
                ("Home.Workspace","סביבת המדיה האישית שלך"),("Home.Title","הספרייה שלך, בדיוק בדרך שלך."),("Home.Subtitle","שמור על התיקיות מסודרות, בדוק שינויים והבא מדיה מיוטיוב בדיוק למקום הרצוי."),("Home.AddRootFolder","הוסף תיקיית שורש"),("Home.NoDiskChanges","שום דבר לא משתנה בדיסק עד לשמירה."),("Home.RootFolders","תיקיות שורש"),("Home.Library","ספרייה"),("Home.RootFolderLabel","תיקיות שורש"),("Home.FoldersConnected","תיקיות מחוברות"),("Home.Workflow","תהליך עבודה"),("Home.StageReviewSave","הכן → בדוק → שמור"),("Home.CommitHint","הספרייה משתנה רק כששומרים."),("Home.YourFolders","התיקיות שלך"),("Home.OpenExplorerHint","פתח תיקייה ועבוד בתוכה בסייר של DownTrack."),("Home.AddFolder","הוסף תיקייה"),("Home.Connected","מחובר"),("Home.OpenExplorer","פתח סייר"),("Home.StartFirstFolder","התחל עם התיקייה הראשונה"),("Home.StartFirstFolderHint","בחר תיקייה ש־DownTrack ינהל. מכאן מתחיל הכול."),("Home.ChooseFolder","בחר תיקייה"),
                ("Explorer.Title","ספרייה / סייר"),("Explorer.Folders","תיקיות"),("Explorer.Back","אחורה"),("Explorer.Forward","קדימה"),("Explorer.Home","בית"),("Explorer.NewFolder","תיקייה חדשה"),("Explorer.Rename","שנה שם"),("Explorer.Delete","מחק"),("Explorer.AddMedia","הוסף מדיה"),("Explorer.StagedSafely","השינויים ממתינים בבטחה"),("Explorer.Name","שם"),("Explorer.Type","סוג"),("Explorer.State","מצב"),("Explorer.EmptyTitle","התיקייה ריקה"),("Explorer.EmptyHint","צור תיקייה או הוסף מדיה. פריטים חדשים יופיעו כאן מיד כשינויים ממתינים."),("Explorer.PendingChanges","שינויים ממתינים"),("Explorer.PendingHint","השינויים האלה עדיין וירטואליים."),("Explorer.Queued","בתור"),("Explorer.NoPending","אין שינויים ממתינים"),("Explorer.Cancel","בטל"),("Explorer.SaveChanges","שמור שינויים"),
                ("AddMedia.Title","הוסף מדיה"),("AddMedia.Subtitle","נתח קודם. בדוק כל פריט. הכן רק את מה שאתה רוצה."),("AddMedia.Analyze","נתח"),("AddMedia.SelectAll","בחר הכול"),("AddMedia.Mp3ToAll","MP3 לכולם"),("AddMedia.Preparing","מכין מדיה…"),("AddMedia.PreparingHint","בודק את מנוע המדיה ומנתח את הקישור."),("AddMedia.Destination","יעד"),("AddMedia.StageSelected","הכן נבחרים"),("AddMedia.Cancel","בטל"),("AddMedia.NoMedia","הדבק קישור ליוטיוב"),("AddMedia.NoMediaHint","הפריטים שבחרת יהפכו לשינויים ממתינים. שום דבר לא מורד כאן."),("AddMedia.Format","פורמט"),("AddMedia.AudioQuality","איכות שמע"),("AddMedia.VideoQuality","איכות וידאו"),
                ("Settings.Title","הגדרות"),("Settings.Subtitle","התאם את DownTrack בלי להתקין מחדש."),("Settings.Language","שפה"),("Settings.LanguageHint","בחר את שפת הממשק. אוטומטי משתמש בשפת התצוגה של Windows."),("Settings.Automatic","אוטומטי (Windows)"),("Settings.Save","סיום")),
            ["ar"] = Merge(en, ("App.Library","المكتبة"),("App.MediaEngine","محرك الوسائط"),("App.SetupRequired","الإعداد مطلوب"),("App.Settings","الإعدادات"),("Home.Title","مكتبتك، بالطريقة التي تريدها."),("Home.AddRootFolder","إضافة مجلد رئيسي"),("Home.NoDiskChanges","لن يتغير شيء على القرص حتى تحفظ."),("Home.YourFolders","مجلداتك"),("Home.AddFolder","إضافة مجلد"),("Home.ChooseFolder","اختيار مجلد"),("Explorer.AddMedia","إضافة وسائط"),("Explorer.NewFolder","مجلد جديد"),("Explorer.Rename","إعادة تسمية"),("Explorer.Delete","حذف"),("Explorer.SaveChanges","حفظ التغييرات"),("AddMedia.Title","إضافة وسائط"),("AddMedia.Analyze","تحليل"),("AddMedia.SelectAll","تحديد الكل"),("AddMedia.Mp3ToAll","MP3 للجميع"),("AddMedia.StageSelected","تجهيز المحدد"),("Settings.Title","الإعدادات"),("Settings.Language","اللغة"),("Settings.Save","تم"),("Settings.Automatic","تلقائي (Windows)")),
            ["es"] = Merge(en, ("App.Library","Biblioteca"),("App.MediaEngine","Motor multimedia"),("App.Settings","Configuración"),("Home.Title","Tu biblioteca, organizada a tu manera."),("Home.AddRootFolder","Añadir carpeta raíz"),("Home.NoDiskChanges","Nada cambia en el disco hasta que guardes."),("Home.YourFolders","Tus carpetas"),("Home.AddFolder","Añadir carpeta"),("Home.ChooseFolder","Elegir carpeta"),("Explorer.AddMedia","Añadir contenido"),("Explorer.NewFolder","Nueva carpeta"),("Explorer.Rename","Cambiar nombre"),("Explorer.Delete","Eliminar"),("Explorer.SaveChanges","Guardar cambios"),("AddMedia.Title","Añadir contenido"),("AddMedia.Analyze","Analizar"),("AddMedia.SelectAll","Seleccionar todo"),("AddMedia.StageSelected","Preparar seleccionados"),("Settings.Title","Configuración"),("Settings.Language","Idioma"),("Settings.Save","Listo")),
            ["fr"] = Merge(en, ("App.Library","Bibliothèque"),("App.MediaEngine","Moteur multimédia"),("App.Settings","Paramètres"),("Home.Title","Votre bibliothèque, organisée comme vous le souhaitez."),("Home.AddRootFolder","Ajouter un dossier racine"),("Home.NoDiskChanges","Rien ne change sur le disque avant l'enregistrement."),("Home.YourFolders","Vos dossiers"),("Home.AddFolder","Ajouter un dossier"),("Home.ChooseFolder","Choisir un dossier"),("Explorer.AddMedia","Ajouter un média"),("Explorer.NewFolder","Nouveau dossier"),("Explorer.Rename","Renommer"),("Explorer.Delete","Supprimer"),("Explorer.SaveChanges","Enregistrer les modifications"),("AddMedia.Title","Ajouter un média"),("AddMedia.Analyze","Analyser"),("AddMedia.SelectAll","Tout sélectionner"),("AddMedia.StageSelected","Préparer la sélection"),("Settings.Title","Paramètres"),("Settings.Language","Langue"),("Settings.Save","Terminé")),
            ["de"] = Merge(en, ("App.Library","Bibliothek"),("App.MediaEngine","Medien-Engine"),("App.Settings","Einstellungen"),("Home.Title","Deine Bibliothek, so organisiert, wie du möchtest."),("Home.AddRootFolder","Stammordner hinzufügen"),("Home.NoDiskChanges","Bis zum Speichern wird nichts auf der Festplatte geändert."),("Home.YourFolders","Deine Ordner"),("Home.AddFolder","Ordner hinzufügen"),("Home.ChooseFolder","Ordner auswählen"),("Explorer.AddMedia","Medien hinzufügen"),("Explorer.NewFolder","Neuer Ordner"),("Explorer.Rename","Umbenennen"),("Explorer.Delete","Löschen"),("Explorer.SaveChanges","Änderungen speichern"),("AddMedia.Title","Medien hinzufügen"),("AddMedia.Analyze","Analysieren"),("AddMedia.SelectAll","Alle auswählen"),("AddMedia.StageSelected","Auswahl vormerken"),("Settings.Title","Einstellungen"),("Settings.Language","Sprache"),("Settings.Save","Fertig")),
            ["it"] = Merge(en, ("App.Library","Libreria"),("App.MediaEngine","Motore multimediale"),("App.Settings","Impostazioni"),("Home.Title","La tua libreria, organizzata come vuoi tu."),("Home.AddRootFolder","Aggiungi cartella principale"),("Home.NoDiskChanges","Niente cambia sul disco finché non salvi."),("Home.YourFolders","Le tue cartelle"),("Home.AddFolder","Aggiungi cartella"),("Home.ChooseFolder","Scegli cartella"),("Explorer.AddMedia","Aggiungi media"),("Explorer.NewFolder","Nuova cartella"),("Explorer.Rename","Rinomina"),("Explorer.Delete","Elimina"),("Explorer.SaveChanges","Salva modifiche"),("AddMedia.Title","Aggiungi media"),("AddMedia.Analyze","Analizza"),("Settings.Title","Impostazioni"),("Settings.Language","Lingua"),("Settings.Save","Fatto")),
            ["pt"] = Merge(en, ("App.Library","Biblioteca"),("App.MediaEngine","Motor de mídia"),("App.Settings","Configurações"),("Home.Title","Sua biblioteca, organizada do seu jeito."),("Home.AddRootFolder","Adicionar pasta raiz"),("Home.NoDiskChanges","Nada muda no disco até você salvar."),("Home.YourFolders","Suas pastas"),("Home.AddFolder","Adicionar pasta"),("Home.ChooseFolder","Escolher pasta"),("Explorer.AddMedia","Adicionar mídia"),("Explorer.NewFolder","Nova pasta"),("Explorer.Rename","Renomear"),("Explorer.Delete","Excluir"),("Explorer.SaveChanges","Salvar alterações"),("AddMedia.Title","Adicionar mídia"),("AddMedia.Analyze","Analisar"),("Settings.Title","Configurações"),("Settings.Language","Idioma"),("Settings.Save","Concluir")),
            ["nl"] = Merge(en, ("App.Library","Bibliotheek"),("App.MediaEngine","Media-engine"),("App.Settings","Instellingen"),("Home.Title","Je bibliotheek, georganiseerd zoals jij wilt."),("Home.AddRootFolder","Hoofdmap toevoegen"),("Home.NoDiskChanges","Er verandert niets op schijf totdat je opslaat."),("Home.YourFolders","Jouw mappen"),("Home.AddFolder","Map toevoegen"),("Home.ChooseFolder","Map kiezen"),("Explorer.AddMedia","Media toevoegen"),("Explorer.NewFolder","Nieuwe map"),("Explorer.Rename","Naam wijzigen"),("Explorer.Delete","Verwijderen"),("Explorer.SaveChanges","Wijzigingen opslaan"),("AddMedia.Title","Media toevoegen"),("AddMedia.Analyze","Analyseren"),("Settings.Title","Instellingen"),("Settings.Language","Taal"),("Settings.Save","Gereed")),
            ["pl"] = Merge(en, ("App.Library","Biblioteka"),("App.MediaEngine","Silnik multimediów"),("App.Settings","Ustawienia"),("Home.Title","Twoja biblioteka, zorganizowana po Twojemu."),("Home.AddRootFolder","Dodaj folder główny"),("Home.NoDiskChanges","Nic nie zmienia się na dysku, dopóki nie zapiszesz."),("Home.YourFolders","Twoje foldery"),("Home.AddFolder","Dodaj folder"),("Home.ChooseFolder","Wybierz folder"),("Explorer.AddMedia","Dodaj multimedia"),("Explorer.NewFolder","Nowy folder"),("Explorer.Rename","Zmień nazwę"),("Explorer.Delete","Usuń"),("Explorer.SaveChanges","Zapisz zmiany"),("AddMedia.Title","Dodaj multimedia"),("AddMedia.Analyze","Analizuj"),("Settings.Title","Ustawienia"),("Settings.Language","Język"),("Settings.Save","Gotowe")),
            ["tr"] = Merge(en, ("App.Library","Kitaplık"),("App.MediaEngine","Medya motoru"),("App.Settings","Ayarlar"),("Home.Title","Kitaplığınız, kendi tarzınızda düzenlendi."),("Home.AddRootFolder","Kök klasör ekle"),("Home.NoDiskChanges","Kaydetmeden diskte hiçbir şey değişmez."),("Home.YourFolders","Klasörleriniz"),("Home.AddFolder","Klasör ekle"),("Home.ChooseFolder","Klasör seç"),("Explorer.AddMedia","Medya ekle"),("Explorer.NewFolder","Yeni klasör"),("Explorer.Rename","Yeniden adlandır"),("Explorer.Delete","Sil"),("Explorer.SaveChanges","Değişiklikleri kaydet"),("AddMedia.Title","Medya ekle"),("AddMedia.Analyze","Analiz et"),("Settings.Title","Ayarlar"),("Settings.Language","Dil"),("Settings.Save","Bitti")),
            ["ru"] = Merge(en, ("App.Library","Библиотека"),("App.MediaEngine","Медиа-движок"),("App.Settings","Настройки"),("Home.Title","Ваша библиотека, организованная по-вашему."),("Home.AddRootFolder","Добавить корневую папку"),("Home.NoDiskChanges","До сохранения ничего не изменяется на диске."),("Home.YourFolders","Ваши папки"),("Home.AddFolder","Добавить папку"),("Home.ChooseFolder","Выбрать папку"),("Explorer.AddMedia","Добавить медиа"),("Explorer.NewFolder","Новая папка"),("Explorer.Rename","Переименовать"),("Explorer.Delete","Удалить"),("Explorer.SaveChanges","Сохранить изменения"),("AddMedia.Title","Добавить медиа"),("AddMedia.Analyze","Анализировать"),("Settings.Title","Настройки"),("Settings.Language","Язык"),("Settings.Save","Готово")),
            ["uk"] = Merge(en, ("App.Library","Бібліотека"),("App.MediaEngine","Медіарушій"),("App.Settings","Налаштування"),("Home.Title","Ваша бібліотека, організована по-вашому."),("Home.AddRootFolder","Додати кореневу папку"),("Home.NoDiskChanges","До збереження нічого не змінюється на диску."),("Home.YourFolders","Ваші папки"),("Home.AddFolder","Додати папку"),("Home.ChooseFolder","Вибрати папку"),("Explorer.AddMedia","Додати медіа"),("Explorer.NewFolder","Нова папка"),("Explorer.Rename","Перейменувати"),("Explorer.Delete","Видалити"),("Explorer.SaveChanges","Зберегти зміни"),("AddMedia.Title","Додати медіа"),("AddMedia.Analyze","Аналізувати"),("Settings.Title","Налаштування"),("Settings.Language","Мова"),("Settings.Save","Готово")),
            ["ja"] = Merge(en, ("App.Library","ライブラリ"),("App.MediaEngine","メディアエンジン"),("App.Settings","設定"),("Home.Title","あなたのライブラリを、あなたらしく整理。"),("Home.AddRootFolder","ルートフォルダーを追加"),("Home.NoDiskChanges","保存するまでディスク上は変更されません。"),("Home.YourFolders","フォルダー"),("Home.AddFolder","フォルダーを追加"),("Home.ChooseFolder","フォルダーを選択"),("Explorer.AddMedia","メディアを追加"),("Explorer.NewFolder","新しいフォルダー"),("Explorer.Rename","名前を変更"),("Explorer.Delete","削除"),("Explorer.SaveChanges","変更を保存"),("AddMedia.Title","メディアを追加"),("AddMedia.Analyze","解析"),("Settings.Title","設定"),("Settings.Language","言語"),("Settings.Save","完了")),
            ["ko"] = Merge(en, ("App.Library","라이브러리"),("App.MediaEngine","미디어 엔진"),("App.Settings","설정"),("Home.Title","원하는 방식으로 정리된 나만의 라이브러리."),("Home.AddRootFolder","루트 폴더 추가"),("Home.NoDiskChanges","저장하기 전에는 디스크에 아무것도 변경되지 않습니다."),("Home.YourFolders","내 폴더"),("Home.AddFolder","폴더 추가"),("Home.ChooseFolder","폴더 선택"),("Explorer.AddMedia","미디어 추가"),("Explorer.NewFolder","새 폴더"),("Explorer.Rename","이름 바꾸기"),("Explorer.Delete","삭제"),("Explorer.SaveChanges","변경 사항 저장"),("AddMedia.Title","미디어 추가"),("AddMedia.Analyze","분석"),("Settings.Title","설정"),("Settings.Language","언어"),("Settings.Save","완료")),
            ["zh-CN"] = Merge(en, ("App.Library","媒体库"),("App.MediaEngine","媒体引擎"),("App.Settings","设置"),("Home.Title","按你的方式整理媒体库。"),("Home.AddRootFolder","添加根文件夹"),("Home.NoDiskChanges","保存之前不会修改磁盘。"),("Home.YourFolders","你的文件夹"),("Home.AddFolder","添加文件夹"),("Home.ChooseFolder","选择文件夹"),("Explorer.AddMedia","添加媒体"),("Explorer.NewFolder","新建文件夹"),("Explorer.Rename","重命名"),("Explorer.Delete","删除"),("Explorer.SaveChanges","保存更改"),("AddMedia.Title","添加媒体"),("AddMedia.Analyze","分析"),("Settings.Title","设置"),("Settings.Language","语言"),("Settings.Save","完成")),
            ["zh-TW"] = Merge(en, ("App.Library","媒體庫"),("App.MediaEngine","媒體引擎"),("App.Settings","設定"),("Home.Title","依照你的方式整理媒體庫。"),("Home.AddRootFolder","新增根資料夾"),("Home.NoDiskChanges","儲存前不會修改磁碟。"),("Home.YourFolders","你的資料夾"),("Home.AddFolder","新增資料夾"),("Home.ChooseFolder","選擇資料夾"),("Explorer.AddMedia","新增媒體"),("Explorer.NewFolder","新增資料夾"),("Explorer.Rename","重新命名"),("Explorer.Delete","刪除"),("Explorer.SaveChanges","儲存變更"),("AddMedia.Title","新增媒體"),("AddMedia.Analyze","分析"),("Settings.Title","設定"),("Settings.Language","語言"),("Settings.Save","完成")),
            ["cs"] = Merge(en, ("App.Library","Knihovna"),("App.MediaEngine","Mediální engine"),("App.Settings","Nastavení"),("Home.Title","Vaše knihovna, uspořádaná podle vás."),("Home.AddRootFolder","Přidat kořenovou složku"),("Home.NoDiskChanges","Na disku se nic nezmění, dokud změny neuložíte."),("Home.YourFolders","Vaše složky"),("Home.AddFolder","Přidat složku"),("Home.ChooseFolder","Vybrat složku"),("Explorer.AddMedia","Přidat média"),("Explorer.NewFolder","Nová složka"),("Explorer.Rename","Přejmenovat"),("Explorer.Delete","Smazat"),("Explorer.SaveChanges","Uložit změny"),("AddMedia.Title","Přidat média"),("AddMedia.Analyze","Analyzovat"),("Settings.Title","Nastavení"),("Settings.Language","Jazyk"),("Settings.Save","Hotovo")),
            ["da"] = Merge(en, ("App.Library","Bibliotek"),("App.MediaEngine","Medieengine"),("App.Settings","Indstillinger"),("Home.Title","Dit bibliotek, organiseret på din måde."),("Home.AddRootFolder","Tilføj rodmappe"),("Home.NoDiskChanges","Intet ændres på disken, før du gemmer."),("Home.YourFolders","Dine mapper"),("Home.AddFolder","Tilføj mappe"),("Home.ChooseFolder","Vælg mappe"),("Explorer.AddMedia","Tilføj medier"),("Explorer.NewFolder","Ny mappe"),("Explorer.Rename","Omdøb"),("Explorer.Delete","Slet"),("Explorer.SaveChanges","Gem ændringer"),("AddMedia.Title","Tilføj medier"),("AddMedia.Analyze","Analyser"),("Settings.Title","Indstillinger"),("Settings.Language","Sprog"),("Settings.Save","Færdig")),
            ["sv"] = Merge(en, ("App.Library","Bibliotek"),("App.MediaEngine","Mediamotor"),("App.Settings","Inställningar"),("Home.Title","Ditt bibliotek, organiserat på ditt sätt."),("Home.AddRootFolder","Lägg till rotmapp"),("Home.NoDiskChanges","Inget ändras på disken förrän du sparar."),("Home.YourFolders","Dina mappar"),("Home.AddFolder","Lägg till mapp"),("Home.ChooseFolder","Välj mapp"),("Explorer.AddMedia","Lägg till media"),("Explorer.NewFolder","Ny mapp"),("Explorer.Rename","Byt namn"),("Explorer.Delete","Ta bort"),("Explorer.SaveChanges","Spara ändringar"),("AddMedia.Title","Lägg till media"),("AddMedia.Analyze","Analysera"),("Settings.Title","Inställningar"),("Settings.Language","Språk"),("Settings.Save","Klar"))
        };
    }

    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}