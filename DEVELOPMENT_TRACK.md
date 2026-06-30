# Трек разработки Order Rush Kitchen

Последнее обновление: 2026-06-25

## Назначение
Единый рабочий документ для отслеживания дальнейшей разработки.

Использовать файл для того, чтобы:
- держать в фокусе текущую цель
- видеть статус по этапам
- не начинать зависимые задачи слишком рано
- фиксировать изменения в объёме работ без потери общей структуры

## Обозначения статусов
- `TODO` не начато
- `IN PROGRESS` в работе
- `BLOCKED` заблокировано зависимостью или решением
- `DONE` завершено и встроено в проект

## Текущая база
- В проекте уже есть прототип с движением игрока, стойками, тарелкой, нарезкой, жаркой, подгоранием, доставкой и базовым UI заказов.
- Текущий цикл игры всё ещё построен вокруг модели `одно блюдо = один рецепт = один заказ = одна сдача`.
- Главное архитектурное ограничение: логика заказа, блюда, тарелки и сдачи слишком сильно завязана на старую single-dish модель.

## Активная цель
Построить гибкую систему заказов, которая поддерживает:
- сложность рецептов и отдельное давление по времени на каждый рецепт
- заказы из нескольких позиций
- выбор ингредиентов и напитков через UI-сетку
- прогрессию по уровням
- универсальную сборку блюд вместо burger-ориентированной тарелки
- более чистое разделение по MVC

## Этапы

### M1. Переработка доменной модели
Статус: `DONE`

Цель:
Ввести корректную модель данных для блюд, напитков, заказов и уровней.

Задачи:
- [ ] Заменить текущую dish-only модель рецепта на более общую модель menu item.
- [ ] Добавить поля для сложности, давления по времени, категории, иконки и веса появления.
- [ ] Ввести runtime-модели `Order`, `OrderItem`, `ActiveOrder`.
- [ ] Ввести модели конфигурации и runtime-состояния для уровней.
- [ ] Отделить определения данных от runtime-состояния.

Критерий готовности:
- Новая модель данных может описывать блюдо, напиток и заказ из нескольких позиций без специальных исключений.
- Новый код больше не зависит от предположения `DishRecipeSo == активный заказ`.

Зависимости:
- нет

### M2. Пересборка системы заказов
Статус: `DONE`

Цель:
Заменить текущую очередь delivery на полноцененный жизненный цикл заказа.

Задачи:
- [x] Заменить текущее поведение `DeliveryService` на генерацию и сопровождение активных заказов.
- [x] Заказы должны появляться при взаимодействии со стойкой принятия заказа, а не по глобальному таймеру.
- [x] Каждый активный заказ должен иметь собственное время жизни или шкалу удовлетворённости.
- [x] Добавить поддержку частичного выполнения заказа из нескольких позиций.
- [x] Сохранить события успеха и провала для UI и аудио.

Критерий готовности:
- Игра умеет создавать, обновлять, истекать, завершать и удалять активные заказы.
- Один заказ может содержать несколько позиций.

Зависимости:
- M1

### M3. Универсальная сборка блюда и сдача заказа
Статус: `DONE`

Цель:
Сделать сборку и сдачу независимыми от burger-ориентированной логики тарелки.

Задачи:
- [ ] Переработать `PlateKitchenObject`, чтобы он поддерживал универсальную сборку блюд.
- [ ] Переработать визуал тарелки так, чтобы он опирался на данные блюда, а не на жёстко заданную композицию бургера.
- [ ] Изменить `DeliveryCounter`, чтобы он умел проверять и сдавать позиции заказа в новой модели.
- [ ] Решить и реализовать, как именно переносятся и сдаются напитки.
- [ ] Сохранить совместимость новых моделей с текущей логикой нарезки и жарки.

Критерий готовности:
- Можно собирать разные блюда с непересекающимися ингредиентами.
- Сдача поддерживает заказы из нескольких позиций и, при необходимости, не только тарелки.

Зависимости:
- M1
- M2

### M4. Стойки выбора через UI-сетку
Статус: `DONE`

Цель:
Заменить стойки-источники одного предмета на переиспользуемые стойки выбора.

Задачи:
- [x] Заменить текущее поведение `ContainerCounter` на открытие UI-сетки ингредиентов.
- [x] Добавить стойку напитков с тем же паттерном взаимодействия.
- [x] Вынести общую логику выбора в переиспользуемый UI/presenter слой.
- [x] После подтверждения выбора выдавать игроку выбранный объект.

Критерий готовности:
- Одна стойка ингредиентов умеет отдавать несколько ингредиентов через UI.
- Стойка напитков использует тот же подход и даёт выбор из нескольких напитков.

Зависимости:
- M1

### M5. Стойка принятия заказа и новый UI заказов
Статус: `DONE`

Цель:
Добавить явное принятие заказа игроком и новый UI состояния заказов.

Задачи:
- [ ] Создать стойку принятия заказа.
- [ ] При взаимодействии создавать или запрашивать новый активный заказ.
- [ ] Заменить текущий UI карточек рецептов на UI карточек заказов.
- [ ] Показывать в UI состав заказа, текущий статус и шкалу удовлетворённости или прогресса.
- [ ] Поддержать отображение частично выполненных заказов.

Критерий готовности:
- Игрок явно принимает заказ со стойки.
- UI ясно показывает активные заказы и запас времени или удовлетворённости по каждому.

Зависимости:
- M2

### M6. Система уровней
Статус: `DONE`

Цель:
Добавить прогрессию по 5 уровням.

Задачи:
- [x] Ввести отдельный сервис прогрессии уровней.
- [x] Определить количество выполненных заказов для завершения каждого уровня.
- [x] Определить, какие блюда и напитки доступны на каждом уровне.
- [x] Повышать сложность заказов и шанс multi-item заказа по мере роста уровня.
- [ ] При необходимости повышать давление по времени или скорость падения удовлетворённости. *(deferred до финального баланса после M7/M8)*
- [x] Сбрасывать runtime-состояние сцены при переходе на следующий уровень.

