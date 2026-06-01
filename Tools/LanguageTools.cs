#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides helpers for resolving the current web request language.
    /// </summary>
    public static class LanguageTools
    {
        #region Static methods

        /// <summary>
        ///     Resolves the current request language from ASP.NET Core localization, the language cookie,
        ///     or the <c>Accept-Language</c> request header.
        /// </summary>
        /// <param name="httpContext">
        ///     The HTTP context whose localization features, cookies, and headers are inspected.
        /// </param>
        /// <returns>
        ///     The resolved language or culture name. When no value is available, returns
        ///     the <see cref="ServerWebHelperConfiguration.CookieLanguage" /> default value, or <c>en-US</c>
        ///     when the language cookie has not been registered yet.
        /// </returns>
        /// <remarks>
        ///     If ASP.NET Core has already resolved an <see cref="IRequestCultureFeature" />, its UI culture
        ///     name is returned first. Cookie values using the ASP.NET request culture format are normalized
        ///     by returning the <c>c=</c> culture component.
        /// </remarks>
        public static string ResolveLanguage(HttpContext httpContext)
        {
            string? rawLang = null;
            IRequestCultureFeature? feature = httpContext.Features.Get<IRequestCultureFeature>();
            if (feature != null)
            {
                return feature.RequestCulture.UICulture.Name;
            }

            CookieString? languageCookie = ServerWebHelperConfiguration.CookieLanguage;
            if (languageCookie?.Exists(httpContext) == true)
            {
                rawLang = languageCookie.GetValue(httpContext);
            }
            else
            {
                rawLang = AcceptLanguageHeaderTools.GetPreferredLanguage(httpContext.Request.Headers["Accept-Language"].ToString());
            }

            if (string.IsNullOrWhiteSpace(rawLang))
            {
                return languageCookie?.DefaultValue ?? "en-US";
            }

            if (rawLang.StartsWith("c=", StringComparison.OrdinalIgnoreCase))
            {
                var parts = rawLang.Split('|');
                foreach (var part in parts)
                {
                    if (part.StartsWith("c="))
                    {
                        return part.Substring(2);
                    }
                }
            }

            return rawLang;
        }

        #endregion
    }
}
