# Phoenix Streak Manual Checkup

- [ ] Toggle `phoenixStreakEnabled` via Home → Settings; confirm the streak card hides/shows and legacy mode disables it.
- [ ] Adjust the daily threshold using the stepper and ensure the value persists after navigating away/restart.
- [ ] Log days under the threshold and confirm the streak counter increments with compassionate copy.
- [ ] Force an over-threshold day and verify the card requests recovery; use a grace token to restore the streak.
- [ ] Confirm grace tokens reset after switching the device clock forward a week (or via mocked test hook).
- [ ] Screen reader announces streak count, status, threshold, and grace info.
