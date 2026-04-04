# HelpEachOther

HelpEachOther is a beginner-friendly **ASP.NET Core MVC** web app where people can ask for help and other community members can volunteer to support them.

## Main Product Idea

The app supports a simple help lifecycle:
1. A signed-in user creates a help request.
2. Another signed-in user volunteers.
3. The request owner marks it completed.
4. The helper earns Soul Points.

This keeps the MVP focused on practical neighbor-to-neighbor coordination.

## MVP Features (Current)

- User registration/login/logout (ASP.NET Core Identity)
- Create help requests
- Browse and filter requests by status
- View request details
- Volunteer for open requests
- Request owner can edit/delete only while request is open
- Request owner can mark in-progress request as completed
- Helper receives **10 Soul Points** when request is completed
- Profile pages:
  - Personal overview
  - My created requests
  - My helping activity

## Tech Stack

- .NET 10
- ASP.NET Core MVC
- Razor Views
- Entity Framework Core
- SQLite
- ASP.NET Core Identity
- Bootstrap

## How to Run Locally

From the repository root:

```bash
cd HelpEachOther
dotnet restore
dotnet ef database update
dotnet run
```

Then open the local URL shown in the terminal.

## Database Setup

The app uses SQLite (default file: `HelpEachOther/helpeachother.db`).

Startup behavior in `Program.cs`:
- Applies pending EF Core migrations automatically (`Database.Migrate()`)
- Seeds demo data when no requests exist

So you can either:
- Run `dotnet ef database update` manually before startup, or
- Let app startup apply migrations for you

## Migrations

Create a migration:

```bash
cd HelpEachOther
dotnet ef migrations add <MigrationName> --output-dir Data/Migrations
```

Apply migrations:

```bash
cd HelpEachOther
dotnet ef database update
```

## Demo/Test User Credentials

If the database has no help requests, startup seed creates:
- **Email/Username:** `demo@help.com`
- **Password:** `Password1!`

## Current Project Status

This project is in **MVP+ iteration**:
- Core request lifecycle is functional (open → in-progress → completed)
- Profile and owner management are present
- Request management and communication flows are the main expansion areas

## Short Roadmap

- Add safe request-owner/helper communication/contact flow
- Expand owner request management UX
- Add moderation/safety basics (reporting/flags)
- Improve validation and guardrails around request transitions
- Improve UI clarity and mobile polish

## Folder Structure Overview

```text
HelpEachOther.sln
HelpEachOther/
  Controllers/
  Services/
  Data/
    Migrations/
  Models/
  ViewModels/
  Views/
  Areas/Identity/
  wwwroot/
  Program.cs
  appsettings.json
docs/
  ARCHITECTURE_OVERVIEW.md
  BUSINESS_RULES.md
  PROJECT_PLAN.md
AGENTS.md
README.md
```

## Notes for Future Contributors

- Keep changes small and incremental.
- Preserve beginner-friendly MVC structure.
- Prefer ViewModels for form input models.
- Keep important business logic centralized in services when reused.
- Keep authorization checks on the server side.
- Avoid unrelated refactors during feature work.
- Update `docs/BUSINESS_RULES.md` and `docs/PROJECT_PLAN.md` when behavior changes.
