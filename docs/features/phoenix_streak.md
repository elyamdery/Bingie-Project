# Phoenix Streak SRS

## Objective
Introduce a resilient, Duolingo-style streak mechanic that tolerates occasional slips and rewards compassionate recovery.

## User Flow
1. User sets maximum daily binge threshold (default 0, adjustable).  
2. Each day under threshold extends the streak; exceeding threshold triggers a “Phoenix Day” flow prompting reflection.  
3. Completing reflection plus coping exercise allows streak recovery using XP or a weekly “grace token.”  
4. Streak milestones grant avatar effects and supportive messages.  
5. Users can hide streak counts or disable the feature entirely.

## Data Requirements
- Daily binge counts per user.  
- Threshold setting and history.  
- Reflection completion status.  
- Grace token inventory and XP ledger linkage.

## Ethical Considerations
- Emphasise self-compassion; language avoids failure framing.  
- Provide full opt-out and hide controls.  
- Ensure recovery options don’t pressure users into coping exercises they dislike.

## Metrics
- Streak participation rate.  
- Recovery completion rate.  
- Relationship between streak participation and logging adherence.

## Dependencies
- XP ledger (recovery cost).  
- Reflections module (existing or new).  
- Notification service for streak reminders.

## Acceptance Criteria
- Streak calculator handles missed days and threshold changes.  
- Grace tokens replenish weekly, configurable by product team.  
- Recovery flow cannot be bypassed to extend streak without reflection.  
- Feature flag `phoenixStreakEnabled` governs availability.  
- Unit tests for streak logic & recovery gating; notification mocks for reminder scheduling.
