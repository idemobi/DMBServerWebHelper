#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerHelper;
using DMBServerWebHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class ServerWebHelperConfigurationTests
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
        ServerWebHelperConfiguration.CookieConsent = null;
        ServerWebHelperConfiguration.CookieLanguage = null;
    }

    #endregion

    [Test]
    public void AfterConfigurationRegistersEmbeddedStaticFileOptionsOnce()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        IConfigurationBuilder configBuilder = new ConfigurationBuilder();
        IConfigurationRoot configRoot = configBuilder.Build();
        ServerWebHelperConfiguration configuration = new ServerWebHelperConfiguration();

        configuration.AfterConfiguration(builder, configBuilder, configRoot);

        int registrations = builder.Services.Count(descriptor =>
            descriptor.ServiceType == typeof(IPostConfigureOptions<StaticFileOptions>) &&
            descriptor.ImplementationType == typeof(ServerWebHelperConfigureOptions));

        Assert.That(registrations, Is.EqualTo(1));
    }
}