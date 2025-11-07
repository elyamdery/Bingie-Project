# Avatar Body Arc Blueprint

## Objective
Give users a compassionate visual of how their avatar’s “energy body” fluctuates based on recent binges/break days.

## Experience
- Home screen surfaces an “Avatar body arc” card (feature-flagged) showing a stylised body that grows softer/tighter based on a daily score.
- A slider or scrubber lets users replay the past 7 days to observe patterns; a horizontal micro-history highlights which days felt heavier.
- Copy stays shame-free (“Avatar tired, offer it a rest snack.”).
- Settings include a toggle plus legacy mode integration to hide the feature quickly.

## Data Inputs
- Daily binge counts (existing `CalendarService` data).
- Avatar feedback snapshot (for supportive language & synergy).

## Accessibility
- High-contrast fills, no gendered imagery, narrated summary for screen readers.
- Animations are subtle; no flashing.

## Next Steps
- Expand body art variants (celebration states, seasonal outfits).
- Add journaling prompt when a tired state repeats 3+ days.
