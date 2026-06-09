#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerWebHelper;
using DMBserverWebHelperUnitTest.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NUnit.Framework;

#endregion

namespace DMBserverWebHelperUnitTest;

[TestFixture]
internal sealed class ServerWebHelperConfigureOptionsTests
{
    [Test]
    public void PostConfigureAddsEmbeddedStaticFileProvider()
    {
        ServerWebHelperConfigureOptions configureOptions = new ServerWebHelperConfigureOptions(new TestWebHostEnvironment());
        StaticFileOptions options = new StaticFileOptions();

        configureOptions.PostConfigure(Options.DefaultName, options);

        IFileInfo fileInfo = options.FileProvider!.GetFileInfo("css/DMBServerWebHelper.css");

        Assert.Multiple(() =>
        {
            Assert.That(options.ContentTypeProvider, Is.Not.Null);
            Assert.That(options.FileProvider, Is.TypeOf<CompositeFileProvider>());
            Assert.That(fileInfo.Exists, Is.True);
        });
    }

    [Test]
    public void PostConfigurePreservesExistingFileProvider()
    {
        IFileProvider existingProvider = new NullFileProvider();
        ServerWebHelperConfigureOptions configureOptions = new ServerWebHelperConfigureOptions(new TestWebHostEnvironment());
        StaticFileOptions options = new StaticFileOptions
        {
            FileProvider = existingProvider
        };

        configureOptions.PostConfigure(Options.DefaultName, options);

        IFileInfo fileInfo = options.FileProvider!.GetFileInfo("css/DMBServerWebHelper.css");

        Assert.Multiple(() =>
        {
            Assert.That(options.FileProvider, Is.TypeOf<CompositeFileProvider>());
            Assert.That(fileInfo.Exists, Is.True);
        });
    }

    [Test]
    public void PostConfigureThrowsWhenNoFileProviderExists()
    {
        TestWebHostEnvironment environment = new TestWebHostEnvironment
        {
            WebRootFileProvider = null!
        };
        ServerWebHelperConfigureOptions configureOptions = new ServerWebHelperConfigureOptions(environment);

        Assert.Throws<InvalidOperationException>(() =>
            configureOptions.PostConfigure(Options.DefaultName, new StaticFileOptions()));
    }
}