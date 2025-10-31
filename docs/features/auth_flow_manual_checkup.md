# Auth Flow Refresh – Manual Checkup

Use this list before tagging the feature as verified or when investigating regressions.

1. **Fresh install login**
   - Launch the app, ensure the login form renders with gradient background and trimmed entries.
   - Attempt login with blank fields → expect inline error alert.
2. **Registration round-trip**
   - Tap “Create an account”, register with unique username/password, confirm success dialog.
   - After navigation back to login, sign in using the new credentials.
3. **Remember me**
   - Enable “Stay signed in”, log in, kill and relaunch the app.
   - Verify auto-navigation into the shell without prompts.
4. **Remember me opt-out**
   - From login, uncheck “Stay signed in” and log in; confirm credentials required on next launch.
5. **Mismatched passwords**
   - Attempt registration with differing password fields → expect inline red guidance.
6. **Invalid credentials**
   - Try an incorrect password for an existing user → expect error alert and no navigation.
