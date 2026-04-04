# PROJECT_PLAN

## Vision

Build a simple community-help platform where asking for and providing help is easy, safe, and transparent.

## Current MVP Scope

- Authentication (register/login/logout)
- Request creation and browsing
- Volunteer assignment
- Owner completion confirmation
- Soul Points reward
- Basic profile and personal request/helping views

## Completed

- Base ASP.NET Core MVC app setup
- EF Core + SQLite integration
- Identity integration with custom `ApplicationUser`
- Help request entity and status flow (`Open`, `InProgress`, `Completed`)
- Request create/details/index pages
- Volunteer action with ownership/status checks
- Completion action with service-level rules + Soul Points award
- Owner edit/delete restrictions for open requests
- Profile pages (overview, my requests, my helping)
- Startup migration + seed flow

## In Progress

- Expanding request management workflows from profile area
- Tightening validation and UX messaging around lifecycle transitions

## Next High-Priority Features

1. Request communication/contact flow
   - Controlled owner-helper communication once helper is assigned
   - Privacy-safe contact visibility rules
2. Request lifecycle improvements
   - Better transition feedback and guardrails
   - Optional cancel/reopen semantics (if approved by product goals)
3. Owner request management
   - Clearer filtering/actions in My Requests
   - Better action affordances for each status
4. Moderation / safety basics
   - Reporting suspicious requests or behavior
   - Basic admin/moderator review path
5. Better validation
   - Clearer input validation messages
   - Consistent server-side validation across all forms
6. UX polish
   - Cleaner status badges, empty states, and action clarity
   - Mobile layout and accessibility improvements

## Lower-Priority Future Ideas

- Reputation tiers based on Soul Points
- Category-specific search and location filtering
- Notification preferences (email/in-app)
- Basic activity timeline on profile

## Technical Cleanup Ideas

- Add focused tests for business-rule services and key controller actions
- Improve logging around state transitions and failures
- Consolidate repeated status-check logic where safe
- Add simple constants/config for user-facing status text

## Risks / Concerns

- Request communication can introduce privacy/safety risks if exposed too broadly
- Business rule drift if logic is duplicated outside services
- Overengineering risk: introducing heavy abstraction too early
- Missing tests may allow lifecycle regressions
