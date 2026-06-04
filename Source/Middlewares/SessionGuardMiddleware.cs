#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Middleware that ensures ASP.NET Core session state is loaded before the request continues.
    /// </summary>
    /// <remarks>
    ///     Register this middleware after <c>UseSession()</c> and before downstream components that
    ///     require available session state, such as captcha validation or session-backed helpers.
    /// </remarks>
    public class SessionGuardMiddleware
    {
        #region Instance fields and properties

        private readonly RequestDelegate _next;

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionGuardMiddleware" /> class.
        /// </summary>
        /// <param name="next">
        ///     The next middleware delegate in the ASP.NET Core request pipeline.
        /// </param>
        public SessionGuardMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Loads the current request session when it is not already available, then invokes the next middleware.
        /// </summary>
        /// <param name="context">
        ///     The current HTTP context.
        /// </param>
        /// <returns>
        ///     A task that completes when the remaining request pipeline has completed.
        /// </returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var session = context.Session;
            if (!session.IsAvailable)
            {
                await session.LoadAsync();
            }


            await _next(context);
        }

        #endregion
    }
}