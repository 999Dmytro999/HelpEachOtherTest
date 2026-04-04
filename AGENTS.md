# AGENTS.md - HelpEachOther Repository Guidance

## What this project is

HelpEachOther is a .NET 10 ASP.NET Core MVC community-help app.
Users create help requests, others volunteer, and request owners close completed work. Helpers earn Soul Points.

## Main business goal

Deliver a simple, safe, understandable request lifecycle:
**Open request -> helper volunteers -> owner confirms completion -> helper gets points**.

## Tech stack

- .NET 10
- ASP.NET Core MVC + Razor Views
- Entity Framework Core + SQLite
- ASP.NET Core Identity
- Bootstrap

## Architecture summary

- MVC web app with server-rendered Razor Views
- `Controllers` handle HTTP flow and authorization entry points
- `ViewModels` are used for form binding and validation
- `Services` contain important reusable business logic
- `ApplicationDbContext` is EF Core data access + relationships
- Identity handles authentication/users

## Important business rules (must preserve)

- Only authenticated users can create requests.
- Only authenticated users can volunteer.
- A user cannot volunteer for their own request.
- Volunteer action is valid only for `Open` requests without a helper.
- Owner can edit/delete request only when status is `Open`.
- Only owner can mark request `Completed`.
- Completion is valid only from `InProgress` with assigned helper.
- Completing a request grants Soul Points to helper exactly once.

## Rules for editing this codebase

- Prefer simple MVC patterns.
- Prefer ViewModels for form binding.
- Avoid binding entities directly in forms.
- Avoid repository pattern unless truly necessary.
- Keep important or reused business logic in services.
- Keep authorization checks on the server side.
- Follow existing naming and folder structure.
- Keep changes incremental; do not rewrite unrelated files.
- Prefer readability over cleverness.
- Do not add frontend frameworks unless explicitly requested.

## What NOT to change casually

- Existing request lifecycle semantics (`Open`, `InProgress`, `Completed`)
- Soul Points award behavior
- Identity configuration and login/register flow
- Database schema relationships between owner/helper and requests
- Controller routes/views conventions unless required

## Coding style expectations

- Use clear names and short methods when possible.
- Keep controller actions straightforward and explicit.
- Keep validation via DataAnnotations + `ModelState`.
- Return early for authorization/validation failures.
- Keep UI text and status/error messages user-friendly.

## Preferred patterns

- Controller orchestrates request/response
- Service enforces core business rules when logic is reused/important
- ViewModel receives form data
- Entity model represents persisted state
- EF Core LINQ queries stay simple and readable

## Preferred next-step development order

1. Solidify request lifecycle edge cases and validations
2. Add owner-helper communication/contact flow with privacy controls
3. Expand owner request management (status filters/actions)
4. Add moderation and safety baseline tools
5. Polish UX and accessibility

## How to approach changes safely

1. Read existing flow in controllers + services first.
2. Implement smallest working change.
3. Preserve current behavior unless requirement explicitly changes it.
4. Validate authorization and status transitions.
5. Update docs when business rules change.

## Avoid overengineering

Do not introduce extra abstraction layers for simple CRUD/MVC flows.
Use direct, maintainable code suitable for beginner-to-intermediate contributors.

## Preserve beginner-friendly structure

This repository is intentionally easy to follow.
When in doubt, choose the most readable option over advanced patterns.

## Respect existing MVC architecture

Do not convert this app to API-first, SPA, or Clean/Hexagonal architecture unless explicitly requested.
Build on the current MVC approach.
