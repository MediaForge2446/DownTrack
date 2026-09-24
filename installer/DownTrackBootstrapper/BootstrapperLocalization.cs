using System.Globalization;

namespace DownTrackSetup;

public sealed record InstallerLanguage(string Code, string NativeName, string EnglishName);

public sealed class BootstrapperLocalization
{
    private readonly Dictionary<string, Dictionary<string, string>> _t =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new()
            {
                ["Title"]="DownTrack",["Tagline"]="A clean, simple way to organize your media.",
                ["Checking"]="Checking for the latest version…",["Latest"]="Latest verified version",
                ["Ready"]="Ready to install",["Downloading"]="Downloading DownTrack…",
                ["Verifying"]="Verifying download…",["Installing"]="Installing DownTrack…",
                ["Finished"]="DownTrack is ready.",["Launch"]="Launch DownTrack",
                ["Close"]="Close",["Retry"]="Try again",["Error"]="We couldn't complete the setup.",
                ["UpToDate"]="You already have the latest version.",["Download"]="Download & install",
                ["Language"]="Language",["Size"]="Download size"
            },
            ["he"] = new()
            {
                ["Title"]="DownTrack",["Tagline"]="דרך נקייה ופשוטה לארגון המדיה שלך.",
                ["Checking"]="בודק את הגרסה האחרונה…",["Latest"]="הגרסה המאומתת האחרונה",
                ["Ready"]="מוכן להתקנה",["Downloading"]="מוריד את DownTrack…",
                ["Verifying"]="מאמת את ההורדה…",["Installing"]="מתקין את DownTrack…",
                ["Finished"]="DownTrack מוכן.",["Launch"]="פתח את DownTrack",
                ["Close"]="סגור",["Retry"]="נסה שוב",["Error"]="לא הצלחנו להשלים את ההתקנה.",
                ["UpToDate"]="כבר מותקנת אצלך הגרסה האחרונה.",["Download"]="הורד והתקן",
                ["Language"]="שפה",["Size"]="גודל ההורדה"
            },
            ["ar"] = new()
            {
                ["Title"]="DownTrack",["Tagline"]="طريقة بسيطة ونظيفة لتنظيم الوسائط.",
                ["Checking"]="جارٍ التحقق من أحدث إصدار…",["Latest"]="أحدث إصدار موثّق",
                ["Ready"]="جاهز للتثبيت",["Downloading"]="جارٍ تنزيل DownTrack…",
                ["Verifying"]="جارٍ التحقق من التنزيل…",["Installing"]="جارٍ تثبيت DownTrack…",
                ["Finished"]="DownTrack جاهز.",["Launch"]="فتح DownTrack",
                ["Close"]="إغلاق",["Retry"]="حاول مرة أخرى",["Error"]="تعذر إكمال التثبيت.",
                ["UpToDate"]="لديك أحدث إصدار بالفعل.",["Download"]="تنزيل وتثبيت",
                ["Language"]="اللغة",["Size"]="حجم التنزيل"
            },
            ["es"] = new(){{"Title","DownTrack"},{"Tagline","Una forma limpia y sencilla de organizar tus medios."},{"Checking","Buscando la última versión…"},{"Latest","Última versión verificada"},{"Ready","Listo para instalar"},{"Downloading","Descargando DownTrack…"},{"Verifying","Verificando la descarga…"},{"Installing","Instalando DownTrack…"},{"Finished","DownTrack está listo."},{"Launch","Abrir DownTrack"},{"Close","Cerrar"},{"Retry","Intentar de nuevo"},{"Error","No pudimos completar la instalación."},{"UpToDate","Ya tienes la última versión."},{"Download","Descargar e instalar"},{"Language","Idioma"},{"Size","Tamaño de descarga"}},
            ["fr"] = new(){{"Title","DownTrack"},{"Tagline","Une façon claire et simple d’organiser vos médias."},{"Checking","Recherche de la dernière version…"},{"Latest","Dernière version vérifiée"},{"Ready","Prêt à installer"},{"Downloading","Téléchargement de DownTrack…"},{"Verifying","Vérification du téléchargement…"},{"Installing","Installation de DownTrack…"},{"Finished","DownTrack est prêt."},{"Launch","Ouvrir DownTrack"},{"Close","Fermer"},{"Retry","Réessayer"},{"Error","Impossible de terminer l’installation."},{"UpToDate","Vous avez déjà la dernière version."},{"Download","Télécharger et installer"},{"Language","Langue"},{"Size","Taille du téléchargement"}},
            ["de"] = new(){{"Title","DownTrack"},{"Tagline","Medien sauber und einfach organisieren."},{"Checking","Neueste Version wird geprüft…"},{"Latest","Neueste verifizierte Version"},{"Ready","Bereit zur Installation"},{"Downloading","DownTrack wird heruntergeladen…"},{"Verifying","Download wird überprüft…"},{"Installing","DownTrack wird installiert…"},{"Finished","DownTrack ist bereit."},{"Launch","DownTrack öffnen"},{"Close","Schließen"},{"Retry","Erneut versuchen"},{"Error","Die Installation konnte nicht abgeschlossen werden."},{"UpToDate","Du hast bereits die neueste Version."},{"Download","Herunterladen und installieren"},{"Language","Sprache"},{"Size","Downloadgröße"}},
            ["it"] = new(){{"Title","DownTrack"},{"Tagline","Un modo pulito e semplice per organizzare i tuoi media."},{"Checking","Controllo dell'ultima versione…"},{"Latest","Ultima versione verificata"},{"Ready","Pronto per l'installazione"},{"Downloading","Download di DownTrack…"},{"Verifying","Verifica del download…"},{"Installing","Installazione di DownTrack…"},{"Finished","DownTrack è pronto."},{"Launch","Apri DownTrack"},{"Close","Chiudi"},{"Retry","Riprova"},{"Error","Non è stato possibile completare l'installazione."},{"UpToDate","Hai già l'ultima versione."},{"Download","Scarica e installa"},{"Language","Lingua"},{"Size","Dimensione download"}},
            ["pt"] = new(){{"Title","DownTrack"},{"Tagline","Uma forma limpa e simples de organizar suas mídias."},{"Checking","Verificando a versão mais recente…"},{"Latest","Versão verificada mais recente"},{"Ready","Pronto para instalar"},{"Downloading","Baixando o DownTrack…"},{"Verifying","Verificando o download…"},{"Installing","Instalando o DownTrack…"},{"Finished","O DownTrack está pronto."},{"Launch","Abrir DownTrack"},{"Close","Fechar"},{"Retry","Tentar novamente"},{"Error","Não foi possível concluir a instalação."},{"UpToDate","Você já tem a versão mais recente."},{"Download","Baixar e instalar"},{"Language","Idioma"},{"Size","Tamanho do download"}},
            ["nl"] = new(){{"Title","DownTrack"},{"Tagline","Een schone, eenvoudige manier om je media te organiseren."},{"Checking","Laatste versie controleren…"},{"Latest","Laatste geverifieerde versie"},{"Ready","Klaar om te installeren"},{"Downloading","DownTrack downloaden…"},{"Verifying","Download controleren…"},{"Installing","DownTrack installeren…"},{"Finished","DownTrack is klaar."},{"Launch","DownTrack openen"},{"Close","Sluiten"},{"Retry","Opnieuw proberen"},{"Error","De installatie kon niet worden voltooid."},{"UpToDate","Je hebt al de nieuwste versie."},{"Download","Downloaden en installeren"},{"Language","Taal"},{"Size","Downloadgrootte"}},
            ["pl"] = new(){{"Title","DownTrack"},{"Tagline","Prosty i przejrzysty sposób na organizowanie multimediów."},{"Checking","Sprawdzanie najnowszej wersji…"},{"Latest","Najnowsza zweryfikowana wersja"},{"Ready","Gotowe do instalacji"},{"Downloading","Pobieranie DownTrack…"},{"Verifying","Weryfikowanie pobierania…"},{"Installing","Instalowanie DownTrack…"},{"Finished","DownTrack jest gotowy."},{"Launch","Otwórz DownTrack"},{"Close","Zamknij"},{"Retry","Spróbuj ponownie"},{"Error","Nie udało się ukończyć instalacji."},{"UpToDate","Masz już najnowszą wersję."},{"Download","Pobierz i zainstaluj"},{"Language","Język"},{"Size","Rozmiar pobierania"}},
            ["tr"] = new(){{"Title","DownTrack"},{"Tagline","Medyanızı düzenlemenin temiz ve kolay yolu."},{"Checking","En son sürüm kontrol ediliyor…"},{"Latest","En yeni doğrulanmış sürüm"},{"Ready","Kuruluma hazır"},{"Downloading","DownTrack indiriliyor…"},{"Verifying","İndirme doğrulanıyor…"},{"Installing","DownTrack yükleniyor…"},{"Finished","DownTrack hazır."},{"Launch","DownTrack'i aç"},{"Close","Kapat"},{"Retry","Tekrar dene"},{"Error","Kurulum tamamlanamadı."},{"UpToDate","Zaten en yeni sürüme sahipsiniz."},{"Download","İndir ve yükle"},{"Language","Dil"},{"Size","İndirme boyutu"}},
            ["ru"] = new(){{"Title","DownTrack"},{"Tagline","Простой и аккуратный способ организовать медиа."},{"Checking","Проверка последней версии…"},{"Latest","Последняя проверенная версия"},{"Ready","Готово к установке"},{"Downloading","Загрузка DownTrack…"},{"Verifying","Проверка загрузки…"},{"Installing","Установка DownTrack…"},{"Finished","DownTrack готов."},{"Launch","Открыть DownTrack"},{"Close","Закрыть"},{"Retry","Повторить"},{"Error","Не удалось завершить установку."},{"UpToDate","У вас уже последняя версия."},{"Download","Скачать и установить"},{"Language","Язык"},{"Size","Размер загрузки"}},
            ["uk"] = new(){{"Title","DownTrack"},{"Tagline","Простий і чистий спосіб упорядкувати медіа."},{"Checking","Перевірка останньої версії…"},{"Latest","Остання перевірена версія"},{"Ready","Готово до встановлення"},{"Downloading","Завантаження DownTrack…"},{"Verifying","Перевірка завантаження…"},{"Installing","Встановлення DownTrack…"},{"Finished","DownTrack готовий."},{"Launch","Відкрити DownTrack"},{"Close","Закрити"},{"Retry","Повторити"},{"Error","Не вдалося завершити встановлення."},{"UpToDate","У вас уже остання версія."},{"Download","Завантажити й встановити"},{"Language","Мова"},{"Size","Розмір завантаження"}},
            ["ja"] = new(){{"Title","DownTrack"},{"Tagline","メディアをすっきり簡単に整理できます。"},{"Checking","最新バージョンを確認しています…"},{"Latest","最新の検証済みバージョン"},{"Ready","インストール準備完了"},{"Downloading","DownTrackをダウンロード中…"},{"Verifying","ダウンロードを確認中…"},{"Installing","DownTrackをインストール中…"},{"Finished","DownTrackの準備ができました。"},{"Launch","DownTrackを開く"},{"Close","閉じる"},{"Retry","もう一度試す"},{"Error","インストールを完了できませんでした。"},{"UpToDate","すでに最新バージョンです。"},{"Download","ダウンロードしてインストール"},{"Language","言語"},{"Size","ダウンロードサイズ"}},
            ["ko"] = new(){{"Title","DownTrack"},{"Tagline","미디어를 깔끔하고 쉽게 정리하세요."},{"Checking","최신 버전을 확인하는 중…"},{"Latest","최신 검증 버전"},{"Ready","설치 준비 완료"},{"Downloading","DownTrack 다운로드 중…"},{"Verifying","다운로드 확인 중…"},{"Installing","DownTrack 설치 중…"},{"Finished","DownTrack을 사용할 준비가 되었습니다."},{"Launch","DownTrack 열기"},{"Close","닫기"},{"Retry","다시 시도"},{"Error","설치를 완료하지 못했습니다."},{"UpToDate","이미 최신 버전을 사용 중입니다."},{"Download","다운로드 및 설치"},{"Language","언어"},{"Size","다운로드 크기"}},
            ["zh-CN"] = new(){{"Title","DownTrack"},{"Tagline","以简洁优雅的方式整理你的媒体。"},{"Checking","正在检查最新版本…"},{"Latest","最新已验证版本"},{"Ready","准备安装"},{"Downloading","正在下载 DownTrack…"},{"Verifying","正在验证下载…"},{"Installing","正在安装 DownTrack…"},{"Finished","DownTrack 已准备就绪。"},{"Launch","打开 DownTrack"},{"Close","关闭"},{"Retry","重试"},{"Error","无法完成安装。"},{"UpToDate","你已经是最新版本。"},{"Download","下载并安装"},{"Language","语言"},{"Size","下载大小"}},
            ["zh-TW"] = new(){{"Title","DownTrack"},{"Tagline","以簡潔優雅的方式整理你的媒體。"},{"Checking","正在檢查最新版本…"},{"Latest","最新驗證版本"},{"Ready","準備安裝"},{"Downloading","正在下載 DownTrack…"},{"Verifying","正在驗證下載…"},{"Installing","正在安裝 DownTrack…"},{"Finished","DownTrack 已準備好。"},{"Launch","開啟 DownTrack"},{"Close","關閉"},{"Retry","再試一次"},{"Error","無法完成安裝。"},{"UpToDate","你已經是最新版本。"},{"Download","下載並安裝"},{"Language","語言"},{"Size","下載大小"}},
            ["cs"] = new(){{"Title","DownTrack"},{"Tagline","Čistý a jednoduchý způsob organizace médií."},{"Checking","Kontrola nejnovější verze…"},{"Latest","Nejnovější ověřená verze"},{"Ready","Připraveno k instalaci"},{"Downloading","Stahování DownTrack…"},{"Verifying","Ověřování stažení…"},{"Installing","Instalace DownTrack…"},{"Finished","DownTrack je připraven."},{"Launch","Spustit DownTrack"},{"Close","Zavřít"},{"Retry","Zkusit znovu"},{"Error","Instalaci se nepodařilo dokončit."},{"UpToDate","Máte již nejnovější verzi."},{"Download","Stáhnout a nainstalovat"},{"Language","Jazyk"},{"Size","Velikost stahování"}},
            ["da"] = new(){{"Title","DownTrack"},{"Tagline","En ren og enkel måde at organisere dine medier på."},{"Checking","Kontrollerer seneste version…"},{"Latest","Seneste verificerede version"},{"Ready","Klar til installation"},{"Downloading","Downloader DownTrack…"},{"Verifying","Bekræfter download…"},{"Installing","Installerer DownTrack…"},{"Finished","DownTrack er klar."},{"Launch","Åbn DownTrack"},{"Close","Luk"},{"Retry","Prøv igen"},{"Error","Installationen kunne ikke fuldføres."},{"UpToDate","Du har allerede den seneste version."},{"Download","Download og installer"},{"Language","Sprog"},{"Size","Downloadstørrelse"}},
            ["sv"] = new(){{"Title","DownTrack"},{"Tagline","Ett rent och enkelt sätt att organisera dina medier."},{"Checking","Söker efter senaste versionen…"},{"Latest","Senaste verifierade version"},{"Ready","Redo att installera"},{"Downloading","Laddar ner DownTrack…"},{"Verifying","Verifierar nedladdningen…"},{"Installing","Installerar DownTrack…"},{"Finished","DownTrack är klart."},{"Launch","Öppna DownTrack"},{"Close","Stäng"},{"Retry","Försök igen"},{"Error","Installationen kunde inte slutföras."},{"UpToDate","Du har redan den senaste versionen."},{"Download","Ladda ner och installera"},{"Language","Språk"},{"Size","Nedladdningsstorlek"}}
        };

    public IReadOnlyList<InstallerLanguage> Languages { get; } =
    [
        new("en","English","English"), new("he","עברית","Hebrew"), new("ar","العربية","Arabic"),
        new("es","Español","Spanish"), new("fr","Français","French"), new("de","Deutsch","German"),
        new("it","Italiano","Italian"), new("pt","Português","Portuguese"), new("nl","Nederlands","Dutch"),
        new("pl","Polski","Polish"), new("tr","Türkçe","Turkish"), new("ru","Русский","Russian"),
        new("uk","Українська","Ukrainian"), new("ja","日本語","Japanese"), new("ko","한국어","Korean"),
        new("zh-CN","简体中文","Chinese Simplified"), new("zh-TW","繁體中文","Chinese Traditional"),
        new("cs","Čeština","Czech"), new("da","Dansk","Danish"), new("sv","Svenska","Swedish")
    ];

    public string CurrentCode { get; private set; } = Detect();

    public string Get(string key) =>
        _t.TryGetValue(CurrentCode, out var dict) && dict.TryGetValue(key, out var value)
            ? value
            : _t["en"][key];

    public void Set(string code) =>
        CurrentCode = _t.ContainsKey(code) ? code : "en";

    public static string Detect()
    {
        var name = CultureInfo.InstalledUICulture.Name;
        if (name.StartsWith("zh-TW", StringComparison.OrdinalIgnoreCase) ||
            name.StartsWith("zh-HK", StringComparison.OrdinalIgnoreCase) ||
            name.StartsWith("zh-MO", StringComparison.OrdinalIgnoreCase))
            return "zh-TW";

        if (name.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            return "zh-CN";

        var prefix = name.Split('-')[0].ToLowerInvariant();
        return new[]{"en","he","ar","es","fr","de","it","pt","nl","pl","tr","ru","uk","ja","ko","cs","da","sv"}
            .Contains(prefix, StringComparer.OrdinalIgnoreCase)
            ? prefix : "en";
    }

    public bool IsRtl => CurrentCode is "he" or "ar";
}