Критерий готовности:
- Прогресс игры строится по уровням, а не только по общему таймеру.
- Поздние уровни создают более сложные и разнообразные заказы.

Зависимости:
- M1
- M2
- M5

### M7. Доработки cooking/domain gaps
Статус: `DONE`

Цель:
Закрыть ограничения текущей доменной модели блюд, которые сознательно не вошли в M6.

Задачи:
- [x] Добавить поддержку duplicate ingredients в составе блюда.
- [x] Вернуть `Double_fried_potato` в активную прогрессию после поддержки дублей.
- [x] Ввести новую cooking process основу для single/multi input -> output.
- [x] Реализовать pot/soup flow на новой cooking process основе.
- [x] Постепенно мигрировать cutting/frying/burning с legacy `ProcessRecipeSo`/`RecipeDatabase`.
- [x] Вернуть `Soup` в активную прогрессию после реализации pot/soup flow.

Критерий готовности:
- Блюда с повторяющимися ингредиентами корректно собираются и резолвятся.
- Soup готовится отдельным понятным flow и может участвовать в заказах.

Зависимости:
- M3
- M5
- M6

### M8. Mobile input и управление
Статус: `DONE`

Цель:
Перевести input на поддержку мобильного управления без поломки текущего desktop flow.

Задачи:
- [x] Зафиксировать целевой mobile input UX.
- [x] Развести desktop/mobile bindings и platform-specific UI без полного рефакторинга `InputService`.
- [x] Добавить мобильные controls для движения, interact и alternate interact.
- [x] Добавить общую UI-кнопку паузы для desktop/mobile.
- [x] Скрывать mobile gameplay controls при открытом selection UI.
- [x] Разделить HUD на Canvas-слои и добавить platform visibility для desktop/mobile UI.
- [x] Проверить selection UI, order UI и level UI на мобильном сценарии.

Критерий готовности:
- Игровой цикл можно пройти на мобильном управлении.
- Desktop input остаётся рабочим.

Зависимости:
- M4
- M5
- M6

### M9. Content integration pass
Статус: `DONE`

Цель:
Интегрировать финальные модели counters и ключевой presentation-контент после стабилизации cooking flow и mobile constraints.

Задачи:
- [x] Заменить временные counter models на итоговые.
- [x] Подогнать scale, colliders, interaction points и hold points под новые модели.
- [x] Проверить читаемость ингредиентов, тарелок, напитков и заказов с игровой камеры.
- [x] Сделать functional layout pass для UI после замены моделей и mobile проверки.
- [x] Перевести основные gameplay/modal UI окна на новый presentation style, иконки и шрифт.
- [x] Зафиксировать оставшиеся visual-polish задачи отдельно от gameplay-интеграции.

Критерий готовности:
- Финальные counters не ломают interaction flow, collision, camera readability и mobile layout.
- Gameplay можно проходить на новых моделях без временных interaction костылей.

Зависимости:
- M7
- M8

### M10. Unity architecture normalization
Статус: `DONE`

Цель:
Нормализовать архитектуру проекта под Unity: уменьшить связность между domain/use-case сервисами, scene adapters, UI view и platform/infrastructure слоями без догматичного "чистого MVC".

Задачи:
- [x] Зафиксировать архитектурные правила M10: domain/use-case/view/presenter/infrastructure boundaries.
- [x] Переработать game/order lifecycle через Zenject `ITickable`/`IInitializable` там, где `MonoBehaviour` сейчас является только tick/subscription adapter.
- [x] Добавить единую pause policy для gameplay/modal/user/ad/level-complete состояний; modal windows должны останавливать gameplay timers/processes.
- [x] Разделить input layer: gameplay input, rebinding, storage, platform-specific UI/control policy.
- [x] Заменить static/legacy loading flow на injectable navigation/loading service, подготовленный к Addressables.
- [x] Нормализовать audio/music managers: service boundaries, volume storage, scene audio adapter, future settings/mixer seam.
- [x] Добавить основу сохранения прогресса пользователя: unlocked/current level, basic profile progress, storage abstraction.
- [x] Разделить UI orchestration и view там, где UI владеет use-case логикой (`OptionsUI`, order UI, pause/game over/level complete flow).
- [x] Провести counter/object interaction cleanup: общие операции holder transfer/destroy/progress/cooking state без лишней иерархии.
- [x] Добавить level-transition reset для player position/state, чтобы переход уровня ощущался как чистый старт.
- [x] Поддержать player-side alternate serve flow, если это вписывается в нормализованную interaction architecture.
- [x] Провести Zenject inventory: bindings, lifetimes, install order, `NonLazy`, scene components, future SDK boundaries.
- [x] Удалить или изолировать legacy после миграций: старые recipe базы, debug/probe leftovers, static/global остатки.
- [x] Нормализовать иерархию кода и namespace: провести inventory, зафиксировать правила и структурировать файлы по доменным зонам.
- [x] Выполнить финальный naming pass M10: привести namespaces, project identifiers и оставшиеся пользовательские/технические упоминания к итоговому названию `Order Rush Kitchen`.

Отдельный gameplay backlog, не блокирующий M10:
- [x] M10.15. Переработать `DeliveryCounter` в staging-модель: блюдо кладётся на delivery counter, связывается с заказом и удаляется при закрытии заказа успехом/провалом.
  - [x] M10.15.1. Возвращать из submission конкретный принявший `ActiveOrder`.
  - [x] M10.15.2. Расширить fulfillment receipt конкретным `OrderItem` и добавить безопасный batch rollback staged-позиций.
  - [x] M10.15.3. Реализовать bound multi-slot `DeliveryCounter` со state flow `Unbound`/`Staging`/`Resolving`.
  - [x] M10.15.4. Настроить staging presentation: устойчивое размещение до трёх блюд, единый staged-scale и скрытие вспомогательного ingredient UI.
  - [x] M10.15.5. Добавить fade cleanup для completed/failed заказа и optional alt-clear для активного staged-заказа.
    - [x] M10.15.5.1. Добавить fade cleanup staged-блюд после completed/failed заказа.
    - [x] M10.15.5.2. Добавить alt-clear активной стойки с безопасным batch rollback и освобождением reservation.
  - [x] M10.15.6. Настроить prefab и провести ручной regression-прогон.
