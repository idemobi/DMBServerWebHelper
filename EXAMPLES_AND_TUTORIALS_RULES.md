# DMBServerWebHelper Examples and Tutorials Rules

## Objective

Define how information pages, instruction pages, concept pages, example pages, demo pages, and tutorials are created for `DMBServerWebHelper`.

These rules apply only when the task explicitly creates or updates:

- information pages,
- instruction pages,
- concept pages,
- example pages,
- demo pages,
- tutorial pages,
- walkthrough pages,
- example partials rendered through `RenderExamplePartialAsync`.

Do not use this file as the rule source for XML API documentation or reference documentation. Use `DOCUMENTATION_RULES.md` for that work.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Default documentation area: `ServerWebHelper`
- Publication target: `../labs_idemobi_com`
- Shared UI stack: `DMBBootstrapBuilder`, `DMBComponentBuilder`, `DMBFormBuilder`, and `DMBPageBuilder`
- Default DocumentationViewer package id for this project: `DMBServerWebHelper`
- Default DocumentationViewer namespace for this project: `DMBServerWebHelper`
- Expected page emphasis: web infrastructure concepts, request pipeline behavior, configuration, session, captcha, localization, cookies, and embedded static assets.

## Publication target

Examples and tutorials must be written in `../labs_idemobi_com`.

Use the existing MVC conventions in that project:

- controller actions in `labs_idemobi_com/Controllers`,
- full pages in `labs_idemobi_com/Views/{FeatureOrComponent}/`,
- reusable example partials in `labs_idemobi_com/Views/Shared/Examples/`,
- generated raw-code mirrors in `labs_idemobi_com/Views/Shared/Examples_Raw/`.

AI may create or update source example partials under `Views/Shared/Examples/`. The developer or prebuild step is responsible for regenerating `Views/Shared/Examples_Raw/` when required.

## Shared UI stack

Example, information, concept, and tutorial pages must use the existing PageBuilder ecosystem components instead of ad-hoc layout markup when a suitable component exists.

Prefer:

- `DMBBootstrapBuilder` for layout, titles, cards, rows, columns, alerts, badges, buttons, tables, tabs, and Bootstrap-oriented UI.
- `DMBComponentBuilder` for reusable non-form visual components available in the project.
- `DMBFormBuilder` for forms, fields, validation examples, and form-state demonstrations.
- `DMBPageBuilder` for page metadata, raw HTML builders, render lifecycle concepts, and low-level HTML composition.

Do not introduce a new frontend framework or independent demo system for examples.

## Page categories

There are two distinct page formats:

- general information pages,
- component-like example pages.

Do not merge the two formats unless the user explicitly asks for a hybrid page.

`DMBServerWebHelper` is primarily a web infrastructure package. Most pages should be general information, instruction, or concept pages. Use the component-like format only when a page is dedicated to one public class or one tightly scoped API family and benefits from multiple rendered examples.

## General information page format

Use this format for package introductions, architecture pages, request pipeline pages, getting started pages, configuration guides, middleware guides, localization guides, captcha guides, and tutorials that are not focused on one component class.

Required structure:

1. Title.
2. Short context paragraph explaining the topic and audience.
3. Explanation sections with deterministic headings.
4. Practical integration or usage section when relevant.
5. Notes, constraints, or next steps.
6. Links to related documentation pages or API reference when useful.

General information pages:

- may include short code snippets rendered with `CodeBlockBuilder`,
- may include diagrams or structured lists when they clarify architecture, instructions, request pipeline behavior, or concepts,
- should not include component galleries unless the page is explicitly an overview gallery,
- should avoid long inline API listings that belong in DocumentationViewer.

## Draw.io diagrams on information and tutorial pages

Information pages, instruction pages, concept pages, architecture pages, request pipeline pages, and tutorials may include Draw.io diagrams when a visual model clarifies the explanation.

Draw.io diagrams must follow `DRAWIO_DIAGRAM_RULES.md`.

In particular:

- use enriched `.drawio.svg` files that remain editable in Draw.io,
- store diagrams under `labs_idemobi_com/wwwroot/drawio/{Area}/{diagram-name}.drawio.svg`,
- keep geometry aligned to the Draw.io grid,
- keep diagrams compatible with light and dark page themes,
- include meaningful alternative text when rendering the diagram in a page,
- use `labs_idemobi_com/wwwroot/drawio/_templates/concept-flow-template.drawio.svg` as the starting template when useful.

Do not use Draw.io diagrams as decoration. Add a diagram only when it explains a real configuration flow, request pipeline, middleware order, captcha lifecycle, localization flow, static asset flow, or security concept.

## Code examples on information pages

Code examples on general information pages must use `CodeBlockBuilder` or the existing `Html.CodeBlock(...)` helper when available.

Do not write raw `<pre><code>` blocks by hand when `CodeBlockBuilder` can render the example.

Code examples must:

- specify the language,
- have a useful title when the page contains more than one snippet,
- enable copy behavior when that is consistent with existing pages,
- stay focused on the concept explained by the page.

