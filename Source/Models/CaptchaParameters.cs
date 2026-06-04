#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

using System;

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Defines the rendering parameters used to generate captcha images.
    /// </summary>
    /// <remarks>
    ///     Instances are consumed by <see cref="CaptchaFactory" /> when rendering the distorted captcha
    ///     text, noise points, and line noise into PNG image bytes.
    /// </remarks>
    [Serializable]
    public class CaptchaParameters
    {
        #region Instance fields and properties

        /// <summary>
        ///     Gets or sets the font family used to draw captcha text.
        /// </summary>
        /// <value>
        ///     The default value is an empty string, allowing SkiaSharp to resolve a default family.
        /// </value>
        public string FontName { set; get; } = string.Empty;

        /// <summary>
        ///     Gets or sets the maximum font size used to measure and draw captcha text.
        /// </summary>
        /// <value>
        ///     The default value is <c>48</c>.
        /// </value>
        public int FontSizeMax { set; get; } = 48;

        /// <summary>
        ///     Gets or sets the maximum distortion level applied to the captcha image.
        /// </summary>
        /// <value>
        ///     The default value is <c>13</c>.
        /// </value>
        public double MaxDistortion { set; get; } = 13;

        /// <summary>
        ///     Gets or sets the minimum distortion level applied to the captcha image.
        /// </summary>
        /// <value>
        ///     The default value is <c>9</c>.
        /// </value>
        public double MinDistortion { set; get; } = 9;

        /// <summary>
        ///     Gets or sets the percentage of pixels used as random noise points.
        /// </summary>
        /// <value>
        ///     The default value is <c>0.002</c>, representing a small fraction of the rendered image area.
        /// </value>
        public double NoisePointsPercent { set; get; } = 0.002;

        /// <summary>
        ///     Gets or sets the exclusive upper bound used when selecting random line stroke widths.
        /// </summary>
        /// <value>
        ///     The default value is <c>3</c>.
        /// </value>
        public int StrokeWidthMax { set; get; } = 3;

        /// <summary>
        ///     Gets or sets the inclusive lower bound used when selecting random line stroke widths.
        /// </summary>
        /// <value>
        ///     The default value is <c>1</c>.
        /// </value>
        public int StrokeWidthMin { set; get; } = 1;

        #endregion
    }
}