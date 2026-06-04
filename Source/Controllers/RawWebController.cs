#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace DMBServerWebHelper.Controllers
{
    /// <summary>
    ///     Provides the base MVC controller type for raw web controllers in the server web helper layer.
    /// </summary>
    /// <remarks>
    ///     Derive from this type when a controller should participate in the shared server web
    ///     conventions while still using the standard ASP.NET Core <see cref="Controller" /> behavior.
    /// </remarks>
    public abstract class RawWebController : Controller
    {
    }
}