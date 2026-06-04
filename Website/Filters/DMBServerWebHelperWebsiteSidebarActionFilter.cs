#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBBootstrapBuilder;
using DMBPageBuilder;
using DMBServerWebHelperLabs.Navigation;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace DMBServerWebHelperWebsite;

internal sealed class DMBServerWebHelperWebsiteSidebarActionFilter : IActionFilter
{
    #region Instance methods

    #region From interface IActionFilter

    /// <summary>
    ///     Completes the action filter lifecycle after the action has executed.
    /// </summary>
    /// <param name="context">The current action executed context.</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    /// <summary>
    ///     Injects the DMBServerWebHelper labs sidebar and breadcrumb for local website pages.
    /// </summary>
    /// <param name="context">The current action execution context.</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not RawBootstrapController controller)
        {
            return;
        }

        string? currentController = context.RouteData.Values["controller"]?.ToString();
        string? currentAction = context.RouteData.Values["action"]?.ToString();

        if (!string.Equals(currentController, "ServerWebHelper", System.StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        controller.SetSidebar(DMBServerWebHelperLabsNavigationAgent.CreateSidebar(currentController, currentAction));
        controller.AddBreadcrumb(
            ActionItemFactory.Url("Home", "/", IconStruct.Bootstrap("bi-house")),
            ActionItemFactory.AspRoute("ServerWebHelper", "Introduction")
                .SetTitle("DMBServerWebHelper")
                .SetIcon(IconStruct.Bootstrap("bi-globe2")),
            ActionItemFactory.AspRoute("ServerWebHelper", string.IsNullOrWhiteSpace(currentAction) ? "Introduction" : currentAction)
                .SetTitle(DMBServerWebHelperLabsNavigationAgent.ResolveActionTitle(currentAction))
                .SetIcon(DMBServerWebHelperLabsNavigationAgent.ResolveActionIcon(currentAction))
        );
    }

    #endregion

    #endregion
}
