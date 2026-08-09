# Development Guidelines & Conventions for YetAnotherMoneyManager

This document contains guidelines, coding standards, and architectural patterns for developing features in YetAnotherMoneyManager.

---

## 🛠️ Server (Backend) Rules & Architecture

### Stack & Solution Structure
- **Framework**: .NET 9 Web API
- **Layering**:
  - `server/WebApi`: ASP.NET Core Controllers and DTO models. Controllers must remain thin and delegate logic to application services.
  - `server/Application`: Core business logic, services (`MoneyManager.Application.Services`), interfaces (`MoneyManager.Application.Interfaces`), and DTOs.
  - `server/Infrastructure`: EF Core DbContext, entity configurations, and migrations.
  - `server/Shared`: Common constants and helper models.
  - `server/MoneyManager.Application.Tests`: Unit tests for application services.

### Application Service Testing
- **100% Service Coverage**: Every application service in `MoneyManager.Application` MUST have comprehensive unit test coverage in `MoneyManager.Application.Tests`.
- **Directory Hierarchy**: Test file structure under `MoneyManager.Application.Tests` must mirror `MoneyManager.Application`.
- **Test Style**: xUnit, NSubstitute / Moq, AutoFixture using the AAA (Arrange-Act-Assert) pattern.

### API Controllers & Dual Endpoint Pattern
- **Date Parameters**: Use `DateOnly` (`YYYY-MM-DD`) for API query parameters.
- **Dual Endpoints**: When `brokerAccountId` is optional/nullable:
  - Provide `GetAll(...)` for summary/aggregate view across all accounts.
  - Provide `GetByBrokerAccount(..., Guid brokerAccountId)` for a single account view.

### Server Commands
- **Build**: `cd server && dotnet build`
- **Run Tests**: `cd server && dotnet test`

---

## 🎨 Client (Frontend) Rules & Architecture

### Stack & Structure
- **Framework**: React 18 + TypeScript + Vite (`client/src`).
- **UI & Styling**: Chakra UI v3, `react-icons`.
- **i18n**: `react-i18next`.
- **Structure**:
  - `client/src/models/<domain>`: Domain interfaces, requests (`*EntityRequest`), responses (`*EntityResponse`).
  - `client/src/api/<domain>`: API client functions and response/request mappers.
  - `client/src/pages`: Feature pages (`BrokerAccount`, `BrokerAccounts`, `SecurityPage`, `Transactions`, etc.).
  - `client/src/shared`: Reusable components (`MoneyCard`, `DateSelect`), hooks, utilities.
  - `client/src/locales`: Localization files (`en.json`, `ru.json`).

### File Placement & Naming Rules (Models, APIs & Mappers)
1. **Entities & DTO Models**:
   - **Path**: `client/src/models/<domain>/` (e.g. `brokers/`, `accounts/`, `securities/`, `transactions/`, `deposits/`, `debts/`, `crypto/`).
   - **Naming**: `<EntityName>.ts` or `<EntityName>Entity.ts` (e.g. `BrokerAccountPortfolioHistoryEntity.ts`, `BrokerAccountEntity.ts`).
   - Include domain entities, request payloads (`*EntityRequest`), and raw API responses (`*EntityResponse`).

2. **API Clients**:
   - **Path**: `client/src/api/<domain>/` (e.g. `api/brokers/`, `api/accounts/`).
   - **Naming**: `<camelCaseEntity>Api.ts` (e.g. `brokerAccountPortfolioHistoryApi.ts`, `brokerAccountSummaryApi.ts`).
   - Use standard HTTP wrappers from `client/src/api/basicApi.ts` (`getEntity`, `getAllEntities`, `postEntity`, etc.).

3. **Mappers (API Mapping)**:
   - **Path**: `client/src/api/<domain>/` (placed next to the corresponding API file).
   - **Naming**: `<camelCaseEntity>ApiMapping.ts` or `<EntityName>Mapping.ts` (e.g. `brokerAccountApiMapping.ts`, `BrokerAccountFundsTransferMapping.ts`).
   - **Functions**: `prepare<Entity>(response)` converts raw API data to domain objects (parsing date strings into `Date`), and `prepare<Entity>Request(entity)` maps UI forms to backend DTO payloads.

### Localization Rules (i18n)
- **Dual Translations Mandatory**: Every user-facing string MUST be added to **both** `client/src/locales/en.json` and `client/src/locales/ru.json`.
- **Key Format**: Use `snake_case` keys categorized by domain/feature (e.g. `broker_account_page_*`, `entity_*`).
- **Hook**: Access strings via `const { t } = useTranslation()`.

### Date Selection & UI Components
- **Date Picker**: Use `react-datepicker` (`DatePicker`) with Chakra UI `<Input width="200px" color="text_primary" backgroundColor="background_primary" borderColor="border_primary" />`.
- **Default Date**: Default to today (`new Date()`). Display as `dd.MM.yyyy` to the user, and format as ISO date `YYYY-MM-DD` for API queries.
- **Layout & Styling**: Use Chakra UI v3 theme tokens (`background_primary`, `text_primary`, `border_primary`, `action_primary`) and `<SimpleGrid columns={2} gap={4}>` with `<MoneyCard>` for metric grids.

### API Client Functions
- **Dual Endpoint Handling**: API functions accept `brokerAccountId: Nullable<string>` and switch URLs conditionally:
  ```ts
  const url = brokerAccountId
      ? `${basicUrl}/GetByBrokerAccount?date=${date}&brokerAccountId=${brokerAccountId}`
      : `${basicUrl}/GetAll?date=${date}`;
  ```

### Client Commands
- **Build**: `cd client && npm run build`
- **Dev Server**: `cd client && npm run dev`

---

## 🔄 Verification Commands & Workflow
Before finalizing any task, always execute:
1. `npm run build` in `client/`
2. `dotnet build` in `server/`
3. `dotnet test` in `server/`
