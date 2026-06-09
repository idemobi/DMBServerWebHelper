#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Drawing;
using System.Globalization;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides conversion helpers between <see cref="Color" /> values and common web color formats.
    /// </summary>
    public static class ColorHelper
    {
        #region Static methods

        private static Color FromArgb(string hex)
        {
            int a = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            int r = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        ///     Converts a hexadecimal web color string to a <see cref="Color" /> value.
        /// </summary>
        /// <param name="hex">
        ///     The hexadecimal color string. Supported formats are <c>RGB</c>, <c>RGBA</c>, <c>RRGGBB</c>, and <c>AARRGGBB</c>,
        ///     with or without a leading <c>#</c>.
        /// </param>
        /// <returns>
        ///     The parsed <see cref="Color" /> value.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="hex" /> is <see langword="null" />, empty, or whitespace.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Thrown when <paramref name="hex" /> does not use a supported length or contains invalid hexadecimal characters.
        /// </exception>
        public static Color FromHex(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) throw new ArgumentException("Hex color is null or empty");

            hex = hex.Trim().TrimStart('#');

            // RGB (3) expands to RRGGBB.
            // RGBA (4) expands to AARRGGBB.
            // RRGGBB (6)
            // AARRGGBB (8)

            if (hex.Length == 3) // RGB
            {
                hex = string.Concat(
                    hex[0], hex[0],
                    hex[1], hex[1],
                    hex[2], hex[2]
                );
                return FromRgb(hex);
            }

            if (hex.Length == 4) // RGBA
            {
                hex = string.Concat(
                    hex[3], hex[3], // Alpha
                    hex[0], hex[0], // Red
                    hex[1], hex[1], // Green
                    hex[2], hex[2] // Blue
                );
                return FromArgb(hex);
            }

            if (hex.Length == 6) // RRGGBB
            {
                return FromRgb(hex);
            }

            if (hex.Length == 8) // AARRGGBB
            {
                return FromArgb(hex);
            }

            throw new FormatException($"Invalid hex color format: #{hex}");
        }

        private static Color FromRgb(string hex)
        {
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

            return Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        ///     Converts a <see cref="Color" /> value to a hexadecimal web color string.
        /// </summary>
        /// <param name="color">
        ///     The color to convert.
        /// </param>
        /// <param name="includeAlpha">
        ///     A value indicating whether the alpha channel should be included before the RGB channels.
        /// </param>
        /// <returns>
        ///     A string in <c>#RRGGBB</c> format, or <c>#AARRGGBB</c> when <paramref name="includeAlpha" /> is
        ///     <see langword="true" />.
        /// </returns>
        public static string ToHex(Color color, bool includeAlpha = false)
        {
            return includeAlpha
                ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"
                : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        ///     Converts a <see cref="Color" /> value to a CSS <c>rgba(...)</c> color expression.
        /// </summary>
        /// <param name="color">
        ///     The color to convert.
        /// </param>
        /// <returns>
        ///     A CSS color expression using invariant culture formatting.
        /// </returns>
        public static string ToRgba(this Color color)
        {
            return string.Format(CultureInfo.InvariantCulture, "rgba({0}, {1}, {2}, {3})",
                color.R, color.G, color.B, color.A / 255.0);
        }

        /// <summary>
        ///     Attempts to parse a hexadecimal web color string.
        /// </summary>
        /// <param name="hex">
        ///     The hexadecimal color string to parse.
        /// </param>
        /// <param name="color">
        ///     When this method returns, contains the parsed color if parsing succeeded; otherwise, the default
        ///     <see cref="Color" />.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> when <paramref name="hex" /> was parsed successfully; otherwise, <see langword="false" />.
        /// </returns>
        public static bool TryFromHex(string hex, out Color color)
        {
            try
            {
                color = FromHex(hex);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException or FormatException)
            {
                color = default;
                return false;
            }
        }

        #endregion
    }
}