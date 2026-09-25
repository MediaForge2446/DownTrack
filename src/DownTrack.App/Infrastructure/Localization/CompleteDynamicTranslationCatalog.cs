namespace DownTrack.Infrastructure.Localization;

internal static class CompleteDynamicTranslationCatalog
{
    public static void Apply(IDictionary<string, Dictionary<string,string>> dictionaries)
    {
        foreach (var (language, values) in Packs)
        {
            if (!dictionaries.TryGetValue(language, out var dictionary))
                continue;

            foreach (var (key, value) in values)
                dictionary[key] = value;
        }
    }

    private static Dictionary<string,string> Pack(params (string key,string value)[] values) =>
        values.ToDictionary(x => x.key, x => x.value, StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<string, Dictionary<string,string>> Packs =
        new Dictionary<string, Dictionary<string,string>>
        {
            ["nl"] = Pack(
                ("Library.RootCount","{0} hoofdmap(pen)"),("Home.EngineNeedsSetup","Media-engine moet worden ingesteld"),("Home.SettingUpEngine","Media-engine instellen…"),
                ("Explorer.SaveChangesArrow","Wijzigingen opslaan  →"),("Explorer.SaveChangesCount","Wijzigingen opslaan ({0})  →"),("Explorer.QueuedOne","1 in wachtrij"),("Explorer.Queued","{0} in wachtrij"),
                ("Explorer.PendingCreate","In behandeling: “{0}” maken."),("Explorer.PendingRename","In behandeling: “{0}” hernoemen."),("Explorer.PendingDelete","In behandeling: “{0}” verwijderen."),("Explorer.MediaAddedOne","Media toegevoegd aan de wachtende wijzigingen."),("Explorer.MediaAddedMany","{0} media-items toegevoegd aan de wachtende wijzigingen."),("Explorer.ApplyingChanges","Wachtende wijzigingen toepassen…"),("Explorer.SomeNeedAttention","Sommige wijzigingen hebben aandacht nodig."),("Explorer.Cancelled","Geannuleerd: {0}"),
                ("AddMedia.PastePrompt","Plak een YouTube-video- of playlist-URL."),("AddMedia.NothingFound","Niets gevonden. Controleer de link en probeer opnieuw."),("AddMedia.OneReady","1 media-item klaar. Pas de opties aan voordat je het toevoegt."),("AddMedia.ManyReady","{0} media-items klaar. Elke rij is onafhankelijk."),("AddMedia.AnalysisCancelled","Analyse geannuleerd."),("AddMedia.ErrorFallback","We konden deze link niet analyseren. Probeer opnieuw."),
                ("Settings.Saved","Instellingen opgeslagen."),("Settings.ToolsReady","Mediahulpmiddelen zijn gereed."),("Settings.ToolsMissing","Een of meer mediahulpmiddelen ontbreken."),("Settings.ToolsUpdated","Mediahulpmiddelen bijgewerkt."),
                ("Updates.Checking","Controleren op updates…"),("Updates.Available","Update beschikbaar: {0}"),("Updates.UpToDate","Je hebt al de nieuwste versie."),("Updates.Downloading","Nieuwste versie downloaden…"),("Updates.Restarting","De update is gestart. DownTrack wordt opnieuw gestart wanneer dat klaar is.")
            ),
            ["pl"] = Pack(
                ("Library.RootCount","{0} folderów głównych"),("Home.EngineNeedsSetup","Silnik multimediów wymaga konfiguracji"),("Home.SettingUpEngine","Konfigurowanie silnika multimediów…"),
                ("Explorer.SaveChangesArrow","Zapisz zmiany  →"),("Explorer.SaveChangesCount","Zapisz zmiany ({0})  →"),("Explorer.QueuedOne","1 w kolejce"),("Explorer.Queued","{0} w kolejce"),
                ("Explorer.PendingCreate","Oczekuje: utworzenie „{0}”."),("Explorer.PendingRename","Oczekuje: zmiana nazwy „{0}”."),("Explorer.PendingDelete","Oczekuje: usunięcie „{0}”."),("Explorer.MediaAddedOne","Dodano multimedia do oczekujących zmian."),("Explorer.MediaAddedMany","Dodano {0} elementów do oczekujących zmian."),("Explorer.ApplyingChanges","Stosowanie oczekujących zmian…"),("Explorer.SomeNeedAttention","Niektóre zmiany wymagają uwagi."),("Explorer.Cancelled","Anulowano: {0}"),
                ("AddMedia.PastePrompt","Wklej adres filmu lub playlisty YouTube."),("AddMedia.NothingFound","Nic nie znaleziono. Sprawdź link i spróbuj ponownie."),("AddMedia.OneReady","1 element multimedialny gotowy. Edytuj opcje przed dodaniem."),("AddMedia.ManyReady","{0} elementów multimedialnych gotowych. Każdy wiersz jest niezależny."),("AddMedia.AnalysisCancelled","Analiza anulowana."),("AddMedia.ErrorFallback","Nie udało się przeanalizować linku. Spróbuj ponownie."),
                ("Settings.Saved","Ustawienia zapisane."),("Settings.ToolsReady","Narzędzia multimedialne są gotowe."),("Settings.ToolsMissing","Brakuje co najmniej jednego narzędzia multimedialnego."),("Settings.ToolsUpdated","Narzędzia multimedialne zaktualizowano."),
                ("Updates.Checking","Sprawdzanie aktualizacji…"),("Updates.Available","Dostępna aktualizacja: {0}"),("Updates.UpToDate","Masz już najnowszą wersję."),("Updates.Downloading","Pobieranie najnowszej wersji…"),("Updates.Restarting","Aktualizacja została rozpoczęta. DownTrack uruchomi się ponownie, gdy będzie gotowy.")
            ),
            ["cs"] = Pack(
                ("Library.RootCount","{0} kořenových složek"),("Home.EngineNeedsSetup","Mediální engine vyžaduje nastavení"),("Home.SettingUpEngine","Nastavování mediálního enginu…"),
                ("Explorer.SaveChangesArrow","Uložit změny  →"),("Explorer.SaveChangesCount","Uložit změny ({0})  →"),("Explorer.QueuedOne","1 ve frontě"),("Explorer.Queued","{0} ve frontě"),
                ("Explorer.PendingCreate","Čeká: vytvořit „{0}“."),("Explorer.PendingRename","Čeká: přejmenovat „{0}“."),("Explorer.PendingDelete","Čeká: odstranit „{0}“."),("Explorer.MediaAddedOne","Média byla přidána mezi čekající změny."),("Explorer.MediaAddedMany","Bylo přidáno {0} mediálních položek mezi čekající změny."),("Explorer.ApplyingChanges","Používání čekajících změn…"),("Explorer.SomeNeedAttention","Některé změny vyžadují pozornost."),("Explorer.Cancelled","Zrušeno: {0}"),
                ("AddMedia.PastePrompt","Vložte adresu videa nebo playlistu YouTube."),("AddMedia.NothingFound","Nic nebylo nalezeno. Zkontrolujte odkaz a zkuste to znovu."),("AddMedia.OneReady","1 mediální položka je připravena. Před přidáním upravte možnosti."),("AddMedia.ManyReady","{0} mediálních položek je připraveno. Každý řádek je nezávislý."),("AddMedia.AnalysisCancelled","Analýza zrušena."),("AddMedia.ErrorFallback","Odkaz se nepodařilo analyzovat. Zkuste to znovu."),
                ("Settings.Saved","Nastavení uloženo."),("Settings.ToolsReady","Mediální nástroje jsou připravené."),("Settings.ToolsMissing","Chybí jeden nebo více mediálních nástrojů."),("Settings.ToolsUpdated","Mediální nástroje byly aktualizovány."),
                ("Updates.Checking","Kontrola aktualizací…"),("Updates.Available","Je k dispozici aktualizace: {0}"),("Updates.UpToDate","Máte již nejnovější verzi."),("Updates.Downloading","Stahování nejnovější verze…"),("Updates.Restarting","Aktualizace byla spuštěna. DownTrack se po dokončení restartuje.")
            ),
            ["tr"] = Pack(
                ("Library.RootCount","{0} kök klasör"),("Home.EngineNeedsSetup","Medya motoru kurulumu gerekli"),("Home.SettingUpEngine","Medya motoru kuruluyor…"),
                ("Explorer.SaveChangesArrow","Değişiklikleri kaydet  →"),("Explorer.SaveChangesCount","Değişiklikleri kaydet ({0})  →"),("Explorer.QueuedOne","1 sırada"),("Explorer.Queued","{0} sırada"),
                ("Explorer.PendingCreate","Bekliyor: “{0}” oluşturulsun."),("Explorer.PendingRename","Bekliyor: “{0}” yeniden adlandırılsın."),("Explorer.PendingDelete","Bekliyor: “{0}” silinsin."),("Explorer.MediaAddedOne","Medya bekleyen değişikliklere eklendi."),("Explorer.MediaAddedMany","{0} medya öğesi bekleyen değişikliklere eklendi."),("Explorer.ApplyingChanges","Bekleyen değişiklikler uygulanıyor…"),("Explorer.SomeNeedAttention","Bazı değişiklikler dikkat gerektiriyor."),("Explorer.Cancelled","İptal edildi: {0}"),
                ("AddMedia.PastePrompt","YouTube video veya oynatma listesi URL'sini yapıştırın."),("AddMedia.NothingFound","Hiçbir şey bulunamadı. Bağlantıyı kontrol edip tekrar deneyin."),("AddMedia.OneReady","1 medya öğesi hazır. Eklenmeden önce seçenekleri düzenleyin."),("AddMedia.ManyReady","{0} medya öğesi hazır. Her satır bağımsızdır."),("AddMedia.AnalysisCancelled","Analiz iptal edildi."),("AddMedia.ErrorFallback","Bu bağlantı analiz edilemedi. Lütfen tekrar deneyin."),
                ("Settings.Saved","Ayarlar kaydedildi."),("Settings.ToolsReady","Medya araçları hazır."),("Settings.ToolsMissing","Bir veya daha fazla medya aracı eksik."),("Settings.ToolsUpdated","Medya araçları güncellendi."),
                ("Updates.Checking","Güncellemeler kontrol ediliyor…"),("Updates.Available","Güncelleme mevcut: {0}"),("Updates.UpToDate","Zaten en son sürüme sahipsiniz."),("Updates.Downloading","En yeni sürüm indiriliyor…"),("Updates.Restarting","Güncelleme başladı. Hazır olduğunda DownTrack yeniden başlatılacak.")
            ),
            ["uk"] = Pack(
                ("Library.RootCount","{0} кореневих папок"),("Home.EngineNeedsSetup","Потрібне налаштування медіадвигуна"),("Home.SettingUpEngine","Налаштування медіадвигуна…"),
                ("Explorer.SaveChangesArrow","Зберегти зміни  →"),("Explorer.SaveChangesCount","Зберегти зміни ({0})  →"),("Explorer.QueuedOne","1 у черзі"),("Explorer.Queued","{0} у черзі"),
                ("Explorer.PendingCreate","Очікує: створити «{0}»."),("Explorer.PendingRename","Очікує: перейменувати «{0}»."),("Explorer.PendingDelete","Очікує: видалити «{0}»."),("Explorer.MediaAddedOne","Медіа додано до очікуваних змін."),("Explorer.MediaAddedMany","До очікуваних змін додано {0} медіа."),("Explorer.ApplyingChanges","Застосування очікуваних змін…"),("Explorer.SomeNeedAttention","Деякі зміни потребують уваги."),("Explorer.Cancelled","Скасовано: {0}"),
                ("AddMedia.PastePrompt","Вставте URL відео або плейлиста YouTube."),("AddMedia.NothingFound","Нічого не знайдено. Перевірте посилання та спробуйте ще раз."),("AddMedia.OneReady","1 медіаоб’єкт готовий. Змініть параметри перед додаванням."),("AddMedia.ManyReady","Готово {0} медіаоб’єктів. Кожен рядок незалежний."),("AddMedia.AnalysisCancelled","Аналіз скасовано."),("AddMedia.ErrorFallback","Не вдалося проаналізувати посилання. Спробуйте ще раз."),
                ("Settings.Saved","Налаштування збережено."),("Settings.ToolsReady","Медіаінструменти готові."),("Settings.ToolsMissing","Відсутній один або кілька медіаінструментів."),("Settings.ToolsUpdated","Медіаінструменти оновлено."),
                ("Updates.Checking","Перевірка оновлень…"),("Updates.Available","Доступне оновлення: {0}"),("Updates.UpToDate","У вас уже остання версія."),("Updates.Downloading","Завантаження останньої версії…"),("Updates.Restarting","Оновлення розпочато. DownTrack перезапуститься після завершення.")
            ),
            ["ru"] = Pack(
                ("Library.RootCount","{0} корневых папок"),("Home.EngineNeedsSetup","Требуется настройка медиадвижка"),("Home.SettingUpEngine","Настройка медиадвижка…"),
                ("Explorer.SaveChangesArrow","Сохранить изменения  →"),("Explorer.SaveChangesCount","Сохранить изменения ({0})  →"),("Explorer.QueuedOne","1 в очереди"),("Explorer.Queued","{0} в очереди"),
                ("Explorer.PendingCreate","Ожидается: создать «{0}»."),("Explorer.PendingRename","Ожидается: переименовать «{0}»."),("Explorer.PendingDelete","Ожидается: удалить «{0}»."),("Explorer.MediaAddedOne","Медиа добавлено в ожидающие изменения."),("Explorer.MediaAddedMany","Добавлено {0} медиаэлементов в ожидающие изменения."),("Explorer.ApplyingChanges","Применение ожидающих изменений…"),("Explorer.SomeNeedAttention","Некоторые изменения требуют внимания."),("Explorer.Cancelled","Отменено: {0}"),
                ("AddMedia.PastePrompt","Вставьте URL видео или плейлиста YouTube."),("AddMedia.NothingFound","Ничего не найдено. Проверьте ссылку и попробуйте снова."),("AddMedia.OneReady","1 медиаэлемент готов. Измените параметры перед добавлением."),("AddMedia.ManyReady","Готово медиаэлементов: {0}. Каждая строка независима."),("AddMedia.AnalysisCancelled","Анализ отменён."),("AddMedia.ErrorFallback","Не удалось проанализировать ссылку. Попробуйте снова."),
                ("Settings.Saved","Настройки сохранены."),("Settings.ToolsReady","Медиаинструменты готовы."),("Settings.ToolsMissing","Отсутствует один или несколько медиаинструментов."),("Settings.ToolsUpdated","Медиаинструменты обновлены."),
                ("Updates.Checking","Проверка обновлений…"),("Updates.Available","Доступно обновление: {0}"),("Updates.UpToDate","У вас уже установлена последняя версия."),("Updates.Downloading","Загрузка последней версии…"),("Updates.Restarting","Обновление запущено. DownTrack перезапустится после завершения.")
            ),
            ["ar"] = Pack(
                ("Library.RootCount","{0} من المجلدات الرئيسية"),("Home.EngineNeedsSetup","يلزم إعداد محرك الوسائط"),("Home.SettingUpEngine","جارٍ إعداد محرك الوسائط…"),
                ("Explorer.SaveChangesArrow","حفظ التغييرات  ←"),("Explorer.SaveChangesCount","حفظ التغييرات ({0})  ←"),("Explorer.QueuedOne","1 في الانتظار"),("Explorer.Queued","{0} في الانتظار"),
                ("Explorer.PendingCreate","معلّق: إنشاء «{0}»."),("Explorer.PendingRename","معلّق: إعادة تسمية «{0}»."),("Explorer.PendingDelete","معلّق: حذف «{0}»."),("Explorer.MediaAddedOne","تمت إضافة الوسائط إلى التغييرات المعلّقة."),("Explorer.MediaAddedMany","تمت إضافة {0} من عناصر الوسائط إلى التغييرات المعلّقة."),("Explorer.ApplyingChanges","جارٍ تطبيق التغييرات المعلّقة…"),("Explorer.SomeNeedAttention","بعض التغييرات تحتاج إلى مراجعة."),("Explorer.Cancelled","أُلغي: {0}"),
                ("AddMedia.PastePrompt","ألصق رابط فيديو أو قائمة تشغيل YouTube."),("AddMedia.NothingFound","لم يتم العثور على شيء. تحقق من الرابط وحاول مرة أخرى."),("AddMedia.OneReady","عنصر وسائط واحد جاهز. عدّل الخيارات قبل الإضافة."),("AddMedia.ManyReady","{0} من عناصر الوسائط جاهزة. كل صف مستقل."),("AddMedia.AnalysisCancelled","تم إلغاء التحليل."),("AddMedia.ErrorFallback","تعذر تحليل الرابط. حاول مرة أخرى."),
                ("Settings.Saved","تم حفظ الإعدادات."),("Settings.ToolsReady","أدوات الوسائط جاهزة."),("Settings.ToolsMissing","توجد أداة وسائط واحدة أو أكثر مفقودة."),("Settings.ToolsUpdated","تم تحديث أدوات الوسائط."),
                ("Updates.Checking","جارٍ التحقق من التحديثات…"),("Updates.Available","يتوفر تحديث: {0}"),("Updates.UpToDate","لديك بالفعل أحدث إصدار."),("Updates.Downloading","جارٍ تنزيل أحدث إصدار…"),("Updates.Restarting","بدأ التحديث. سيُعاد تشغيل DownTrack بعد الانتهاء.")
            ),
            ["el"] = Pack(
                ("Library.RootCount","{0} βασικοί φάκελοι"),("Home.EngineNeedsSetup","Απαιτείται ρύθμιση της μηχανής πολυμέσων"),("Home.SettingUpEngine","Ρύθμιση της μηχανής πολυμέσων…"),
                ("Explorer.SaveChangesArrow","Αποθήκευση αλλαγών  →"),("Explorer.SaveChangesCount","Αποθήκευση αλλαγών ({0})  →"),("Explorer.QueuedOne","1 σε αναμονή"),("Explorer.Queued","{0} σε αναμονή"),
                ("Explorer.PendingCreate","Σε αναμονή: δημιουργία «{0}»."),("Explorer.PendingRename","Σε αναμονή: μετονομασία «{0}»."),("Explorer.PendingDelete","Σε αναμονή: διαγραφή «{0}»."),("Explorer.MediaAddedOne","Τα πολυμέσα προστέθηκαν στις εκκρεμείς αλλαγές."),("Explorer.MediaAddedMany","Προστέθηκαν {0} στοιχεία στις εκκρεμείς αλλαγές."),("Explorer.ApplyingChanges","Εφαρμογή εκκρεμών αλλαγών…"),("Explorer.SomeNeedAttention","Ορισμένες αλλαγές χρειάζονται προσοχή."),("Explorer.Cancelled","Ακυρώθηκε: {0}"),
                ("AddMedia.PastePrompt","Επικολλήστε URL βίντεο ή playlist του YouTube."),("AddMedia.NothingFound","Δεν βρέθηκε τίποτα. Ελέγξτε τον σύνδεσμο και δοκιμάστε ξανά."),("AddMedia.OneReady","1 στοιχείο πολυμέσων είναι έτοιμο. Επεξεργαστείτε τις επιλογές πριν την προσθήκη."),("AddMedia.ManyReady","{0} στοιχεία πολυμέσων είναι έτοιμα. Κάθε γραμμή είναι ανεξάρτητη."),("AddMedia.AnalysisCancelled","Η ανάλυση ακυρώθηκε."),("AddMedia.ErrorFallback","Δεν ήταν δυνατή η ανάλυση του συνδέσμου. Δοκιμάστε ξανά."),
                ("Settings.Saved","Οι ρυθμίσεις αποθηκεύτηκαν."),("Settings.ToolsReady","Τα εργαλεία πολυμέσων είναι έτοιμα."),("Settings.ToolsMissing","Λείπει ένα ή περισσότερα εργαλεία πολυμέσων."),("Settings.ToolsUpdated","Τα εργαλεία πολυμέσων ενημερώθηκαν."),
                ("Updates.Checking","Έλεγχος για ενημερώσεις…"),("Updates.Available","Διαθέσιμη ενημέρωση: {0}"),("Updates.UpToDate","Έχετε ήδη την πιο πρόσφατη έκδοση."),("Updates.Downloading","Λήψη της πιο πρόσφατης έκδοσης…"),("Updates.Restarting","Η ενημέρωση ξεκίνησε. Το DownTrack θα επανεκκινηθεί μετά την ολοκλήρωση.")
            ),
            ["ro"] = Pack(
                ("Library.RootCount","{0} foldere rădăcină"),("Home.EngineNeedsSetup","Este necesară configurarea motorului media"),("Home.SettingUpEngine","Se configurează motorul media…"),
                ("Explorer.SaveChangesArrow","Salvează modificările  →"),("Explorer.SaveChangesCount","Salvează modificările ({0})  →"),("Explorer.QueuedOne","1 în așteptare"),("Explorer.Queued","{0} în așteptare"),
                ("Explorer.PendingCreate","În așteptare: creează „{0}”."),("Explorer.PendingRename","În așteptare: redenumește „{0}”."),("Explorer.PendingDelete","În așteptare: șterge „{0}”."),("Explorer.MediaAddedOne","Media a fost adăugată la modificările în așteptare."),("Explorer.MediaAddedMany","Au fost adăugate {0} elemente media la modificările în așteptare."),("Explorer.ApplyingChanges","Se aplică modificările în așteptare…"),("Explorer.SomeNeedAttention","Unele modificări necesită atenție."),("Explorer.Cancelled","Anulat: {0}"),
                ("AddMedia.PastePrompt","Lipește URL-ul unui videoclip sau al unui playlist YouTube."),("AddMedia.NothingFound","Nu s-a găsit nimic. Verifică linkul și încearcă din nou."),("AddMedia.OneReady","1 element media este gata. Editează opțiunile înainte de adăugare."),("AddMedia.ManyReady","{0} elemente media sunt gata. Fiecare rând este independent."),("AddMedia.AnalysisCancelled","Analiza a fost anulată."),("AddMedia.ErrorFallback","Linkul nu a putut fi analizat. Încearcă din nou."),
                ("Settings.Saved","Setările au fost salvate."),("Settings.ToolsReady","Instrumentele media sunt gata."),("Settings.ToolsMissing","Lipsește unul sau mai multe instrumente media."),("Settings.ToolsUpdated","Instrumentele media au fost actualizate."),
                ("Updates.Checking","Se verifică actualizările…"),("Updates.Available","Actualizare disponibilă: {0}"),("Updates.UpToDate","Ai deja cea mai recentă versiune."),("Updates.Downloading","Se descarcă cea mai recentă versiune…"),("Updates.Restarting","Actualizarea a început. DownTrack se va reporni după finalizare.")
            ),
            ["ja"] = Pack(
                ("Library.RootCount","{0} 個のルートフォルダー"),("Home.EngineNeedsSetup","メディアエンジンの設定が必要です"),("Home.SettingUpEngine","メディアエンジンを設定しています…"),
                ("Explorer.SaveChangesArrow","変更を保存  →"),("Explorer.SaveChangesCount","変更を保存（{0}）  →"),("Explorer.QueuedOne","1 件待機中"),("Explorer.Queued","{0} 件待機中"),
                ("Explorer.PendingCreate","保留中：『{0}』を作成します。"),("Explorer.PendingRename","保留中：『{0}』の名前を変更します。"),("Explorer.PendingDelete","保留中：『{0}』を削除します。"),("Explorer.MediaAddedOne","メディアを保留中の変更に追加しました。"),("Explorer.MediaAddedMany","{0} 件のメディアを保留中の変更に追加しました。"),("Explorer.ApplyingChanges","保留中の変更を適用しています…"),("Explorer.SomeNeedAttention","一部の変更に注意が必要です。"),("Explorer.Cancelled","キャンセルしました：{0}"),
                ("AddMedia.PastePrompt","YouTube の動画またはプレイリスト URL を貼り付けてください。"),("AddMedia.NothingFound","何も見つかりませんでした。リンクを確認してもう一度お試しください。"),("AddMedia.OneReady","1 件のメディア項目を準備しました。追加前にオプションを編集できます。"),("AddMedia.ManyReady","{0} 件のメディア項目を準備しました。各行は個別に設定できます。"),("AddMedia.AnalysisCancelled","解析をキャンセルしました。"),("AddMedia.ErrorFallback","このリンクを解析できませんでした。もう一度お試しください。"),
                ("Settings.Saved","設定を保存しました。"),("Settings.ToolsReady","メディアツールの準備ができました。"),("Settings.ToolsMissing","1 つ以上のメディアツールがありません。"),("Settings.ToolsUpdated","メディアツールを更新しました。"),
                ("Updates.Checking","更新を確認しています…"),("Updates.Available","更新があります：{0}"),("Updates.UpToDate","すでに最新バージョンです。"),("Updates.Downloading","最新バージョンをダウンロードしています…"),("Updates.Restarting","更新を開始しました。完了後に DownTrack が再起動します。")
            ),
            ["ko"] = Pack(
                ("Library.RootCount","루트 폴더 {0}개"),("Home.EngineNeedsSetup","미디어 엔진 설정이 필요합니다"),("Home.SettingUpEngine","미디어 엔진을 설정하는 중…"),
                ("Explorer.SaveChangesArrow","변경 사항 저장  →"),("Explorer.SaveChangesCount","변경 사항 저장 ({0})  →"),("Explorer.QueuedOne","1개 대기 중"),("Explorer.Queued","{0}개 대기 중"),
                ("Explorer.PendingCreate","대기 중: “{0}” 만들기."),("Explorer.PendingRename","대기 중: “{0}” 이름 변경."),("Explorer.PendingDelete","대기 중: “{0}” 삭제."),("Explorer.MediaAddedOne","미디어가 대기 중인 변경 사항에 추가되었습니다."),("Explorer.MediaAddedMany","{0}개의 미디어 항목이 대기 중인 변경 사항에 추가되었습니다."),("Explorer.ApplyingChanges","대기 중인 변경 사항을 적용하는 중…"),("Explorer.SomeNeedAttention","일부 변경 사항에 확인이 필요합니다."),("Explorer.Cancelled","취소됨: {0}"),
                ("AddMedia.PastePrompt","YouTube 동영상 또는 재생목록 URL을 붙여넣으세요."),("AddMedia.NothingFound","찾을 수 없습니다. 링크를 확인하고 다시 시도하세요."),("AddMedia.OneReady","미디어 항목 1개가 준비되었습니다. 추가하기 전에 옵션을 편집하세요."),("AddMedia.ManyReady","미디어 항목 {0}개가 준비되었습니다. 각 행은 개별적으로 설정됩니다."),("AddMedia.AnalysisCancelled","분석이 취소되었습니다."),("AddMedia.ErrorFallback","이 링크를 분석할 수 없습니다. 다시 시도하세요."),
                ("Settings.Saved","설정이 저장되었습니다."),("Settings.ToolsReady","미디어 도구가 준비되었습니다."),("Settings.ToolsMissing","하나 이상의 미디어 도구가 없습니다."),("Settings.ToolsUpdated","미디어 도구가 업데이트되었습니다."),
                ("Updates.Checking","업데이트 확인 중…"),("Updates.Available","업데이트가 있습니다: {0}"),("Updates.UpToDate","이미 최신 버전입니다."),("Updates.Downloading","최신 버전을 다운로드하는 중…"),("Updates.Restarting","업데이트가 시작되었습니다. 완료되면 DownTrack이 다시 시작됩니다.")
            ),
            ["zh-Hans"] = Pack(
                ("Library.RootCount","{0} 个根文件夹"),("Home.EngineNeedsSetup","需要设置媒体引擎"),("Home.SettingUpEngine","正在设置媒体引擎…"),
                ("Explorer.SaveChangesArrow","保存更改  →"),("Explorer.SaveChangesCount","保存更改（{0}）  →"),("Explorer.QueuedOne","1 个排队中"),("Explorer.Queued","{0} 个排队中"),
                ("Explorer.PendingCreate","待处理：创建“{0}”。"),("Explorer.PendingRename","待处理：重命名“{0}”。"),("Explorer.PendingDelete","待处理：删除“{0}”。"),("Explorer.MediaAddedOne","媒体已加入待处理更改。"),("Explorer.MediaAddedMany","已将 {0} 个媒体项目加入待处理更改。"),("Explorer.ApplyingChanges","正在应用待处理更改…"),("Explorer.SomeNeedAttention","部分更改需要注意。"),("Explorer.Cancelled","已取消：{0}"),
                ("AddMedia.PastePrompt","粘贴 YouTube 视频或播放列表网址。"),("AddMedia.NothingFound","未找到内容。请检查链接后重试。"),("AddMedia.OneReady","1 个媒体项目已准备好。添加前可以编辑选项。"),("AddMedia.ManyReady","{0} 个媒体项目已准备好。每行可独立设置。"),("AddMedia.AnalysisCancelled","分析已取消。"),("AddMedia.ErrorFallback","无法分析此链接，请重试。"),
                ("Settings.Saved","设置已保存。"),("Settings.ToolsReady","媒体工具已准备就绪。"),("Settings.ToolsMissing","缺少一个或多个媒体工具。"),("Settings.ToolsUpdated","媒体工具已更新。"),
                ("Updates.Checking","正在检查更新…"),("Updates.Available","有可用更新：{0}"),("Updates.UpToDate","你已经是最新版本。"),("Updates.Downloading","正在下载最新版本…"),("Updates.Restarting","更新已开始。完成后 DownTrack 将重新启动。")
            ),
            ["zh-Hant"] = Pack(
                ("Library.RootCount","{0} 個根資料夾"),("Home.EngineNeedsSetup","需要設定媒體引擎"),("Home.SettingUpEngine","正在設定媒體引擎…"),
                ("Explorer.SaveChangesArrow","儲存變更  →"),("Explorer.SaveChangesCount","儲存變更（{0}）  →"),("Explorer.QueuedOne","1 個排隊中"),("Explorer.Queued","{0} 個排隊中"),
                ("Explorer.PendingCreate","待處理：建立「{0}」。"),("Explorer.PendingRename","待處理：重新命名「{0}」。"),("Explorer.PendingDelete","待處理：刪除「{0}」。"),("Explorer.MediaAddedOne","媒體已加入待處理變更。"),("Explorer.MediaAddedMany","已將 {0} 個媒體項目加入待處理變更。"),("Explorer.ApplyingChanges","正在套用待處理變更…"),("Explorer.SomeNeedAttention","部分變更需要注意。"),("Explorer.Cancelled","已取消：{0}"),
                ("AddMedia.PastePrompt","貼上 YouTube 影片或播放清單網址。"),("AddMedia.NothingFound","找不到內容。請檢查連結後再試一次。"),("AddMedia.OneReady","1 個媒體項目已準備好。新增前可以編輯選項。"),("AddMedia.ManyReady","{0} 個媒體項目已準備好。每列可獨立設定。"),("AddMedia.AnalysisCancelled","分析已取消。"),("AddMedia.ErrorFallback","無法分析此連結，請再試一次。"),
                ("Settings.Saved","設定已儲存。"),("Settings.ToolsReady","媒體工具已準備就緒。"),("Settings.ToolsMissing","缺少一個或多個媒體工具。"),("Settings.ToolsUpdated","媒體工具已更新。"),
                ("Updates.Checking","正在檢查更新…"),("Updates.Available","有可用更新：{0}"),("Updates.UpToDate","你已經是最新版本。"),("Updates.Downloading","正在下載最新版本…"),("Updates.Restarting","更新已開始。完成後 DownTrack 會重新啟動。")
            )
        };
}
