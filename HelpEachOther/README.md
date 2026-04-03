# HelpEachOther (MVP)

HelpEachOther is a beginner-friendly ASP.NET Core MVC app where people can create help requests and volunteers can complete them to earn **Soul Points**.

## Stack
- .NET 10
- ASP.NET Core MVC + Razor Views
- Entity Framework Core
- SQLite
- ASP.NET Core Identity
- Bootstrap 5

## MVP Features
- Register / login / logout (Identity)
- Create help request (authenticated users)
- Browse requests (public)
- Request details + volunteer flow
- Owner can mark request completed
- Helper earns **10 Soul Points** when request is completed
- Profile page with stats

## Project Structure

- `Controllers/`
  - `HomeController` - landing page
  - `HelpRequestsController` - request CRUD-like MVP flows (index/details/create/volunteer/complete)
  - `ProfileController` - user profile + personal activity pages
- `Models/`
  - `ApplicationUser` (extends IdentityUser)
  - `HelpRequest`
  - `HelpRequestStatus`
  - `HelpCategories`
- `ViewModels/`
  - `CreateHelpRequestViewModel`
  - `ProfileViewModel`
- `Data/`
  - `ApplicationDbContext`
  - `SeedData` (demo data on first run)
- `Services/`
  - `HelpRequestService` (single place for completion + Soul Points rule)
- `Views/`
  - Razor UI pages for Home, HelpRequests, Profile, Shared layout
- `wwwroot/`
  - static files (site.css)

## Business Rule Location (Important)
The Soul Points logic is centralized in:
- `Services/HelpRequestService.cs` (`CompleteRequestAsync`)

It validates ownership/status/helper and awards points exactly once when completing a request.

## Run Locally
1. Install .NET 10 SDK
2. From project folder:
   ```bash
   cd HelpEachOther
   dotnet restore
   dotnet ef database update
   dotnet run
   ```
3. Open browser at the URL shown in terminal.

## Migrations
Create initial migration:
```bash
cd HelpEachOther
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
```

Apply migration:
```bash
dotnet ef database update
```

> On startup, the app also runs `Database.Migrate()` and seeds demo data if requests table is empty.

## Demo Seed Data
If DB has no requests, startup seeds:
- A demo user (`demo@help.com`)
- A few open help requests in different categories/cities

