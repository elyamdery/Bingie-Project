# Anti-Binge System Blueprint

## Vision
- Provide a supportive, data-backed experience that helps users understand and reduce binge episodes.
- Ensure every binge event is captured, surfaced in history/statistics, and paired with positive reinforcement.
- Keep the architecture MAUI-friendly with clear seams for future wellness features (journaling, coaching, analytics).

## Functional Pillars
1. **Binge Capture Loop**
   - Trigger: primary CTA on `MainPage`.
   - Action: persist `BingeEntry` (UTC timestamp, optional mood/context) through `IDataStore`.
   - Feedback: instant UI acknowledgement (dot, toast) plus background refresh of history/calendar/statistics.
2. **Reflection & Insight**
   - Calendar heat-map via `CalendarService`.
   - Daily drill-down (`DayStatisticsPage`) listing entries with context.
   - History analytics card combines binge list, 7-day moving averages (`BingeAnalytics` + `BingeTrendDrawable`) and quick summaries/streaks.
3. **Supportive Content**
   - Curated library of 100+ compassionate quotes surfaced on `ExplorePage`.
   - Playful micro-challenges encourage mindful resets and habit nudges.

## Architectural Shape
- **Data Layer**: `SqliteConnectionFactory` → `DatabaseService` implementing `IDataStore<T>` for `BingeEntry` + `User`.
- **Domain Services**:
  - `CalendarService` orchestrates temporal aggregations.
  - `AuthService` handles credential flow.
- **Presentation**:
  - `AppShell` exposes Home / History / Explore tabs with History acting as the analytics hub.
  - ViewModels (future work) should replace code-behind logic for better testability.
- **Telemetry**: Serilog captures critical flows (login, binge creation) with PII-safe metadata.

## Implementation Roadmap
1. **Instrumentation**
   - Persist binge CTA interactions asynchronously and replace disruptive alerts with inline feedback.
   - Broadcast lightweight messages so active views refresh (short-term: `MessagingCenter`, long-term: `WeakReferenceMessenger`).
2. **Navigation & UX**
   - Centralise trend analytics within History with expandable chart + summary tiles.
   - Keep Explore playful with gradients, card animations, and quick challenges.
3. **Security & Preferences**
   - Align trimming logic in `AuthService`.
   - Replace stored plaintext password with secure flag/token.
   - Add settings hook for clearing remembered credentials.
4. **Testing**
   - Unit tests for `AuthService` (trim symmetry, remember-me contract).
   - Integration-like tests for `CalendarService` monthly counts and daily queries.
   - Analytics specs validating trend moving averages + quote catalogue integrity.
   - UI smoke tests (future stretch: UITest harness).

## Risks & Mitigations
- **UI coupling**: heavy code-behind; mitigate by migrating to MVVM gradually.
- **Concurrency**: simultaneous binge events may collide; add locking or transactional guarantees later.
- **Data growth**: SQLite fine for now; add pruning/export once usage grows.

## Metrics for Success
- 100% capture rate for binge events initiated via CTA.
- Calendar & statistics reflect new entries immediately.
- Zero plaintext secrets persisted locally.
- Test suite covering new behaviors with deterministic results.
