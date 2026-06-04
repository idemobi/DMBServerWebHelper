#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System;
using System.Text.Json.Serialization;
using DMBServerHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides the base generic configuration behavior for web helper configurations.
    /// </summary>
    /// <typeparam name="T">
    ///     The concrete web configuration type managed by the generic configuration pipeline.
    /// </typeparam>
    /// <remarks>
    ///     This base class extends <see cref="GenericConfiguration{T}" /> with web-specific debug
    ///     loading and localization resource injection support.
    /// </remarks>
    public abstract class WebGenericConfiguration<T> : GenericConfiguration<T> where T : IServerWebConfig, new()
    {
        #region Static fields and properties

        private static readonly IStringLocalizerFactory LocalizerFactory = new ResourceManagerStringLocalizerFactory(
            Options.Create(new LocalizationOptions()),
            NullLoggerFactory.Instance);

        #endregion

        #region Static methods

        /// <summary>
        ///     Loads the shared web configuration for a development application builder.
        /// </summary>
        /// <param name="builder">
        ///     The web application builder that receives common configuration services.
        /// </param>
        /// <param name="runtimeCompileForDev">
        ///     Compatibility flag kept for callers that previously requested Razor runtime compilation.
        /// </param>
        /// <param name="path">
        ///     Compatibility path kept for callers that previously configured Razor runtime compilation file providers.
        /// </param>
        /// <remarks>
        ///     Razor runtime compilation is obsolete in ASP.NET Core. This method now only loads the
        ///     common configuration lifecycle; development workflows should use Hot Reload or the
        ///     default build-time Razor compilation instead of runtime compilation.
        /// </remarks>
        public static void LoadConfigForDebug(
            WebApplicationBuilder builder,
            bool runtimeCompileForDev = false,
            string path = "../NuGet/"
        )
        {
            LoadCommonConfig(builder);
        }

        #endregion

        #region Instance fields and properties

        [JsonIgnore] private ICombinedStringLocalizer? Localizer { get; set; }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Injects data annotation and internal localization resources into the shared web localizers.
        /// </summary>
        /// <param name="builder">
        ///     The host application builder associated with the configuration lifecycle.
        /// </param>
        /// <param name="dataAnnotationClass">
        ///     The resource marker type used for data annotation localization, or <see langword="null" />
        ///     when no data annotation resource should be injected.
        /// </param>
        /// <param name="internalLocalizerClass">
        ///     The resource marker type used for internal localization, or <see langword="null" /> when
        ///     no internal resource should be injected.
        /// </param>
        /// <remarks>
        ///     The method resolves resource localizers directly from the resource marker types and registers
        ///     them with <see cref="WebLocalizer" /> so MVC validation and internal package text can share
        ///     the same localization infrastructure without creating a temporary service provider.
        /// </remarks>
        public void AddAnnotationLocalization(
            IHostApplicationBuilder builder,
            Type? dataAnnotationClass,
            Type? internalLocalizerClass
        )
        {
            ArgumentNullException.ThrowIfNull(builder);

            static IStringLocalizer ResolveLocalizer(Type resourceType) => LocalizerFactory.Create(resourceType);

            static ICombinedStringLocalizer CallGenericGetLocalizer(Type type)
            {
                var method = typeof(WebLocalizer)
                    .GetMethod(nameof(WebLocalizer.GetLocalizer))!
                    .MakeGenericMethod(type);

                return (ICombinedStringLocalizer)method.Invoke(null, null)!;
            }

            // DataAnnotation
            if (dataAnnotationClass is not null)
                WebLocalizer.DataAnnotationLocalizer.InjectResource(
                    dataAnnotationClass.Name,
                    ResolveLocalizer(dataAnnotationClass)
                );

            // Global
            if (internalLocalizerClass is not null)
                WebLocalizer.InternalLocalizer.InjectResource(
                    internalLocalizerClass.Name,
                    ResolveLocalizer(internalLocalizerClass)
                );

            // Internal
            if (internalLocalizerClass is not null)
            {
                var localizer = CallGenericGetLocalizer(internalLocalizerClass);
                localizer.InjectResource(
                    internalLocalizerClass.Name,
                    ResolveLocalizer(internalLocalizerClass)
                );
                Localizer = localizer;
            }
        }

        #endregion
    }
}
