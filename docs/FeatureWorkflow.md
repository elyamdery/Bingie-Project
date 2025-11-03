# Feature Implementation Workflow

Use this playbook for every feature we ship on Bingie so branches stay tidy and reviews predictable.

1. **Spin up a feature branch**  
   - Name it `feature/<feature-key>-impl` off `version5`.  
   - Keep commits scoped to the feature; avoid drive-by fixes unless they unblock the work.

2. **Build to the SRS**  
   - Follow the relevant spec under `docs/features/`.  
   - Gate new UI/logic behind the documented feature flags (`FeatureFlags` class).

3. **Add coverage**  
   - Unit and integration specs for new services/state.  
   - Write or extend E2E-style tests when flows span multiple services (e.g., auth login/register).  
   - Run `dotnet test` twice (`dotnet test`, then `dotnet test --no-build`) locally before asking for review.

4. **Document completion**  
   - Drop `_done` and `_manual_checkup` notes beside the feature SRS describing what shipped and how to verify manually.  
   - Append a WorkLog entry summarising the changes and tests executed.

5. **Merge to `version5`**  
   - After the double test run passes, merge the feature branch into `version5`.  
   - Update `ver5_dev` with the latest `version5` when staging the integration branch.

6. **Integration branch (`ver5_dev`)**  
   - Use `ver5_dev` as the pre-commit integration lane; merge completed feature branches in order.  
   - Run the full suite again after each merge to catch cross-feature regressions.

7. **Promotion**  
   - Once all features in scope are green on `ver5_dev`, fast-forward `version5` and open a PR to `main` if release-ready.

8. **Bug tracking**  
   - Log regressions or investigations in `docs/BugDiscoveryCodex.md` so fixes stay traceable alongside feature work.
