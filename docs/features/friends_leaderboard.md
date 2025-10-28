# Friends Leaderboard SRS

## Objective
Offer optional peer accountability via weekly leaderboards based on supportive metrics (XP, coping actions, streak length) while prioritising privacy and consent.

## User Flow
1. User opts in, chooses a nickname, and selects which metrics (XP, streak length, coping count) to share.  
2. User joins or creates a circle (max 15 members).  
3. Weekly leaderboard resets; top performers receive “support tokens” they can gift as encouragement.  
4. Members can send supportive messages or flag concerns for private follow-up.  
5. User can leave or mute a circle at any time.

## Data Requirements
- Social graph (circle membership).  
- Aggregated metrics per user per week.  
- Support token transactions.  
- Privacy preferences.

## Ethical Considerations
- Explicit opt-in with metric selection.  
- Allow anonymous participation (nickname only).  
- Provide tools to mute, leave, or report a circle.  
- Ensure messaging is moderated/limited to prevent shaming.

## Metrics
- Opt-in rate vs. total active users.  
- Circle retention and churn.  
- Number of support tokens sent per week.

## Dependencies
- Social graph service (new).  
- Metrics aggregation pipeline.  
- Notification service for circle updates.

## Acceptance Criteria
- Users cannot see others’ raw binge counts unless explicitly shared.  
- Leaderboards reset weekly with archival history available privately.  
- Support tokens limited to positive templates.  
- Feature flag `leaderboardEnabled` gates backend and UI.  
- Unit tests for ranking logic, privacy enforcement, and API permissions.
