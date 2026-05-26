# DMBServerWebHelper Project Map

## Purpose

Map the structure of `DMBServerWebHelper` so AI assistants can find the right files quickly.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`

## Root files

- `DMBServerWebHelper.csproj`: project file and package metadata.
- `README.md`: package overview and documentation entry point.
- `AGENTS.md`: local AI instructions.
- `AI_CONTEXT.md`: project context for AI assistants.
- `DOCUMENTATION_RULES.md`: XML and reference documentation rules.
- `EXAMPLES_AND_TUTORIALS_RULES.md`: website page, example, and tutorial rules.
- `DRAWIO_DIAGRAM_RULES.md`: editable Draw.io diagram rules.
- `DELIVERY_CHECKLIST.md`: pre-delivery checklist.
- `ARCHITECTURE_DECISIONS.md`: durable architecture decisions.
- `LOCALIZATION_NOMENCLATURE.md`: localization key rules.
- `LOCAL_DEVELOPMENT_RUNBOOK.md`: local workflow guide.
- `TROUBLESHOOTING.md`: common issue guide.
- `GLOSSARY.md`: common term definitions.
- `RequestCounter.cs`: lightweight request count utility.

## Configuration

Folder: `Configuration`

- `ServerWebHelperConfiguration.cs`: central ASP.NET Core service and middleware configuration helper.
- `ServerWebHelperConfigureOptions.cs`: static file options configuration for embedded package assets.
- `WebGenericConfiguration.cs`: generic web configuration composition for application-specific startup flows.

Use this folder for shared web application setup behavior.

## Controllers

Folder: `Controllers`

- `RawWebController.cs`: base MVC controller for shared web infrastructure conventions.

Use this folder only for reusable controller base types or controller-level infrastructure.

## Facades

Folder: `Facades`

- `IServerWebConfig.cs`: web configuration facade contract extending server configuration behavior.

Use this folder for public abstraction contracts.

## Middlewares

Folder: `Middlewares`

- `SessionGuardMiddleware.cs`: session-loading middleware.
- `SessionGuardExtensions.cs`: extension method for registering the session guard in the request pipeline.

Use this folder for ASP.NET Core middleware and registration helpers.

## Models

Folder: `Models`

- `CaptchaFactory.cs`: captcha image generation and validation helper.
- `CaptchaParameters.cs`: captcha rendering and behavior parameters.

Use this folder for state/configuration models and focused feature helpers that expose public package behavior.

## Resources

Folder: `Resources`

- `DMBServerWebHelperInternalLocalization.Designer.cs`: generated internal localization accessors.
- `DMBServerWebHelperDataAnnotationLocalization.Designer.cs`: generated data annotation localization accessors.

Do not edit generated designer files manually unless the generation workflow requires it.

## Tools

Folder: `Tools`

- `LanguageTools.cs`: language resolution helpers.
- `CountryWebTools.cs`: country code resolution helpers from web request signals.
- `CountryRegionInfoTools.cs`: country and region parsing helpers.
- `ColorHelper.cs`: color conversion helpers.
- `SecurityHashTools.cs`: security hash helpers.

Use this folder for small, deterministic utility APIs.

## Static assets

Folder: `wwwroot`

- `css`: embedded CSS assets.
- `fonts`: embedded font assets.
- `js`: embedded JavaScript assets.

Static assets are exposed through ASP.NET Core static file provider composition.

## Generated output

Folders such as `bin` and `obj` are generated output and should not be edited manually.

## Related projects

- `DMBServerHelper`: base server helper package.
- `DMBPageBuilder`: low-level page and HTML builder package.
- `DMBBootstrapBuilder`: Bootstrap-oriented visual builder package.
- `DMBComponentBuilder`: reusable visual component package.
- `DMBFormBuilder`: form builder package.
- `labs_idemobi_com`: publication host for examples, tutorials, information pages, and diagrams.