## Links and actions on information pages

Links that behave like page actions must be generated through `ActionItem` implementations and rendered with `ButtonRender` when possible.

Prefer:

- `AspRouteActionItem` or `ActionItemFactory.AspRoute(...)` for MVC route links,
- `UrlActionItem` for external or absolute URLs,
- `ButtonRender` to render action buttons consistently with BootstrapBuilder.

Use plain inline anchors only for natural text links inside paragraphs where a button/action would be visually inappropriate.

## Component-like page format

Use this format for a page dedicated to one public class or one tightly scoped API family that needs multiple examples.

Every component-like page must follow the same high-level order:

1. Title.
2. Explanation of the API or API family.
3. DocumentationViewer button linking to the relevant API documentation.
4. Gallery of examples rendered with `@await Html.RenderExamplePartialAsync(...)` when examples are useful.
5. Showcase list or usage matrix.

### Title

Use `TitleBuilder` for the main title.

The title should name the class or API family clearly. Prefer the public class name when the page is class-specific, for example `CaptchaFactory`, `SessionGuardMiddleware`, or `ServerWebHelperConfiguration`.

### API explanation

The explanation must be short and practical.

It should cover:

- what the API configures, generates, validates, or resolves,
- the primary use case,
- the most important configuration points,
- any important security, lifecycle, session, localization, or middleware-order concern.

Do not duplicate the full API reference on the page.

### DocumentationViewer button

Every component-like page must include a visible button linking to the generated API documentation.

Use the existing DocumentationViewer route through the `Documentation` controller:

```csharp
@Url.Action("Show", "Documentation", new
{
    groupName = "NuGet",
    packageId = "DMBServerWebHelper",
    version = "0.9",
    namespaceName = "DMBServerWebHelper",
    objectName = "CaptchaFactory"
})
```

Adjust `packageId`, `namespaceName`, and `objectName` to the API being documented.

When rendering the DocumentationViewer button, prefer an `ActionItem` implementation rendered through `ButtonRender` instead of a handwritten `<a class="btn ...">` element.

### Example gallery

The gallery must render example partials through:

```csharp
@await Html.RenderExamplePartialAsync("Examples/{ApiOrClassName}/{ExamplePartialName}", "Readable example title", model: Model)
```

The source partials must live under:

```text
labs_idemobi_com/Views/Shared/Examples/{ApiOrClassName}/{ExamplePartialName}.cshtml
```

The generated raw-code mirrors are expected under:

```text
labs_idemobi_com/Views/Shared/Examples_Raw/{ApiOrClassName}/{ExamplePartialName}_Raw.cshtml
```

Use the public class or API family name as the folder name for new example pages. For example:

- `Views/Shared/Examples/CaptchaFactory/_S001GenerateCaptcha.cshtml`
- `Views/Shared/Examples/CaptchaFactory/_S002ValidateCaptcha.cshtml`
- `Views/Shared/Examples/SessionGuardMiddleware/_S001MiddlewareOrder.cshtml`
- `Views/Shared/Examples/ServerWebHelperConfiguration/_S001ConfigureServices.cshtml`

## Minimum example coverage

When a class or API family gets a dedicated example gallery, include at least:

- normal usage,
- missing or empty input behavior when applicable,
- invalid or error behavior when applicable,
- one realistic application integration example.

Add more examples when the API has important security, localization, session, middleware order, or static asset behavior.

## Showcase list

After the gallery, include a showcase list that demonstrates the API surface beyond the basic examples.

The showcase may be:

- a configuration matrix,
- a request lifecycle list,
- a middleware order table,
- a localization fallback table,
- a security considerations list,
- a static asset path mapping table.

The showcase must remain readable and should not replace the example gallery. The gallery shows copyable usage; the showcase helps compare available states or options.

## Tutorial expectations

Use tutorials for step-by-step workflows, not for simple API reference pages.

Tutorials must:

- explain prerequisites and assumptions,
- provide step-by-step usage,
- state the expected result for each step,
- include common mistakes and recovery guidance,
- link to the relevant concept pages or DocumentationViewer API pages.

## Naming and maintainability

- Keep page, controller, and folder names aligned with the public class or feature name.
- Prefer clear names over abbreviations.
- Keep examples self-contained and realistic.
- Keep example partials small enough to be readable in the rendered code panel.
- Keep repeated page structure consistent so future server web helper pages can be added mechanically.
- Do not leave placeholder pages such as `NEED TO SHOW EXAMPLES`.

## Delivery checklist for examples

Before finishing an example or tutorial task, verify:

- the page is under `labs_idemobi_com`,
- the page uses existing BootstrapBuilder, ComponentBuilder, FormBuilder, or PageBuilder components where appropriate,
- code examples use `CodeBlockBuilder` or `Html.CodeBlock(...)`,
- action links use `ActionItem` with `ButtonRender` when appropriate,
- Draw.io diagrams follow `DRAWIO_DIAGRAM_RULES.md`,
- DocumentationViewer links target `DMBServerWebHelper`,
- no build/test command was run unless the user explicitly requested it.
