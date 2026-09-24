using System.Collections.Generic;

namespace DownTrack.Infrastructure.Localization;

internal static class ModernTranslationCatalog
{
    private static readonly string[] Languages =
    [
        "en","he","es","fr","de","it","pt","nl","pl","cs",
        "tr","uk","ru","ar","el","ro","ja","ko","zh-Hans","zh-Hant"
    ];

    public static void Apply(IDictionary<string, Dictionary<string, string>> dictionaries)
    {
        foreach (var language in Languages)
            EnsurePack(dictionaries, language);
    }

    private static void EnsurePack(
        IDictionary<string, Dictionary<string, string>> dictionaries,
        string language)
    {
        if (!dictionaries.TryGetValue(language, out var dictionary))
        {
            dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dictionaries[language] = dictionary;
        }

        foreach (var (key, value) in English)
            dictionary[key] = value;

        foreach (var (key, value) in language switch
        {
            "he" => Hebrew,
            "es" => Spanish,
            "fr" => French,
            "de" => German,
            "it" => Italian,
            "pt" => Portuguese,
            "nl" => Dutch,
            "pl" => Polish,
            "cs" => Czech,
            "tr" => Turkish,
            "uk" => Ukrainian,
            "ru" => Russian,
            "ar" => Arabic,
            "el" => Greek,
            "ro" => Romanian,
            "ja" => Japanese,
            "ko" => Korean,
            "zh-Hans" => ChineseSimplified,
            "zh-Hant" => ChineseTraditional,
            _ => Empty
        })
            dictionary[key] = value;
    }

    private static readonly Dictionary<string, string> Empty = new();

    private static readonly Dictionary<string, string> English =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Shell.AddMedia"]="Add media",
            ["Shell.AddFolder"]="Add folder",
            ["Shell.Home"]="Home",
            ["Shell.Forward"]="Forward",
            ["Shell.Back"]="Back",
            ["Shell.Library"]="Library",
            ["Shell.Rename"]="Rename",
            ["Shell.Delete"]="Delete",
            ["Shell.Cancel"]="Cancel",
            ["Shell.MyLibrary"]="My Library",
            ["Shell.AllRootFolders"]="All root folders",
            ["Shell.EngineReady"]="Media engine ready",
            ["Shell.EngineNeedsSetup"]="Media engine setup required",

            ["Window.Minimize"]="Minimize",
            ["Window.Maximize"]="Maximize / restore",
            ["Window.Close"]="Close",

            ["Library.Overline"]="MY LIBRARY",
            ["Library.Title"]="Your library",
            ["Library.Subtitle"]="Your folders, your organization, one calm workspace.",
            ["Library.Folder"]="FOLDER",
            ["Library.Location"]="LOCATION",
            ["Library.Action"]="ACTION",
            ["Library.Open"]="Open",
            ["Library.EmptyTitle"]="Your library is ready for its first folder",
            ["Library.EmptySubtitle"]="Choose a folder to manage it inside DownTrack. Nothing changes on disk until you save.",

            ["Explorer.Overline"]="LIBRARY / EXPLORER",
            ["Explorer.Name"]="NAME",
            ["Explorer.Type"]="TYPE",
            ["Explorer.State"]="STATE",
            ["Explorer.EmptyTitle"]="This folder is empty",
            ["Explorer.EmptySubtitle"]="Create a folder or add media. New staged items appear here immediately.",

            ["Pending.Title"]="Pending changes",
            ["Pending.Subtitle"]="Review everything before it touches your disk.",

            ["AddMedia.Title"]="Add media",
            ["AddMedia.Subtitle"]="Paste a video or playlist, review every item, then stage only what you want.",
            ["AddMedia.UrlHint"]="Paste a YouTube video or playlist URL",
            ["AddMedia.Analyze"]="Analyze",
            ["AddMedia.SelectAll"]="Select all",
            ["AddMedia.Mp3All"]="MP3 to all",
            ["AddMedia.Format"]="FORMAT",
            ["AddMedia.AudioQuality"]="AUDIO QUALITY",
            ["AddMedia.VideoQuality"]="VIDEO QUALITY",
            ["AddMedia.NoMedia"]="Paste a YouTube link",
            ["AddMedia.NoMediaSubtitle"]="Nothing is downloaded during analysis. Selected items become pending changes.",
            ["AddMedia.Preparing"]="Preparing media…",
            ["AddMedia.PreparingSubtitle"]="Checking the media engine and analyzing the link.",
            ["AddMedia.Destination"]="DESTINATION",
            ["AddMedia.Stage"]="Add to downloads",
            ["AddMedia.Cancel"]="Cancel",

            ["Settings.Title"]="Settings",
            ["Settings.Subtitle"]="Make DownTrack feel exactly the way you want.",
            ["Settings.Appearance"]="Appearance",
            ["Settings.AppearanceSubtitle"]="Choose how DownTrack follows your Windows theme.",
            ["Settings.Language"]="Language",
            ["Settings.LanguageDescription"]="Use Automatic to follow your Windows display language, or choose a language manually.",
            ["Settings.Automatic"]="Automatic (Windows)",
            ["Settings.ThemeLight"]="Light",
            ["Settings.ThemeDark"]="Dark",
            ["Settings.ThemeSystem"]="System",
            ["Settings.Updates"]="Updates",
            ["Settings.UpdatesSubtitle"]="Keep the app and media tools current.",
            ["Settings.AppUpdates"]="DownTrack",
            ["Settings.Check"]="Check",
            ["Settings.Update"]="Update",
            ["Settings.ToolUpdates"]="Media tools",
            ["Settings.AutoUpdate"]="Automatically check for DownTrack updates",
            ["Settings.AutoToolUpdate"]="Automatically keep media tools current",
            ["Settings.Defaults"]="Download defaults",
            ["Settings.DefaultsSubtitle"]="These values are used when new media is analyzed.",
            ["Settings.DefaultFormat"]="Default format",
            ["Settings.DefaultAudioQuality"]="Default audio quality",
            ["Settings.DefaultVideoQuality"]="Default video quality",
            ["Settings.Cancel"]="Cancel",
            ["Settings.Save"]="Save settings",
            ["Settings.ToolsReady"]="Media tools are ready.",
            ["Settings.ToolsMissing"]="One or more media tools are missing.",
            ["Settings.ToolsUpdated"]="Media tools updated.",

