# DomainPlayground

A .NET playground project for practicing **Domain-Driven Design (DDD)**, **Clean Architecture**, and **CQRS**, built around a real-world feature: a **Role-Based Access Control (RBAC) authorization system**.

> This is a learning/reference project. The focus is on architecture and design decisions, not on building a production-ready product (yet).

## What's inside

- **Clean Architecture** with strict dependency rules: `Domain` has zero outward dependencies; `Application` depends only on `Domain`; `Infrastructure` and `EndPoint.API` depend inward.
- **CQRS**: separate read (`Persistence.Queries`) and write (`Persistence.Commands`) models, each with its own `DbContext`.
- **DDD tactical patterns**: Aggregates, Value Objects, Domain Events, strongly-typed IDs, Factory methods with invariant validation.
- **MediatR pipeline behaviors** for cross-cutting concerns: validation, authorization, and transactional save (`ValidationBehavior`, `AuthorizationBehavior`, `UnitOfWorkBehavior`).
- **Attribute-driven permission system**: permissions are declared directly on commands/queries (`[HasPermission(...)]`) and synced into the database automatically on startup — no manual permission management.
- **JWT authentication** with refresh-token rotation, kept fully decoupled from the domain (ASP.NET Identity only handles credentials; everything else lives in the domain).

## Architecture

```
src/
├── Core/
│   ├── DomainPlayground.Core.Domain          # Entities, Aggregates, Value Objects, Domain Events
│   └── DomainPlayground.Core.Application     # Commands, Queries, Handlers, Behaviors, Interfaces
├── Infrastructure/
│   ├── DomainPlayground.Infrastructure.Identity              # ASP.NET Identity, JWT issuing
│   ├── DomainPlayground.Infrastructure.Persistence.Commands  # Write-side EF Core DbContext
│   └── DomainPlayground.Infrastructure.Persistence.Queries   # Read-side EF Core DbContext (read models)
├── Shared/
│   └── DomainPlayground.SharedKernel          # Base Entity/AggregateRoot/ValueObject/Result/Error
└── EndPoint/
    └── DomainPlayground.EndPoint.API          # ASP.NET Core Web API (controllers, DI composition)

tests/
├── DomainPlayground.Core.Domain.UnitTests
└── DomainPlayground.Core.Application.UnitTests
```

### Bounded contexts (feature folders)

Inside `Application` and `Domain`, code is organized by **bounded context**, not by technical layer:

```
Authorization/
├── Permissions/     # Aggregate: catalog of permission keys
├── Roles/           # Aggregate: a named set of permissions
└── UserAccesses/    # Aggregate: a user's assigned roles + per-user overrides
```

Each aggregate owns its own transactional boundary. Aggregates reference each other only by ID — never by direct object reference.

## How authorization works

This project implements **Role-Based Access Control (RBAC)**: access is never granted to a user directly — it's granted to a **role**, and users are assigned to one or more roles.

```
User → Role → Permission
```

On top of that base model, each user can carry **personal overrides** — a small, explicit exception layer for cases where a role alone isn't precise enough (e.g. temporarily revoking one permission from a specific user without creating a whole new role for it):

```
Effective permissions = (permissions from the user's active roles)
                       + (per-user Allow overrides)
                       − (per-user Deny overrides)
```

`Deny` always takes precedence over `Allow`.

The system is wired together as follows:

1. Every command/query that requires a permission declares it right on the class:
   ```csharp
   [HasPermission(nameof(ModuleCodes.Authorization), "roles.create", "Create role")]
   public sealed record CreateRoleCommand(string Name) : ICommand;
   ```
2. On startup, `PermissionSynchronizer` reflects over the assembly, collects every `[HasPermission]` attribute, and syncs the `Permissions` table — adding new ones, renaming existing ones, and removing ones that no longer exist in code. Permissions are never hand-maintained in the database.
3. `AuthorizationBehavior`, a MediatR pipeline behavior, resolves the current user's effective permissions and checks them *before* the handler runs. Handlers never perform authorization checks themselves.
4. A role flagged `IsSystem = true` (e.g. `Admin`) is immutable at the domain level — it cannot be renamed, disabled, or have its permissions changed through the API, regardless of who's calling it.

## Tech stack

| Concern | Technology |
|---|---|
| Runtime | .NET 10 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core (SQL Server) |
| Mediator / CQRS | MediatR |
| Validation | FluentValidation |
| Auth | ASP.NET Identity + JWT Bearer |
| Testing | xUnit, FluentAssertions, Moq |

## Getting started

### Prerequisites

- .NET 10 SDK
- SQL Server (local or containerized)

### Configuration

Set the following via `dotnet user-secrets` (do **not** commit secrets):

```bash
cd src/EndPoint/DomainPlayground.EndPoint.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:CommandDatabase" "<your-connection-string>"
dotnet user-secrets set "ConnectionStrings:QueryDatabase" "<your-connection-string>"
dotnet user-secrets set "JwtSettings:Secret" "<a-strong-random-secret>"
dotnet user-secrets set "SeedAdmin:Password" "<password-for-the-seeded-admin-user>"
```

### Run

```bash
dotnet restore
dotnet build
dotnet run --project src/EndPoint/DomainPlayground.EndPoint.API
```

On startup, the application automatically:
- applies pending EF Core migrations,
- synchronizes permissions from code into the database,
- seeds a default `Admin` role (with every permission) and a default admin user.

### Run tests

```bash
dotnet test
```

## Design notes worth knowing

- **Identity is infrastructure, not domain.** `ApplicationUser` (ASP.NET Identity) only handles credentials. Business-relevant user data lives in its own `UserAggregate` in the domain, linked only by a shared `UserId`.
- **Read side never touches aggregates.** Queries project directly from lightweight read models onto the same physical tables the write side manages — no loading full aggregates just to read data.
- **`Result<T>` over exceptions** for expected failure cases (validation, not-found, conflicts). Exceptions are reserved for truly exceptional situations.

## License

No license has been set yet. Add one if you intend to make this project reusable by others.