- [x] M10.16. Вынести delivery staging workflow из `DeliveryCounter` в plain controller/session.
  - [x] M10.16.1. Разделить order/reservation/session state и Unity scene adapter.
  - [x] M10.16.2. Подтвердить Unity compilation и ручной regression delivery staging.

Критерий готовности:
- Gameplay/use-case логику можно читать и тестировать отдельно от UI и scene objects.
- `MonoBehaviour`-классы в основном играют роль scene adapters/view/input entry points.
- Managers split подготовлен к Addressables, Firebase, AppLovin и возможному UniTask без протекания SDK-кода в gameplay/UI.
- User progress сохраняется через абстракцию storage, а не напрямую из UI/gameplay классов.

Зависимости:
- M2
- M3
- M4
- M5
- M6

### M11. Локализация
Статус: `DONE`

Цель:
Подготовить проект к поддержке нескольких языков интерфейса и игрового текста без протекания локализационной инфраструктуры в gameplay/domain-логику.

Задачи:
- [x] M11.1. Провести инвентаризацию локализации и зафиксировать правила: локали, исходный язык, формат ключей, что локализуем и что остаётся техническим.
- [x] M11.2. Добавить основу локализации: Unity Localization package, локали `en`/`ru`, таблицы строк и выбор локали на уровне приложения с сохранением.
- [x] M11.3. Перевести статичные TMP-тексты сцен и префабов на локализуемые строки.
- [x] M11.4. Перевести динамические UI-строки из кода на локализуемые форматные строки.
- [x] M11.5. Добавить переключение языка в `OptionsUI` через controller/service без прямой инфраструктурной логики во view-классе.
- [x] M11.6. Провести регрессионную и layout-проверку для `en`/`ru` на MainMenu, GameScene, Options, Orders, LevelComplete/GameOver и mobile layout.

Критерий готовности:
- Основной UI и игровой текст переключаются между локалями без ручной правки кода.
- Добавление нового языка не требует переписывания логики.
- Жёстко зашитые пользовательские строки не остаются в активных UI flow, кроме явно признанных технических/placeholder-строк.
- `MenuItemDefinitionSo.displayName` не мигрируется в M11, так как display name menu items сейчас не отображается в пользовательском UI.

Зависимости:
- M5
- M6
- M7

### M12. SDK integration
Статус: `DONE`

Цель:
Интегрировать Firebase через инфраструктурный слой без протекания SDK-кода в gameplay/UI.

Задачи:
- [x] Добавить Firebase foundation: initialization, status/error handling и базовые infrastructure bindings.
- [x] Ввести analytics boundary (`IAnalyticsService`) и покрыть ключевые gameplay/UI events без прямых SDK-вызовов из игровых классов.
- [x] Ввести crash reporting boundary для Crashlytics/non-fatal unexpected states.
- [x] Провести Android smoke test: cold start, смена сцен, Firebase init, отсутствие SDK errors в логах.

Критерий готовности:
- Firebase инициализируется через app/infrastructure слой.
- Gameplay/UI зависят только от внутренних interfaces, а не от SDK API.
- Analytics/Crashlytics работают как техническая telemetry/crash diagnostics без пользовательской/profile статистики.

Зависимости:
- M10
- M11

### M13. Финальная полировка, баланс и presentation
Статус: `TODO`

Цель:
Довести presentation-слой, баланс и визуальные детали до целевого состояния после стабилизации gameplay, mobile, localization и SDK integration.

Задачи:
- [ ] Обновить визуал тарелки и предметов под финальный набор блюд и напитков.
- [ ] Заменить временные UI-фоны, цвета-плейсхолдеры и декоративные элементы.
- [ ] Поправить анимации игрока и взаимодействий.
- [ ] Доработать UI после стабилизации геймплейного цикла и mobile layout.
- [ ] Выполнить финальный баланс времени, заказов и сложности уровней.
- [ ] Проверить, что финальный контент согласован с локализацией.

Критерий готовности:
- Визуальная часть соответствует новым системам и целевому контенту проекта.

Зависимости:
- M3
- M4
- M5
- M6
- M8
- M9
- M11
- M12

## Рекомендуемый порядок выполнения
1. M1. Переработка доменной модели
2. M2. Пересборка системы заказов
3. M3. Универсальная сборка блюда и сдача заказа
4. M4. Стойки выбора через UI-сетку
5. M5. Стойка принятия заказа и новый UI заказов
6. M6. Система уровней
7. M7. Доработки cooking/domain gaps
8. M8. Mobile input и управление
9. M9. Content integration pass
10. M10. Unity architecture normalization
11. M11. Локализация
12. M12. SDK integration
13. M13. Финальная полировка, баланс и presentation

## Ближайший фокус
Статус: `TODO`

Фокус:
- Подготовить итоговый commit M12 и переход к M13.
- M13: финальная полировка, баланс, release hygiene и presentation.
- Follow-up для M13/release hygiene: Android App Info warning в Localization, размер Android build (~400-412 MB), проверка package size через AAB/release build.

