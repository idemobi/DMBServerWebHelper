# DMBServerWebHelper Documentation Rules

## Language

- Documentation must be written in English.
- XML documentation comments must be written in English.

## Target audience

- Primary: developers maintaining or integrating `DMBServerWebHelper`.
- Secondary: developers building MVC/Razor applications with the PageBuilder ecosystem.
- Tertiary: AI assistants consuming structured project rules and technical context.

Documentation must be useful without private chat context. A reader should understand which ASP.NET Core behavior is configured, which middleware order matters, how session/localization/captcha/static assets are handled, and what constraints apply before reading the implementation.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Primary API families: ASP.NET Core configuration helpers, middleware helpers, MVC controller base types, session helpers, request localization helpers, captcha helpers, embedded static asset setup, and web utility tools.
- Important types to reference when relevant: `ServerWebHelperConfiguration`, `WebGenericConfiguration<T>`, `ServerWebHelperConfigureOptions`, `SessionGuardMiddleware`, `SessionGuardExtensions`, `RawWebController`, `CaptchaFactory`, `CaptchaParameters`, `LanguageTools`, `CountryWebTools`, `CountryRegionInfoTools`, `ColorHelper`, `SecurityHashTools`, `RequestCounter`, and `IServerWebConfig`.
- Publication host: `labs_idemobi_com`
- Documentation generation strategy: DocumentationBuilder-first; AI prepares content, the developer executes generation.

## Strict C# XML documentation policy

- Always write XML HeaderDoc for:
  - public classes,
  - public interfaces,
  - public structs,
  - public methods,
  - public constructors,
  - public properties,
  - public fields,
  - public constants,
  - public events,
  - public delegates,
  - public enums,
  - public enum values,
  - public extension methods.
- Also write XML HeaderDoc for protected members when they are part of the inheritance contract or are expected to be overridden by derived applications.
- Internal and private members do not require XML HeaderDoc unless they explain complex configuration, security, rendering, or middleware behavior that would otherwise be difficult to maintain.
- XML documentation must use valid C# XML syntax.
- Prefer these tags:
  - `<summary>`
  - `<param>`
  - `<typeparam>`
  - `<returns>`
  - `<value>`
  - `<remarks>`
  - `<exception>`
  - `<see cref="..."/>`
  - `<seealso cref="..."/>`
- Use `<inheritdoc/>` only when the inherited documentation is accurate for the current member. Do not hide different behavior behind inherited text.

## XML documentation quality standard

XML documentation must explain the public contract, not repeat the member name.

For classes and interfaces, document:

- the class role in ASP.NET Core application composition,
- the service, middleware, controller, helper, or utility responsibility,
- the relationship with important types such as `ServerWebHelperConfiguration`, `WebGenericConfiguration<T>`, `CaptchaFactory`, `SessionGuardMiddleware`, or `IServerWebConfig`,
- lifecycle expectations, including whether the type is used directly, through application startup, by middleware, or by MVC infrastructure.

For methods and constructors, document:

- what the member registers, configures, resolves, generates, validates, or returns,
- every parameter and the expected format when relevant,
- returned values and fallback behavior,
- side effects such as registering services, changing middleware order, loading session, adding file providers, writing captcha session values, or reading request headers,
- validation rules and exceptions,
- whether `null`, empty strings, invalid formats, duplicate calls, or repeated calls have special behavior.

For properties, fields, and constants, document:

- the meaning of the value,
- the default value when meaningful,
- whether consumers may set it directly,
- how it affects request handling, cookies, sessions, localization, captcha output, static assets, or security behavior.

For enums and enum values, document:

- where the enum is used,
- how each value maps to behavior, configuration, request handling, output, or fallback behavior.

For extension methods, document:

- the receiver type,
- the middleware, service, or helper behavior added,
- the intended ASP.NET Core usage pattern,
- ordering requirements when relevant.

## Project API documentation requirements

- Configuration APIs must identify which ASP.NET Core services, options, or middleware they configure.
- Middleware APIs must document request pipeline ordering assumptions and side effects.
- Session-related APIs must document session loading, keys, expiration behavior, and unavailable-session behavior when relevant.
- Cookie-related APIs must document cookie names, purpose, security expectations, and compatibility concerns.
- Localization APIs must document culture sources, fallback behavior, request header usage, and resource expectations.
- Captcha APIs must document image generation, session storage, validation comparison behavior, expiration assumptions, and user input handling.
- Embedded static asset APIs must document assembly provider usage, request paths, and composition with existing `StaticFileOptions`.
- Utility APIs must document accepted input formats, returned values, fallback values, and exceptions.
- Security-sensitive APIs must mention risks related to user-controlled values, session state, cookies, hashing, captcha validation, or request headers.

