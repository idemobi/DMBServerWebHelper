# DMBServerWebHelper Architecture Decisions

## Purpose

Record durable architecture decisions that AI assistants and maintainers must preserve unless a change request explicitly supersedes them.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`

## Decisions

### Keep ASP.NET infrastructure centralized

`ServerWebHelperConfiguration` and `WebGenericConfiguration<T>` are the main places for reusable ASP.NET Core service and middleware setup.

New shared server-side web setup should be added there only when it is broadly useful to PageBuilder ecosystem applications.

### Keep middleware order explicit

Middleware registration is behavior, not decoration.

Changes to `UseApp(...)`, session handling, localization, exception handling, status code pages, or static file registration must document the intended order and any ordering assumptions.

### Keep session usage deliberate

Session is used by features such as captcha validation and session guard behavior.

Do not add new session dependencies without documenting:

- when the session is loaded,
- which keys are used,
- how failures are handled,
- what happens when session is unavailable.

### Treat cookies and localization as public behavior

Cookie names, request culture resolution, and localization setup affect application behavior and user experience.

Changing defaults requires documentation updates and compatibility review.

### Keep embedded asset registration reusable

Embedded `wwwroot` assets are exposed through ASP.NET static file options.

Do not replace this with application-specific paths unless explicitly requested. Prefer extending the existing provider composition behavior.

### Keep utility helpers focused

Tool classes such as `LanguageTools`, `CountryWebTools`, `ColorHelper`, and `SecurityHashTools` should remain small and deterministic.

Avoid turning them into service containers or application-specific policy engines.

### Keep examples outside the package

Example pages, tutorials, diagrams, and explanatory pages are published through `labs_idemobi_com` when requested.

The package should not embed documentation website pages directly.
