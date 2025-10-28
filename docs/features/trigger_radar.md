# Trigger Radar Chart SRS

## Objective
Visualise time-of-day/day-of-week risk zones and improvements to help users plan proactive support.

## User Flow
1. Aggregate binge logs and associated triggers into time buckets.  
2. Render radar/polar chart with color-coded intensity and trend indicators.  
3. Provide plain-language interpretation and suggest scheduling adjustments (“Consider prepping a snack Sundays at 8 pm”).  
4. Allow export/share of the chart with therapist or support person.

## Data Requirements
- Timestamped binge entries and trigger tags.  
- Aggregated weekly/monthly stats.  
- User annotations (optional).

## Ethical Considerations
- Include copy explaining that spikes are normal, avoiding blame.  
- Allow users to exclude specific periods or delete data before aggregation.  
- Ensure shared exports redact personal identifiers.

## Metrics
- Chart view rate.  
- Follow-through on suggested adjustments (tracked via “Mark done”).  
- Change in binge frequency in targeted periods.

## Dependencies
- Analytics aggregation pipeline.  
- Charting library capable of accessibility compliance.  
- Story guide (for suggestion text reuse).

## Acceptance Criteria
- Aggregation handles time zones and daylight savings.  
- Chart accessible (screen-reader descriptions, contrast).  
- Suggestions deduplicated and actionable.  
- Feature flag `triggerRadarEnabled` controls availability.  
- Unit tests for aggregation & suggestion generation; UI snapshot for chart rendering.
