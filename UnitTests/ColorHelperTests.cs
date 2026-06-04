#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Drawing;
using DMBServerWebHelper;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class ColorHelperTests
{
    [TestCase("#0C2238", 255, 12, 34, 56)]
    [TestCase("0F8", 255, 0, 255, 136)]
    [TestCase("0F8A", 170, 0, 255, 136)]
    [TestCase("#800C2238", 128, 12, 34, 56)]
    public void FromHexParsesSupportedFormats(string hex, int expectedAlpha, int expectedRed, int expectedGreen, int expectedBlue)
    {
        Color color = ColorHelper.FromHex(hex);

        Assert.Multiple(() =>
        {
            Assert.That(color.A, Is.EqualTo(expectedAlpha));
            Assert.That(color.R, Is.EqualTo(expectedRed));
            Assert.That(color.G, Is.EqualTo(expectedGreen));
            Assert.That(color.B, Is.EqualTo(expectedBlue));
        });
    }

    [Test]
    public void ToHexCanIncludeAlpha()
    {
        string hex = ColorHelper.ToHex(Color.FromArgb(128, 12, 34, 56), includeAlpha: true);

        Assert.That(hex, Is.EqualTo("#800C2238"));
    }

    [Test]
    public void ToHexReturnsRgbByDefault()
    {
        string hex = ColorHelper.ToHex(Color.FromArgb(128, 12, 34, 56));

        Assert.That(hex, Is.EqualTo("#0C2238"));
    }

    [Test]
    public void ToRgbaUsesInvariantDecimalFormatting()
    {
        string rgba = Color.FromArgb(128, 12, 34, 56).ToRgba();

        Assert.That(rgba, Is.EqualTo("rgba(12, 34, 56, 0.5019607843137255)"));
    }

    [Test]
    public void TryFromHexReturnsFalseForInvalidInput()
    {
        bool result = ColorHelper.TryFromHex("not-a-color", out Color color);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(color, Is.EqualTo(default(Color)));
        });
    }
}