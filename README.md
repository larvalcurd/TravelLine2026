# TravelLine2026

Учебный репозиторий с заданиями, выполненными в рамках практики в TravelLine. Здесь собраны консольные приложения на C# и итоговая работа — Web API для системы бронирования средств размещения.

Проекты имеют учебный характер: они демонстрируют освоенные подходы и не позиционируются как готовые production-сервисы. Наиболее объёмная работа — [BookingApp](webapi-task/README.md): многослойный ASP.NET Core Web API с поиском доступных номеров и управлением бронированиями.

## Состав репозитория

| Проект | Тип | Что реализовано |
| --- | --- | --- |
| [webapi-task/BookingApp](webapi-task/README.md) | ASP.NET Core Web API | Управление объектами размещения и категориями номеров, поиск доступности, создание, фильтрация и отмена бронирований. |
| [CarFactory](CarFactory/README.md) | Консольное приложение | Конфигуратор автомобиля с абстрактными фабриками брендов, проверками совместимости и расчётом характеристик. |
| [Fighters](Fighters/README.md) | Консольная игра | Создание бойцов из взаимозаменяемых компонентов и пошаговая симуляция боя. |
| [Casino](Casino/README.md) | Консольное приложение | Игра со ставками, балансом и случайным результатом раунда. |
| [OrderManager](OrderManager/README.md) | Консольное приложение | Интерактивное оформление заказа с проверкой пользовательского ввода и подтверждением. |

## Технологии

- C# и .NET 10;
- ASP.NET Core Web API;
- Entity Framework Core и SQLite — в `webapi-task`;
- PostgreSQL-провайдер Npgsql подключён в зависимостях Web API, при этом текущая конфигурация приложения использует SQLite;
- FluentValidation, Swagger / Swashbuckle — в `webapi-task`.

## Быстрый старт

Для работы нужен .NET SDK 10 — все проекты нацелены на `net10.0`.

Каждое консольное приложение запускается напрямую по файлу проекта:

```powershell
dotnet run --project .\Casino\CasinoSolution\CasinoSolution\CasinoSolution.csproj
dotnet run --project .\OrderManager\OrderManager\OrderManager.csproj
dotnet run --project .\CarFactory\CarFactory\CarFactory.csproj
dotnet run --project .\Fighters\Fighters\Fighters.csproj
```

Инструкции по запуску и API основного проекта находятся в [документации BookingApp](webapi-task/README.md).

## Структура

```text
TravelLine2026/
├── webapi-task/       # итоговое задание: система бронирования
├── CarFactory/        # конфигуратор автомобиля
├── Fighters/          # консольная игра-симулятор боя
├── Casino/            # игра со ставками
└── OrderManager/      # оформление заказа в консоли
```

## Что показывает репозиторий

- базовую работу с C#, ООП, интерфейсами, перечислениями и коллекциями;
- применение паттерна «абстрактная фабрика» и политик совместимости в `CarFactory`;
- разделение моделей предметной области и сценариев приложения в `Fighters`;
- проектирование REST API с контроллерами, DTO и кодами HTTP-ответов;
- разделение Web API на слои Domain, Infrastructure и WebApi, DI и репозитории;
- валидацию входных данных и централизованную обработку доменных ошибок.