## Журнал решений
- 2026-04-11: Текущая архитектура уже частично сервисная, но игровая доменная модель всё ещё построена вокруг старого single-dish цикла.
- 2026-04-11: Новые фичи не стоит наращивать поверх старых предположений `DeliveryService`.
- 2026-04-11: Локализация добавлена как один из последних этапов, после стабилизации основных систем и до финального контентного прохода.
- 2026-04-13: Этап M1 завершён. Новые menu item, level и runtime order модели добавлены параллельно старой системе без её замены.
- 2026-04-14: M2 начат. Добавлен базовый OrderService с хранением ActiveOrder, ручным созданием заказов, Tick и отдельной очисткой неактивных заказов.
- 2026-04-17: M2 всё ещё в работе. Generation policy и причины отказа формализованы; для закрытия этапа требуется перевести runtime с legacy delivery-контура на новый order lifecycle и добавить совместимые события для будущих UI/audio слоёв.
- 2026-04-18: В M2 добавлены lifecycle-события в `IOrderService/OrderService` (created/updated/completed/failed/removed) с единым `OrderServiceEventArgs`; `ActiveOrder` оставлен доменной моделью без внешних событий.
- 2026-04-18: Legacy delivery spawn-контур отключён (удалён `DeliveryManager` из сцены, создание заказов оставлено только через `OrderCounter -> IOrderFlowService`).
- 2026-04-19: Этап M2 завершён. Legacy delivery-контур удалён, создание/жизненный цикл/сдача заказов переведены на `OrderService` + `OrderFlowService` + `OrderSubmissionService`; фокус переключен на M3.
- 2026-04-19: Для M3 принято решение по тарелке: вводим двухфазную модель `Assembly/Served`; блюдо фиксируется только явным действием сервировки (alternate interact), без автоматической конвертации по составу ингредиентов.
- 2026-04-20: Закрыт подэтап M3.1: `PlateKitchenObject` переведён на состояние `Assembly/Served`, добавлены новые события состояния и обновлён рендер тарелки через `Serving` (`PlateContentVisual`/`PlateIconsUI`) без burger-специфики.
- 2026-04-20: Закрыт подэтап M3.3: добавлена явная сервировка на `ClearCounter.InteractAlternate` (резолв блюда из ингредиентов и перевод тарелки `Assembly -> Served`), `DeliveryCounter` принимает к сдаче только `Served`-тарелки.
- 2026-04-25: M3 закрыт по результатам ручного прогона сценариев (тарелка, напиток, смешанный поток): flow работает штатно.
- 2026-04-25: Закрыт подэтап M4.1: добавлен `IItemSelectionService` с режимами выбора и состоянием; `ContainerCounter`/`DrinkCounter` переведены на открытие selection-потока вместо прямой выдачи.
- 2026-04-26: Закрыт подэтап M4.3: реализован минимальный рабочий UI выбора с полками, подтверждением выбора через `IItemSelectionService` и выдачей предмета в target holder.
- 2026-04-26: M4 закрыт: выбор ингредиентов и напитков переведён на UI-сетку с контекстными опциями от стойки, подтверждение выбора выдаёт объект через `ItemSelectionService`, добавлена modal-блокировка взаимодействий игрока при открытой панели.
- 2026-04-26: Закрыт подэтап M5.1: `OrderCounter` переведён на явный контракт результата взаимодействия (`OnOrderAccepted`/`OnOrderAcceptFailed`) поверх `IOrderFlowService.TryCreateOrder()`.
- 2026-04-26: Для M5 зафиксирован целевой mobile UX: компактная верхняя лента заказов; рабочий лимит `3` активных заказа и `до 2` позиций в заказе, чтобы UI не перекрывал геймплей.
- 2026-04-26: Для M5 зафиксирован макет UI заказов: зона карточек в верхней части экрана; каждая карточка содержит один общий таймер/прогресс и отдельные `OrderItem`-секции (иконка блюда + список ингредиентов именно этого блюда); completed-секция маркируется отдельно, без объединения ингредиентов разных блюд.
- 2026-05-01: Этап M5 закрыт: добавлены `OrderCounter`-события результата, UI-индикация принятия/отказа, обновлён `OrdersTopBar` с финальным статусным отображением, включена задержка удаления неактивных заказов для видимого фидбека.
- 2026-05-04: Закрыт подэтап M6.1: добавлен completion flow завершения уровня с окном результата, переходом на следующий уровень и очисткой активных заказов; переход в меню оставлен вне scope до отдельного решения по navigation/loading.
- 2026-05-05: Закрыт подэтап M6.2: добавлен HUD текущего уровня и прогресса заказов `X/Y` через отдельный `LevelProgressUI`, подписанный на прогресс уровня и смену текущего уровня.
- 2026-05-06: Закрыт подэтап M6.3: импортирован и подключён новый контентный набор для прогрессии уровней (`KitchenObjectSo`, `MenuItemSo`, process recipes, ингредиенты, напитки и scene wiring); `Soup` и `Double_fried_potato` оставлены вне активного пула до отдельных этапов по soup flow и duplicate ingredients.
- 2026-05-06: Закрыт подэтап M6.4: настроены 5 `LevelDefinitionSo` с ростом `requiredCompletedOrders`, `maxActiveOrders`, `multiItemOrderChance` и `min/maxItemsPerOrder`.
- 2026-05-07: Закрыт подэтап M6.5: добавлен reset runtime-состояния при переходе уровня через `ILevelResettable` и `LevelSceneResetService`; очищаются предметы на holder'ах, плита, тарелки, активные заказы и таймер уровня.
- 2026-05-07: Этап M6 закрыт после ручной проверки полного прохода уровней 1-5: прогрессия, unlock контента, HUD, completion flow и reset runtime-состояния работают штатно; финальная настройка давления времени отложена до баланса после M7/M8.
- 2026-05-07: В roadmap добавлен отдельный M9 `Content integration pass` перед MVC cleanup: финальные counter models, scale/colliders/hold points и functional UI layout после mobile constraints отделены от финальной полировки.
- 2026-05-07: Закрыт подэтап M7.1: добавлена проверка partial composition через `PlateCompositionValidator`/`PlateAssemblyService`, counters переведены на единый путь добавления ингредиентов на тарелку, разрешены duplicate ingredients и `Double_fried_potato` включён в прогрессию с `minLevel = 4`.
- 2026-05-07: Для M7.2 принято решение не вводить отдельную pot-only recipe system. Вместо этого вводим новую cooking process основу `single/multi input -> output`, которую сначала использует pot/soup flow, а затем на неё постепенно мигрируют cutting/frying/burning; legacy `ProcessRecipeSo`/`RecipeDatabase` не расширяем под pot.
- 2026-05-08: Закрыт подэтап M7.2.1: добавлена новая cooking process модель (`CookingProcessRecipeSo`, timed/action recipes, list asset) и `CookingProcessRecipeResolver`; подключён новый recipe list в `GameInstaller`, старый runtime пока не мигрирован.
- 2026-05-09: Закрыт подэтап M7.2.2: `CuttingCounter` и `StoveCounter` переведены на новую cooking process модель, runtime больше не использует legacy `RecipeDatabase`.
- 2026-05-09: Закрыт подэтап M7.3: добавлен `PotCounter`, pot/soup recipe flow, общий composition icons UI для plate/pot/served menu item и `Soup` возвращён в активную прогрессию.
- 2026-05-09: Этап M7 завершён: duplicate ingredients, новая cooking process основа, миграция текущих process counters и soup flow встроены в gameplay.
- 2026-05-10: M8 начат. Зафиксирован mobile UX: landscape, fixed joystick слева, две gameplay-кнопки справа снизу (`Interact`/`AlternateInteract`), общая pause-кнопка в HUD, selection UI скрывает gameplay controls.
- 2026-05-10: Закрыт подэтап M8.1: в `Actions.inputactions` добавлены mobile bindings через virtual gamepad (`leftStick`, `buttonSouth`, `buttonEast`, `start`), собран `MobileControlCanvas` и добавлена общая `PauseButtonUI`.
- 2026-05-11: Закрыт подэтап M8.2: добавлен `MobileGameplayControlsUI`, который скрывает mobile gameplay controls на время selection flow через `IItemSelectionService` и `CanvasGroup`, оставляя pause button отдельным UI-элементом.
- 2026-05-11: Закрыт подэтап M8.3: игровой UI разделён на `DynamicHudCanvas`, `StaticHudCanvas`, `MobileHudCanvas` и `ModalCanvas`; `OptionsUI` разделён на common и desktop input секции; добавлен `PlatformUiVisibility` для desktop/mobile UI roots.
- 2026-05-11: Закрыт подэтап M8.4: mobile/desktop UI flow проверен функционально; `OrdersTopBar` и `SelectionUI` признаны рабочими, но визуально сырыми, финальная подгонка размеров отложена до content/layout pass после финальных моделей, камеры, иконок и фонов.
- 2026-05-11: Этап M8 завершён: mobile input добавлен через New Input System virtual gamepad, mobile gameplay controls и pause UI подключены, HUD разделён по Canvas-слоям, desktop/mobile UI roots управляются через `PlatformUiVisibility`; полный cleanup `InputService`/rebinding оставлен на архитектурный этап.
- 2026-05-11: M9 начат. Первый шаг: inventory counter prefabs, scene instances и доступных финальных моделей перед заменой визуала и настройкой scale/colliders/hold points.
- 2026-05-15: Закрыт основной M9 content/UI pass: финальные counter visuals интегрированы в `GameScene`, подогнаны scale/colliders/hold points, обновлены иконки ингредиентов/menu items, selection UI получил разные backgrounds/layout для ingredients/drinks, `OrdersTopBar` упрощён до компактных карточек с popup details по нажатию, добавлен world-space background за пределами кухни.
- 2026-05-18: Закрыт UI presentation pass внутри M9: `GamePauseUI`, `OptionsUI`, `GameOverUI`, `GameStartCountdownUI`, `LevelProgressUI` и связанные modal/HUD элементы приведены к новому стилю с обновлёнными фонами, иконками, шрифтом и desktop/mobile layout.
- 2026-05-18: Этап M9 завершён. Content integration, functional UI layout и presentation pass доведены до gameplay-ready состояния; оставшаяся косметическая полировка без gameplay/blocker рисков остаётся в M12.
- 2026-05-18: M10 начат как Unity architecture normalization, а не чистый MVC. Принято решение переработать `Managers` кроме `Installer`, использовать Zenject lifecycle для service-level runtime loops, подготовить seams под Addressables/Firebase/AppLovin/UniTask и добавить сохранение пользовательского прогресса.
- 2026-05-18: Закрыт подэтап M10.2: `GameManager` и `OrderManager` заменены на Zenject runtime adapters (`GameRuntime`, `OrderRuntime`) через `ITickable`/`IInitializable`; scene-level tick adapters удалены из `GameScene`.
- 2026-05-19: Для pause policy принято решение объединить `GameOver` и `LevelComplete` в одну причину `LevelRunEnded`: pause service отвечает за остановку gameplay, а различие success/fail остаётся в level run result/UI flow.
- 2026-05-19: M10.3 частично готов: `UserPause` и `ModalPause` переведены на `GamePauseService`/`IGameClock`; game/order timers, plates, stove/pot cooking, player movement/interactions и modal windows стали pause-aware. Осталось подключить `LevelRunEnded` для game over/level complete и удалить старый pause API из `GameService`.
- 2026-05-19: Закрыт подэтап M10.3: добавлена единая pause policy через `GamePauseService`/`IGameClock`, покрыты `UserPause`, `Modal` и `LevelRunEnded`; старый pause API (`TogglePauseGame`, `OnGamePaused`, `OnGameUnpaused`, `Time.timeScale` в `GameService`) удалён.
- 2026-05-20: Закрыт подэтап M10.4.1: `IInputService` разделён на `IGameplayInputService` и `IInputRebindingService`; `InputService` переведён на Zenject lifecycle, desktop rebinding больше не зависит от порядка bindings, добавлены cancel/guard для interactive rebinding и formatting `Escape -> Esc`.
- 2026-05-22: Закрыт подэтап M10.4.2: `Player` input/selection cleanup выполнен; selection raycast теперь следует визуальному направлению персонажа, `interactionLayerMask` отделён от movement collision, `IPlayerInteractable` сознательно перенесён в будущий counter/object interaction cleanup.
- 2026-05-22: Закрыт M10.4: input layer разделён на gameplay/rebinding/storage, player-side input usage нормализован; platform-specific UI оставлен через существующий `PlatformUiVisibility` как достаточный scene adapter до появления build-time/Addressables policy.
- 2026-05-22: Для M10.5 зафиксирована navigation policy: переход между уровнями в текущей кухне выполняется через reset runtime-состояния и замену level config без перезагрузки сцены; `Retry`, `Start` и `Return to Menu` из UI выполняют scene-level navigation. Будущие разные кухни рассматриваются как location-level transition через scene loading, но без преждевременной реализации.
- 2026-05-23: Закрыт M10.5: legacy static `LoadingManager`/`LoadingScene` заменены на app-level `ProjectContext`, `INavigationService`, `ISceneLoader`, async scene loading через UniTask и persistent `LoadingScreenUI`; `MainMenuUI`, `GamePauseUI` и `GameOverUI` переведены на injectable navigation.
- 2026-05-28: В M10.6 закрыт gameplay SFX slice: audio settings/storage нормализованы, one-shot/global/loop playback разделены, counter/plate/order audio переведены на scene adapters, покрыты базовые gameplay events и `InvalidAction`.
- 2026-05-28: Закрыт M10.6: audio/music layer нормализован; gameplay SFX и music lifecycle разделены на app-level settings/storage, reusable playback players, event/loop services, scene adapters и `MusicService`/`MusicTrackLibrarySo` с запуском треков через scene starters.
- 2026-05-31: Закрыт M10.7: добавлен app-level `UserProgressService`/`IUserProgressStorage` поверх `PlayerPrefs`, сохранение `current/max unlocked level`, старт уровня из сохранённого прогресса и обновление прогресса при completion/переходе уровня.
- 2026-05-31: Для M10.8 проведена UI inventory. Решено начать с MainMenu level selection как первого потребителя `UserProgressService`; далее идти через pause/gameover/levelcomplete orchestration, `OptionsUI`, Orders UI и modal policy cleanup без ввода тяжёлого UI framework.
- 2026-06-02: Закрыт M10.8.1: `MainMenuScene` получила новый diorama/background UI, title/menu assets и `LevelSelectionUI`; выбор уровня строится от `LevelDatabase`/`IUserProgressService`, locked уровни некликабельны и отображаются через lock overlay, выбранный уровень сохраняется и запускает `GameScene`.
- 2026-06-02: Закрыт M10.8.2: pause, game over и level complete UI переведены на intent-events; navigation/pause/options orchestration вынесена в отдельные controllers, а `OptionsUI.Show(Action)` заменён на простой open/close contract.
- 2026-06-04: Закрыт M10.8.3: `OptionsUI` вынесен в reusable prefab для `GameScene` и `MainMenuScene`; view отделён от audio/input orchestration через `OptionsUIController`, rebinding вынесен в app-level `InputRebindingService`, а gameplay pause-закрытие оставлено в отдельном `GameOptionsPauseController`.
- 2026-06-04: Закрыт M10.8.4: `OrdersPanelUI` отделён от `IOrderService`; lifecycle карточек и выбор заказа остались во view, а подписки на order events, tick-refresh и открытие `OrderDetailsUI` вынесены в `OrdersPanelUIController`.
- 2026-06-04: Закрыт M10.8.5: `ItemSelectionUI` отделён от `IItemSelectionService`; selection view теперь отдаёт close/item-selected intents, `ItemSelectionButtonUI` больше не подтверждает выбор напрямую, а service orchestration вынесена в `ItemSelectionUIController` с сохранением modal pause policy через `IItemSelectionService`.
- 2026-06-05: M10.8 закрыт. UI naming/folder pass (`M10.8.6`) сознательно отложен до будущей legacy/namespace cleanup, чтобы совместить переносы файлов и namespace с общей чисткой, а не создавать отдельный scene/prefab serialization churn.
- 2026-06-11: Закрыт M10.9.1: введён `IPlayerInteractable`; `Player` обнаруживает, хранит и вызывает выбранную цель через interaction contract вместо прямой зависимости от `BaseCounter`, а `SelectedCounterVisual` намеренно оставлен counter-specific.
- 2026-06-11: Закрыт M10.9.2: `ObjectHolder` получил безопасные transfer/destroy operations; простые interaction flows и level reset переведены на общий holder API с сохранением виртуального поведения `Player.SetObject` и counter-specific audio.
- 2026-06-11: Закрыт M10.9.3: `PlateAssemblyService` получил общий add-and-consume flow, который изменяет состав тарелки до потребления scene object и сохраняет ingredient при отказе; `ClearCounter`, `CuttingCounter`, `StoveCounter` мигрированы на этот use-case.
- 2026-06-11: Закрыт M10.9.4: process counters переведены на безопасные holder operations и явные state/progress reset flows без новой cooking hierarchy; `ObjectHolder` усилен атомарным transfer, `TrySetObject`, `TrySpawnAndSet` и защищённым receive-hook для player-specific side effects.
- 2026-06-11: M10.9 закрыт после успешной Unity compilation и ручной проверки interaction, selection visual, transfer/destroy, plate assembly, cutting, frying/burning, pot cooking и level reset. Разделение `BaseCounter`/`ObjectHoldingCounter` и автоматические holder tests оставлены как отдельные будущие улучшения, не блокирующие текущий cleanup.
- 2026-06-11: Закрыт M10.10: переход на следующий уровень теперь сбрасывает позицию, поворот, held object, walking и interaction selection игрока; runtime-состояние сцены очищается, а level run заново проходит waiting/countdown/gameplay flow без перезагрузки сцены.
- 2026-06-12: Закрыт M10.11: правила сервировки тарелки вынесены в `PlateServingService`; alternate interaction получил явный handled-result, а игрок может сервировать удерживаемую тарелку через безопасный fallback после действия выбранной стойки.
- 2026-06-12: Закрыт M10.12: проведён Zenject inventory; подтверждены корректные границы `ProjectContext`/scene scopes и отсутствие необходимости в execution order, удалены избыточные self/`NonLazy` bindings, а `GameInstaller` и `ProjectInstaller` сгруппированы по ответственности.
- 2026-06-13: Завершена cleanup-часть M10.13: удалены подтверждённые legacy recipe/debug/probe ресурсы и старый контент, актуальные assets разложены по рабочим папкам с сохранением GUID; ручной gameplay-прогон и аудит ссылок на удалённые GUID не выявили потерь активного контента.
- 2026-06-13: Закрыт M10.13 после полного ручного прогона: legacy/debug/static cleanup завершён, активные resources/prefabs сохранены, ссылки и основные gameplay/UI/navigation flows работают штатно. В M10 добавлен отдельный M10.14 для нормализации иерархии кода и namespace.
- 2026-06-13: Проведён inventory M10.14: обнаружены global namespace, legacy-зоны `Managers`/общий `UI`/`ScriptableObjects`, технические namespace `Order.Runtime`/`Level.UI` и отсутствие собственного asmdef. Принято структурировать код доменными пакетами, сохранять `.meta`, завершать namespace на доменной зоне и не вводить asmdef до стабилизации зависимостей.
- 2026-06-14: Зафиксировано итоговое название проекта `Order Rush Kitchen`. Для M10.14 выбран корневой namespace `OrderRushKitchen`; после структурирования кода выполнен финальный naming pass по оставшимся упоминаниям временного названия.
- 2026-06-14: Реализация M10.14 завершена: production-код сгруппирован по доменным папкам, namespaces приведены к корню `OrderRushKitchen` без технических суффиксов, Unity project identifiers переименованы в `Order Rush Kitchen`; сохранность script GUID и отсутствие compile errors подтверждены, перед закрытием этапа требуется ручной regression-прогон.
- 2026-06-14: Внутренняя структура крупных доменных пакетов уточнена техническими подпапками `Services`, `Storage`, `Controllers`, `Runtime` и `Visuals` без изменения доменных namespaces; editor tools подтверждены в отдельной editor assembly, дубли из корня проекта удалены.
- 2026-06-14: M10.14 и M10 закрыты после успешной Unity compilation и полного ручного regression-прогона. Архитектурные границы, lifecycle, pause/input/navigation/audio/progress/UI orchestration, interaction/reset flows, legacy cleanup, структура файлов и namespace приведены в консистентное состояние; следующий шаг — итоговый commit M10 и переход к M11.
- 2026-06-14: M10 повторно открыт для M10.15 по решению реализовать отложенный delivery staging до итогового commit. Принято оставить order-state mutation в `OrderService`, а хранение и lifecycle доставленных scene objects — в multi-slot `DeliveryCounter`.
- 2026-06-14: Закрыт M10.15.1: submission result возвращает конкретный принявший `ActiveOrder`, сохраняет текущий first-match fulfillment и защищён от неконсистентных success/failure состояний.
- 2026-06-14: Для полного M10.15 зафиксирована модель одной стойки, привязанной к одному заказу: первый staged item выбирает заказ, последующие принимаются только в него; completed/failed заказ переходит в fade cleanup. Alt-clear признан обоснованным только для активного заказа и требует receipt на конкретный `OrderItem` и атомарный batch rollback.
- 2026-06-14: Закрыт M10.15.2: fulfillment receipt расширен конкретным `OrderItem`, добавлен targeted submission для привязанного заказа и атомарный batch rollback только для активного заказа с одним `OnOrderUpdated`.
- 2026-06-14: В M10.15.3 добавлен scene-level `OrderStagingReservationService`: свободный delivery counter резервирует подходящий незарезервированный заказ до переноса блюда, а затем использует только targeted submission. Delivery-типы сгруппированы в `Counters/Delivery` без изменения namespace.
- 2026-06-14: Закрыт M10.15.3 после ручной проверки: свободные стойки резервируют разные подходящие заказы, один заказ нельзя выполнить через несколько delivery counter, а последующие блюда принимаются только привязанной стойкой.
- 2026-06-14: Staging presentation выделен в отдельный M10.15.4. Fade cleanup и alt-clear сдвинуты в M10.15.5.
- 2026-06-15: Закрыт M10.15.4 после ручной проверки: три staging slot размещены треугольником, успешно сданные блюда получают единый presentation-scale `0.625`, а вспомогательный `IngredientsIconsUI` скрывается. Bounds-based fit и editor validation отброшены как избыточные для текущего контента.
- 2026-06-16: Закрыт M10.15.5.1: completed/failed delivery order переводит привязанную стойку в `Resolving`, staged-блюда плавно уменьшаются через pause-aware `IGameClock`, а cleanup/reset выполняется после завершения анимации без преждевременного destroy из `OnOrderRemoved`.
- 2026-06-16: Закрыт M10.15.5.2 и M10.15.5: alt-clear активной staged delivery стойки выполняет batch rollback fulfilled `OrderItem`, затем переиспользует общий `Resolving` fade cleanup; staged objects удаляются после анимации, а reservation освобождается через общий reset.
- 2026-06-16: M10.15 закрыт после prefab/manual regression: bound multi-slot delivery staging, presentation-scale, hidden ingredient UI, fade cleanup, alt-clear rollback и повторное использование delivery counter работают в едином lifecycle.
- 2026-06-16: Начат финальный архитектурный подэтап M10.16: `DeliveryCounter` разделён на Unity scene adapter и plain `DeliveryCounterController`; Unity compilation/manual regression delivery staging ещё требуют подтверждения перед закрытием M10.16.
- 2026-06-16: M10.16 закрыт после Unity/manual regression: delivery staging workflow вынесен из `DeliveryCounter` в plain `DeliveryCounterController`, а counter оставлен scene adapter для interaction, slot transfer и presentation lifecycle.
- 2026-06-16: M10 закрыт после финального архитектурного аудита и ручной проверки M10.16. Основные Unity architecture boundaries, lifecycle, pause/input/navigation/audio/progress/UI orchestration, interaction/reset, delivery staging, legacy cleanup, namespaces и project naming приведены в консистентное состояние.
- 2026-06-16: M11 начат на ветке `feature/M11-localization`. Принят scope: локализуем активные UI/static/dynamic тексты и переключение языка; `MenuItemDefinitionSo.displayName` не трогаем, пока названия menu items не отображаются в пользовательском UI.
- 2026-06-16: Закрыт M11.1: проведена инвентаризация пользовательских строк в активных UI flow, зафиксированы `en`/`ru`, исходный язык `en`, формат ключей, рекомендуемые таблицы строк и список технических строк вне локализации.
- 2026-06-18: Закрыт M11.2: подключён Unity Localization, созданы локали `en`/`ru` и базовые string table collections; app-level `LocalizationService` поддерживает выбор, циклическое переключение, событие, сохранение и восстановление локали через `PlayerPrefs`.
- 2026-06-25: M11 закрыт после ручной regression/layout-проверки: активные UI flow переведены на `en`/`ru`, динамические строки вынесены в format tables, язык переключается из `OptionsUI` через localization service и сохраняется между запусками.
- 2026-06-25: Финальная полировка перенесена с M12 на M13. M12 выделен под Firebase/AppLovin SDK integration, чтобы SDK initialization, ads flow, pause/resume и mobile behavior стабилизировать до финального presentation/balance pass.
- 2026-06-25: M12 начат на ветке `feature/M12-sdk-integration` после merge/commit M11 в `develop`. Утверждён детальный `M12_SDK_INTEGRATION_PLAN.md`; текущий подэтап — M12.1 Final SDK Plan And Project Setup.
- 2026-06-26: Закрыт M12.1: финализирован Android package id `com.w1r2s.orderrushkitchen`, создан Firebase project/app, добавлен `google-services.json`, импортированы Firebase Analytics/Crashlytics SDK 13.13.0 и External Dependency Manager 1.2.187, Android Resolver Force Resolve прошёл с info-сообщениями. AppLovin credentials вынесены в external dependency до появления public website/store URL; следующий подэтап — M12.2 SDK Settings And Bootstrap Foundation.
- 2026-06-28: Scope M12 уточнён: активный этап остаётся Firebase-only (`Core`, `Analytics`, `Crashlytics`). AppLovin/rewarded ads, fake ads, `IAdsService` и `Continue (AD)` не готовим, пока нет Google Play/store URL, public website/privacy page и AppLovin account/app/ad unit; если монетизация не будет разблокирована, игра может выйти без рекламы.
- 2026-06-28: Закрыт Firebase foundation: добавлены `SdkSettings`, `ISdkInitializationService`, `SdkInitializationService`, ProjectContext binding и centralized Firebase dependency check. Ручная проверка подтвердила `[SDK] Firebase initialized.`, warning `Database URL not set` признан не блокирующим для Analytics/Crashlytics scope, disabled fallback через `sdkEnabled = false` работает и не ломает игру.
- 2026-06-30: Закрыты M12.3 analytics/crash boundaries: добавлены `IAnalyticsService`, `ICrashReportingService`, debug/editor adapters, Firebase adapters, SDK health events `app_start`/`firebase_init_succeeded`/`firebase_init_failed`, Crashlytics custom key `sdk_state`; Editor manual run подтвердил debug telemetry без отправки в production Firebase/Crashlytics.
- 2026-06-30: Закрыта M12.4a level run analytics: добавлены `level_started`, `level_completed`, `level_failed`, `level_retried`, `level_returned_to_menu` через `ILevelRunAnalyticsService`/`LevelRunAnalyticsController`; Editor manual run подтвердил debug events для level lifecycle и Game Over actions.
- 2026-06-30: Закрыта M12.4b order flow analytics: добавлены `order_completed`, `order_failed`, `order_accept_succeeded`, `order_accept_failed`, `order_submission_succeeded`, `order_submission_failed`; accept/submission events подключены через Zenject decorators `Decorate<IOrderFlowService>()` и `Decorate<IOrderSubmissionService>()`; Editor manual run подтвердил success/failure debug events, включая `order_submission_failed`.
- 2026-06-30: Закрыта M12.4c navigation failure analytics: добавлен decorator `SceneLoadAnalyticsDecorator` для `ISceneLoader`, событие `scene_load_failed` и Crashlytics non-fatal context; Editor manual run с временно повреждённым scene name подтвердил debug analytics/crash reporting, временное повреждение возвращено перед коммитом.
- 2026-06-30: Android production-like smoke подтвердил запуск на устройстве, прохождение нескольких уровней, Firebase user/events в консоли Firebase и отсутствие gameplay-blocking SDK errors. Найдены follow-up items: зафиксировать landscape-only orientation; отдельно разобраться с non-blocking localization warning при запуске в `en`.
- 2026-06-30: Android landscape-only orientation проверена на устройстве: приложение запускается и работает в горизонтальной ориентации. Non-development build остаётся крупным (~400-412 MB), size optimization переносится в отдельный follow-up после закрытия M12.
- 2026-06-30: M12 закрыт: Firebase SDK integration стабилизирована через infrastructure boundaries, Analytics/Crashlytics проверены в Editor и на Android, прямые Firebase API не протекают в gameplay/UI, AppLovin/rewarded ads оставлены в conditional backlog. Addressables Android build state добавлен в `.gitignore` как generated artifact.


