# DMBServerWebHelper Troubleshooting

## Purpose

Collect common issues and investigation paths for `DMBServerWebHelper`.

## Project-specific section

When copying this file to another PageBuilder ecosystem project, update this section first.

- Project name: `DMBServerWebHelper`
- Project folder: `DMBServerWebHelper`
- Project role: ASP.NET web helper package for server-side PageBuilder applications.
- Main dependency: `DMBServerHelper`
- Publication host: `labs_idemobi_com`

## Session is unavailable

Check:

- session services are registered before middleware usage,
- `UseSession()` runs before code that reads or writes session,
- `UseSessionGuard()` is registered in the intended pipeline order,
- cookies required for session are accepted by the browser,
- the application is not reading session after the response has started.

Document any pipeline order change in XML docs and guidance pages.

## Captcha validation fails

Check:

- the captcha value was generated before validation,
- the expected captcha value is stored in session,
- the same session is used between image generation and form submission,
- user input normalization matches the implementation,
- session expiration or cookie rejection did not clear the stored value,
- captcha parameters are valid for image generation.

Treat captcha behavior as security-sensitive. Avoid weakening validation without explicit approval.

## Captcha image is blank or malformed

Check:

- `CaptchaParameters` size, font, noise, and color values,
- SkiaSharp native assets are available for the target runtime,
- image encoding succeeds,
- generated base64 output is rendered with the correct data URI prefix when used in a page.

## Request culture is wrong

Check:

- supported cultures and default culture are configured,
- request localization middleware is registered before culture-dependent rendering,
- Accept-Language headers contain expected values,
- cookie or query-string culture providers are not overriding the expected value,
- resource keys exist in the expected resource family.

## Embedded static assets are missing

Check:

- assets exist under `wwwroot`,
- assets are embedded by the project configuration,
- `ServerWebHelperConfigureOptions` is applied,
- the composite file provider preserves the host application's existing static file provider,
- the request path matches the published asset path.

## Cookie behavior is unexpected

Check:

- cookie names and options configured by `ServerWebHelperConfiguration`,
- cookie policy middleware order,
- SameSite, Secure, HttpOnly, and consent behavior,
- browser consent or privacy settings,
- application-specific overrides.

## MVC or validation localization does not apply

Check:

- MVC services are registered through the expected configuration path,
- data annotation localization resources exist,
- `WebLocalizer` or equivalent localization services are registered,
- validation adapter provider configuration was not replaced by application code.

## Request counter does not change

Check:

- the request counter increment is registered in the application pipeline,
- middleware ordering allows it to run for the requested paths,
- static file requests may bypass application endpoints depending on pipeline order.

## Documentation page issues

When pages in `labs_idemobi_com` are wrong or inconsistent:

- read `EXAMPLES_AND_TUTORIALS_RULES.md`,
- use `CodeBlockBuilder` or `Html.CodeBlock(...)` for code examples,
- use `ActionItem` with `ButtonRender` for action links,
- use `DRAWIO_DIAGRAM_RULES.md` for editable diagrams,
- keep DocumentationViewer links targeting `DMBServerWebHelper`.
