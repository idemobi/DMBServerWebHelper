#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Globalization;
using DMBServerWebHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class LanguageToolsTests
{
    #region Setup/Teardown

    [SetUp]
    public void SetUp()
    {
        ServerWebHelperConfiguration.CookieLanguage = null;
    }

    [TearDown]
    public void TearDown()
    {
        ServerWebHelperConfiguration.CookieLanguage = null;
    }

    #endregion

    [Test]
    public void ResolveLanguageFallsBackToDefaultForMalformedAcceptLanguage()
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "fr-FR;q=broken";

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("en-US"));
    }

    [Test]
    public void ResolveLanguageIgnoresZeroQualityAcceptLanguage()
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "fr-CA;q=0,en-US;q=0.5";

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("en-US"));
    }

    [Test]
    public void ResolveLanguageKeepsHeaderOrderWhenQualityIsEqual()
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "fr-FR;q=0.8,en-US;q=0.8";

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("fr-FR"));
    }

    [Test]
    public void ResolveLanguageReturnsDefaultWhenNoSignalExists()
    {
        DefaultHttpContext context = new DefaultHttpContext();

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("en-US"));
    }

    [Test]
    public void ResolveLanguageUsesAcceptLanguageWhenNoFeatureOrCookieExists()
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "fr-CA,fr;q=0.9";

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("fr-CA"));
    }

    [Test]
    public void ResolveLanguageUsesHighestQualityAcceptLanguage()
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "fr-CA;q=0.4,en-US;q=0.9,fr;q=0.8";

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("en-US"));
    }

    [Test]
    public void ResolveLanguageUsesRequestCultureFeatureFirst()
    {
        DefaultHttpContext context = new DefaultHttpContext();
        context.Features.Set<IRequestCultureFeature>(
            new RequestCultureFeature(
                new RequestCulture(new CultureInfo("fr-FR")),
                provider: null));

        string language = LanguageTools.ResolveLanguage(context);

        Assert.That(language, Is.EqualTo("fr-FR"));
    }
}