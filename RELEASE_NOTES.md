# Release Notes

## 2025-08-18 Migration Prep (Unversioned)

### Summary
Initial groundwork laid to migrate the legacy ASP.NET MVC 5 (.NET Framework 4.8) sample to ASP.NET Core:

- Added `CoreHost/CoreHostApp` ASP.NET Core 8.0 MVC scaffold as a reference target runtime.
- Added `aspnet-get-started.Tests` NUnit/NUnitLite test project for the existing MVC5 controllers.
- Implemented console export of NUnit XML results to aid CI integration.
- Documented migration strategy, key breaking changes, and recommended phased approach (see `MIGRATION.md`).

### Rationale
The classic project targets .NET Framework 4.8 (`System.Web` + MVC 5.2.7) which is Windows / Mono oriented and not cross‑platform friendly. Moving to ASP.NET Core unlocks modern runtime features, performance, and long‑term support while simplifying Azure App Service deployment.

### Added Artifacts
- `CoreHost/CoreHostApp` – baseline ASP.NET Core app.
- `aspnet-get-started.Tests` – unit tests (3 passing) for `HomeController`.
- `MIGRATION.md` – detailed migration guide.

### Not Yet Done
- Full port of views/controllers/models to ASP.NET Core.
- Middleware / routing parity validation.
- Authentication / authorization migration.
- Static content bundling (currently using legacy bundling & minification).

### Next Recommended Steps
1. Incrementally port controllers & views into the Core project.
2. Introduce integration tests using `WebApplicationFactory` once controllers are migrated.
3. Remove unused Mono specific build artifacts once parity is achieved.
4. Add GitHub Actions workflow to build & test both projects during transition.

---
