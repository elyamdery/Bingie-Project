# Auth Flow Refresh – Done

## Delivered Enhancements
- Rebuilt login and registration pages with a minimalist gradient card layout that avoids layout freezes and keeps focus states predictable.
- Hardened remember-me persistence by hashing tokens, trimming credentials, and clearing stale tokens when the user opts out.
- Added resilient navigation so return trips from registration land on the login page without crashes.
- Documented troubleshooting commands in `docs/DevGuide.md` so the team can rebuild or run focused tests quickly.

## Test Coverage
- Unit coverage extended in `AuthServiceTests` for trimming, token issuance, duplicate guardrails, and login failure paths.
- End-to-end automation in `AuthFlowE2ETests` covering register → login → remember-me → revoke.
- Manual smoke: `dotnet test tests/Bingie.Tests/Bingie.Tests.csproj -v minimal` (quick focused run).

## Follow-Ups
- Migrate remaining `MessagingCenter` usage to CommunityToolkit messaging once the shell refactor lands.
- Surface server-side validation messages on registration if/when we add a backing API.
