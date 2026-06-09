#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Globalization;
using DMBServerHelper;
using Microsoft.AspNetCore.Http;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides helpers for resolving country information from web request language signals.
    /// </summary>
    public static class CountryWebTools
    {
        #region Static fields and properties

        /// <summary>
        ///     A lazily initialized collection containing the ISO country codes recognized by the system.
        /// </summary>
        /// <remarks>
        ///     This set is constructed by iterating over all specific cultures defined in the system and extracting
        ///     their associated country or region codes. The comparison for country codes is case-insensitive.
        /// </remarks>
        private static readonly Lazy<HashSet<string>> KnownCountryCodes = new(CreateKnownCountryCodes);

        #endregion

        #region Static methods

        /// <summary>
        ///     Constructs a collection of recognized ISO country codes by iterating through all specific cultures
        ///     and extracting the associated region or country codes.
        /// </summary>
        /// <returns>
        ///     A case-insensitive <see cref="HashSet{T}" /> of two-letter ISO country/region codes.
        ///     If any culture-specific region information cannot be resolved, it is skipped without raising an error.
        /// </returns>
        /// <remarks>
        ///     The method retrieves region information by examining the <see cref="CultureInfo.Name" /> property for
        ///     each specific culture available in the system. Invalid or unsupported culture names are handled
        ///     gracefully by ignoring them.
        /// </remarks>
        private static HashSet<string> CreateKnownCountryCodes()
        {
            HashSet<string> result = new(StringComparer.OrdinalIgnoreCase);
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                try
                {
                    RegionInfo region = new RegionInfo(culture.Name);
                    result.Add(region.TwoLetterISORegionName);
                }
                catch (ArgumentException)
                {
                }
            }

            return result;
        }

        /// <summary>
        ///     Resolves an uppercase country code from the request <c>Accept-Language</c> header.
        /// </summary>
        /// <param name="context">
        ///     The HTTP context whose request headers are inspected, or <see langword="null" />.
        /// </param>
        /// <returns>
        ///     The country portion of the first accepted culture, a default country for language-only cultures,
        ///     or <c>None</c> when the context or culture cannot be resolved.
        /// </returns>
        /// <remarks>
        ///     When the header is empty, the method falls back to <see cref="ServerHelperConfiguration" />
        ///     base language, then to <c>en-US</c>. Invalid culture names return <c>None</c>.
        /// </remarks>
        public static string GetCountryCode(HttpContext? context)
        {
            if (context == null)
            {
                return "None";
            }

            bool hasAcceptLanguageHeader = context.Request.Headers.ContainsKey("Accept-Language");
            string rawLang = context.Request.Headers["Accept-Language"].ToString();
            IReadOnlyList<string> acceptedLanguages = AcceptLanguageHeaderTools.GetAcceptedLanguages(rawLang);

            if (acceptedLanguages.Count == 0)
            {
                if (hasAcceptLanguageHeader)
                {
                    return "None";
                }

                acceptedLanguages = [ServerHelperConfiguration.Config.BaseLanguage ?? "en-US"];
            }

            foreach (string acceptedLanguage in acceptedLanguages)
            {
                try
                {
                    CultureInfo culture = CultureInfo.GetCultureInfo(acceptedLanguage);
                    string[] parts = culture.Name.Split('-');
                    string countryCode = parts.Length > 1 ? parts[1] : GetDefaultCountryForLanguage(parts[0]);
                    if (IsKnownCountryCode(countryCode))
                    {
                        return countryCode.ToUpperInvariant();
                    }
                }
                catch (CultureNotFoundException)
                {
                }
            }

            return "None";
        }

        /// <summary>
        ///     Resolves the country code string for the current request.
        /// </summary>
        /// <param name="context">
        ///     The HTTP context whose <c>Accept-Language</c> header is inspected, or <see langword="null" />.
        /// </param>
        /// <returns>
        ///     The resolved uppercase country code, or <c>None</c> when no country can be resolved.
        /// </returns>
        /// <remarks>
        ///     This method currently delegates to <see cref="GetCountryCode" />.
        /// </remarks>
        public static string GetCountryString(HttpContext? context)
        {
            return GetCountryCode(context);
        }

        /// <summary>
        ///     Resolves the default country code associated with a given language code.
        /// </summary>
        /// <param name="lang">
        ///     A two-letter ISO language code (e.g., "en" for English, "fr" for French).
        ///     This code is used to determine the corresponding default country.
        /// </param>
        /// <returns>
        ///     A two-letter ISO country code representing the default country for the given language
        ///     (e.g., "US" for "en", "FR" for "fr"). If the language code is unrecognized, a default value of "US" is returned.
        /// </returns>
        /// <remarks>
        ///     This method uses a predefined mapping of language codes to default country codes
        ///     and defaults to "US" if the provided language code does not have a mapped value.
        ///     The returned value is case-insensitive and will be normalized to uppercase.
        /// </remarks>
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

        /// <summary>
        ///     Determines whether the provided country code is recognized as a valid ISO country/region code
        ///     within the system's predefined collection.
        /// </summary>
        /// <param name="countryCode">
        ///     The two-letter ISO country/region code to validate. The comparison is case-insensitive.
        /// </param>
        /// <returns>
        ///     A boolean value indicating whether the specified country code is part of the recognized set.
        ///     Returns <c>true</c> if the country code is known; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsKnownCountryCode(string countryCode)
        {
            return KnownCountryCodes.Value.Contains(countryCode);
        }

        #endregion
    }
}