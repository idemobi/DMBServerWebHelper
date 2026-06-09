#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

#endregion

namespace DMBserverWebHelperUnitTest.Helpers;

internal sealed class TestWebHostEnvironment : IWebHostEnvironment
{
    #region Instance fields and properties

    #region From interface IWebHostEnvironment

    public string ApplicationName { get; set; } = "DMBServerWebHelperUnitTest";

    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();

    public string ContentRootPath { get; set; } = Path.GetTempPath();

    public string EnvironmentName { get; set; } = "UnitTest";

    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();

    public string WebRootPath { get; set; } = Path.GetTempPath();

    #endregion

    #endregion
}