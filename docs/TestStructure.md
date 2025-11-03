# Test Structure Guide

## Overview
- All automated tests live under `tests/Bingie.Tests/`, an xUnit project targeting `net8.0`.
- The project references application services and models via linked source files; no production assemblies are built separately for testing.
- In-memory test doubles (`tests/Bingie.Tests/TestDoubles`) stand in for SQLite stores so specs run quickly and deterministically.

## Suite Breakdown
- **AuthServiceTests**: Validates registration, login trimming, remember-me token issuance/clearing, and duplicate guardrails.
- **AuthFlowE2ETests**: Covers the end-to-end register → login → remember-me → revoke sequence using the same in-memory store.
- **CalendarServiceTests** and **BingeAnalyticsTests**: Exercise calendar aggregation and analytics summaries.
- **AvatarFeedbackServiceTests**: Ensure avatar scoring, state transitions, hide toggles, and monthly celebration logic behave per SRS expectations.
- **StoryGuideServiceTests**: (from feature branch) verify trigger logging, experiment rotation, XP grant, and weekly episode summaries.
- **PointsSystemServiceTests**: Assert quest generation, daily limits, pause behavior, XP ledger updates, and cosmetic unlock flow.
- **QuotesServiceTests**: Provide content integrity checks for the inspiration catalogue.

## Patterns & Conventions
- Every service spec follows the Arrange/Act/Assert pattern with clear method naming (`MethodName_Scenario_ExpectedOutcome`).
- Feature flags are overridden via `FeatureFlags.Override*` helpers to isolate scenarios.
- Date/time inputs are passed explicitly (UTC) so tests avoid `DateTime.Now`.
- Shared helpers (in-memory stores, fake repositories) live in `TestDoubles`, keeping tests concise.

## Running Tests
1. Full suite: `dotnet test` (build + run).
2. Quick rerun without rebuild: `dotnet test --no-build`.
3. CI-safe command is the same; coverage collection is available via the `coverlet.collector` reference if needed.

Adopt these patterns when adding new specs so the suite stays fast, reliable, and easy to extend.
