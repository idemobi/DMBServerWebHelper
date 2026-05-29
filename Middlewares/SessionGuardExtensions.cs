#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

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
        ///     Adds <see cref="SessionGuardMiddleware" /> to the ASP.NET Core request pipeline.
        /// </summary>
        /// <param name="app">
        ///     The application builder receiving the session guard middleware.
        /// </param>
        /// <returns>
        ///     The same <see cref="IApplicationBuilder" /> instance so additional middleware can be chained.
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