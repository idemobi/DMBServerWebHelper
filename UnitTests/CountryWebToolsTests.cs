#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerHelper;
using DMBServerWebHelper;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class CountryWebToolsTests
{
    #region Setup/Teardown

    [SetUp]
    public void SetUp()
    {
        ServerHelperConfiguration.Config = new ServerHelperConfiguration();
    }

    [TearDown]
    public void TearDown()
    {
        ServerHelperConfiguration.Config = new ServerHelperConfiguration();
    }

    #endregion

    [Test]
    public void GetCountryCodeFallsBackToConfiguredBaseLanguage()
    {
        ServerHelperConfiguration.Config.BaseLanguage = "fr-FR";
        DefaultHttpContext context = new DefaultHttpContext();

        string country = CountryWebTools.GetCountryCode(context);

        Assert.That(country, Is.EqualTo("FR"));
    }

    [Test]
    public void GetCountryCodeFallsBackToConfiguredBaseLanguageWhenHeaderIsMissing()
    {
        ServerHelperConfiguration.Config.BaseLanguage = "en-US";
        DefaultHttpContext context = new DefaultHttpContext();

        string country = CountryWebTools.GetCountryCode(context);

        Assert.That(country, Is.EqualTo("US"));
    }

    [Test]
    public void GetCountryCodeReturnsNoneForNullContext()
    {
        string country = CountryWebTools.GetCountryCode(null);

        Assert.That(country, Is.EqualTo("None"));
    }

    [TestCase("fr-CA,fr;q=0.9", "CA")]
    [TestCase("fr-CA;q=0.4,en-US;q=0.9,fr;q=0.8", "US")]
    [TestCase("fr-FR;q=0,en-GB;q=0.7", "GB")]
    [TestCase("zz-ZZ;q=1,fr-FR;q=0.8", "FR")]
    [TestCase("fr-FR;q=0.8,en-US;q=0.8", "FR")]
    [TestCase("*,fr-FR;q=0.8", "FR")]
    [TestCase("fr", "FR")]
    [TestCase("zz-ZZ;q=1", "None")]
    public void GetCountryCodeUsesAcceptLanguageHeader(string acceptLanguage, string expectedCountry)
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = acceptLanguage;

        string country = CountryWebTools.GetCountryCode(context);

        Assert.That(country, Is.EqualTo(expectedCountry));
    }
}
