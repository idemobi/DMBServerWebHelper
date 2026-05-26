# DMBServerWebHelper AI Context

## Purpose

This file gives AI assistants the minimum project context required to work safely in `DMBServerWebHelper`.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`
- Primary documentation audience: maintainers of MVC/Razor applications using the PageBuilder ecosystem.

## What this project is

`DMBServerWebHelper` is a shared ASP.NET Core infrastructure package.

It provides:

- MVC and web application configuration helpers,
- middleware setup helpers,
- request localization setup,
- session guard middleware,
- embedded static file provider registration,
- captcha generation and validation,
- small web utility helpers for language, country, color, and hashing behavior.

## What this project is not

This project is not:

- a visual component library,
- a form builder,
- a page layout builder,
- a DocumentationViewer implementation,
- an application-specific website.

Visual examples and tutorial pages belong in `labs_idemobi_com` when requested.

## Main concepts

- `ServerWebHelperConfiguration` coordinates web service registration and middleware usage.
- `WebGenericConfiguration<T>` supports application-specific configuration composition.
- `ServerWebHelperConfigureOptions` injects embedded static files into ASP.NET static file options.
- `SessionGuardMiddleware` ensures session state is loaded before downstream middleware or endpoints need it.
- `CaptchaFactory` generates and validates captcha images backed by session storage.
- Tool classes provide small focused helpers for language, country, color, and security hashing operations.

## Change strategy

- Keep changes localized to the relevant feature family.
- Preserve public API names and behavior unless the request explicitly asks for a breaking change.
- Document public API behavior in XML comments when the code is touched.
- Update README and local rule files when project behavior or documentation strategy changes.

## Documentation strategy

- Use `DOCUMENTATION_RULES.md` for XML docs, README/reference docs, and DocumentationBuilder-ready documentation.
- Use `EXAMPLES_AND_TUTORIALS_RULES.md` only for pages, examples, tutorials, and walkthroughs.
- Use `DRAWIO_DIAGRAM_RULES.md` when diagrams clarify middleware, configuration, captcha, localization, or static asset flows.
- Keep all generated documentation in English unless the user explicitly requests another language for user-facing website content.
