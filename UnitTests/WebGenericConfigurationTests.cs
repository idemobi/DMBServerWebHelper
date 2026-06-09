#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerHelper;
using DMBServerWebHelper;
using Microsoft.AspNetCore.Builder;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class WebGenericConfigurationTests
{
    [Test]
    public void AddAnnotationLocalizationDoesNotRequireTemporaryServiceProvider()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        ServerWebHelperConfiguration configuration = new ServerWebHelperConfiguration();

        Assert.DoesNotThrow(() =>
            configuration.AddAnnotationLocalization(
                builder,
                typeof(WebGenericConfigurationTests),
                typeof(WebGenericConfigurationTests)));
    }

    [Test]
    public void AddAnnotationLocalizationInjectsResolvableResources()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        ServerWebHelperConfiguration configuration = new ServerWebHelperConfiguration();

        configuration.AddAnnotationLocalization(
            builder,
            typeof(UnitTestLocalization),
            typeof(UnitTestLocalization));

        var typedValue = WebLocalizer.GetLocalizer<UnitTestLocalization>()["UNIT_TEST_LOCALIZER_VALUE"];
        var dataAnnotationValue = WebLocalizer.GetDataAnnotation("UNIT_TEST_LOCALIZER_VALUE");
        var internalValue = WebLocalizer.GetInternal("UNIT_TEST_LOCALIZER_VALUE");

        Assert.Multiple(() =>
        {
            Assert.That(typedValue.ResourceNotFound, Is.False);
            Assert.That(typedValue.Value, Is.EqualTo("Resolved from test resources"));
            Assert.That(dataAnnotationValue.Value, Is.EqualTo("Resolved from test resources"));
            Assert.That(internalValue.Value, Is.EqualTo("Resolved from test resources"));
        });
    }
}

internal sealed class UnitTestLocalization
{
}