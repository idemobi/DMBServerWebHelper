#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    /// Contains utility methods for parsing and processing the Accept-Language HTTP header,
    /// used to extract prioritized language preferences from client requests.
    /// </summary>
    internal static class AcceptLanguageHeaderTools
    {
        #region Static methods

        /// <summary>
        /// Parses the provided Accept-Language HTTP header value to extract a list of accepted languages
        /// sorted by their quality values and preferences.
        /// </summary>
        /// <param name="acceptLanguageHeader">
        /// A string representing the value of the Accept-Language HTTP header.
        /// This value specifies the client's language preferences in a prioritized order.
        /// </param>
        /// <returns>
        /// An ordered, read-only list of language codes extracted from the Accept-Language header,
        /// sorted by quality (descending) and their order of appearance. If the header is null, empty,
        /// or cannot be parsed, an empty list is returned.
        /// </returns>
        internal static IReadOnlyList<string> GetAcceptedLanguages(string? acceptLanguageHeader)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguageHeader))
            {
                return Array.Empty<string>();
            }

            return acceptLanguageHeader
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(TryParseLanguage)
                .Where(language => language != null)
                .Select(language => language!.Value)
                .OrderByDescending(language => language.Quality)
                .ThenBy(language => language.Index)
                .Select(language => language.Value)
                .ToArray();
        }

        private static (string Value, double Quality, int Index)? TryParseLanguage(string rawLanguage, int index)
        {
            string[] parts = rawLanguage.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 0)
            {
                return null;
            }

            string value = parts[0];
            double quality = 1.0;

            foreach (string parameter in parts.Skip(1))
            {
                if (!parameter.StartsWith("q=", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string rawQuality = parameter.Substring(2);
                if (!double.TryParse(rawQuality, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out quality))
                {
                    return null;
                }

                if (quality < 0 || quality > 1)
                {
                    return null;
                }
            }

            if (quality <= 0 || !IsSupportedLanguage(value))
            {
                return null;
            }

            return (value, quality, index);
        }

        private static bool IsSupportedLanguage(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "*")
            {
                return false;
            }

            try
            {
                CultureInfo.GetCultureInfo(value);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the preferred language from the provided Accept-Language HTTP header value.
        /// This is determined by selecting the first language from the ordered list of accepted languages
        /// based on their quality values and preferences.
        /// </summary>
        /// <param name="acceptLanguageHeader">
        /// A string representing the value of the Accept-Language HTTP header. This value specifies
        /// the client's language preferences in a prioritized order.
        /// </param>
        /// <returns>
        /// A string containing the preferred language code extracted from the Accept-Language header,
        /// or null if the header is null, empty, or cannot be parsed.
        /// </returns>
        internal static string? GetPreferredLanguage(string? acceptLanguageHeader)
        {
            return GetAcceptedLanguages(acceptLanguageHeader).FirstOrDefault();
        }

        #endregion
    }
}
