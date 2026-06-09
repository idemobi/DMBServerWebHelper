#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Adds the package embedded static assets to ASP.NET Core static file options.
    /// </summary>
    /// <remarks>
    ///     The post-configuration composes the host web root file provider with a
    ///     <see cref="ManifestEmbeddedFileProvider" /> rooted at the package <c>wwwroot</c> folder.
    /// </remarks>
    public class ServerWebHelperConfigureOptions : IPostConfigureOptions<StaticFileOptions>
    {
        #region Constants

        // TODO: Adjust the name to the constant nomenclature.
        const string K_BasePath = "wwwroot";

        #endregion

        #region Instance fields and properties

        private IWebHostEnvironment Environment { get; }

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServerWebHelperConfigureOptions" /> class.
        /// </summary>
        /// <param name="sEnvironment">
        ///     The web host environment that provides the application web root file provider.
        /// </param>
        public ServerWebHelperConfigureOptions(IWebHostEnvironment sEnvironment)
        {
            Environment = sEnvironment;
        }

        #endregion

        #region Instance methods

        #region From interface IPostConfigureOptions<StaticFileOptions>

        /// <summary>
        ///     Post-configures static file options by adding the package embedded <c>wwwroot</c> assets.
        /// </summary>
        /// <param name="sName">
        ///     The named options instance being configured.
        /// </param>
        /// <param name="sOptions">
        ///     The static file options to update.
        /// </param>
        /// <remarks>
        ///     The method preserves an existing file provider when one is already configured and appends
        ///     the package embedded file provider through a <see cref="CompositeFileProvider" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="sName" /> or <paramref name="sOptions" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when neither the options nor the environment provide a file provider.
        /// </exception>
        public void PostConfigure(string? sName, StaticFileOptions sOptions)
        {
            sName = sName ?? throw new ArgumentNullException(nameof(sName));
            sOptions = sOptions ?? throw new ArgumentNullException(nameof(sOptions));

            sOptions.ContentTypeProvider = sOptions.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            if (sOptions.FileProvider == null && Environment.WebRootFileProvider == null)
            {
                throw new InvalidOperationException("Missing FileProvider.");
            }

            sOptions.FileProvider = sOptions.FileProvider ?? Environment.WebRootFileProvider;
            var tFilesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, K_BasePath);
            sOptions.FileProvider = new CompositeFileProvider(sOptions.FileProvider, tFilesProvider);
        }

        #endregion

        #endregion
    }
}