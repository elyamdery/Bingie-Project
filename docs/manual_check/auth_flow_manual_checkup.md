# Auth Flow Refresh – Manual Checkup

Use this list before tagging the feature as verified or when investigating regressions.

- [ ] **Fresh install login**
  - Launch the app, ensure the login form renders with gradient background and trimmed entries.
  - Attempt login with blank fields → expect inline error alert.
- [ ] **Registration round-trip**
  - Tap “Create an account”, register with unique username/password, confirm success dialog.
  - After navigation back to login, sign in using the new credentials.
- [ ] **Remember me**
  - Enable “Stay signed in”, log in, kill and relaunch the app.
  - Verify auto-navigation into the shell without prompts.
- [ ] **Remember me opt-out**
  - From login, uncheck “Stay signed in” and log in; confirm credentials required on next launch.
- [ ] **Mismatched passwords**
  - Attempt registration with differing password fields → expect inline red guidance.
- [ ] **Invalid credentials**
  - Try an incorrect password for an existing user → expect error alert and no navigation.
- [ ] **Profile header**
  - After login, confirm the right-side header shows username, level, and weekly XP.
  - Tap `Sign out` → expect confirmation dialog and return to login page with remember-me cleared.
- [ ] **Quick settings toggles**
  - Open `Settings` in the header; flip avatar/story/points switches and verify UI hides/shows the corresponding sections.
  - Enable `Legacy mode` → all feature toggles off and header copy switches to “Legacy mode”.
