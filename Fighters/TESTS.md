# Fighters — Модульные тесты

## Общая информация

Проект `Fighters.Tests` содержит модульные тесты для классов с бизнес-логикой игры «Fighters».
Тестирование выполнено с помощью **xUnit** и **Moq**.  
Структура тестового проекта повторяет структуру основного проекта `Fighters`.

- Все тестовые классы именуются по шаблону `<ИмяТестируемогоКласса>Tests`.
- Тестовые методы — в стиле Microsoft: `<Метод>_<Условие>_<ОжидаемоеПоведение>`.
- Все внешние зависимости классов (интерфейсы) замоканы с использованием `Mock<T>`.
- Дублирование тестовых данных исключено: используются приватные фабрики (`CreateFighter`, `CreateRandomizerMock`, `CreateOutputMock` и др.).

## Протестированные классы

### 1. `Fighter` (`FighterTests`)

**Расположение:** `Fighters.Tests/Models/Fighters/FighterTests.cs`  
**Тестируемый класс:** `Fighters.Models.Fighters.Fighter`

Покрыты все публичные методы и свойства:

#### Конструктор
- `Constructor_WhenValidArguments_SetsName` – устанавливает имя.
- `Constructor_WhenHealthProvided_CalculatesMaxHealth` – максимальное здоровье = здоровье расы + класса.
- `Constructor_WhenMaxHealthPositive_SetsCurrentHealthToMax` – текущее здоровье равно максимальному.
- `Constructor_WhenMaxHealthPositive_SetsIsAliveTrue` – боец жив при положительном здоровье.
- `Constructor_WhenMaxHealthZero_SetsIsAliveFalse` – при 0 HP `IsAlive` = `false`.
- `Constructor_CalculatesInitiativeBonus` – бонус инициативы = сумма инициативы расы и класса.

#### `Attack()`
- Базовый урон (сумма урона расы, класса, оружия).
- Применение множителя урона (0.9).
- Критический удар (удвоение урона).
- Отсутствие крита не изменяет урон.
- Комбинация крита и множителя.
- Вызов `target.TakeDamage()` с ожидаемым значением.
- Отчёт содержит имена атакующего и защитника.
- Отчёт содержит фактический урон, возвращённый `TakeDamage`.
- `DefenderDied` = `true` при смерти цели, `false` при выживании.
- Интеграция с реальным `Fighter`: проверка снижения здоровья с учётом брони.

#### `TakeDamage()`
- Урон больше брони: здоровье уменьшается.
- Урон равен броне: здоровье не меняется.
- Урон меньше брони: здоровье не меняется.
- Урон превышает здоровье: `CurrentHealth` становится 0.
- Возвращаемое значение — фактически полученный урон.
- Полное блокирование урона: возвращает 0.
- Летальный урон: `IsAlive` становится `false`.

#### `RestoreHealth()`
- После повреждений: восстанавливает здоровье до `MaxHealth`.
- При полном здоровье: ничего не меняет.

**Дублирование устранено:**
- `CreateFighter()` – фабрика бойца с настраиваемыми параметрами.
- `CreateRandomizerMock()` – централизованная настройка мока `IBattleRandomizer` для атак.

---

### 2. `BattleRandomizer` (`BattleRandomizerTests`)

**Расположение:** `Fighters.Tests/Models/BattleRandomizer/BattleRandomizerTests.cs`  
**Тестируемый класс:** `Fighters.Models.BattleRandomizer.BattleRandomizer`

Протестирован метод `PickRandom()`:
- `PickRandom_WhenItemsEmpty_ThrowsArgumentException` – при пустом списке выбрасывается исключение с корректным `ParamName`.

---

### 3. `Battle` (`BattleTests`)

**Расположение:** `Fighters.Tests/Fighters/BattleTests.cs`  
**Тестируемый класс:** `Fighters.Battle`

Для упрощения тестирования класс `Battle` был рефакторирован: добавлена зависимость от интерфейса `IGameOutput` (вместо прямых вызовов `Console.WriteLine`). Это позволило проверять выводимые сообщения через моки.

Покрыты все ключевые сценарии работы боевого цикла:

#### Валидация перед началом боя
- `Start_FightersListIsEmpty_PrintsErrorAndDoesNotStartBattle` – 0 бойцов.
- `Start_FightersListHasOneFighter_PrintsErrorAndDoesNotStartBattle` – 1 боец.

#### Инициализация и восстановление
- `Start_WhenBattleStarts_RestoresAllFightersHealth` – вызов `RestoreHealth()` у каждого бойца.

#### Механика раундов
- `Start_WhenRoundStarts_RollsInitiativeForEachAliveFighter` – бросок инициативы для всех живых.
- `Start_FightersHaveDifferentInitiative_AttacksInInitiativeOrder` – ходы выполняются в порядке убывания итоговой инициативы.
- `Start_WhenFighterActs_PicksTargetFromOtherAliveFighters` – случайный выбор цели среди других живых бойцов.
- `Start_WhenTargetDies_RemovesTargetAndPrintsElimination` – убитый боец удаляется из списка живых и выводится сообщение.
- `Start_WhenFighterWasEliminatedBeforeTurn_DoesNotAllowItToAttack` – боец, убитый раньше своего хода, пропускается.

#### Завершение боя
- `Start_WhenOnlyOneFighterRemains_PrintsWinner` – выводится победитель.
- `Start_WithThreeFighters_ContinuesUntilSingleWinnerRemains` – полный цикл боя с тремя участниками, включая несколько раундов и проверку всех вызовов атак.

**Дублирование устранено:**
- `CreateRandomizerMock()` – базовый мок рандомайзера с детерминированным поведением.
- `CreateFighterMock()` – создание мока `IFighter` с типичными настройками.
- `CreateQuickBattleFighters()` – пара бойцов, где один убивает другого с одного удара (для тестов, где нужна быстрая развязка).
- `CreateOutputMock()` – мок для `IGameOutput`.

Все фабрики позволяют переопределять поведение в конкретных тестах через `SetupSequence` или callback-и.

---

## Инструменты

- **xUnit** — тестовый фреймворк.
- **Moq** — создание и настройка моков для всех зависимостей (интерфейсов).
- Структура тестового проекта повторяет структуру `Fighters`.
- Запуск тестов: `dotnet test`

## Соответствие требованиям ТЗ

- Названия классов и методов — по шаблону.
- Все внедряемые зависимости замоканы.
- Для генерации тестовых данных используются приватные фабрики, исключающие дублирование.
- Проведён рефакторинг боевого кода для улучшения тестируемости (`Battle` + `IGameOutput`).
- Тесты покрывают все ветвления и краевые случаи публичного API.