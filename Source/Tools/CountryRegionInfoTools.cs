#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System;
using System.Globalization;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides helpers for resolving country and region information.
    /// </summary>
    public static class CountryRegionInfoTools
    {
        #region Static methods

        /// <summary>
        ///     Attempts to parse a two-letter country code into a <see cref="RegionInfo" /> instance.
        /// </summary>
        /// <param name="countryCode">
        ///     The country code to parse. The value is trimmed and converted to uppercase before parsing.
        /// </param>
        /// <param name="region">
        ///     When this method returns, contains the parsed region when parsing succeeds; otherwise, <see langword="null" />.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> when <paramref name="countryCode" /> is a valid two-letter region code;
        ///     otherwise, <see langword="false" />.
        /// </returns>
        public static bool TryParse(string? countryCode, out RegionInfo? region)
        {
            region = null;

            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return false;
            }

            string code = countryCode.Trim().ToUpperInvariant();
            if (code.Length != 2)
            {
                return false;
            }

            try
            {
                region = new RegionInfo(code);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        #endregion
    }
}