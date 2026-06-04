#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Globalization;
using DMBServerWebHelper;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class CountryRegionInfoToolsTests
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase("FRA")]
    [TestCase("??")]
    public void TryParseRejectsInvalidCountryCode(string? countryCode)
    {
        bool result = CountryRegionInfoTools.TryParse(countryCode, out RegionInfo? region);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(region, Is.Null);
        });
    }

    [Test]
    public void TryParseReturnsRegionForValidTwoLetterCountryCode()
    {
        bool result = CountryRegionInfoTools.TryParse(" fr ", out RegionInfo? region);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(region, Is.Not.Null);
            Assert.That(region!.TwoLetterISORegionName, Is.EqualTo("FR"));
        });
    }
}