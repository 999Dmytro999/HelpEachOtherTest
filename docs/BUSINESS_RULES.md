# BUSINESS_RULES

This file is the central source of truth for current HelpEachOther app rules.

## 1) Request Creation

- Only authenticated users can create help requests.
- Request fields required: Title, Description, Category, City.
- New requests are created with status `Open`.

## 2) Who Can Volunteer

- Only authenticated users can volunteer.
- A user **cannot** volunteer for their own request.
- A request can be volunteered for only when:
  - status is `Open`, and
  - no helper is currently assigned.
- When someone volunteers:
  - `HelperId` is set,
  - status changes to `InProgress`.

## 3) Request Statuses

- `Open`: Request is available for volunteers.
- `InProgress`: A helper is assigned and work is ongoing.
- `Completed`: Work is confirmed complete by the owner.

Expected lifecycle: `Open -> InProgress -> Completed`.

## 4) Completion Rules

- Only the **request owner** can mark a request as completed.
- Completion is allowed only when status is `InProgress`.
- Completion requires an assigned helper.
- On completion:
  - status becomes `Completed`,
  - `CompletedAt` is set,
  - helper is awarded Soul Points.

## 5) Soul Points Rules

- Soul Points are granted when owner completes a valid in-progress request.
- Current value: **10 points** per completed help request.
- Points are granted to the assigned helper.
- Points should not be double-awarded for the same request.

## 6) Open-Only Owner Actions

The owner can edit or delete a request only while status is `Open`.

If request is `InProgress` or `Completed`, edit/delete should be rejected.

## 7) Validation Expectations

### Registration

- Email is required and must be valid format.
- Password is required.
- ConfirmPassword must match Password.
- Current Identity password policy is intentionally relaxed for MVP usability (minimum length 6, no digit/upper/lower/special required).

### Request forms

- Title: required, max 100 chars.
- Description: required, max 1000 chars.
- Category: required, max 100 chars.
- City: required, max 100 chars.

Server-side validation is mandatory; client-side validation is supportive only.

## 8) Visibility / Contact Info (Current and Future)

Current MVP does not implement a dedicated private messaging or contact-exchange workflow.

Future communication features must:
- expose contact details only to directly involved users (owner + assigned helper),
- avoid public display of sensitive personal contact info,
- include server-side authorization checks for all contact endpoints/actions.

## 9) Security / Authorization Rules

- Authorization checks must remain on the server side.
- Never rely only on hidden fields/UI conditions for permissions.
- Any action changing request ownership, helper assignment, status, or points must verify current user identity and allowed transition.
- Anti-forgery validation should be kept for state-changing form posts.
