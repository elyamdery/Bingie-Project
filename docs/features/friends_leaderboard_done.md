# Friends Leaderboard – Implementation Complete

Status: ✅ Delivered on branch `feature/friends-leaderboard-impl`.

Highlights:
- Added `FriendsLeaderboardService` + SQLite schema for circles, opt-in privacy, weekly snapshots, and support token history behind `FeatureFlags.LeaderboardEnabled`.
- Introduced `FriendsLeaderboardPage` tab with opt-in wizard, live rankings, encouragement feed, and quick toggles wired to Home settings.
- New unit suite `FriendsLeaderboardServiceTests` covers ranking weights, consent masking, support tokens, and capacity checks.

See manual validation checklist in `docs/manual_check/friends_leaderboard_manual_checkup.md`.
