#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.IO;
using DMBBootstrapBuilder;
using DMBServerWebHelperLabs.Navigation;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace DMBServerWebHelperWebsite;

internal sealed class DMBServerWebHelperWebsiteMenuBarSectionProvider : IMenuBarSectionProvider
{
    #region Instance fields and properties

    #region From interface IMenuBarSectionProvider

    /// <summary>
    ///     Gets the provider display order in the local website navbar.
    /// </summary>
    public int Order => 100;

    #endregion

    #endregion

    #region Instance methods

    #region From interface IMenuBarSectionProvider

    /// <summary>
    ///     Builds the local website menu items for DMBServerWebHelper labs pages.
    /// </summary>
    /// <param name="writer">The current HTML writer.</param>
    /// <param name="html">The current HTML helper.</param>
    /// <returns>The menu module result containing the DMBServerWebHelper menu group.</returns>
    public MenuBarModuleResult Build(TextWriter writer, IHtmlHelper html)
    {
        MenuBarModuleResult result = new();

        result.ActionList.Add(DMBServerWebHelperLabsNavigationAgent.CreateMenuGroup());

        return result;
    }

    /// <summary>
    ///     Determines whether the provider is enabled for the current request.
    /// </summary>
    /// <param name="html">The current HTML helper.</param>
    /// <returns><c>true</c> because the local website always displays the module menu.</returns>
    public bool IsEnabled(IHtmlHelper html)
    {
        return true;
    }

    #endregion

    #endregion
}
