# Migration Guide: ASP.NET MVC 5 (.NET Framework 4.8) to ASP.NET Core 8

Date: 2025-08-18
Status: In Progress (Phase 0 – Preparation)

## Goals
- Move from classic `System.Web` MVC 5.2.7 application (`aspnet-get-started`) targeting .NET Framework 4.8 to ASP.NET Core 8 for cross‑platform, performance, modern hosting, and long-term support.
- Preserve functional parity (routes, views, controller actions) while modernizing project layout, dependency injection, configuration, static files, and build/test tooling.

## High-Level Phases
| Phase | Focus | Output |
|-------|-------|--------|
| 0 | Preparation & baseline | Scaffold Core project, baseline tests (DONE) |
| 1 | Core skeleton alignment | Create domain + shared libraries if needed |
| 2 | Controller & View port | All MVC controllers & Razor views moved |
| 3 | Infrastructure & Middleware | Filters, DI services, logging, error handling |
| 4 | Performance & Security hardening | Caching, HTTPS, headers, auth migrations |
| 5 | Cleanup & Decommission | Remove legacy project, finalize CI/CD |

## Project Comparison
| Aspect | Legacy (.NET 4.8) | Target (ASP.NET Core) |
|--------|------------------|-----------------------|
| Framework | .NET Framework 4.8 | .NET 8 (LTS) |
| Server | IIS / IIS Express / Mono + xsp | Kestrel + reverse proxy (App Service builtin) |
| Config | `Web.config` (XML) | `appsettings*.json`, environment vars, minimal `Program.cs` |
| DI | Manual / static | Built‑in IServiceCollection / constructor injection |
| Routing | Global.asax + RouteConfig | Endpoint routing (MapControllerRoute) |
| Views | Razor (MVC5) | Razor (Core) – similar syntax, some helpers differ |
| Bundling | System.Web.Optimization | Bundler & minifier alternatives or static web assets / modern build tools |
| Testing | (Added) NUnitLite unit tests | xUnit/NUnit + WebApplicationFactory integration |

## Key Breaking Changes & Adjustments
1. Global Application Lifecycle
   - `Global.asax` removed. Startup logic goes into `Program.cs` (builder/services) and middleware pipeline.

2. Configuration
   - App settings from `Web.config` migrate to `appsettings.json` + `appsettings.Development.json`.
   - Connection strings move under `ConnectionStrings` section.

3. Dependency Injection
   - Controllers should use constructor injection instead of directly accessing static services or HttpContext-dependent singletons.

4. Filters & Attributes
   - Custom filters port to ASP.NET Core `IFilterMetadata` or specific interfaces (`IActionFilter`, etc.).

5. Bundling & Static Content
   - Replace `System.Web.Optimization` with one of:
     - Use no bundling initially (HTTP/2 mitigates request overhead).
     - Adopt a frontend build (e.g., Vite, Webpack) outputting versioned assets in `wwwroot`.

6. Razor Differences
   - HtmlHelpers mostly compatible; some namespaces change.
   - Anti-forgery tokens generated differently via tag helpers (`<form asp-action="Index">`).

7. Authentication / Authorization (if added later)
   - Forms Auth -> ASP.NET Core Identity / external providers.
   - `Authorize` attribute similar but lives in `Microsoft.AspNetCore.Authorization`.

8. Session & TempData
   - Must explicitly add session services & middleware.

9. Logging
   - Use built‑in `ILogger<T>` abstraction; remove any `System.Diagnostics` direct usage or adapt via providers.

10. Deployment
    - Azure App Service for Linux/Windows supports Core out-of-box; no IIS-specific configuration needed.

## Detailed Port Steps
### Step 1 – Inventory & Trim
- Inventory controllers: `HomeController` only currently.
- Views: `_Layout.cshtml`, `_ViewStart.cshtml`, `Home/Index|About|Contact`, `Shared/Error`.

### Step 2 – Create Core Structure (DONE)
- `CoreHost/CoreHostApp` created via `dotnet new mvc`.
- Confirm dotnet 8.0 SDK present.

### Step 3 – Port Views
- Copy legacy layout & partials into `CoreHostApp/Views/Shared` adjusting:
  - Remove `@Styles.Render` / `@Scripts.Render` helpers → replace with `<link>` / `<script>` tags referencing static files.
  - Move CSS/JS into `wwwroot` or keep under `wwwroot/lib/legacy`.

### Step 4 – Port Controller
- Copy code; update namespace to match Core project.
- Return types remain `IActionResult` (rename ActionResult generics if needed).

### Step 5 – Configure Routing & Services
- In `Program.cs`, ensure `builder.Services.AddControllersWithViews();` (already present).
- Add any filters (global) using `options.Filters.Add(new ...)` if needed.

### Step 6 – Static Assets
- Move `Content/` → `wwwroot/css`, `Scripts/` → `wwwroot/js`, `fonts/` → `wwwroot/fonts`.
- Update references in `_Layout.cshtml`.

### Step 7 – Tests
- Migrate NUnitLite unit tests to target Core project (multi-target, or create new `CoreHostApp.Tests`).
- Add integration tests with `WebApplicationFactory<Program>`.

### Step 8 – CI/CD
- Add GitHub Actions workflow:
  - Build both legacy and core projects during transition.
  - Run unit + integration tests.
  - Publish artifact of Core app only.

### Step 9 – Parity Validation
- Manual diff of routes & responses.
- Confirm view rendering & layout equivalence.

### Step 10 – Decommission Legacy
- Remove legacy project & Mono dependencies once confidence achieved.
- Update solution to only include Core & tests.

## Sample Mapping
| Legacy Path | Core Path |
|-------------|-----------|
| `Content/Site.css` | `wwwroot/css/site.css` |
| `Scripts/jquery-3.4.1.min.js` | `wwwroot/lib/jquery/dist/jquery.min.js` (or via CDN) |
| `Views/Shared/_Layout.cshtml` | `Views/Shared/_Layout.cshtml` (port, adjust helpers) |

## Minimal Port Example (HomeController)
```csharp
// In CoreHostApp/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;

namespace CoreHostApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }
    }
}
```

## Testing Strategy
- Keep legacy unit tests to lock existing behavior.
- Duplicate tests against Core port once controllers/views moved.
- Add integration tests verifying layout elements (branding text, nav links) and response headers.

## Risk & Mitigation
| Risk | Mitigation |
|------|------------|
| Behavioral drift in views | Snapshot tests / integration tests | 
| Missing bundling features | Leverage HTTP/2 + later add build pipeline |
| Unexpected Mono issues | Prioritize early Core parity to remove Mono dependency |

## Rollback Plan
- Keep `main` branch stable; perform migration on feature branch (`feature/core-port`).
- If blockers arise, continue serving legacy site while finishing port.

## Completion Criteria
- All controllers & views run under ASP.NET Core.
- Tests green (unit + integration).
- CI publishes only Core app.
- Legacy project removed from solution.

---
