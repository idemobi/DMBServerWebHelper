# DMBServerWebHelper Glossary

## Purpose

Define common terms used in `DMBServerWebHelper` documentation and AI instructions.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`

## Terms

### Server web helper

The package-level concept for reusable ASP.NET Core web infrastructure shared by PageBuilder ecosystem applications.

### Server web configuration

Startup and runtime configuration coordinated by `ServerWebHelperConfiguration`, including services, middleware, request localization, cookies, session, MVC, and static assets.

### Generic web configuration

Application-specific configuration composition through `WebGenericConfiguration<T>`.

### Session guard

Middleware behavior that ensures session state is loaded before downstream application code requires it.

### Captcha factory

The `CaptchaFactory` API that generates captcha images and validates user-entered captcha values with session-backed state.

### Captcha parameters

The `CaptchaParameters` API that controls captcha rendering settings such as size, noise, distortion, fonts, and colors.

### Request localization

ASP.NET Core culture resolution for requests, including supported cultures, default culture, and request language signals.

### Embedded static assets

Files embedded in the package and exposed through ASP.NET Core static file providers.

### Raw web controller

The `RawWebController` base controller for MVC controllers that need the shared server web infrastructure conventions.

### Web localizer

The localization abstraction used by the PageBuilder ecosystem to resolve localized text in web contexts.

### Request counter

The lightweight `RequestCounter` utility that tracks total request counts when the application pipeline increments it.

### DocumentationViewer

The documentation browsing feature in `labs_idemobi_com` that displays generated API documentation for NuGet packages.

### DocumentationBuilder

The documentation generation process that extracts and renders API documentation. AI prepares content; the developer executes the generator.
