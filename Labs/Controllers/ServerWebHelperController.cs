#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBBootstrapBuilder;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace DMBServerWebHelperLabs.Controllers
{
    /// <summary>
    ///     Provides documentation pages for DMBServerWebHelper.
    /// </summary>
    public class ServerWebHelperController : RawBootstrapController
    {
        #region Instance methods

        /// <summary>
        ///     Renders the DMBServerWebHelper architecture page.
        /// </summary>
        /// <returns>The architecture view.</returns>
        public IActionResult Architecture()
        {
            SetTitle("DMBServerWebHelper - Architecture");
            SetDescription("DMBServerWebHelper architecture");
            SetKeywords("DMBServerWebHelper", "ServerWebHelper", "Architecture", "ASP.NET Core");
            return View();
        }

        /// <summary>
        ///     Renders the DMBServerWebHelper examples page.
        /// </summary>
        /// <returns>The examples view.</returns>
        public IActionResult Examples()
        {
            SetTitle("DMBServerWebHelper - Examples");
            SetDescription("DMBServerWebHelper examples for language, localization, request state, and web helpers");
            SetKeywords("DMBServerWebHelper", "ServerWebHelper", "Examples", "Language", "Localization", "Session", "Request");
            return View();
        }

        /// <summary>
        ///     Renders the DMBServerWebHelper getting started page.
        /// </summary>
        /// <returns>The getting started view.</returns>
        public IActionResult GettingStarted()
        {
            SetTitle("DMBServerWebHelper - Getting Started");
            SetDescription("DMBServerWebHelper getting started guide");
            SetKeywords("DMBServerWebHelper", "ServerWebHelper", "Getting Started", "ASP.NET Core");
            return View();
        }

        /// <summary>
        ///     Renders the DMBServerWebHelper introduction page.
        /// </summary>
        /// <returns>The introduction view.</returns>
        public IActionResult Introduction()
        {
            SetTitle("DMBServerWebHelper - Introduction");
            SetDescription("DMBServerWebHelper overview");
            SetKeywords("DMBServerWebHelper", "ServerWebHelper", "Foundation", "ASP.NET Core");
            return View();
        }

        #endregion
    }
}