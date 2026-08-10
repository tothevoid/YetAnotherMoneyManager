# Development Guidelines for YetAnotherMoneyManager

This document contains guidelines, coding standards, and architectural patterns for developing features in YetAnotherMoneyManager.

---

## 🛠️ Server (Backend) Guidelines

### Stack & Architecture
- **Framework**: .NET 10 Web API
- **Layering**:
  - `server/WebApi`: ASP.NET Core Controllers and DTO models. Controllers must remain thin and delegate logic to application services.
  - `server/Application`: Core business logic, services (`MoneyManager.Application.Services`), interfaces (`MoneyManager.Application.Interfaces`), and DTOs.
  - `server/Infrastructure`: EF Core DbContext, entity configurations, and migrations.
  - `server/Shared`: Common constants and helper models.
  - `server/MoneyManager.Application.Tests`: Unit tests for application services.
- **Orphan Directory Prohibition**: Do NOT create or restore legacy directories (`server/BLL`, `server/DAL`, `server/Common`, `server/MoneyManager`, `server/server`). Only project folders in `MoneyManager.sln` are valid.

### EF Core & Service Layer Performance Guidelines
- **Read Query Optimization**: All read-only queries in services MUST use `.AsNoTracking()` to avoid unnecessary change-tracking memory allocations.
- **Batch Async Execution**: Avoid sequential `await` inside `foreach` loops for multi-account computations. Use `Task.WhenAll` or aggregated SQL queries.
- **Direct Bulk Deletes**: Prefer EF Core `ExecuteDeleteAsync()` for primary-key deletions rather than fetching detached entity instances prior to deletion.

### Application Service Testing
- **100% Service Coverage**: Every application service in `MoneyManager.Application` MUST have comprehensive unit test coverage in `MoneyManager.Application.Tests`.
- **Directory Hierarchy**: Test file structure under `MoneyManager.Application.Tests` must mirror `MoneyManager.Application`.
- **Test Style**: xUnit, NSubstitute / Moq, AutoFixture using the AAA (Arrange-Act-Assert) pattern.

### API Controllers & Dual Endpoint Pattern
- **Date Parameters**: Use `DateOnly` (`YYYY-MM-DD`) for API query parameters.
- **Unused Parameters Prohibition**: Controller action method signatures MUST NOT accept parameters that are not passed to underlying services.
- **Dual Endpoints**: When `brokerAccountId` is optional/nullable:
  - Provide `GetAll(...)` for summary/aggregate view across all accounts.
  - Provide `GetByBrokerAccount(..., Guid brokerAccountId)` for a single account view.
- **Error Responses**: Exception handling must return standard RFC 7807 JSON `ProblemDetails` payloads.

### Server Commands
- **Build**: `cd server && dotnet build`
- **Run Tests**: `cd server && dotnet test`

---

## 🎨 Client (Frontend) Guidelines

### Stack & Architecture
- **Framework**: React 18 + TypeScript + Vite
- **UI & Styling**: Chakra UI v3, `react-icons`
- **i18n**: `react-i18next`
- **Directory Scope**: ALL client source files (components, features, pages, hooks) MUST reside under `client/src/`. Do NOT create feature folders at `client/features`.
- **Structure**:
  - `client/src/features`: Shared domain feature modules (e.g. Navigation, UserProfileSettingsModal).
  - `client/src/models/<domain>`: Domain interfaces, requests, responses.
  - `client/src/api/<domain>`: API client functions and response/request mappers.
  - `client/src/pages`: Feature pages (`BrokerAccount`, `BrokerAccounts`, `SecurityPage`, `Transactions`, etc.).
  - `client/src/shared`: Reusable components (`MoneyCard`, `DateSelect`), hooks, utilities.
  - `client/src/locales`: Localization files (`en.json`, `ru.json`).

### File Placement & Naming Rules (Models, APIs & Mappers)

1. **Entities & Models**:
   - **Path**: `client/src/models/<domain>/` (grouped by domain folder: `brokers/`, `accounts/`, `securities/`, `transactions/`, `deposits/`, `debts/`, `crypto/`).
   - **Naming**: `<EntityName>.ts` or `<EntityName>Entity.ts` (e.g. `BrokerAccountPortfolioHistoryEntity.ts`, `BrokerAccountEntity.ts`).
   - Defines TypeScript interfaces for entities, request payloads (`*EntityRequest`), and raw API responses (`*EntityResponse`).

2. **API Clients**:
   - **Path**: `client/src/api/<domain>/` (in domain subfolder: `api/brokers/`, `api/accounts/`, etc.).
   - **Naming**: `<camelCaseEntity>Api.ts` (e.g. `brokerAccountPortfolioHistoryApi.ts`, `brokerAccountSummaryApi.ts`).
   - Uses basic HTTP helpers from `client/src/api/basicApi.ts` (`getEntity`, `getAllEntities`, `postEntity`, etc.).

3. **Mappers (API Mapping)**:
   - **Path**: `client/src/api/<domain>/` (placed next to the API file).
   - **Naming**: `<camelCaseEntity>ApiMapping.ts` or `<EntityName>Mapping.ts` (e.g. `brokerAccountApiMapping.ts`, `BrokerAccountFundsTransferMapping.ts`).
   - **Functions**: Use shared helpers (`parseEntityDates`, `formatRequestDates`) to avoid duplicated mapping boilerplate.

### Localization Rules (i18n)
- **Dual Translations Mandatory**: Every user-facing string MUST be added to **both** `client/src/locales/en.json` and `client/src/locales/ru.json`.
- **Key Format**: Use `snake_case` keys categorized by domain/feature (e.g., `broker_account_page_*`, `entity_*`).
- **Hook**: Access strings via `const { t } = useTranslation()`.
- **Preserve Blank Lines**: Keep blank lines in `ru.json` and `en.json` between logical key blocks for semantic grouping. Do NOT delete or format away empty lines.

### UI/UX & Component Guidelines
- **Loading & Empty States**: Pages MUST use Skeleton loaders (`CardSkeleton`, `TableSkeleton`) during data fetches and `<EmptyStatePlaceholder>` when entity collections are empty.
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

## 🔄 Verification Workflow
Before finalizing any task, always execute:
1. `npm run build` in `client/`
2. `dotnet build` in `server/`
3. `dotnet test` in `server/`
