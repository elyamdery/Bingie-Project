# Anti-Binge Points System SRS

## Objective
Reward healthy actions (logging, coping exercises, hydration, etc.) with XP/points to reinforce consistent engagement while keeping rewards cosmetic and optional.

## User Flow
1. Define point values per action in a configuration table.  
2. Daily “Quest Board” surfaces 3–5 small actions; completing an action grants XP.  
3. XP fills a “Glow Meter” tied to avatar accessories/background themes.  
4. Weekly dashboard displays XP trend and allows redemption for cosmetic unlocks.  
5. Users can pause rewards, which hides quests and halts XP accrual.

## Data Requirements
- Action definitions and XP values.  
- XP transactions with timestamps.  
- Inventory of unlockable cosmetics.  
- User settings: pause rewards, equipped items.

## Ethical Considerations
- No monetary purchases.  
- Make XP optional via “Pause rewards”.  
- Ensure quests include restorative options, not just productivity.

## Metrics
- Quest completion rate.  
- Distribution of XP accrual.  
- Impact on daily logging retention.

## Dependencies
- XP ledger service.  
- Avatar customization system.  
- Notification service (optional reminders).

## Acceptance Criteria
- XP engine handles duplicate actions & daily limits.  
- Quest board refreshes daily with configurable randomness.  
- Cosmetic unlocks remain cosmetic only.  
- Feature flag `pointsSystemEnabled` controls exposure.  
- Unit tests for XP calculations & quest state; E2E for quest flow.
