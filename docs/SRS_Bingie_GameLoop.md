# Bingie Game Loop SRS (Index)

## 1. Overview
- **Goal:** Transform Bingie into a habit-supportive, gamified experience while maintaining compassion and user control.
- **Scope:** Avatar feedback, guided trigger logging, XP/points system, social leaderboard, trigger analytics, and resilient streak mechanics. Each feature is documented separately so we can iterate or roll back without affecting the rest.
- **Audience:** Product, engineering, design, data, QA, clinical advisors.

## 2. Shared Systems & Principles
| Topic | Decisions |
| --- | --- |
| Tone | Supportive, self-efficacy language; celebrate awareness versus abstinence. |
| Scoring | Single XP ledger (`user_progress`) powering avatar levels, badge unlocks, and streak recovery boosts. |
| Opt-outs | Every gamified surface must expose opt-out/visibility settings; default social sharing OFF. |
| Data Retention | New derived data (trigger tags, XP, streak logs) stored alongside binge logs and deletable upon user request. |
| Analytics | Track feature adoption with anonymized events. |
| Privacy | Leaderboard uses display names/avatars, never raw counts without consent. |

## 3. Feature Specs
- [Avatar Body Feedback](features/avatar_feedback.md)
- [Binge Story Guide](features/story_guide.md)
- [Anti-Binge Points System](features/points_system.md)
- [Friends Leaderboard](features/friends_leaderboard.md)
- [Trigger Radar Chart](features/trigger_radar.md)
- [Phoenix Streak](features/phoenix_streak.md)

Each feature doc covers objectives, user flows, data needs, ethical considerations, metrics, dependencies, and acceptance criteria. Remove an individual file to de-scope a feature without rewriting this index.

## 4. Implementation Roadmap
| Feature | Branch Prefix | Primary Tests | Rollback Plan |
| --- | --- | --- | --- |
| Avatar Body Feedback | `feature/avatar-feedback-*` | Scoring unit tests, animation snapshots, XP integration | Feature flag `avatarFeedbackEnabled` |
| Binge Story Guide | `feature/story-guide-*` | Trigger logging unit tests, content selection, analytics events | Feature flag `storyGuideEnabled` |
| Anti-Binge Points System | `feature/points-system-*` | XP calc tests, quest board state, E2E quests | Feature flag `pointsSystemEnabled` |
| Friends Leaderboard | `feature/leaderboard-*` | Ranking tests, privacy opt-in, API auth | Feature flag `leaderboardEnabled` |
| Trigger Radar Chart | `feature/trigger-radar-*` | Aggregation tests, chart rendering, suggestion text | Feature flag `triggerRadarEnabled` |
| Phoenix Streak | `feature/phoenix-streak-*` | Streak calculator tests, recovery gating, notification mocks | Feature flag `phoenixStreakEnabled` |

- **Testing Strategy:** Each feature branch requires unit, integration, and UI snapshot coverage plus validation of analytics events in staging.
- **Release Process:** Staged rollout via feature flags; collect telemetry & feedback before expanding.
- **Documentation:** Update product wiki, support scripts, and in-app FAQ as features ship.

## 5. Revision History
- **v1.1 (2025-02-14):** Split feature specifications into individual docs for modular planning.
- **v1.0 (2025-02-14):** Original consolidated draft.