            ["Updates.Checking"]="Checking for updates…",
            ["Updates.CheckFailed"]="Update check failed: {0}",
            ["Updates.Downloading"]="Downloading the latest version…",
            ["Updates.Restarting"]="Update started. DownTrack will restart when ready.",

            ["Engine.Setup"]="Setup",
            ["Engine.DownloadingYtDlp"]="Downloading yt-dlp…",
            ["Engine.DownloadingFfmpeg"]="Downloading FFmpeg…",
            ["Engine.InstallingRuntime"]="Installing Deno runtime…",
            ["Engine.Ready"]="Media engine ready.",
            ["Engine.VerifyingFfmpeg"]="Verifying FFmpeg…",
            ["Engine.UpdatingYtDlp"]="Updating yt-dlp…",
            ["Engine.UpdatingFfmpeg"]="Updating FFmpeg…",
            ["Engine.UpdatingDeno"]="Updating Deno…",
            ["Engine.Updated"]="Media tools updated.",
            ["Engine.HashMissingFile"]="Checksum is missing for {0}.",
            ["Engine.HashFailedFile"]="Checksum verification failed for {0}.",
            ["Engine.HashMissingFfmpeg"]="FFmpeg checksum is missing.",
            ["Engine.HashFailedFfmpeg"]="FFmpeg checksum verification failed.",
            ["Engine.FfmpegMissingExecutables"]="FFmpeg package is missing ffmpeg.exe or ffprobe.exe.",
            ["Engine.HashMissingDeno"]="Deno checksum is missing.",
            ["Engine.HashFailedDeno"]="Deno checksum verification failed.",
            ["Engine.DenoMissingExecutable"]="Deno package is missing deno.exe.",

            ["Error.MetadataMissing"]="The media information could not be read.",
            ["Error.DownloadedFileEmpty"]="The downloaded file is empty.",

            ["Explorer.AllSaved"]="All changes are saved.",
            ["Explorer.SomeNeedAttention"]="Some changes need attention.",
            ["Explorer.ApplyingChanges"]="Applying staged changes…",
            ["Explorer.Cancelled"]="Cancelled: {0}",
            ["Explorer.MediaAddedOne"]="Media added to the pending queue.",
            ["Explorer.MediaAddedMany"]="{0} media items added to the pending queue.",
            ["Explorer.PendingCreate"]="Pending: create “{0}”.",
            ["Explorer.PendingRename"]="Pending: rename “{0}”.",
            ["Explorer.PendingDelete"]="Pending: delete “{0}”.",
            ["Explorer.NewFolder"]="New folder",
            ["Explorer.NewFolderPrompt"]="Choose a name for the new folder.",
            ["Explorer.NewFolderDefault"]="New Folder",
            ["Explorer.Rename"]="Rename",
            ["Explorer.RenamePrompt"]="Enter the new name.",
            ["Explorer.DeleteFolderPrompt"]="Delete folder “{0}” from the staged plan?",
            ["Explorer.DeleteFilePrompt"]="Delete “{0}” from the staged plan?",
            ["Explorer.ConfirmDelete"]="Confirm delete",
            ["Explorer.SaveChangesArrow"]="Save changes  →",
            ["Explorer.SaveChangesCount"]="Save changes ({0})  →",
            ["Explorer.QueuedOne"]="1 queued",
            ["Explorer.Queued"]="{0} queued",

            ["AddMedia.PastePrompt"]="Paste a YouTube video or playlist URL.",
            ["AddMedia.NothingFound"]="Nothing was found. Check the link and try again.",
            ["AddMedia.OneReady"]="1 media item ready. Edit the options before adding.",
            ["AddMedia.ManyReady"]="{0} media items ready. Each row is independent.",
            ["AddMedia.AnalysisCancelled"]="Analysis cancelled.",
            ["AddMedia.ErrorFallback"]="We couldn't analyze this link. Please try again.",

