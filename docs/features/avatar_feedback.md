# Avatar Body Feedback SRS

## Objective
Provide a playful avatar that reflects wellbeing trends (weekly/monthly binge scores) using energy metaphors rather than weight stigma, reinforcing positive behaviour without shaming.

## User Flow
1. User logs binges as usual.  
2. Weekly summary calculates a binge score using configurable thresholds.  
3. Avatar morphs through animation states (energized, steady, tired) based on score deltas and streak improvements.  
4. Monthly recap compares rolling score to previous month, triggering a special animation and supportive copy.  
5. Users can toggle avatar feedback off at any time.

## Data Requirements
- Weekly binge count and severity weighting.  
- Monthly blended score history.  
- Avatar state history and unlocks.  
- User settings: hide avatar changes, threshold preferences.

## Ethical Considerations
- Use “energy/glow/posture” metaphors; avoid “fat/thin” labels.  
- Provide immediate “Hide avatar changes” toggle.  
- Include supportive messaging for negative trends and encourage compassion.

## Metrics
- Avatar engagement rate.  
- Hide toggle usage.  
- Correlation between avatar interactions and daily logging adherence.

## Dependencies
- Binge log store.  
- XP ledger for level-based unlocks.  
- Animation engine and asset pipeline.

## Acceptance Criteria
- Weekly and monthly scores calculated correctly for configurable thresholds.  
- Avatar states map to score ranges with accessible animations.  
- Hide toggle immediately removes morphing and stores preference.  
- Feature flag `avatarFeedbackEnabled` gates all UI.  
- Unit tests cover scoring & state transitions; UI snapshots cover animation state selection.

