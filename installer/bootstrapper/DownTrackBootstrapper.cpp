#define UNICODE
#define _UNICODE
#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winhttp.h>
#include <bcrypt.h>
#include <shellapi.h>
#include <shlwapi.h>
#include <shlobj.h>
#include <commctrl.h>

#include <atomic>
#include <cstdint>
#include <fstream>
#include <mutex>
#include <sstream>
#include <string>
#include <thread>
#include <vector>
#include <algorithm>
#include <functional>
#include <cwctype>
#include <iterator>

constexpr int ID_LANGUAGE = 1001;
constexpr int ID_INSTALL = 1002;
constexpr int ID_OPEN = 1003;
constexpr int ID_CLOSE = 1004;

#pragma comment(lib, "winhttp.lib")
#pragma comment(lib, "bcrypt.lib")
#pragma comment(lib, "shell32.lib")
#pragma comment(lib, "shlwapi.lib")
#pragma comment(lib, "comctl32.lib")

namespace {

constexpr wchar_t kWindowClass[] = L"DownTrackWebInstaller";
constexpr wchar_t kWindowTitle[] = L"DownTrack";
constexpr wchar_t kManifestUrl[] =
    L"https://github.com/MediaForge2446/DownTrack/releases/download/nightly/latest.ini";

constexpr UINT WM_APP_STATE = WM_APP + 10;
constexpr UINT WM_APP_FINISHED = WM_APP + 11;

enum class ScreenState {
    Checking,
    Downloading,
    Verifying,
    Installing,
    Complete,
    UpToDate,
    Error
};

struct Language {
    const wchar_t* code;
    const wchar_t* name;
    const wchar_t* tagline;
    const wchar_t* checking;
    const wchar_t* downloading;
    const wchar_t* verifying;
    const wchar_t* installing;
    const wchar_t* complete;
    const wchar_t* upToDate;
    const wchar_t* error;
    const wchar_t* start;
    const wchar_t* open;
    const wchar_t* close;
    const wchar_t* retry;
    const wchar_t* moments;
};

const Language kLanguages[] = {
    {L"auto", L"Automatic (Windows)", L"Your media. Your library. Always up to date.",
     L"Checking for the latest version…", L"Downloading DownTrack…", L"Verifying your download…",
     L"Installing DownTrack…", L"DownTrack is ready.", L"You already have the latest version.",
     L"We couldn’t complete the installation.", L"Install latest version", L"Open DownTrack", L"Close",
     L"Try again", L"This will only take a few moments."},

    {L"en", L"English", L"Your media. Your library. Always up to date.",
     L"Checking for the latest version…", L"Downloading DownTrack…", L"Verifying your download…",
     L"Installing DownTrack…", L"DownTrack is ready.", L"You already have the latest version.",
     L"We couldn’t complete the installation.", L"Install latest version", L"Open DownTrack", L"Close",
     L"Try again", L"This will only take a few moments."},

    {L"he", L"עברית", L"המדיה שלך. הספרייה שלך. תמיד מעודכנת.",
     L"בודק את הגרסה העדכנית ביותר…", L"מוריד את DownTrack…", L"מאמת את ההורדה…",
     L"מתקין את DownTrack…", L"DownTrack מוכן.", L"הגרסה העדכנית ביותר כבר מותקנת.",
     L"לא ניתן היה להשלים את ההתקנה.", L"התקן את הגרסה האחרונה", L"פתח את DownTrack", L"סגור",
     L"נסה שוב", L"זה ייקח רק כמה רגעים."},

    {L"es", L"Español", L"Tus medios. Tu biblioteca. Siempre actualizada.",
     L"Comprobando la última versión…", L"Descargando DownTrack…", L"Verificando la descarga…",
     L"Instalando DownTrack…", L"DownTrack está listo.", L"Ya tienes la última versión.",
     L"No se pudo completar la instalación.", L"Instalar la última versión", L"Abrir DownTrack", L"Cerrar",
     L"Intentar de nuevo", L"Esto solo tardará unos momentos."},

    {L"fr", L"Français", L"Vos médias. Votre bibliothèque. Toujours à jour.",
     L"Vérification de la dernière version…", L"Téléchargement de DownTrack…", L"Vérification du téléchargement…",
     L"Installation de DownTrack…", L"DownTrack est prêt.", L"Vous avez déjà la dernière version.",
     L"Impossible de terminer l’installation.", L"Installer la dernière version", L"Ouvrir DownTrack", L"Fermer",
     L"Réessayer", L"Cela ne prendra que quelques instants."},

    {L"de", L"Deutsch", L"Deine Medien. Deine Bibliothek. Immer aktuell.",
     L"Neueste Version wird geprüft…", L"DownTrack wird heruntergeladen…", L"Download wird überprüft…",
     L"DownTrack wird installiert…", L"DownTrack ist bereit.", L"Die neueste Version ist bereits installiert.",
     L"Die Installation konnte nicht abgeschlossen werden.", L"Neueste Version installieren", L"DownTrack öffnen", L"Schließen",
     L"Erneut versuchen", L"Dies dauert nur wenige Augenblicke."},

    {L"it", L"Italiano", L"I tuoi contenuti. La tua libreria. Sempre aggiornata.",
     L"Controllo dell’ultima versione…", L"Download di DownTrack…", L"Verifica del download…",
     L"Installazione di DownTrack…", L"DownTrack è pronto.", L"Hai già l’ultima versione.",
     L"Impossibile completare l’installazione.", L"Installa l’ultima versione", L"Apri DownTrack", L"Chiudi",
     L"Riprova", L"Ci vorranno solo pochi istanti."},

    {L"pt", L"Português", L"Sua mídia. Sua biblioteca. Sempre atualizada.",
     L"Verificando a versão mais recente…", L"Baixando o DownTrack…", L"Verificando o download…",
     L"Instalando o DownTrack…", L"O DownTrack está pronto.", L"Você já tem a versão mais recente.",
     L"Não foi possível concluir a instalação.", L"Instalar a versão mais recente", L"Abrir o DownTrack", L"Fechar",
     L"Tentar novamente", L"Isso levará apenas alguns instantes."},

    {L"nl", L"Nederlands", L"Jouw media. Jouw bibliotheek. Altijd actueel.",
     L"Laatste versie controleren…", L"DownTrack downloaden…", L"Download controleren…",
     L"DownTrack installeren…", L"DownTrack is klaar.", L"Je hebt al de nieuwste versie.",
     L"De installatie kon niet worden voltooid.", L"Nieuwste versie installeren", L"DownTrack openen", L"Sluiten",
     L"Opnieuw proberen", L"Dit duurt maar een paar momenten."},

    {L"pl", L"Polski", L"Twoje media. Twoja biblioteka. Zawsze aktualne.",
     L"Sprawdzanie najnowszej wersji…", L"Pobieranie DownTrack…", L"Weryfikowanie pobranego pliku…",
     L"Instalowanie DownTrack…", L"DownTrack jest gotowy.", L"Masz już najnowszą wersję.",
     L"Nie udało się ukończyć instalacji.", L"Zainstaluj najnowszą wersję", L"Otwórz DownTrack", L"Zamknij",
     L"Spróbuj ponownie", L"To potrwa tylko chwilę."},

    {L"cs", L"Čeština", L"Vaše média. Vaše knihovna. Vždy aktuální.",
     L"Kontrola nejnovější verze…", L"Stahování DownTrack…", L"Ověřování stažení…",
     L"Instalace DownTrack…", L"DownTrack je připraven.", L"Již máte nejnovější verzi.",
     L"Instalaci se nepodařilo dokončit.", L"Instalovat nejnovější verzi", L"Otevřít DownTrack", L"Zavřít",
     L"Zkusit znovu", L"Zabere to jen pár okamžiků."},

    {L"tr", L"Türkçe", L"Medyanız. Kitaplığınız. Her zaman güncel.",
     L"En yeni sürüm kontrol ediliyor…", L"DownTrack indiriliyor…", L"İndirme doğrulanıyor…",
     L"DownTrack kuruluyor…", L"DownTrack hazır.", L"Zaten en son sürüme sahipsiniz.",
     L"Kurulum tamamlanamadı.", L"En son sürümü yükle", L"DownTrack'i aç", L"Kapat",
     L"Tekrar dene", L"Bu yalnızca birkaç dakika sürecek."},

    {L"uk", L"Українська", L"Ваші медіа. Ваша бібліотека. Завжди актуальні.",
     L"Перевірка останньої версії…", L"Завантаження DownTrack…", L"Перевірка завантаження…",
     L"Встановлення DownTrack…", L"DownTrack готовий.", L"У вас уже остання версія.",
     L"Не вдалося завершити встановлення.", L"Встановити останню версію", L"Відкрити DownTrack", L"Закрити",
     L"Спробувати ще раз", L"Це займе лише кілька хвилин."},

    {L"ru", L"Русский", L"Ваши медиа. Ваша библиотека. Всегда актуальны.",
     L"Проверка последней версии…", L"Загрузка DownTrack…", L"Проверка загрузки…",
     L"Установка DownTrack…", L"DownTrack готов.", L"У вас уже установлена последняя версия.",
     L"Не удалось завершить установку.", L"Установить последнюю версию", L"Открыть DownTrack", L"Закрыть",
     L"Повторить", L"Это займет всего несколько минут."},

    {L"ar", L"العربية", L"وسائطك. مكتبتك. محدثة دائمًا.",
     L"جارٍ التحقق من أحدث إصدار…", L"جارٍ تنزيل DownTrack…", L"جارٍ التحقق من التنزيل…",
     L"جارٍ تثبيت DownTrack…", L"DownTrack جاهز.", L"لديك بالفعل أحدث إصدار.",
     L"تعذر إكمال التثبيت.", L"تثبيت أحدث إصدار", L"فتح DownTrack", L"إغلاق",
     L"حاول مرة أخرى", L"لن يستغرق ذلك سوى بضع لحظات."},

    {L"el", L"Ελληνικά", L"Τα πολυμέσα σας. Η βιβλιοθήκη σας. Πάντα ενημερωμένα.",
     L"Έλεγχος της πιο πρόσφατης έκδοσης…", L"Λήψη DownTrack…", L"Επαλήθευση λήψης…",
     L"Εγκατάσταση DownTrack…", L"Το DownTrack είναι έτοιμο.", L"Έχετε ήδη την πιο πρόσφατη έκδοση.",
     L"Δεν ήταν δυνατή η ολοκλήρωση της εγκατάστασης.", L"Εγκατάσταση τελευταίας έκδοσης", L"Άνοιγμα DownTrack", L"Κλείσιμο",
     L"Δοκιμή ξανά", L"Θα χρειαστούν μόνο λίγα λεπτά."},

    {L"ro", L"Română", L"Media ta. Biblioteca ta. Mereu actualizată.",
     L"Se verifică cea mai recentă versiune…", L"Se descarcă DownTrack…", L"Se verifică descărcarea…",
     L"Se instalează DownTrack…", L"DownTrack este gata.", L"Aveți deja cea mai recentă versiune.",
     L"Instalarea nu a putut fi finalizată.", L"Instalează cea mai recentă versiune", L"Deschide DownTrack", L"Închide",
     L"Încearcă din nou", L"Va dura doar câteva momente."},

    {L"ja", L"日本語", L"あなたのメディア。あなたのライブラリ。いつでも最新。",
     L"最新バージョンを確認しています…", L"DownTrackをダウンロードしています…", L"ダウンロードを確認しています…",
     L"DownTrackをインストールしています…", L"DownTrackの準備ができました。", L"すでに最新バージョンです。",
     L"インストールを完了できませんでした。", L"最新バージョンをインストール", L"DownTrackを開く", L"閉じる",
     L"もう一度試す", L"完了まであと少しです。"},

    {L"ko", L"한국어", L"내 미디어. 내 라이브러리. 항상 최신 상태.",
     L"최신 버전을 확인하는 중…", L"DownTrack을 다운로드하는 중…", L"다운로드를 확인하는 중…",
     L"DownTrack을 설치하는 중…", L"DownTrack을 사용할 준비가 되었습니다.", L"이미 최신 버전이 설치되어 있습니다.",
     L"설치를 완료하지 못했습니다.", L"최신 버전 설치", L"DownTrack 열기", L"닫기",
     L"다시 시도", L"잠시만 기다려 주세요."},

    {L"zhcn", L"简体中文", L"你的媒体。你的媒体库。始终保持最新。",
     L"正在检查最新版本…", L"正在下载 DownTrack…", L"正在验证下载…",
     L"正在安装 DownTrack…", L"DownTrack 已准备就绪。", L"你已经拥有最新版本。",
     L"无法完成安装。", L"安装最新版本", L"打开 DownTrack", L"关闭",
     L"重试", L"只需等待片刻。"},

    {L"zhtw", L"繁體中文", L"你的媒體。你的資料庫。永遠保持最新。",
     L"正在檢查最新版本…", L"正在下載 DownTrack…", L"正在驗證下載…",
     L"正在安裝 DownTrack…", L"DownTrack 已準備就緒。", L"你已經擁有最新版本。",
     L"無法完成安裝。", L"安裝最新版本", L"開啟 DownTrack", L"關閉",
     L"重試", L"只需要稍候片刻。"}
};

constexpr size_t kLanguageCount = sizeof(kLanguages) / sizeof(kLanguages[0]);

HWND g_hwnd = nullptr;
HWND g_language = nullptr;
HWND g_primary = nullptr;
HWND g_secondary = nullptr;
HFONT g_font = nullptr;
HFONT g_fontBold = nullptr;
HBRUSH g_windowBrush = nullptr;
std::mutex g_mutex;

size_t g_languageIndex = 0;
ScreenState g_state = ScreenState::Checking;
int g_progress = 0;
std::wstring g_version;
std::wstring g_downloadDetail;
std::wstring g_errorDetail;
std::wstring g_payloadUrl;
std::wstring g_payloadHash;
uint64_t g_payloadSize = 0;
std::atomic<bool> g_busy{false};

std::wstring BaseDir() {
    wchar_t buffer[MAX_PATH]{};
    DWORD len = GetEnvironmentVariableW(L"LOCALAPPDATA", buffer, MAX_PATH);
    if (len == 0 || len >= MAX_PATH) {
        return L"";
    }
    return std::wstring(buffer);
}

std::wstring InstallerSettingsPath() {
    const auto base = BaseDir();
    return base.empty() ? L"" : base + L"\\DownTrack\\installer-language.ini";
}

void SaveLanguageMode(const std::wstring& code) {
    const auto path = InstallerSettingsPath();
    if (path.empty()) return;

    const auto slash = path.find_last_of(L"\\");
    if (slash != std::wstring::npos) {
        CreateDirectoryW(path.substr(0, slash).c_str(), nullptr);
    }
    WritePrivateProfileStringW(L"Installer", L"Language", code.c_str(), path.c_str());
}

std::wstring DetectLanguage() {
    wchar_t locale[LOCALE_NAME_MAX_LENGTH]{};
    if (GetUserDefaultLocaleName(locale, LOCALE_NAME_MAX_LENGTH) <= 0) {
        return L"en";
    }
    std::wstring value(locale);
    std::transform(value.begin(), value.end(), value.begin(),
                   [](wchar_t c) { return static_cast<wchar_t>(towlower(c)); });

    const auto dash = value.find(L'-');
    const auto code = value.substr(0, dash == std::wstring::npos ? value.size() : dash);

    for (size_t i = 1; i < kLanguageCount; ++i) {
        if (code == kLanguages[i].code) return code;
    }
    return L"en";
}

std::wstring LoadLanguageMode() {
    const auto path = InstallerSettingsPath();
    if (path.empty()) return L"auto";

    wchar_t buffer[32]{};
    GetPrivateProfileStringW(
        L"Installer", L"Language", L"auto", buffer, static_cast<DWORD>(std::size(buffer)), path.c_str());
    return buffer;
}

size_t FindLanguage(const std::wstring& code) {
    for (size_t i = 0; i < kLanguageCount; ++i) {
        if (code == kLanguages[i].code) return i;
    }
    return 0;
}

const Language& Lng() {
    return kLanguages[g_languageIndex];
}

bool IsRtl() {
    return std::wstring(kLanguages[g_languageIndex].code) == L"he" ||
           std::wstring(kLanguages[g_languageIndex].code) == L"ar";
}

void ApplyLanguage() {
    const auto selected = (g_languageIndex < kLanguageCount) ? g_languageIndex : 1;
    if (g_language) {
        SendMessageW(g_language, CB_SETCURSEL, static_cast<WPARAM>(selected), 0);
    }

    SetWindowTextW(g_primary, (g_state == ScreenState::Complete || g_state == ScreenState::UpToDate)
                               ? Lng().open : Lng().start);
    SetWindowTextW(g_secondary, Lng().close);
    SetWindowTextW(g_hwnd, kWindowTitle);
    InvalidateRect(g_hwnd, nullptr, FALSE);
}

void SetState(ScreenState state, int progress = -1, const std::wstring& detail = L"") {
    {
        std::lock_guard<std::mutex> lock(g_mutex);
        g_state = state;
        if (progress >= 0) g_progress = progress;
        if (!detail.empty()) g_downloadDetail = detail;
    }
    PostMessageW(g_hwnd, WM_APP_STATE, 0, 0);
}

std::wstring StateTitle(ScreenState state) {
    switch (state) {
        case ScreenState::Checking: return Lng().checking;
        case ScreenState::Downloading: return Lng().downloading;
        case ScreenState::Verifying: return Lng().verifying;
        case ScreenState::Installing: return Lng().installing;
        case ScreenState::Complete: return Lng().complete;
        case ScreenState::UpToDate: return Lng().upToDate;
        case ScreenState::Error: return Lng().error;
    }
    return Lng().checking;
}

std::wstring FormatMb(uint64_t value) {
    std::wstringstream ss;
    ss << (value / (1024ULL * 1024ULL));
    return ss.str();
}

bool ParseUrl(const std::wstring& url, std::wstring& host, std::wstring& path, INTERNET_PORT& port, bool& https) {
    URL_COMPONENTS c{};
    c.dwStructSize = sizeof(c);
    wchar_t hostBuffer[256]{};
    wchar_t pathBuffer[2048]{};
    c.lpszHostName = hostBuffer;
    c.dwHostNameLength = static_cast<DWORD>(std::size(hostBuffer));
    c.lpszUrlPath = pathBuffer;
    c.dwUrlPathLength = static_cast<DWORD>(std::size(pathBuffer));

    if (!WinHttpCrackUrl(url.c_str(), 0, ICU_DECODE, &c)) {
        return false;
    }

    host.assign(c.lpszHostName, c.dwHostNameLength);
    path.assign(c.lpszUrlPath, c.dwUrlPathLength);
    if (c.dwExtraInfoLength) {
        path.append(c.lpszExtraInfo, c.dwExtraInfoLength);
    }
    port = c.nPort;
    https = (c.nScheme == INTERNET_SCHEME_HTTPS);
    return true;
}

bool DownloadFile(
    const std::wstring& url,
    const std::wstring& destination,
    std::function<void(uint64_t, uint64_t)> progress) {

    std::wstring host, path;
    INTERNET_PORT port = INTERNET_DEFAULT_HTTPS_PORT;
    bool https = true;
    if (!ParseUrl(url, host, path, port, https)) return false;

    HINTERNET session = WinHttpOpen(
        L"DownTrackWebInstaller/1.0",
        WINHTTP_ACCESS_TYPE_AUTOMATIC_PROXY,
        WINHTTP_NO_PROXY_NAME,
        WINHTTP_NO_PROXY_BYPASS,
        0);
    if (!session) return false;

    WinHttpSetTimeouts(session, 10000, 10000, 10000, 30000);

    HINTERNET connection = WinHttpConnect(session, host.c_str(), port, 0);
    if (!connection) {
        WinHttpCloseHandle(session);
        return false;
    }

    DWORD flags = https ? WINHTTP_FLAG_SECURE : 0;
    HINTERNET request = WinHttpOpenRequest(
        connection, L"GET", path.c_str(), nullptr,
        WINHTTP_NO_REFERER, WINHTTP_DEFAULT_ACCEPT_TYPES, flags);
    if (!request) {
        WinHttpCloseHandle(connection);
        WinHttpCloseHandle(session);
        return false;
    }

    const wchar_t* accept[] = {L"*/*", nullptr};
    WinHttpAddRequestHeaders(request, L"Accept: */*\r\n", -1L, WINHTTP_ADDREQ_FLAG_ADD);
    if (!WinHttpSendRequest(request, WINHTTP_NO_ADDITIONAL_HEADERS, 0,
                            WINHTTP_NO_REQUEST_DATA, 0, 0, 0) ||
        !WinHttpReceiveResponse(request, nullptr)) {
        WinHttpCloseHandle(request);
        WinHttpCloseHandle(connection);
        WinHttpCloseHandle(session);
        return false;
    }

    DWORD status = 0;
    DWORD statusSize = sizeof(status);
    WinHttpQueryHeaders(
        request,
        WINHTTP_QUERY_STATUS_CODE | WINHTTP_QUERY_FLAG_NUMBER,
        WINHTTP_HEADER_NAME_BY_INDEX,
        &status,
        &statusSize,
        WINHTTP_NO_HEADER_INDEX);

    if (status < 200 || status >= 300) {
        WinHttpCloseHandle(request);
        WinHttpCloseHandle(connection);
        WinHttpCloseHandle(session);
        return false;
    }

    uint64_t total = 0;
    DWORD contentLength = 0;
    DWORD contentLengthSize = sizeof(contentLength);
    if (WinHttpQueryHeaders(
            request,
            WINHTTP_QUERY_CONTENT_LENGTH | WINHTTP_QUERY_FLAG_NUMBER,
            WINHTTP_HEADER_NAME_BY_INDEX,
            &contentLength,
            &contentLengthSize,
            WINHTTP_NO_HEADER_INDEX)) {
        total = contentLength;
    }

    std::ofstream out(destination, std::ios::binary | std::ios::trunc);
    if (!out) {
        WinHttpCloseHandle(request);
        WinHttpCloseHandle(connection);
        WinHttpCloseHandle(session);
        return false;
    }

    std::vector<uint8_t> buffer(64 * 1024);
    uint64_t received = 0;

    while (true) {
        DWORD read = 0;
        if (!WinHttpReadData(request, buffer.data(), static_cast<DWORD>(buffer.size()), &read)) {
            out.close();
            WinHttpCloseHandle(request);
            WinHttpCloseHandle(connection);
            WinHttpCloseHandle(session);
            return false;
        }
        if (read == 0) break;

        out.write(reinterpret_cast<const char*>(buffer.data()), read);
        if (!out) {
            out.close();
            WinHttpCloseHandle(request);
            WinHttpCloseHandle(connection);
            WinHttpCloseHandle(session);
            return false;
        }

        received += read;
        if (progress) progress(received, total);
    }

    out.close();
    WinHttpCloseHandle(request);
    WinHttpCloseHandle(connection);
    WinHttpCloseHandle(session);
    return true;
}

bool Sha256File(const std::wstring& path, std::wstring& hex) {
    BCRYPT_ALG_HANDLE alg = nullptr;
    BCRYPT_HASH_HANDLE hash = nullptr;
    NTSTATUS st = BCryptOpenAlgorithmProvider(&alg, BCRYPT_SHA256_ALGORITHM, nullptr, 0);
    if (st < 0) return false;

    DWORD objectSize = 0;
    DWORD resultSize = 0;
    st = BCryptGetProperty(
        alg, BCRYPT_OBJECT_LENGTH, reinterpret_cast<PUCHAR>(&objectSize),
        sizeof(objectSize), &resultSize, 0);
    if (st < 0) {
        BCryptCloseAlgorithmProvider(alg, 0);
        return false;
    }

    std::vector<UCHAR> object(objectSize);
    st = BCryptCreateHash(alg, &hash, object.data(), objectSize, nullptr, 0, 0);
    if (st < 0) {
        BCryptCloseAlgorithmProvider(alg, 0);
        return false;
    }

    std::ifstream in(path, std::ios::binary);
    if (!in) {
        BCryptDestroyHash(hash);
        BCryptCloseAlgorithmProvider(alg, 0);
        return false;
    }

    std::vector<char> buffer(64 * 1024);
    while (in) {
        in.read(buffer.data(), static_cast<std::streamsize>(buffer.size()));
        const auto count = static_cast<DWORD>(in.gcount());
        if (count > 0) {
            st = BCryptHashData(hash, reinterpret_cast<PUCHAR>(buffer.data()), count, 0);
            if (st < 0) {
                BCryptDestroyHash(hash);
                BCryptCloseAlgorithmProvider(alg, 0);
                return false;
            }
        }
    }

    UCHAR digest[32]{};
    st = BCryptFinishHash(hash, digest, sizeof(digest), 0);
    BCryptDestroyHash(hash);
    BCryptCloseAlgorithmProvider(alg, 0);
    if (st < 0) return false;

    wchar_t digit[3]{};
    hex.clear();
    for (UCHAR b : digest) {
        swprintf_s(digit, L"%02x", b);
        hex += digit;
    }
    return true;
}

bool RunHiddenPowerShell(const std::wstring& command) {
    std::wstring line =
        L"powershell.exe -NoLogo -NoProfile -NonInteractive -ExecutionPolicy Bypass -WindowStyle Hidden -Command \\\"" 
        + command + L"\\\"";

    STARTUPINFOW si{};
    si.cb = sizeof(si);
    si.dwFlags = STARTF_USESHOWWINDOW;
    si.wShowWindow = SW_HIDE;
    PROCESS_INFORMATION pi{};

    std::vector<wchar_t> buffer(line.begin(), line.end());
    buffer.push_back(L'\0');

    if (!CreateProcessW(
            nullptr,
            buffer.data(),
            nullptr,
            nullptr,
            FALSE,
            CREATE_NO_WINDOW,
            nullptr,
            nullptr,
            &si,
            &pi)) {
        return false;
    }

    WaitForSingleObject(pi.hProcess, INFINITE);
    DWORD exitCode = 1;
    GetExitCodeProcess(pi.hProcess, &exitCode);
    CloseHandle(pi.hThread);
    CloseHandle(pi.hProcess);
    return exitCode == 0;
}

bool DeleteDirectory(const std::wstring& path) {
    if (!PathFileExistsW(path.c_str())) return true;
    const auto escaped = path;
    const std::wstring command =
        L"Remove-Item -LiteralPath '" + escaped + L"' -Recurse -Force -ErrorAction Stop";
    return RunHiddenPowerShell(command);
}

bool ExtractPayload(const std::wstring& zip, const std::wstring& target) {
    CreateDirectoryW(target.c_str(), nullptr);
    const std::wstring command =
        L"New-Item -ItemType Directory -Force -LiteralPath '" + target + L"' | Out-Null; "
        L"Expand-Archive -LiteralPath '" + zip + L"' -DestinationPath '" + target + L"' -Force";
    return RunHiddenPowerShell(command);
}

bool CreateShortcuts(const std::wstring& appPath, const std::wstring& installDir) {
    wchar_t desktopPath[MAX_PATH]{};
    if (SHGetFolderPathW(nullptr, CSIDL_DESKTOPDIRECTORY, nullptr, SHGFP_TYPE_CURRENT, desktopPath) != S_OK) {
        return false;
    }
    const std::wstring desktop = std::wstring(desktopPath) + L"\\DownTrack.lnk";

    wchar_t programs[MAX_PATH]{};
    if (SHGetFolderPathW(nullptr, CSIDL_PROGRAMS, nullptr, SHGFP_TYPE_CURRENT, programs) != S_OK) {
        return false;
    }
    const std::wstring startFolder = std::wstring(programs) + L"\\DownTrack";
    const std::wstring startMenu = startFolder + L"\\DownTrack.lnk";

    const std::wstring command =
        L"New-Item -ItemType Directory -Force -LiteralPath '" + startFolder + L"' | Out-Null; "
        L"$w=New-Object -ComObject WScript.Shell; "
        L"$s=$w.CreateShortcut('" + desktop + L"'); "
        L"$s.TargetPath='" + appPath + L"'; $s.WorkingDirectory='" + installDir + L"'; $s.Save(); "
        L"$s=$w.CreateShortcut('" + startMenu + L"'); "
        L"$s.TargetPath='" + appPath + L"'; $s.WorkingDirectory='" + installDir + L"'; $s.Save();";
    return RunHiddenPowerShell(command);
}

std::wstring InstallDir() {
    const auto base = BaseDir();
    return base.empty() ? L"" : base + L"\\Programs\\DownTrack";
}

std::wstring ManifestPath() {
    wchar_t temp[MAX_PATH]{};
    GetTempPathW(MAX_PATH, temp);
    CreateDirectoryW((std::wstring(temp) + L"DownTrack").c_str(), nullptr);
    return std::wstring(temp) + L"DownTrack\\latest.ini";
}

std::wstring PayloadTempPath() {
    wchar_t temp[MAX_PATH]{};
    GetTempPathW(MAX_PATH, temp);
    CreateDirectoryW((std::wstring(temp) + L"DownTrack").c_str(), nullptr);
    return std::wstring(temp) + L"DownTrack\\DownTrack-Payload.zip";
}

std::wstring InstalledVersion() {
    const auto path = InstallDir() + L"\\DownTrack.Install.ini";
    wchar_t version[64]{};
    GetPrivateProfileStringW(
        L"Install", L"Version", L"", version, static_cast<DWORD>(std::size(version)), path.c_str());
    return version;
}

bool ReadManifest() {
    {
        std::lock_guard<std::mutex> lock(g_mutex);
        g_version.clear();
        g_payloadUrl.clear();
        g_payloadHash.clear();
        g_payloadSize = 0;
    }

    const auto manifestPath = ManifestPath();
    DeleteFileW(manifestPath.c_str());

    SetState(ScreenState::Checking, 2);
    if (!DownloadFile(kManifestUrl, manifestPath, nullptr)) return false;

    wchar_t buffer[4096]{};

    GetPrivateProfileStringW(L"release", L"Version", L"", buffer, 4096, manifestPath.c_str());
    const auto version = std::wstring(buffer);
    if (version.empty()) return false;

    GetPrivateProfileStringW(L"release", L"PayloadUrl", L"", buffer, 4096, manifestPath.c_str());
    const auto payloadUrl = std::wstring(buffer);
    if (payloadUrl.rfind(L"https://github.com/mediaforge2446/downtrack/releases/download/", 0) != 0 &&
        payloadUrl.rfind(L"https://github.com/MediaForge2446/DownTrack/releases/download/", 0) != 0) {
        return false;
    }

    GetPrivateProfileStringW(L"release", L"PayloadSha256", L"", buffer, 4096, manifestPath.c_str());
    const auto hash = std::wstring(buffer);

    GetPrivateProfileStringW(L"release", L"PayloadSize", L"0", buffer, 4096, manifestPath.c_str());
    const auto size = _wcstoui64(buffer, nullptr, 10);

    if (hash.size() != 64 || size == 0) return false;

    {
        std::lock_guard<std::mutex> lock(g_mutex);
        g_version = version;
        g_payloadUrl = payloadUrl;
        g_payloadHash = hash;
        g_payloadSize = size;
    }
    return true;
}

void InstallationWorker() {
    if (g_busy.exchange(true)) return;

    auto fail = [](const std::wstring& detail) {
        {
            std::lock_guard<std::mutex> lock(g_mutex);
            g_errorDetail = detail;
        }
        SetState(ScreenState::Error, 0);
    };

    if (!ReadManifest()) {
        fail(L"Unable to reach the latest DownTrack release.");
        g_busy = false;
        return;
    }

    std::wstring version;
    std::wstring payloadUrl;
    std::wstring payloadHash;
    uint64_t payloadSize = 0;
    {
        std::lock_guard<std::mutex> lock(g_mutex);
        version = g_version;
        payloadUrl = g_payloadUrl;
        payloadHash = g_payloadHash;
        payloadSize = g_payloadSize;
    }

    if (!InstalledVersion().empty() && InstalledVersion() == version) {
        SetState(ScreenState::UpToDate, 100);
        SetWindowLongPtrW(g_primary, GWL_ID, ID_OPEN);
        EnableWindow(g_primary, TRUE);
        EnableWindow(g_secondary, TRUE);
        g_busy = false;
        return;
    }

    const auto payloadPath = PayloadTempPath();
    DeleteFileW(payloadPath.c_str());

    SetState(ScreenState::Downloading, 3, L"0 MB / " + FormatMb(payloadSize) + L" MB");
    const bool downloaded = DownloadFile(
        payloadUrl,
        payloadPath,
        [&](uint64_t received, uint64_t total) {
            const uint64_t denominator = total ? total : payloadSize;
            const int p = denominator ? static_cast<int>((received * 85ULL) / denominator) : 5;
            SetState(ScreenState::Downloading, std::clamp(p, 3, 88),
                     FormatMb(received) + L" MB / " + FormatMb(denominator) + L" MB");
        });

    if (!downloaded) {
        fail(L"Download failed. Check your internet connection and try again.");
        g_busy = false;
        return;
    }

    SetState(ScreenState::Verifying, 90, Lng().moments);
    std::wstring actualHash;
    if (!Sha256File(payloadPath, actualHash) || _wcsicmp(actualHash.c_str(), payloadHash.c_str()) != 0) {
        fail(L"The downloaded file failed integrity verification.");
        g_busy = false;
        return;
    }

    const auto installDir = InstallDir();
    SetState(ScreenState::Installing, 94, Lng().moments);

    if (!DeleteDirectory(installDir)) {
        fail(L"Please close DownTrack and try again.");
        g_busy = false;
        return;
    }

    if (!ExtractPayload(payloadPath, installDir)) {
        fail(L"Windows could not extract the DownTrack package.");
        g_busy = false;
        return;
    }

    const auto appPath = installDir + L"\\DownTrack.exe";
    if (!PathFileExistsW(appPath.c_str())) {
        fail(L"The DownTrack package is missing DownTrack.exe.");
        g_busy = false;
        return;
    }

    CreateShortcuts(appPath, installDir);
    WritePrivateProfileStringW(
        L"Install", L"Version", version.c_str(),
        (installDir + L"\\DownTrack.Install.ini").c_str());

    {
        std::lock_guard<std::mutex> lock(g_mutex);
        g_version = version;
    }

    SetState(ScreenState::Complete, 100, Lng().moments);
    SetWindowLongPtrW(g_primary, GWL_ID, ID_OPEN);
    EnableWindow(g_primary, TRUE);
    EnableWindow(g_secondary, TRUE);
    g_busy = false;
}

void StartInstallation() {
    if (g_busy) return;
    EnableWindow(g_primary, FALSE);
    std::thread(InstallationWorker).detach();
}

void OpenDownTrack() {
    const auto appPath = InstallDir() + L"\\DownTrack.exe";
    if (PathFileExistsW(appPath.c_str())) {
        ShellExecuteW(nullptr, L"open", appPath.c_str(), nullptr, InstallDir().c_str(), SW_SHOWNORMAL);
    }
    DestroyWindow(g_hwnd);
}

void CloseInstaller() {
    if (g_busy) return;
    DestroyWindow(g_hwnd);
}

void DrawTextBlock(HDC hdc, const std::wstring& text, RECT rect, HFONT font, COLORREF color, UINT format) {
    const auto oldFont = SelectObject(hdc, font);
    SetTextColor(hdc, color);
    SetBkMode(hdc, TRANSPARENT);
    DrawTextW(hdc, text.c_str(), -1, &rect, format);
    SelectObject(hdc, oldFont);
}

void DrawRoundRectFill(HDC hdc, const RECT& r, int radius, COLORREF color) {
    HBRUSH brush = CreateSolidBrush(color);
    HPEN pen = CreatePen(PS_NULL, 0, color);
    const auto oldBrush = SelectObject(hdc, brush);
    const auto oldPen = SelectObject(hdc, pen);
    RoundRect(hdc, r.left, r.top, r.right, r.bottom, radius, radius);
    SelectObject(hdc, oldBrush);
    SelectObject(hdc, oldPen);
    DeleteObject(brush);
    DeleteObject(pen);
}

void DrawWindow(HDC hdc, RECT client) {
    FillRect(hdc, &client, g_windowBrush);

    const COLORREF bg = RGB(248, 248, 255);
    const COLORREF white = RGB(255, 255, 255);
    const COLORREF ink = RGB(24, 24, 40);
    const COLORREF muted = RGB(119, 121, 142);
    const COLORREF purple = RGB(111, 72, 246);
    const COLORREF pink = RGB(246, 92, 132);
    const COLORREF cyan = RGB(76, 205, 224);

    HBRUSH bgBrush = CreateSolidBrush(bg);
    FillRect(hdc, &client, bgBrush);
    DeleteObject(bgBrush);

    RECT header{0, 0, client.right, 78};
    HBRUSH whiteBrush = CreateSolidBrush(white);
    FillRect(hdc, &header, whiteBrush);
    DeleteObject(whiteBrush);

    RECT logo{28, 18, 66, 56};
    DrawRoundRectFill(hdc, logo, 10, pink);
    DrawTextBlock(hdc, L"D", logo, g_fontBold, RGB(255, 255, 255), DT_CENTER | DT_VCENTER | DT_SINGLELINE);

    RECT title{78, 16, 300, 42};
    DrawTextBlock(hdc, kWindowTitle, title, g_fontBold, ink, DT_LEFT | DT_SINGLELINE | DT_VCENTER);

    RECT tagline{78, 43, 360, 66};
    DrawTextBlock(hdc, Lng().tagline, tagline, g_font, muted, DT_LEFT | DT_SINGLELINE | DT_VCENTER);

    RECT card{28, 102, client.right - 28, 404};
    DrawRoundRectFill(hdc, card, 18, white);

    RECT dot1{client.right / 2 - 18, 134, client.right / 2 - 10, 142};
    DrawRoundRectFill(hdc, dot1, 4, purple);
    RECT dot2{client.right / 2 + 0, 134, client.right / 2 + 8, 142};
    DrawRoundRectFill(hdc, dot2, 4, pink);
    RECT dot3{client.right / 2 + 18, 134, client.right / 2 + 26, 142};
    DrawRoundRectFill(hdc, dot3, 4, cyan);

    RECT stateRect{48, 160, client.right - 48, 202};
    ScreenState state;
    int progress;
    std::wstring detail;
    {
        std::lock_guard<std::mutex> lock(g_mutex);
        state = g_state;
        progress = g_progress;
        detail = g_downloadDetail;
    }
    DrawTextBlock(hdc, StateTitle(state), stateRect, g_fontBold, ink, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

    RECT versionRect{48, 205, client.right - 48, 234};
    std::wstring versionText = g_version.empty() ? Lng().moments : (L"Latest version: " + g_version);
    if (state == ScreenState::UpToDate) versionText = g_version;
    DrawTextBlock(hdc, versionText, versionRect, g_font, muted, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

    RECT detailRect{48, 242, client.right - 48, 270};
    DrawTextBlock(hdc, detail, detailRect, g_font, purple, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

    RECT bar{65, 288, client.right - 65, 299};
    DrawRoundRectFill(hdc, bar, 6, RGB(230, 232, 241));
    RECT fill = bar;
    fill.right = fill.left + ((fill.right - fill.left) * progress / 100);
    if (fill.right > fill.left) DrawRoundRectFill(hdc, fill, 6, purple);

    if (state == ScreenState::Error) {
        RECT err{48, 315, client.right - 48, 357};
        std::wstring errorText;
        {
            std::lock_guard<std::mutex> lock(g_mutex);
            errorText = g_errorDetail;
        }
        DrawTextBlock(hdc, errorText, err, g_font, RGB(194, 65, 83), DT_CENTER | DT_WORDBREAK);
    }

    const bool rtl = IsRtl();
    RECT footerText{28, client.bottom - 68, client.right - 220, client.bottom - 36};
    DrawTextBlock(hdc, Lng().moments, footerText, g_font, muted,
                  (rtl ? DT_RIGHT : DT_LEFT) | DT_SINGLELINE | DT_VCENTER);
}

void DrawButton(const DRAWITEMSTRUCT* dis) {
    const bool primary = dis->CtlID == ID_INSTALL || dis->CtlID == ID_OPEN;
    const bool enabled = IsWindowEnabled(dis->hwndItem);
    const COLORREF purple = RGB(111, 72, 246);
    const COLORREF border = RGB(220, 222, 232);
    const COLORREF bg = enabled ? (primary ? purple : RGB(255, 255, 255)) : RGB(239, 240, 246);
    const COLORREF text = enabled ? (primary ? RGB(255, 255, 255) : RGB(40, 40, 52)) : RGB(150, 151, 165);

    RECT r = dis->rcItem;
    r.left += 1; r.top += 1; r.right -= 1; r.bottom -= 1;
    DrawRoundRectFill(dis->hDC, r, 9, bg);

    if (!primary) {
        HBRUSH brush = CreateSolidBrush(border);
        FrameRect(dis->hDC, &r, brush);
        DeleteObject(brush);
    }

    wchar_t label[128]{};
    GetWindowTextW(dis->hwndItem, label, static_cast<int>(std::size(label)));
    DrawTextBlock(dis->hDC, label, r, g_fontBold, text, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
        case WM_CREATE: {
            g_font = CreateFontW(-16, 0, 0, 0, FW_NORMAL, FALSE, FALSE, FALSE,
                                 DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
                                 CLEARTYPE_QUALITY, DEFAULT_PITCH | FF_SWISS, L"Segoe UI");
            g_fontBold = CreateFontW(-17, 0, 0, 0, FW_SEMIBOLD, FALSE, FALSE, FALSE,
                                     DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
                                     CLEARTYPE_QUALITY, DEFAULT_PITCH | FF_SWISS, L"Segoe UI");

            g_windowBrush = CreateSolidBrush(RGB(248, 248, 255));

            g_language = CreateWindowExW(
                0, WC_COMBOBOXW, L"", WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
                18, 17, 190, 260, hwnd, reinterpret_cast<HMENU>(ID_LANGUAGE),
                GetModuleHandleW(nullptr), nullptr);

            for (size_t i = 0; i < kLanguageCount; ++i) {
                SendMessageW(g_language, CB_ADDSTRING, 0,
                             reinterpret_cast<LPARAM>(kLanguages[i].name));
            }

            g_primary = CreateWindowExW(
                0, L"BUTTON", L"", WS_CHILD | WS_VISIBLE | BS_OWNERDRAW,
                0, 0, 0, 0, hwnd, reinterpret_cast<HMENU>(ID_INSTALL),
                GetModuleHandleW(nullptr), nullptr);
            g_secondary = CreateWindowExW(
                0, L"BUTTON", L"", WS_CHILD | WS_VISIBLE | BS_OWNERDRAW,
                0, 0, 0, 0, hwnd, reinterpret_cast<HMENU>(ID_CLOSE),
                GetModuleHandleW(nullptr), nullptr);

            SendMessageW(g_language, WM_SETFONT, reinterpret_cast<WPARAM>(g_font), TRUE);
            SendMessageW(g_primary, WM_SETFONT, reinterpret_cast<WPARAM>(g_fontBold), TRUE);
            SendMessageW(g_secondary, WM_SETFONT, reinterpret_cast<WPARAM>(g_fontBold), TRUE);

            const auto saved = LoadLanguageMode();
            const auto code = (saved == L"auto") ? DetectLanguage() : saved;
            g_languageIndex = FindLanguage(code);
            SendMessageW(g_language, CB_SETCURSEL, static_cast<WPARAM>(g_languageIndex), 0);
            ApplyLanguage();

            return 0;
        }

        case WM_SIZE: {
            const int width = LOWORD(lParam);
            const int height = HIWORD(lParam);
            MoveWindow(g_language, width - 210, 17, 190, 260, TRUE);
            MoveWindow(g_primary, width - 145, height - 52, 125, 36, TRUE);
            MoveWindow(g_secondary, width - 270, height - 52, 110, 36, TRUE);
            InvalidateRect(hwnd, nullptr, FALSE);
            return 0;
        }

        case WM_COMMAND: {
            if (HIWORD(wParam) == CBN_SELCHANGE &&
                LOWORD(wParam) == ID_LANGUAGE) {
                const auto index = static_cast<size_t>(SendMessageW(g_language, CB_GETCURSEL, 0, 0));
                if (index < kLanguageCount) {
                    g_languageIndex = index;
                    SaveLanguageMode(kLanguages[index].code);
                    ApplyLanguage();
                }
                return 0;
            }

            if (HIWORD(wParam) == BN_CLICKED && LOWORD(wParam) == ID_INSTALL) {
                const auto id = static_cast<int>(GetWindowLongPtrW(g_primary, GWL_ID));
                if (id == ID_OPEN) {
                    OpenDownTrack();
                } else {
                    StartInstallation();
                }
                return 0;
            }

            if (HIWORD(wParam) == BN_CLICKED && LOWORD(wParam) == ID_CLOSE) {
                CloseInstaller();
                return 0;
            }
            return 0;
        }

        case WM_TIMER:
            if (wParam == 1) {
                KillTimer(hwnd, 1);
                StartInstallation();
                return 0;
            }
            break;

        case WM_APP_STATE:
            ApplyLanguage();
            InvalidateRect(hwnd, nullptr, FALSE);
            return 0;

        case WM_APP_FINISHED:
            ApplyLanguage();
            return 0;

        case WM_DRAWITEM:
            DrawButton(reinterpret_cast<const DRAWITEMSTRUCT*>(lParam));
            return TRUE;

        case WM_PAINT: {
            PAINTSTRUCT ps{};
            HDC hdc = BeginPaint(hwnd, &ps);
            RECT client{};
            GetClientRect(hwnd, &client);
            DrawWindow(hdc, client);
            EndPaint(hwnd, &ps);
            return 0;
        }

        case WM_DESTROY:
            if (g_busy) {
                // The worker is detached intentionally; the process will normally
                // remain alive only while installation is active because Close is disabled.
                return 0;
            }
            if (g_font) DeleteObject(g_font);
            if (g_fontBold) DeleteObject(g_fontBold);
            if (g_windowBrush) DeleteObject(g_windowBrush);
            PostQuitMessage(0);
            return 0;
    }

    return DefWindowProcW(hwnd, msg, wParam, lParam);
}

} // namespace

int APIENTRY wWinMain(HINSTANCE hInstance, HINSTANCE, PWSTR, int nCmdShow) {
    HANDLE mutex = CreateMutexW(nullptr, TRUE, L"Local\\DownTrack.WebInstaller.SingleInstance");
    if (!mutex) return 1;

    if (GetLastError() == ERROR_ALREADY_EXISTS) {
        HWND existing = FindWindowW(kWindowClass, nullptr);
        if (existing) {
            ShowWindow(existing, SW_SHOWNORMAL);
            SetForegroundWindow(existing);
        }
        CloseHandle(mutex);
        return 0;
    }

    INITCOMMONCONTROLSEX icc{};
    icc.dwSize = sizeof(icc);
    icc.dwICC = ICC_STANDARD_CLASSES;
    InitCommonControlsEx(&icc);

    WNDCLASSEXW wc{};
    wc.cbSize = sizeof(wc);
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.hIcon = LoadIconW(nullptr, IDI_APPLICATION);
    wc.hCursor = LoadCursorW(nullptr, IDC_ARROW);
    wc.hbrBackground = CreateSolidBrush(RGB(248, 248, 255));
    wc.lpszClassName = kWindowClass;
    wc.style = CS_HREDRAW | CS_VREDRAW;

    if (!RegisterClassExW(&wc)) {
        CloseHandle(mutex);
        return 1;
    }

    const int width = 720;
    const int height = 480;
    RECT screen{};
    SystemParametersInfoW(SPI_GETWORKAREA, 0, &screen, 0);
    const int x = screen.left + ((screen.right - screen.left) - width) / 2;
    const int y = screen.top + ((screen.bottom - screen.top) - height) / 2;

    g_hwnd = CreateWindowExW(
        WS_EX_APPWINDOW,
        kWindowClass,
        kWindowTitle,
        WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX,
        x, y, width, height,
        nullptr, nullptr, hInstance, nullptr);

    if (!g_hwnd) {
        CloseHandle(mutex);
        return 1;
    }

    ShowWindow(g_hwnd, nCmdShow == SW_HIDE ? SW_SHOWNORMAL : nCmdShow);
    UpdateWindow(g_hwnd);
    SetForegroundWindow(g_hwnd);
    SetTimer(g_hwnd, 1, 450, nullptr);

    MSG msg{};
    while (GetMessageW(&msg, nullptr, 0, 0) > 0) {
        TranslateMessage(&msg);
        DispatchMessageW(&msg);
    }

    CloseHandle(mutex);
    return static_cast<int>(msg.wParam);
}
