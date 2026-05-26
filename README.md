# DMBServerWebHelper

## Purpose

`DMBServerWebHelper` provides ASP.NET-oriented web infrastructure helpers for the PageBuilder ecosystem.

It centralizes common server-side web behavior such as MVC setup, session handling, request localization, embedded static assets, cookie policy integration, captcha generation, and small web utility APIs.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Primary consumers: MVC/Razor applications such as `labs_idemobi_com`.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`

## Scope

This package includes:

- application configuration helpers for ASP.NET Core services and middleware,
- session guard middleware,
- request localization helpers,
- embedded static file registration,
- captcha image generation and validation helpers,
- country, language, color, and security hash utility methods,
- a lightweight request counter,
- base MVC controller support through `RawWebController`.

This package does not define visual Bootstrap components, form builders, or page layout builders. Those responsibilities belong to the related PageBuilder ecosystem packages.

## Main entry points

- `ServerWebHelperConfiguration`
- `WebGenericConfiguration<T>`
- `ServerWebHelperConfigureOptions`
- `SessionGuardMiddleware`
- `SessionGuardExtensions`
- `CaptchaFactory`
- `CaptchaParameters`
- `LanguageTools`
- `CountryWebTools`
- `CountryRegionInfoTools`
- `ColorHelper`
- `SecurityHashTools`
- `RequestCounter`
- `RawWebController`
- `IServerWebConfig`

## Documentation strategy

Documentation must be written so it can be consumed by developers and AI assistants without private chat context.

Use the local rule files:

- [AGENTS.md](AGENTS.md)
- [AI_CONTEXT.md](AI_CONTEXT.md)
- [DOCUMENTATION_RULES.md](DOCUMENTATION_RULES.md)
- [EXAMPLES_AND_TUTORIALS_RULES.md](EXAMPLES_AND_TUTORIALS_RULES.md)
- [DRAWIO_DIAGRAM_RULES.md](DRAWIO_DIAGRAM_RULES.md)
- [PROJECT_MAP.md](PROJECT_MAP.md)
- [LOCALIZATION_NOMENCLATURE.md](LOCALIZATION_NOMENCLATURE.md)
- [DELIVERY_CHECKLIST.md](DELIVERY_CHECKLIST.md)

Documentation pages, examples, tutorials, and diagrams are published through `labs_idemobi_com` when applicable.

## Development constraints

- Keep public APIs backward compatible unless explicitly requested.
- Keep middleware ordering and service registration behavior explicit.
- Document security-sensitive behavior such as cookies, sessions, captcha validation, and user-controlled values.
- Do not run `dotnet build`, `dotnet test`, `dotnet restore`, or `dotnet format` unless explicitly requested.
