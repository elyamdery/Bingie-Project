# Bug Discovery Codex

Track open investigations and confirmed issues while we iterate on Bingie.

| ID | Status | Area | Description | Notes / Next Steps |
| --- | --- | --- | --- | --- |
| B-001 | Resolved | Points System | XP appeared static after logging because ExplorePage cached the initial dashboard. | Added a `PointsDashboardUpdated` notification after each log and subscribed ExplorePage to refresh immediately; manual retest confirmed XP jumps without restarting. |

Update this table whenever we discover, reproduce, or resolve a bug so it complements `WorkLogCodex`.
