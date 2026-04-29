# Banking System API

Backend-проект на C#/.NET: API для банковской системы с аккаунтами, сессиями и операциями по счёту.

Проект показывает луковую архитектуру: доменная модель, application-сценарии, инфраструктура хранения/авторизации и HTTP-интерфейс разделены по отдельным проектам.

## Возможности

- создание админской сессии по системному паролю;
- создание банковского счёта;
- создание пользовательской сессии по номеру счёта и PIN-коду;
- просмотр баланса;
- пополнение счёта;
- снятие денег;
- просмотр истории операций;
- обработка ошибок авторизации и некорректных операций.

Все операции со счётом проходят через application layer. Хранение реализовано in-memory, но бизнес-логика зависит от абстракций репозиториев, а не от конкретного способа хранения.

## Стек

- C# / .NET 9
- ASP.NET Core Web API
- Microsoft.Extensions.DependencyInjection
- xUnit
- NSubstitute
- Swagger / OpenAPI

## Структура

```text
src/
├── BankingSystem.Api/
├── BankingSystem.Application/
├── BankingSystem.Application.Abstractions/
├── BankingSystem.Application.Contracts/
├── BankingSystem.Domain/
├── BankingSystem.Infrastructure.Authentication/
├── BankingSystem.Infrastructure.Persistence/
└── BankingSystem.Presentation.Http/

tests/
└── BankingSystem.Tests/
```

## Архитектура

Проект разделён на слои:

- `Domain` — сущности и доменные правила.
- `Application` — сценарии работы системы: создание сессий, создание счёта, баланс, пополнение, снятие и история операций.
- `Application.Contracts` — request/response модели и контракты, через которые внешний слой обращается к application layer.
- `Application.Abstractions` — интерфейсы внешних зависимостей, которые нужны application layer.
- `Infrastructure.Authentication` — реализация зависимостей, связанных с системным паролем и генерацией номеров счетов.
- `Infrastructure.Persistence` — in-memory реализации репозиториев.
- `Presentation.Http` — HTTP-контроллеры на ASP.NET Core.
- `Api` — точка входа, DI, Swagger и конфигурация.

Ключевая идея: application layer описывает, какие зависимости ему нужны, а infrastructure layer предоставляет конкретные реализации. Благодаря этому сценарии приложения не зависят напрямую от HTTP-слоя и in-memory хранения.

## Основной сценарий через API

После запуска можно открыть Swagger и выполнить последовательность запросов.

### 1. Создать админскую сессию

```http
POST /api/admin/session?password=demo-admin-password
```

### 2. Создать счёт

```http
POST /api/admin/session/account?adminSessionId=<admin-session-id>&pinCode=1234
```

### 3. Создать пользовательскую сессию

```http
POST /api/user/session?accountNumber=<account-number>&pinCode=1234
```

### 4. Выполнить операции со счётом

```http
POST /api/user/session/balance?userSessionId=<user-session-id>
POST /api/user/session/replenish?userSessionId=<user-session-id>&amount=1000
POST /api/user/session/withdraw?userSessionId=<user-session-id>&amount=250
POST /api/user/session/history?userSessionId=<user-session-id>
```

## Запуск

```bash
git clone <repository-url>
cd <repository-folder>
dotnet restore
dotnet build
dotnet test
```

Запуск API:

```bash
dotnet run --project src/BankingSystem.Api/BankingSystem.Api.csproj
```

Swagger доступен по адресу:

```text
http://localhost:5129/swagger
```

## Конфигурация

Системный пароль администратора задаётся в `appsettings.json`:

```json
{
  "System": {
    "AdminPassword": "demo-admin-password"
  }
}
```

Это демонстрационное значение. В реальном приложении, очевидно, пароль не должен храниться в репозитории.

## Тесты

Тесты проверяют бизнес-логику на уровне application layer, без HTTP и без реального хранилища.

Покрыты ключевые сценарии:

- успешное снятие денег;
- ошибка при недостаточном балансе;
- успешное пополнение счёта.

Для изоляции зависимостей используются моки репозиториев через NSubstitute.
