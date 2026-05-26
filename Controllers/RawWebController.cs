#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// DMBServerWebHelper.csproj RawWebController.cs create at 2026/04/07 21:04:27
// ©2024-2026 idéMobi SARL FRANCE

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
    ///     conventions while still using the standard ASP.NET Core <see cref="Controller"/> behavior.
    /// </remarks>
    public abstract class RawWebController : Controller
    {
    }
}
