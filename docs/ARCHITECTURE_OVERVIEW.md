# ARCHITECTURE_OVERVIEW

This is a simple MVC app. If you come from Java + Spring MVC, think of this as the same pattern with ASP.NET conventions.

## Request flow in simple words

1. User signs in.
2. User creates a help request (`Open`).
3. Another user volunteers -> request becomes `InProgress`.
4. Owner confirms work done -> request becomes `Completed`.
5. Helper gains Soul Points.

## MVC structure

- **Models**: database entities and enums (`HelpRequest`, `ApplicationUser`, statuses)
- **ViewModels**: form input contracts for create/edit pages
- **Views**: Razor pages that render HTML
- **Controllers**: receive HTTP requests, validate user and model, call services/EF, return views

## What `Program.cs` does

- Configures EF Core with SQLite
- Configures ASP.NET Core Identity
- Registers MVC + Razor Pages + services
- Applies database migrations on startup
- Seeds initial demo data
- Configures middleware pipeline and routes

## What Controllers do

- `HelpRequestsController`: list/details/create/edit/delete/volunteer/complete actions
- `ProfileController`: profile summary, my requests, my helping pages
- `HomeController`: landing/basic pages

Controllers should orchestrate flow, not hold heavy business logic.

## What Services do

- `HelpRequestService` holds important completion logic:
  - validates who can complete,
  - validates valid status/helper,
  - sets completion fields,
  - awards Soul Points.

## What Models do

- Represent persisted data and relationships.
- Example: `HelpRequest` has owner, optional helper, status, timestamps.

## What ViewModels do

- Bind and validate form input for MVC actions.
- Keep input surface separate from persistence entities.

## What Views do

- Render server-side UI with Razor.
- Show forms, request lists, details, and profile pages.

## What `ApplicationDbContext` does

- Defines EF Core `DbSet`s (currently `HelpRequests` plus Identity tables via base class).
- Configures owner/helper relationships and delete behavior.

## How Identity fits in

- Handles registration, login, logout, and user storage.
- `ApplicationUser` extends Identity user with `DisplayName` and `SoulPoints`.

## How completion flow works

- Owner submits complete action.
- Controller passes request id + current user id to `HelpRequestService`.
- Service validates business rules and performs atomic update.
- If valid, status becomes `Completed` and helper gets Soul Points.