## Examples in XML documentation

Use `<example>` when it materially improves understanding of:

- application startup configuration,
- middleware ordering,
- captcha generation and validation,
- request localization setup,
- embedded static file registration,
- utility input and output formats.

Examples must be short, realistic, and compile-oriented. Prefer C# examples for startup, middleware, helper, and utility APIs.

## Markdown documentation policy

- Follow PageBuilder markdown conventions in:
  - `../MARKDOWN_GUIDELINES.md`
- Keep this structure where applicable:
  1. Context
  2. Explanation
  3. Example
  4. Notes / constraints

## Draw.io diagrams for conceptual documentation

Information pages, instruction pages, concept pages, architecture pages, and request pipeline pages may use Draw.io diagrams when they clarify a real model or flow.

Draw.io diagrams must follow:

- `DRAWIO_DIAGRAM_RULES.md`

Required baseline:

- save diagrams as enriched `.drawio.svg` files that remain editable in Draw.io,
- store diagrams under `labs_idemobi_com/wwwroot/drawio/{Area}/{diagram-name}.drawio.svg`,
- align shapes and connectors to the Draw.io grid,
- keep diagrams compatible with both light and dark page themes,
- include meaningful alternative text and surrounding explanatory text when rendered in a page,
- start from `labs_idemobi_com/wwwroot/drawio/_templates/concept-flow-template.drawio.svg` when a simple concept-flow template is appropriate.

Do not use Draw.io diagrams in XML documentation comments. XML documentation may reference concepts that are diagrammed on pages, but the diagram artifact belongs to the website documentation layer.

## DocumentationBuilder-first rule

Documentation in this module must be authored with a DocumentationBuilder-first objective.

- Write docs so they can be extracted and rendered without manual rewrite.
- Keep headings deterministic and stable.
- Keep examples self-contained and realistically useful.
- Avoid implicit references to chat history or hidden context.
- Prefer stable type and member names that DocumentationBuilder can cross-reference.
- Use `<see cref="..."/>` and `<seealso cref="..."/>` for related PageBuilder types whenever it improves navigation.

## Separation from examples and tutorials

`EXAMPLES_AND_TUTORIALS_RULES.md` is not a general documentation rule source.

- Use this file for API documentation, XML HeaderDoc, README updates, reference pages, and DocumentationBuilder-ready documentation.
- Use `../MARKDOWN_GUIDELINES.md` for general Markdown formatting rules.
- Use `EXAMPLES_AND_TUTORIALS_RULES.md` only when the task explicitly creates or updates example pages, demo pages, information pages, instruction pages, concept pages, tutorials, or tutorial-like walkthroughs.
- Do not import example-page requirements into XML documentation or reference documentation unless the task also changes examples or tutorials.

### Target publication project

- `../labs_idemobi_com` from PageBuilder root.

### Execution responsibility

- AI prepares documentation content, structure, and metadata.
- The developer executes DocumentationBuilder.
- AI must not claim DocumentationBuilder execution unless it was actually run.

## Minimum update policy

If public configuration behavior, middleware behavior, localization behavior, captcha behavior, embedded asset behavior, or utility behavior changes, update in the same change set:

- local `README.md`,
- relevant XML docs,
- impacted guidance/examples when the task includes pages.

If a new user-facing example, information page, or tutorial is added, apply `EXAMPLES_AND_TUTORIALS_RULES.md` to that page work.

## Review checklist for documentation changes

- The documentation names the real ServerWebHelper concept, not a copied source project concept.
- All public and protected-contract API members touched by the change have valid XML documentation.
- Summaries are specific enough to help IntelliSense users choose the right API.
- Parameters, return values, generic parameters, exceptions, and side effects are documented where applicable.
- Examples reflect current code behavior and realistic ASP.NET Core usage.
- Draw.io diagrams, when added, follow `DRAWIO_DIAGRAM_RULES.md`.
- DocumentationBuilder can extract the content without needing hidden context or manual rewrite.
