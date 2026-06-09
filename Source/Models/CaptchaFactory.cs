#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Security.Cryptography;
using System.Text;
using DMBServerHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Generates captcha values, renders captcha PNG images, and validates captcha input stored in session state.
    /// </summary>
    /// <remarks>
    ///     Captcha values are stored through a private <see cref="SessionString" /> named <c>Captcha</c>.
    ///     Consumers must ensure ASP.NET Core session is configured and available before calling methods
    ///     that read or write captcha state.
    /// </remarks>
    public static class CaptchaFactory
    {
        #region Static fields and properties

        private static SKColor BackgroundColor { get; set; } = SKColors.Transparent;
        private static readonly SessionString Captcha = new("Captcha", "Captcha", "Captcha", SessionDefinitionGroup.Invisible, string.Empty);

        private static Func<(int oldX, int oldY, double distortionLevel, int w, int h), (int newX, int newY)> DistortionFunc { get; } =
            oldPos =>
            {
                int newX = (int)(oldPos.oldX + (oldPos.distortionLevel * Math.Sin(Math.PI * oldPos.oldY / 64.0)));
                int newY = (int)(oldPos.oldY + (oldPos.distortionLevel * Math.Cos(Math.PI * oldPos.oldX / 64.0)));
                if (newX < 0 || newX >= oldPos.w) newX = 0;
                if (newY < 0 || newY >= oldPos.h) newY = 0;

                return (newX, newY);
            };

        private static readonly Random KRandom = new();

        private static SKColor[] LinesColor { get; set; } =
        [
            SKColors.Red,
            SKColors.Green,
            SKColors.Yellow,
            SKColors.Blue,
            SKColors.Violet
        ];

        private static SKColor NoisePointColor { get; set; } = SKColors.Black;

        private static Func<(int w, int h, double noisePointsPercent), IEnumerable<(int x, int y)>> NoisePointMapGenFunc { get; } =
            data =>
            {
                int noisePointCount = (int)(data.w * data.h * data.noisePointsPercent);
                return Enumerable.Range(0, noisePointCount)
                    .Select(_ => (RandomNumberGenerator.GetInt32(data.w), RandomNumberGenerator.GetInt32(data.h)))
                    .ToArray();
            };

        private static SKColor PaintColor { get; set; } = SKColors.DarkGray;

        #endregion

        #region Static methods

        /// <summary>
        ///     Renders the captcha currently stored in the session as a PNG file stream result.
        /// </summary>
        /// <param name="httpContext">
        ///     The HTTP context whose session contains the stored captcha value.
        /// </param>
        /// <param name="parameters">
        ///     Optional rendering parameters. When <see langword="null" />, default <see cref="CaptchaParameters" /> are used.
        /// </param>
        /// <returns>
        ///     A <see cref="FileStreamResult" /> containing a PNG representation of the stored captcha value.
        /// </returns>
        /// <remarks>
        ///     When no captcha value is available, the image is rendered from the fallback text <c>Error</c>.
        /// </remarks>
        public static FileStreamResult GenerateCaptcha(HttpContext httpContext, CaptchaParameters? parameters = null)
        {
            string value = TryGetStoredCaptcha(httpContext, out string storedCaptcha) ? storedCaptcha : "Error";
            byte[] image = GetCaptcha(value, parameters);
            return new FileStreamResult(new MemoryStream(image), "image/png");
        }

        private static byte[] GetCaptcha(string captchaText, CaptchaParameters? parameters = null)
        {
            parameters ??= new CaptchaParameters();

            byte[] imageBytes;
            SKRect size = new();
            int compensateDeepCharacters;
            int image2dX;
            int image2dY;

            using SKTypeface typeface = SKTypeface.FromFamilyName(parameters.FontName, SKFontStyle.Italic);
            using SKFont captchaFont = new(typeface, parameters.FontSizeMax);

            compensateDeepCharacters = (int)captchaFont.Size / 5;
            if (string.Equals(captchaText, captchaText.ToUpperInvariant(), StringComparison.Ordinal))
            {
                compensateDeepCharacters = 0;
            }

            captchaFont.MeasureText(captchaText, out size);
            image2dX = (int)size.Width + parameters.FontSizeMax / 2;
            image2dY = (int)size.Height + parameters.FontSizeMax / 2 + compensateDeepCharacters;

            using (SKBitmap image2d = new(image2dX, image2dY, SKColorType.Bgra8888, SKAlphaType.Premul))
                using (SKCanvas canvas = new(image2d))
                {
                    canvas.DrawColor(BackgroundColor);

                    using (SKPaint drawStyle = new())
                    {
                        drawStyle.Color = PaintColor;
                        drawStyle.IsAntialias = true;
                        canvas.DrawText(captchaText, parameters.FontSizeMax / 4f, image2dY - parameters.FontSizeMax / 4f - compensateDeepCharacters, SKTextAlign.Left, captchaFont, drawStyle);
                    }

                    SKImageInfo imageInfo = new(image2dX, image2dY, SKColorType.Bgra8888, SKAlphaType.Premul);
                    using SKSurface plainSkSurface = SKSurface.Create(imageInfo);
                    SKCanvas plainCanvas = plainSkSurface.Canvas;
                    plainCanvas.Clear(BackgroundColor);

                    using (SKPaint paintInfo = new())
                    {
                        paintInfo.Color = PaintColor;
                        paintInfo.IsAntialias = true;
                        plainCanvas.DrawText(captchaText, parameters.FontSizeMax / 4f, image2dY - parameters.FontSizeMax / 4f - compensateDeepCharacters, SKTextAlign.Left, captchaFont, paintInfo);
                    }

                    plainCanvas.Flush();

                    SKImageInfo imageInfoSurface = new(image2dX, image2dY, SKColorType.Bgra8888, SKAlphaType.Premul);
                    using SKSurface captchaSkSurface = SKSurface.Create(imageInfoSurface);
                    SKCanvas captchaCanvas = captchaSkSurface.Canvas;
                    double distortionLevel = parameters.MinDistortion + (parameters.MaxDistortion - parameters.MinDistortion) * GetSecureDouble();
                    if (RandomNumberGenerator.GetInt32(2) == 0)
                    {
                        distortionLevel *= -1;
                    }

                    SKPixmap plainPixmap = plainSkSurface.PeekPixels();
                    for (int x = 0; x < image2dX; x++)
                    {
                        for (int y = 0; y < image2dY; y++)
                        {
                            (int newX, int newY) = DistortionFunc((x, y, distortionLevel, image2dX, image2dY));
                            SKColor originalPixel = plainPixmap.GetPixelColor(newX, newY);
                            captchaCanvas.DrawPoint(x, y, originalPixel);
                        }
                    }

                    IEnumerable<(int x, int y)> noisePointMap = NoisePointMapGenFunc((image2dX, image2dY, parameters.NoisePointsPercent));
                    foreach ((int x, int y) in noisePointMap)
                    {
                        captchaCanvas.DrawPoint(x, y, NoisePointColor);
                    }

                    SKPaint drawLineNoise = new();
                    for (int i = 0; i < LinesColor.Length; i++)
                    {
                        drawLineNoise.Color = LinesColor[i];
                        drawLineNoise.StrokeWidth = RandomNumberGenerator.GetInt32(parameters.StrokeWidthMin, parameters.StrokeWidthMax);
                        captchaCanvas.DrawLine(RandomNumberGenerator.GetInt32(image2dX), RandomNumberGenerator.GetInt32(image2dY), RandomNumberGenerator.GetInt32(image2dX), RandomNumberGenerator.GetInt32(image2dY), drawLineNoise);
                    }

                    captchaCanvas.Flush();

                    using SKData png = captchaSkSurface.Snapshot().Encode(SKEncodedImageFormat.Png, 100);
                    imageBytes = png.ToArray();
                }

            return imageBytes;
        }

        /// <summary>Returns a cryptographically secure random double in [0.0, 1.0).</summary>
        private static double GetSecureDouble()
        {
            Span<byte> bytes = stackalloc byte[8];
            RandomNumberGenerator.Fill(bytes);
            ulong value = BitConverter.ToUInt64(bytes);
            return (value >> 11) / (double)(1UL << 53);
        }

        /// <summary>
        ///     Reads the captcha value currently stored in session.
        /// </summary>
        /// <param name="httpContext">
        ///     The HTTP context whose session contains the captcha value, or <see langword="null" />.
        /// </param>
        /// <returns>
        ///     The stored captcha value, or the underlying session definition fallback when no value is available.
        /// </returns>
        public static string GetStoredCaptcha(HttpContext? httpContext)
        {
            return Captcha.GetValue(httpContext);
        }

        /// <summary>
        ///     Generates a random captcha string using characters chosen to reduce visual ambiguity.
        /// </summary>
        /// <param name="length">
        ///     The requested number of characters.
        /// </param>
        /// <returns>
        ///     A random captcha string containing exactly <paramref name="length" /> characters.
        /// </returns>
        /// <remarks>
        ///     The character set intentionally excludes many visually ambiguous letters and digits.
        /// </remarks>
        public static string RandomCaptchaNoMistake(uint length)
        {
            StringBuilder result = new();
            const string chars = "cdefhkmnpqrtwxyCEFHKMNPRTWXY379";
            int charLength = chars.Length;

            while (result.Length < length)
            {
                result.Append(chars[KRandom.Next(0, charLength)]);
            }

            return result.ToString();
        }

        /// <summary>
        ///     Creates a new random captcha value, stores it in session, and returns the rendered PNG as base64 text.
        /// </summary>
        /// <param name="httpContext">
        ///     The HTTP context whose session receives the generated captcha value.
        /// </param>
        /// <param name="parameters">
        ///     Optional rendering parameters. When <see langword="null" />, default <see cref="CaptchaParameters" /> are used.
        /// </param>
        /// <returns>
        ///     The PNG image bytes encoded with base64. The returned value does not include a data URI prefix.
        /// </returns>
        /// <remarks>
        ///     The generated value is converted to uppercase before it is stored in session.
        /// </remarks>
        public static string RandomCaptchaToImage(HttpContext httpContext, CaptchaParameters? parameters = null)
        {
            string captcha = RandomCaptchaNoMistake(8).ToUpperInvariant();
            Captcha.SetValue(httpContext, captcha);
            byte[] image = GetCaptcha(captcha, parameters);
            return Convert.ToBase64String(image);
        }

        /// <summary>
        ///     Compares user-provided captcha text with the captcha value stored in session.
        /// </summary>
        /// <param name="httpContext">
        ///     The HTTP context whose session contains the expected captcha value.
        /// </param>
        /// <param name="captcha">
        ///     The user-provided captcha text to validate.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> when the provided text matches the stored captcha using an
        ///     ordinal case-insensitive comparison; otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        ///     A stored captcha is consumed after a validation attempt so the same value cannot be replayed.
        /// </remarks>
        public static bool TestCaptcha(HttpContext httpContext, string captcha)
        {
            if (!TryGetStoredCaptcha(httpContext, out string stored))
            {
                return false;
            }

            Captcha.DeleteFrom(httpContext);

            return !string.IsNullOrWhiteSpace(captcha) &&
                   string.Equals(captcha, stored, StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryGetStoredCaptcha(HttpContext? httpContext, out string storedCaptcha)
        {
            storedCaptcha = string.Empty;

            if (Captcha.Exists(httpContext) == false)
            {
                return false;
            }

            storedCaptcha = Captcha.GetValue(httpContext);
            return string.IsNullOrWhiteSpace(storedCaptcha) == false;
        }

        #endregion
    }
}