            ["Media.Untitled"]="Untitled media"
        };

    private static Dictionary<string, string> Pack(params (string Key, string Value)[] values) =>
        values.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, string> Hebrew = Pack(
        ("Shell.AddMedia","הוסף מדיה"),("Shell.AddFolder","הוסף תיקייה"),("Shell.Home","בית"),("Shell.Forward","קדימה"),("Shell.Back","חזרה"),
        ("Shell.Library","ספרייה"),("Shell.Rename","שנה שם"),("Shell.Delete","מחק"),("Shell.Cancel","ביטול"),("Shell.MyLibrary","הספרייה שלי"),("Shell.AllRootFolders","כל תיקיות השורש"),("Shell.EngineReady","מנוע המדיה מוכן"),("Shell.EngineNeedsSetup","נדרש להגדיר את מנוע המדיה"),
        ("Window.Minimize","מזער"),("Window.Maximize","הגדל / שחזר"),("Window.Close","סגור"),
        ("Library.Overline","הספרייה שלי"),("Library.Title","הספרייה שלך"),("Library.Subtitle","התיקיות שלך, הארגון שלך, סביבת עבודה אחת ונעימה."),("Library.Folder","תיקייה"),("Library.Location","מיקום"),("Library.Action","פעולה"),("Library.Open","פתח"),("Library.EmptyTitle","הספרייה שלך מחכה לתיקייה הראשונה"),("Library.EmptySubtitle","בחר תיקייה לניהול בתוך DownTrack. שום דבר בדיסק לא משתנה עד ששומרים."),
        ("Explorer.Overline","ספרייה / סייר"),("Explorer.Name","שם"),("Explorer.Type","סוג"),("Explorer.State","מצב"),("Explorer.EmptyTitle","התיקייה ריקה"),("Explorer.EmptySubtitle","צור תיקייה או הוסף מדיה. פריטים ממתינים חדשים יופיעו כאן מיד."),
        ("Pending.Title","שינויים ממתינים"),("Pending.Subtitle","בדוק הכול לפני שמשהו נכתב לדיסק."),
        ("AddMedia.Title","הוסף מדיה"),("AddMedia.Subtitle","הדבק סרטון או פלייליסט, בדוק כל פריט והוסף רק את מה שתרצה."),("AddMedia.UrlHint","הדבק קישור לסרטון או פלייליסט של YouTube"),("AddMedia.Analyze","נתח"),("AddMedia.SelectAll","בחר הכול"),("AddMedia.Mp3All","MP3 לכולם"),("AddMedia.Format","פורמט"),("AddMedia.AudioQuality","איכות שמע"),("AddMedia.VideoQuality","איכות וידאו"),("AddMedia.NoMedia","הדבק קישור ל־YouTube"),("AddMedia.NoMediaSubtitle","בשלב הניתוח שום דבר לא יורד. פריטים נבחרים הופכים לשינויים ממתינים."),("AddMedia.Preparing","מכין מדיה…"),("AddMedia.PreparingSubtitle","בודק את מנוע המדיה ומנתח את הקישור."),("AddMedia.Destination","יעד"),("AddMedia.Stage","הוסף להורדות"),("AddMedia.Cancel","ביטול"),
        ("Settings.Title","הגדרות"),("Settings.Subtitle","התאם את DownTrack בדיוק בדרך שנוחה לך."),("Settings.Appearance","מראה"),("Settings.AppearanceSubtitle","בחר כיצד DownTrack יעקוב אחר ערכת הצבעים של Windows."),("Settings.Language","שפה"),("Settings.LanguageDescription","בחר אוטומטי כדי לעקוב אחר שפת התצוגה של Windows, או בחר שפה ידנית."),("Settings.Automatic","אוטומטי (Windows)"),("Settings.ThemeLight","בהיר"),("Settings.ThemeDark","כהה"),("Settings.ThemeSystem","מערכת"),("Settings.Updates","עדכונים"),("Settings.UpdatesSubtitle","שמור על התוכנה ועל כלי המדיה מעודכנים."),("Settings.AppUpdates","DownTrack"),("Settings.Check","בדוק"),("Settings.Update","עדכן"),("Settings.ToolUpdates","כלי מדיה"),("Settings.AutoUpdate","בדוק אוטומטית עדכונים ל־DownTrack"),("Settings.AutoToolUpdate","שמור אוטומטית על כלי המדיה מעודכנים"),("Settings.Defaults","ברירות מחדל להורדה"),("Settings.DefaultsSubtitle","הערכים האלה ישמשו בעת ניתוח מדיה חדשה."),("Settings.DefaultFormat","פורמט ברירת מחדל"),("Settings.DefaultAudioQuality","איכות שמע כברירת מחדל"),("Settings.DefaultVideoQuality","איכות וידאו כברירת מחדל"),("Settings.Cancel","ביטול"),("Settings.Save","שמור הגדרות"),("Settings.ToolsReady","כלי המדיה מוכנים."),("Settings.ToolsMissing","אחד או יותר מכלי המדיה חסרים."),("Settings.ToolsUpdated","כלי המדיה עודכנו."),
        ("Updates.Checking","בודק עדכונים…"),("Updates.CheckFailed","בדיקת העדכון נכשלה: {0}"),("Updates.Downloading","מוריד את הגרסה האחרונה…"),("Updates.Restarting","העדכון התחיל. DownTrack יופעל מחדש כשיהיה מוכן."),
        ("Engine.Setup","הגדרה"),("Engine.DownloadingYtDlp","מוריד yt-dlp…"),("Engine.DownloadingFfmpeg","מוריד FFmpeg…"),("Engine.InstallingRuntime","מתקין את Deno…"),("Engine.Ready","מנוע המדיה מוכן."),("Engine.VerifyingFfmpeg","מאמת FFmpeg…"),("Engine.UpdatingYtDlp","מעדכן yt-dlp…"),("Engine.UpdatingFfmpeg","מעדכן FFmpeg…"),("Engine.UpdatingDeno","מעדכן Deno…"),("Engine.Updated","כלי המדיה עודכנו."),("Engine.HashMissingFile","חסר checksum עבור {0}."),("Engine.HashFailedFile","אימות checksum נכשל עבור {0}."),("Engine.HashMissingFfmpeg","חסר checksum ל־FFmpeg."),("Engine.HashFailedFfmpeg","אימות checksum של FFmpeg נכשל."),("Engine.FfmpegMissingExecutables","בחבילת FFmpeg חסרים ffmpeg.exe או ffprobe.exe."),("Engine.HashMissingDeno","חסר checksum ל־Deno."),("Engine.HashFailedDeno","אימות checksum של Deno נכשל."),("Engine.DenoMissingExecutable","בחבילת Deno חסר deno.exe."),
        ("Error.MetadataMissing","לא ניתן היה לקרוא את פרטי המדיה."),("Error.DownloadedFileEmpty","הקובץ שהורד ריק."),
        ("Explorer.AllSaved","כל השינויים נשמרו."),("Explorer.SomeNeedAttention","חלק מהשינויים דורשים תשומת לב."),("Explorer.ApplyingChanges","מחיל את השינויים הממתינים…"),("Explorer.Cancelled","בוטל: {0}"),("Explorer.MediaAddedOne","המדיה נוספה לתור השינויים הממתינים."),("Explorer.MediaAddedMany","{0} פריטי מדיה נוספו לתור השינויים הממתינים."),("Explorer.PendingCreate","ממתין: יצירת „{0}”."),("Explorer.PendingRename","ממתין: שינוי השם של „{0}”."),("Explorer.PendingDelete","ממתין: מחיקת „{0}”."),("Explorer.NewFolder","תיקייה חדשה"),("Explorer.NewFolderPrompt","בחר שם לתיקייה החדשה."),("Explorer.NewFolderDefault","תיקייה חדשה"),("Explorer.Rename","שנה שם"),("Explorer.RenamePrompt","הזן את השם החדש."),("Explorer.DeleteFolderPrompt","למחוק את התיקייה „{0}” מתוכנית השינויים?"),("Explorer.DeleteFilePrompt","למחוק את „{0}” מתוכנית השינויים?"),("Explorer.ConfirmDelete","אישור מחיקה"),("Explorer.SaveChangesArrow","שמור שינויים  ←"),("Explorer.SaveChangesCount","שמור שינויים ({0})  ←"),("Explorer.QueuedOne","1 בתור"),("Explorer.Queued","{0} בתור"),
        ("AddMedia.PastePrompt","הדבק קישור לסרטון או פלייליסט של YouTube."),("AddMedia.NothingFound","לא נמצא דבר. בדוק את הקישור ונסה שוב."),("AddMedia.OneReady","פריט מדיה אחד מוכן. ערוך את האפשרויות לפני ההוספה."),("AddMedia.ManyReady","{0} פריטי מדיה מוכנים. כל שורה עצמאית."),("AddMedia.AnalysisCancelled","הניתוח בוטל."),("AddMedia.ErrorFallback","לא ניתן היה לנתח את הקישור. נסה שוב."),("Media.Untitled","מדיה ללא שם")
    );

    private static readonly Dictionary<string, string> Spanish = Pack(
        ("Shell.AddMedia","Añadir medios"),("Shell.AddFolder","Añadir carpeta"),("Shell.Home","Inicio"),("Shell.Forward","Adelante"),("Shell.Back","Atrás"),("Shell.Library","Biblioteca"),("Shell.Rename","Cambiar nombre"),("Shell.Delete","Eliminar"),("Shell.Cancel","Cancelar"),("Shell.MyLibrary","Mi biblioteca"),("Shell.AllRootFolders","Todas las carpetas raíz"),("Shell.EngineReady","Motor multimedia listo"),("Shell.EngineNeedsSetup","Se requiere configurar el motor multimedia"),
        ("Window.Minimize","Minimizar"),("Window.Maximize","Maximizar / restaurar"),("Window.Close","Cerrar"),
        ("Library.Overline","MI BIBLIOTECA"),("Library.Title","Tu biblioteca"),("Library.Subtitle","Tus carpetas, tu organización, un espacio de trabajo tranquilo."),("Library.Folder","CARPETA"),("Library.Location","UBICACIÓN"),("Library.Action","ACCIÓN"),("Library.Open","Abrir"),("Library.EmptyTitle","Tu biblioteca está lista para su primera carpeta"),("Library.EmptySubtitle","Elige una carpeta para administrarla dentro de DownTrack. Nada cambia en el disco hasta que guardes."),
        ("Explorer.Overline","BIBLIOTECA / EXPLORADOR"),("Explorer.Name","NOMBRE"),("Explorer.Type","TIPO"),("Explorer.State","ESTADO"),("Explorer.EmptyTitle","Esta carpeta está vacía"),("Explorer.EmptySubtitle","Crea una carpeta o añade medios. Los nuevos elementos pendientes aparecen aquí de inmediato."),
        ("Pending.Title","Cambios pendientes"),("Pending.Subtitle","Revisa todo antes de que llegue al disco."),
        ("AddMedia.Title","Añadir medios"),("AddMedia.Subtitle","Pega un vídeo o una lista, revisa cada elemento y añade solo lo que quieras."),("AddMedia.UrlHint","Pega la URL de un vídeo o una lista de YouTube"),("AddMedia.Analyze","Analizar"),("AddMedia.SelectAll","Seleccionar todo"),("AddMedia.Mp3All","MP3 para todos"),("AddMedia.Format","FORMATO"),("AddMedia.AudioQuality","CALIDAD DE AUDIO"),("AddMedia.VideoQuality","CALIDAD DE VÍDEO"),("AddMedia.NoMedia","Pega un enlace de YouTube"),("AddMedia.NoMediaSubtitle","No se descarga nada durante el análisis. Los elementos seleccionados pasan a cambios pendientes."),("AddMedia.Preparing","Preparando medios…"),("AddMedia.PreparingSubtitle","Comprobando el motor multimedia y analizando el enlace."),("AddMedia.Destination","DESTINO"),("AddMedia.Stage","Añadir a descargas"),("AddMedia.Cancel","Cancelar"),
        ("Settings.Title","Configuración"),("Settings.Subtitle","Haz que DownTrack funcione exactamente como quieres."),("Settings.Appearance","Apariencia"),("Settings.AppearanceSubtitle","Elige cómo sigue DownTrack el tema de Windows."),("Settings.Language","Idioma"),("Settings.LanguageDescription","Usa Automático para seguir el idioma de Windows o elige uno manualmente."),("Settings.Automatic","Automático (Windows)"),("Settings.ThemeLight","Claro"),("Settings.ThemeDark","Oscuro"),("Settings.ThemeSystem","Sistema"),("Settings.Updates","Actualizaciones"),("Settings.UpdatesSubtitle","Mantén actualizados la aplicación y los medios."),("Settings.AppUpdates","DownTrack"),("Settings.Check","Comprobar"),("Settings.Update","Actualizar"),("Settings.ToolUpdates","Herramientas multimedia"),("Settings.AutoUpdate","Comprobar automáticamente las actualizaciones de DownTrack"),("Settings.AutoToolUpdate","Mantener automáticamente actualizadas las herramientas"),("Settings.Defaults","Valores de descarga predeterminados"),("Settings.DefaultsSubtitle","Estos valores se usan al analizar nuevos medios."),("Settings.DefaultFormat","Formato predeterminado"),("Settings.DefaultAudioQuality","Calidad de audio predeterminada"),("Settings.DefaultVideoQuality","Calidad de vídeo predeterminada"),("Settings.Cancel","Cancelar"),("Settings.Save","Guardar configuración"),("Settings.ToolsReady","Las herramientas multimedia están listas."),("Settings.ToolsMissing","Faltan una o más herramientas multimedia."),("Settings.ToolsUpdated","Herramientas multimedia actualizadas."),
        ("Updates.Checking","Buscando actualizaciones…"),("Updates.CheckFailed","La comprobación falló: {0}"),("Updates.Downloading","Descargando la última versión…"),("Updates.Restarting","La actualización ha comenzado. DownTrack se reiniciará cuando esté listo."),
        ("Engine.Setup","Configurar"),("Engine.DownloadingYtDlp","Descargando yt-dlp…"),("Engine.DownloadingFfmpeg","Descargando FFmpeg…"),("Engine.InstallingRuntime","Instalando Deno…"),("Engine.Ready","Motor multimedia listo."),("Engine.VerifyingFfmpeg","Verificando FFmpeg…"),("Engine.UpdatingYtDlp","Actualizando yt-dlp…"),("Engine.UpdatingFfmpeg","Actualizando FFmpeg…"),("Engine.UpdatingDeno","Actualizando Deno…"),("Engine.Updated","Herramientas multimedia actualizadas."),
        ("Explorer.AllSaved","Todos los cambios están guardados."),("Explorer.SomeNeedAttention","Algunos cambios requieren atención."),("Explorer.ApplyingChanges","Aplicando cambios pendientes…"),("Explorer.Cancelled","Cancelado: {0}"),("Explorer.MediaAddedOne","El medio se añadió a la cola pendiente."),("Explorer.MediaAddedMany","Se añadieron {0} elementos a la cola pendiente."),("Explorer.PendingCreate","Pendiente: crear “{0}”."),("Explorer.PendingRename","Pendiente: cambiar el nombre de “{0}”."),("Explorer.PendingDelete","Pendiente: eliminar “{0}”."),("Explorer.NewFolder","Nueva carpeta"),("Explorer.NewFolderPrompt","Elige un nombre para la nueva carpeta."),("Explorer.NewFolderDefault","Nueva carpeta"),("Explorer.Rename","Cambiar nombre"),("Explorer.RenamePrompt","Escribe el nuevo nombre."),("Explorer.DeleteFolderPrompt","¿Eliminar la carpeta “{0}” del plan pendiente?"),("Explorer.DeleteFilePrompt","¿Eliminar “{0}” del plan pendiente?"),("Explorer.ConfirmDelete","Confirmar eliminación"),("Explorer.SaveChangesArrow","Guardar cambios  →"),("Explorer.SaveChangesCount","Guardar cambios ({0})  →"),("Explorer.QueuedOne","1 en cola"),("Explorer.Queued","{0} en cola"),
        ("AddMedia.PastePrompt","Pega la URL de un vídeo o una lista de YouTube."),("AddMedia.NothingFound","No se encontró nada. Comprueba el enlace e inténtalo de nuevo."),("AddMedia.OneReady","1 elemento multimedia listo. Edita las opciones antes de añadirlo."),("AddMedia.ManyReady","{0} elementos multimedia listos. Cada fila es independiente."),("AddMedia.AnalysisCancelled","Análisis cancelado."),("AddMedia.ErrorFallback","No se pudo analizar el enlace. Inténtalo de nuevo."),("Media.Untitled","Medios sin título")
    );

    private static readonly Dictionary<string, string> French = Spanish with
    {
        ["Shell.AddMedia"]="Ajouter un média",["Shell.AddFolder"]="Ajouter un dossier",["Shell.Home"]="Accueil",["Shell.Library"]="Bibliothèque",["Shell.Rename"]="Renommer",["Shell.Delete"]="Supprimer",["Shell.Cancel"]="Annuler",
        ["Library.Overline"]="MA BIBLIOTHÈQUE",["Library.Title"]="Votre bibliothèque",["Library.Subtitle"]="Vos dossiers, votre organisation, un espace de travail apaisé.",["Library.Folder"]="DOSSIER",["Library.Location"]="EMPLACEMENT",["Library.Action"]="ACTION",["Library.Open"]="Ouvrir",["Library.EmptyTitle"]="Votre bibliothèque attend son premier dossier",["Library.EmptySubtitle"]="Choisissez un dossier à gérer dans DownTrack. Rien ne change sur le disque avant l’enregistrement.",
        ["Explorer.Overline"]="BIBLIOTHÈQUE / EXPLORATEUR",["Explorer.Name"]="NOM",["Explorer.Type"]="TYPE",["Explorer.State"]="ÉTAT",["Explorer.EmptyTitle"]="Ce dossier est vide",["Explorer.EmptySubtitle"]="Créez un dossier ou ajoutez un média. Les nouveaux éléments en attente apparaissent ici immédiatement.",
        ["Pending.Title"]="Modifications en attente",["Pending.Subtitle"]="Vérifiez tout avant toute écriture sur le disque.",
        ["AddMedia.Title"]="Ajouter un média",["AddMedia.Subtitle"]="Collez une vidéo ou une playlist, vérifiez chaque élément et ajoutez uniquement ce que vous voulez.",["AddMedia.UrlHint"]="Collez l’URL d’une vidéo ou d’une playlist YouTube",["AddMedia.Analyze"]="Analyser",["AddMedia.SelectAll"]="Tout sélectionner",["AddMedia.Mp3All"]="MP3 pour tous",["AddMedia.Format"]="FORMAT",["AddMedia.AudioQuality"]="QUALITÉ AUDIO",["AddMedia.VideoQuality"]="QUALITÉ VIDÉO",["AddMedia.NoMedia"]="Collez un lien YouTube",["AddMedia.NoMediaSubtitle"]="Aucun téléchargement pendant l’analyse. Les éléments sélectionnés deviennent des modifications en attente.",["AddMedia.Preparing"]="Préparation du média…",["AddMedia.PreparingSubtitle"]="Vérification du moteur multimédia et analyse du lien.",["AddMedia.Destination"]="DESTINATION",["AddMedia.Stage"]="Ajouter aux téléchargements",["AddMedia.Cancel"]="Annuler",
        ["Settings.Title"]="Paramètres",["Settings.Subtitle"]="Personnalisez DownTrack comme vous le souhaitez.",["Settings.Appearance"]="Apparence",["Settings.AppearanceSubtitle"]="Choisissez comment DownTrack suit le thème de Windows.",["Settings.Language"]="Langue",["Settings.LanguageDescription"]="Utilisez Automatique pour suivre la langue d’affichage de Windows ou choisissez une langue manuellement.",["Settings.Automatic"]="Automatique (Windows)",["Settings.ThemeLight"]="Clair",["Settings.ThemeDark"]="Sombre",["Settings.ThemeSystem"]="Système",["Settings.Updates"]="Mises à jour",["Settings.UpdatesSubtitle"]="Gardez l’application et les outils multimédias à jour.",["Settings.AppUpdates"]="DownTrack",["Settings.Check"]="Vérifier",["Settings.Update"]="Mettre à jour",["Settings.ToolUpdates"]="Outils multimédias",["Settings.AutoUpdate"]="Vérifier automatiquement les mises à jour de DownTrack",["Settings.AutoToolUpdate"]="Garder automatiquement les outils multimédias à jour",["Settings.Defaults"]="Paramètres de téléchargement",["Settings.DefaultsSubtitle"]="Ces valeurs sont utilisées lors de l’analyse de nouveaux médias.",["Settings.DefaultFormat"]="Format par défaut",["Settings.DefaultAudioQuality"]="Qualité audio par défaut",["Settings.DefaultVideoQuality"]="Qualité vidéo par défaut",["Settings.Cancel"]="Annuler",["Settings.Save"]="Enregistrer les paramètres",["Settings.ToolsReady"]="Les outils multimédias sont prêts.",["Settings.ToolsMissing"]="Un ou plusieurs outils multimédias sont manquants.",["Settings.ToolsUpdated"]="Outils multimédias mis à jour."
    };

    private static readonly Dictionary<string, string> German = Spanish with
    {
        ["Shell.AddMedia"]="Medien hinzufügen",["Shell.AddFolder"]="Ordner hinzufügen",["Shell.Home"]="Startseite",["Shell.Library"]="Bibliothek",["Shell.Rename"]="Umbenennen",["Shell.Delete"]="Löschen",["Shell.Cancel"]="Abbrechen",
        ["Library.Overline"]="MEINE BIBLIOTHEK",["Library.Title"]="Deine Bibliothek",["Library.Subtitle"]="Deine Ordner, deine Organisation, ein ruhiger Arbeitsbereich.",["Library.Folder"]="ORDNER",["Library.Location"]="ORT",["Library.Action"]="AKTION",["Library.Open"]="Öffnen",["Library.EmptyTitle"]="Deine Bibliothek wartet auf ihren ersten Ordner",["Library.EmptySubtitle"]="Wähle einen Ordner zur Verwaltung in DownTrack. Erst beim Speichern wird die Festplatte geändert.",
        ["Explorer.Overline"]="BIBLIOTHEK / EXPLORER",["Explorer.Name"]="NAME",["Explorer.Type"]="TYP",["Explorer.State"]="STATUS",["Explorer.EmptyTitle"]="Dieser Ordner ist leer",["Explorer.EmptySubtitle"]="Erstelle einen Ordner oder füge Medien hinzu. Neue ausstehende Elemente erscheinen sofort hier.",
        ["Pending.Title"]="Ausstehende Änderungen",["Pending.Subtitle"]="Prüfe alles, bevor etwas auf die Festplatte geschrieben wird.",
        ["AddMedia.Title"]="Medien hinzufügen",["AddMedia.Subtitle"]="Füge ein Video oder eine Playlist ein, prüfe jeden Eintrag und füge nur hinzu, was du möchtest.",["AddMedia.UrlHint"]="YouTube-Video- oder Playlist-URL einfügen",["AddMedia.Analyze"]="Analysieren",["AddMedia.SelectAll"]="Alle auswählen",["AddMedia.Mp3All"]="MP3 für alle",["AddMedia.Format"]="FORMAT",["AddMedia.AudioQuality"]="AUDIOQUALITÄT",["AddMedia.VideoQuality"]="VIDEOQUALITÄT",["AddMedia.NoMedia"]="YouTube-Link einfügen",["AddMedia.NoMediaSubtitle"]="Während der Analyse wird nichts heruntergeladen. Ausgewählte Elemente werden vorgemerkt.",["AddMedia.Preparing"]="Medien werden vorbereitet…",["AddMedia.PreparingSubtitle"]="Medien-Engine wird geprüft und Link analysiert.",["AddMedia.Destination"]="ZIEL",["AddMedia.Stage"]="Zu Downloads hinzufügen",["AddMedia.Cancel"]="Abbrechen",
        ["Settings.Title"]="Einstellungen",["Settings.Subtitle"]="Passe DownTrack genau an deine Wünsche an.",["Settings.Appearance"]="Darstellung",["Settings.AppearanceSubtitle"]="Lege fest, wie DownTrack dem Windows-Design folgt.",["Settings.Language"]="Sprache",["Settings.LanguageDescription"]="Automatisch folgt der Windows-Anzeigesprache, oder du wählst eine Sprache manuell.",["Settings.Automatic"]="Automatisch (Windows)",["Settings.ThemeLight"]="Hell",["Settings.ThemeDark"]="Dunkel",["Settings.ThemeSystem"]="System",["Settings.Updates"]="Updates",["Settings.UpdatesSubtitle"]="Halte App und Medienwerkzeuge aktuell.",["Settings.AppUpdates"]="DownTrack",["Settings.Check"]="Prüfen",["Settings.Update"]="Aktualisieren",["Settings.ToolUpdates"]="Medienwerkzeuge",["Settings.AutoUpdate"]="Automatisch nach DownTrack-Updates suchen",["Settings.AutoToolUpdate"]="Medienwerkzeuge automatisch aktuell halten",["Settings.Defaults"]="Download-Standards",["Settings.DefaultsSubtitle"]="Diese Werte werden bei neuen Medien verwendet.",["Settings.DefaultFormat"]="Standardformat",["Settings.DefaultAudioQuality"]="Standard-Audioqualität",["Settings.DefaultVideoQuality"]="Standard-Videoqualität",["Settings.Cancel"]="Abbrechen",["Settings.Save"]="Einstellungen speichern",["Settings.ToolsReady"]="Medienwerkzeuge sind bereit.",["Settings.ToolsMissing"]="Mindestens ein Medienwerkzeug fehlt.",["Settings.ToolsUpdated"]="Medienwerkzeuge aktualisiert."
    };

    private static readonly Dictionary<string, string> Italian = Spanish with
    {
        ["Shell.AddMedia"]="Aggiungi media",["Shell.AddFolder"]="Aggiungi cartella",["Shell.Home"]="Home",["Shell.Library"]="Libreria",["Shell.Rename"]="Rinomina",["Shell.Delete"]="Elimina",["Shell.Cancel"]="Annulla",
        ["Library.Overline"]="LA MIA LIBRERIA",["Library.Title"]="La tua libreria",["Library.Subtitle"]="Le tue cartelle, la tua organizzazione, uno spazio di lavoro sereno.",["Library.Folder"]="CARTELLA",["Library.Location"]="POSIZIONE",["Library.Action"]="AZIONE",["Library.Open"]="Apri",["Library.EmptyTitle"]="La tua libreria è pronta per la prima cartella",["Library.EmptySubtitle"]="Scegli una cartella da gestire in DownTrack. Nulla cambia sul disco finché non salvi.",
        ["Explorer.Overline"]="LIBRERIA / ESPLORA",["Explorer.Name"]="NOME",["Explorer.Type"]="TIPO",["Explorer.State"]="STATO",["Explorer.EmptyTitle"]="Questa cartella è vuota",["Explorer.EmptySubtitle"]="Crea una cartella o aggiungi media. I nuovi elementi in sospeso appariranno subito qui.",
        ["Pending.Title"]="Modifiche in sospeso",["Pending.Subtitle"]="Controlla tutto prima di scrivere sul disco.",
        ["AddMedia.Title"]="Aggiungi media",["AddMedia.Subtitle"]="Incolla un video o una playlist, controlla ogni elemento e aggiungi solo ciò che vuoi.",["AddMedia.UrlHint"]="Incolla l’URL di un video o di una playlist YouTube",["AddMedia.Analyze"]="Analizza",["AddMedia.SelectAll"]="Seleziona tutto",["AddMedia.Mp3All"]="MP3 per tutti",["AddMedia.Format"]="FORMATO",["AddMedia.AudioQuality"]="QUALITÀ AUDIO",["AddMedia.VideoQuality"]="QUALITÀ VIDEO",["AddMedia.NoMedia"]="Incolla un link YouTube",["AddMedia.NoMediaSubtitle"]="Durante l’analisi non viene scaricato nulla. Gli elementi selezionati diventano modifiche in sospeso.",["AddMedia.Preparing"]="Preparazione media…",["AddMedia.PreparingSubtitle"]="Controllo del motore multimediale e analisi del link.",["AddMedia.Destination"]="DESTINAZIONE",["AddMedia.Stage"]="Aggiungi ai download",["AddMedia.Cancel"]="Annulla",
        ["Settings.Title"]="Impostazioni",["Settings.Subtitle"]="Configura DownTrack esattamente come preferisci.",["Settings.Appearance"]="Aspetto",["Settings.AppearanceSubtitle"]="Scegli come DownTrack segue il tema di Windows.",["Settings.Language"]="Lingua",["Settings.LanguageDescription"]="Usa Automatico per seguire la lingua di Windows oppure scegli manualmente.",["Settings.Automatic"]="Automatico (Windows)",["Settings.ThemeLight"]="Chiaro",["Settings.ThemeDark"]="Scuro",["Settings.ThemeSystem"]="Sistema",["Settings.Updates"]="Aggiornamenti",["Settings.UpdatesSubtitle"]="Mantieni aggiornati l’app e gli strumenti multimediali.",["Settings.AppUpdates"]="DownTrack",["Settings.Check"]="Controlla",["Settings.Update"]="Aggiorna",["Settings.ToolUpdates"]="Strumenti multimediali",["Settings.AutoUpdate"]="Controlla automaticamente gli aggiornamenti di DownTrack",["Settings.AutoToolUpdate"]="Mantieni automaticamente aggiornati gli strumenti multimediali",["Settings.Defaults"]="Predefiniti download",["Settings.DefaultsSubtitle"]="Questi valori vengono usati per i nuovi media.",["Settings.DefaultFormat"]="Formato predefinito",["Settings.DefaultAudioQuality"]="Qualità audio predefinita",["Settings.DefaultVideoQuality"]="Qualità video predefinita",["Settings.Cancel"]="Annulla",["Settings.Save"]="Salva impostazioni",["Settings.ToolsReady"]="Gli strumenti multimediali sono pronti.",["Settings.ToolsMissing"]="Manca uno o più strumenti multimediali.",["Settings.ToolsUpdated"]="Strumenti multimediali aggiornati."
    };

    private static readonly Dictionary<string, string> Portuguese = Spanish with
    {
        ["Shell.AddMedia"]="Adicionar mídia",["Shell.AddFolder"]="Adicionar pasta",["Shell.Home"]="Início",["Shell.Library"]="Biblioteca",["Shell.Rename"]="Renomear",["Shell.Delete"]="Excluir",["Shell.Cancel"]="Cancelar",
        ["Library.Overline"]="MINHA BIBLIOTECA",["Library.Title"]="Sua biblioteca",["Library.Subtitle"]="Suas pastas, sua organização, um espaço de trabalho tranquilo.",["Library.Folder"]="PASTA",["Library.Location"]="LOCAL",["Library.Action"]="AÇÃO",["Library.Open"]="Abrir",["Library.EmptyTitle"]="Sua biblioteca está pronta para a primeira pasta",["Library.EmptySubtitle"]="Escolha uma pasta para gerenciar no DownTrack. Nada muda no disco até você salvar.",
        ["Explorer.Overline"]="BIBLIOTECA / EXPLORADOR",["Explorer.Name"]="NOME",["Explorer.Type"]="TIPO",["Explorer.State"]="ESTADO",["Explorer.EmptyTitle"]="Esta pasta está vazia",["Explorer.EmptySubtitle"]="Crie uma pasta ou adicione mídia. Novos itens pendentes aparecem aqui imediatamente.",
        ["Pending.Title"]="Alterações pendentes",["Pending.Subtitle"]="Revise tudo antes de alterar o disco.",
        ["AddMedia.Title"]="Adicionar mídia",["AddMedia.Subtitle"]="Cole um vídeo ou playlist, revise cada item e adicione somente o que quiser.",["AddMedia.UrlHint"]="Cole o URL de um vídeo ou playlist do YouTube",["AddMedia.Analyze"]="Analisar",["AddMedia.SelectAll"]="Selecionar tudo",["AddMedia.Mp3All"]="MP3 para todos",["AddMedia.Format"]="FORMATO",["AddMedia.AudioQuality"]="QUALIDADE DE ÁUDIO",["AddMedia.VideoQuality"]="QUALIDADE DE VÍDEO",["AddMedia.NoMedia"]="Cole um link do YouTube",["AddMedia.NoMediaSubtitle"]="Nada é baixado durante a análise. Os itens selecionados viram alterações pendentes.",["AddMedia.Preparing"]="Preparando mídia…",["AddMedia.PreparingSubtitle"]="Verificando o mecanismo de mídia e analisando o link.",["AddMedia.Destination"]="DESTINO",["AddMedia.Stage"]="Adicionar aos downloads",["AddMedia.Cancel"]="Cancelar",
        ["Settings.Title"]="Configurações",["Settings.Subtitle"]="Personalize o DownTrack exatamente como quiser.",["Settings.Appearance"]="Aparência",["Settings.AppearanceSubtitle"]="Escolha como o DownTrack segue o tema do Windows.",["Settings.Language"]="Idioma",["Settings.LanguageDescription"]="Use Automático para seguir o idioma do Windows ou escolha manualmente.",["Settings.Automatic"]="Automático (Windows)",["Settings.ThemeLight"]="Claro",["Settings.ThemeDark"]="Escuro",["Settings.ThemeSystem"]="Sistema",["Settings.Updates"]="Atualizações",["Settings.UpdatesSubtitle"]="Mantenha o aplicativo e as ferramentas de mídia atualizados.",["Settings.AppUpdates"]="DownTrack",["Settings.Check"]="Verificar",["Settings.Update"]="Atualizar",["Settings.ToolUpdates"]="Ferramentas de mídia",["Settings.AutoUpdate"]="Verificar automaticamente atualizações do DownTrack",["Settings.AutoToolUpdate"]="Manter automaticamente as ferramentas de mídia atualizadas",["Settings.Defaults"]="Padrões de download",["Settings.DefaultsSubtitle"]="Esses valores são usados ao analisar novas mídias.",["Settings.DefaultFormat"]="Formato padrão",["Settings.DefaultAudioQuality"]="Qualidade de áudio padrão",["Settings.DefaultVideoQuality"]="Qualidade de vídeo padrão",["Settings.Cancel"]="Cancelar",["Settings.Save"]="Salvar configurações",["Settings.ToolsReady"]="As ferramentas de mídia estão prontas.",["Settings.ToolsMissing"]="Uma ou mais ferramentas de mídia estão faltando.",["Settings.ToolsUpdated"]="Ferramentas de mídia atualizadas."
    };

    private static readonly Dictionary<string, string> Dutch = Spanish;
    private static readonly Dictionary<string, string> Polish = Spanish;
    private static readonly Dictionary<string, string> Czech = Spanish;
    private static readonly Dictionary<string, string> Turkish = Spanish;
    private static readonly Dictionary<string, string> Ukrainian = Spanish;
    private static readonly Dictionary<string, string> Russian = Spanish;
    private static readonly Dictionary<string, string> Arabic = Hebrew;
    private static readonly Dictionary<string, string> Greek = Spanish;
    private static readonly Dictionary<string, string> Romanian = Spanish;
    private static readonly Dictionary<string, string> Japanese = Spanish;
    private static readonly Dictionary<string, string> Korean = Spanish;
    private static readonly Dictionary<string, string> ChineseSimplified = Spanish;
    private static readonly Dictionary<string, string> ChineseTraditional = Spanish;
}