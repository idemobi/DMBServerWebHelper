# DMBServerWebHelper Delivery Checklist

## Purpose

Use this checklist before finishing changes in `DMBServerWebHelper`.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`

## Code checklist

- Public API compatibility was preserved, or the breaking change was explicitly requested.
- New or changed public members have useful English XML documentation.
- Middleware order changes are intentional and documented.
- Service registration changes are additive or clearly justified.
- Session usage is explicit and failure behavior is documented.
- Cookie, antiforgery, localization, and captcha behavior were reviewed as security-sensitive.
- Embedded static asset behavior still uses the established provider composition pattern.
- No unrelated files were reformatted or refactored.

## Documentation checklist

- README was updated when project behavior or usage changed.
- `DOCUMENTATION_RULES.md` was followed for XML docs and reference documentation.
- `EXAMPLES_AND_TUTORIALS_RULES.md` was used only for example, demo, information, instruction, concept, or tutorial pages.
- `DRAWIO_DIAGRAM_RULES.md` was followed when diagrams were added or updated.
- Documentation names `DMBServerWebHelper` concepts, not copied source-project concepts.
- Documentation is written in English unless the task explicitly requested another language for website content.

## Localization checklist

- Localization keys follow `LOCALIZATION_NOMENCLATURE.md`.
- Existing key families were reused where possible.
- Resource changes were kept consistent with generated designer files.

## Verification checklist

- Do not run `dotnet build`, `dotnet test`, `dotnet restore`, or `dotnet format` unless explicitly requested.
- If no build or tests were run, say so in the final response.
- If only text checks were run, name those checks precisely.
- Mention any remaining risks or manual validation needs.

## Final response checklist

- Summarize changed files.
- Mention that build/test were not run unless explicitly requested and actually executed.
- List follow-up items only when they are useful and specific.
