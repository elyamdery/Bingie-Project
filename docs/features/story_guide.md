# Binge Story Guide SRS

## Objective
Transform binge entries into narrative insights using predefined top-7 trigger reasons, encouraging experimentation with coping strategies.

## User Flow
1. After logging a binge/urge, user selects a trigger from the curated list or chooses “something else”.  
2. App records trigger and suggests a contextually relevant “story chapter” describing patterns and coping experiments.  
3. Weekly “story episode” summarises repeated triggers and offers two experiments; user can mark one to try.  
4. Completing an experiment grants XP and unlocks narrative progress.

## Data Requirements
- Trigger taxonomy and localization.  
- Per-entry trigger selections, including free-text overrides.  
- Experiment selection/completion status.  
- Narrative progression state.

## Ethical Considerations
- Always provide “skip story” option.  
- Respect user privacy by keeping narrative local unless user explicitly shares.  
- Ensure copy is supportive, acknowledging that triggers are normal.

## Metrics
- Trigger selection rate vs. skip.  
- Experiment adoption and completion.  
- Reduction in repeated trigger frequency over time.

## Dependencies
- Content CMS for story chapters.  
- XP ledger (rewards).  
- Analytics pipeline for trigger aggregation.

## Acceptance Criteria
- Trigger list configurable via CMS with safe defaults.  
- Experiments cannot be suggested twice consecutively without user completion.  
- XP awarded only once per experiment completion.  
- Feature flag `storyGuideEnabled` toggles messaging and UI.  
- Unit tests for trigger logging and experiment lifecycle; analytics events validated in staging.
