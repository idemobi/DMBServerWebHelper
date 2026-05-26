using System.Globalization;
using DMBServerHelper;
using Microsoft.AspNetCore.Http;

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides helpers for resolving country information from web request language signals.
    /// </summary>
    public static class CountryWebTools
    {
        /// <summary>
        ///     Resolves the country code string for the current request.
        /// </summary>
        /// <param name="context">
        ///     The HTTP context whose <c>Accept-Language</c> header is inspected, or <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     The resolved uppercase country code, or <c>None</c> when no country can be resolved.
        /// </returns>
        /// <remarks>
        ///     This method currently delegates to <see cref="GetCountryCode"/>.
        /// </remarks>
        public static string GetCountryString(HttpContext? context)
        {
            return GetCountryCode(context);
        }

        /// <summary>
        ///     Resolves an uppercase country code from the request <c>Accept-Language</c> header.
        /// </summary>
        /// <param name="context">
        ///     The HTTP context whose request headers are inspected, or <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     The country portion of the first accepted culture, a default country for language-only cultures,
        ///     or <c>None</c> when the context or culture cannot be resolved.
        /// </returns>
        /// <remarks>
        ///     When the header is empty, the method falls back to <see cref="ServerHelperConfiguration.Config"/>
        ///     base language, then to <c>en-US</c>. Invalid culture names return <c>None</c>.
        /// </remarks>
        public static string GetCountryCode(HttpContext? context)
        {
            if (context == null)
            {
                return "None";
            }

            string rawLang = context.Request.Headers["Accept-Language"].ToString();
            string? firstLang = rawLang.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();

            if (string.IsNullOrEmpty(firstLang))
            {
                firstLang = ServerHelperConfiguration.Config.BaseLanguage ?? "en-US";
            }

            try
            {
                CultureInfo culture = CultureInfo.GetCultureInfo(firstLang);
                string[] parts = culture.Name.Split('-');
                string countryCode = parts.Length > 1 ? parts[1] : GetDefaultCountryForLanguage(parts[0]);
                return countryCode.ToUpperInvariant();
            }
            catch (CultureNotFoundException)
            {
            }

            return "None";
        }

        private static string GetDefaultCountryForLanguage(string lang)
        {
            return lang.ToLowerInvariant() switch
            {
                "en" => "US",
                "fr" => "FR",
                "de" => "DE",
                "es" => "ES",
                "it" => "IT",
                "nl" => "NL",
                "pt" => "BR",
                "ru" => "RU",
                "pl" => "PL",
                "sv" => "SE",
                "da" => "DK",
                "fi" => "FI",
                "no" => "NO",
                "cs" => "CZ",
                "sk" => "SK",
                "hu" => "HU",
                "ro" => "RO",
                "bg" => "BG",
                "el" => "GR",
                "tr" => "TR",
                "he" => "IL",
                "uk" => "UA",
                "ja" => "JP",
                "zh" => "CN",
                "ko" => "KR",
                "th" => "TH",
                "vi" => "VN",
                "hi" => "IN",
                "bn" => "BD",
                "ta" => "IN",
                "ms" => "MY",
                "id" => "ID",
                "fa" => "IR",
                "ar" => "SA",
                "ur" => "PK",
                "sw" => "KE",
                "am" => "ET",
                "ha" => "NG",
                "yo" => "NG",
                "zu" => "ZA",
                "af" => "ZA",
                "is" => "IS",
                "ga" => "IE",
                "et" => "EE",
                "lv" => "LV",
                "lt" => "LT",
                "sl" => "SI",
                "hr" => "HR",
                "sr" => "RS",
                "mk" => "MK",
                _ => "US"
            };
        }
    }
}
