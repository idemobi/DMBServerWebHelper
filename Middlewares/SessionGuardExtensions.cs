#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// DMBServerWebHelper.csproj SessionGuardExtensions.cs create at 2026/04/07 21:04:27
// ©2024-2026 idéMobi SARL FRANCE

#endregion

#region

using Microsoft.AspNetCore.Builder;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides middleware registration helpers for the session guard.
    /// </summary>
    public static class SessionGuardExtensions
    {
        #region Static methods

        /// <summary>
        ///     Adds <see cref="SessionGuardMiddleware"/> to the ASP.NET Core request pipeline.
        /// </summary>
        /// <param name="app">
        ///     The application builder receiving the session guard middleware.
        /// </param>
        /// <returns>
        ///     The same <see cref="IApplicationBuilder"/> instance so additional middleware can be chained.
        /// </returns>
        /// <remarks>
        ///     Call this method after <c>UseSession()</c> so the session feature exists before the guard attempts to load it.
        /// </remarks>
        public static IApplicationBuilder UseSessionGuard(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SessionGuardMiddleware>();
        }

        #endregion
    }
}
