# Phoenix Streak – Implementation Complete

Status: ✅ Delivered on branch `feature/phoenix-streak-impl`.

Highlights:
- Added `PhoenixStreakService` to track compassionate streaks, manage grace tokens, and store thresholds via SQLite.
- Home now surfaces a Phoenix Streak card (threshold stepper, status copy, grace button) plus a settings toggle for `phoenixStreakEnabled`.
- Manual QA checklist and service-level unit tests ensure computation/recovery behave as expected.

See manual validation checklist in `docs/manual_check/phoenix_streak_manual_checkup.md`.
