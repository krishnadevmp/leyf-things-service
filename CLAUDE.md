# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from the `LeyfThings/` directory unless noted.

```bash
# Run the API (http on :5128, https on :7223, Swagger at /swagger)
dotnet run --project LeyfThings/LeyfThings.csproj

# Build
dotnet build LeyfThings/LeyfThings.csproj

# EF Core migrations (run from LeyfThings/)
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

No test project exists yet.

## Architecture

**Single-project ASP.NET Core 8 Web API** (`LeyfThings/`) with three layers:

```
Controllers → Services (interfaces) → AppDbContext (EF Core → SQL Server)
```

- Controllers are thin — they call one service method and return the result. No business logic, no null checks (services throw instead).
- Services own all business logic and data access directly via `AppDbContext` (no repository pattern).
- `OpenAIService` calls Azure OpenAI to parse natural language into a `GoalDTO`, then `GoalService.CreateGoalAsync` persists it.

**Exception handling flow:**
`ExceptionHandlingMiddleware` (registered first in the pipeline) catches all exceptions and maps them to structured `ErrorResponse` JSON. Three domain exceptions exist: `NotFoundException` → 404, `ValidationException` → 400, `ExternalServiceException` → 503. ModelState validation failures are also reformatted to `ErrorResponse` via `InvalidModelStateResponseFactory`.

**Domain model:**
`Goal` owns a collection of `MileStone`s (one-to-many). On `UpdateGoalAsync`, the existing milestones are fully replaced. `MileStone.Goal` navigation property is `[JsonIgnore]` to avoid circular serialization.

**Status and priority fields** are free-form strings, not enums.

## Configuration

`appsettings.json` and `appsettings.*.json` are gitignored — copy them locally. Required keys:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<SQL Server connection string>"
  },
  "AzureOpenAI": {
    "Endpoint": "<Azure OpenAI endpoint>",
    "ApiKey": "<API key>",
    "DeploymentName": "<deployment name>"
  }
}
```

CORS is hardcoded to allow `http://localhost:5173` (Vite frontend dev server).
