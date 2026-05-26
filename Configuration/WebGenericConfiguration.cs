#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// DMBServerWebHelper.csproj WebGenericConfiguration.cs create at 2026/04/07 21:04:27
// ©2024-2026 idéMobi SARL FRANCE

#endregion

#region

using System.Text.Json.Serialization;
using DMBServerHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
#if DEBUG
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
#endif

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
    ///     This base class extends <see cref="GenericConfiguration{T}"/> with web-specific debug
    ///     loading and localization resource injection support.
    /// </remarks>
    public abstract class WebGenericConfiguration<T> : GenericConfiguration<T> where T : IServerWebConfig, new()
    {
        #region Static methods

        /// <summary>
        ///     Loads the shared web configuration for a development application builder.
        /// </summary>
        /// <param name="builder">
        ///     The web application builder that receives common configuration services.
        /// </param>
        /// <param name="runtimeCompileForDev">
        ///     A value indicating whether Razor runtime compilation should be enabled in debug builds.
        /// </param>
        /// <param name="path">
        ///     The relative path from the application content root to the NuGet source folder used for
        ///     runtime compilation file providers.
        /// </param>
        /// <remarks>
        ///     Runtime compilation registration is compiled only in <c>DEBUG</c> builds. When enabled,
        ///     the method adds a physical file provider for the configured package namespace.
        /// </remarks>
        public static void LoadConfigForDebug(
            WebApplicationBuilder builder,
            bool runtimeCompileForDev = false,
            string path = "../NuGet/"
        )
        {
            LoadCommonConfig(builder);
            // add razor
            Type configurationType = Config.GetType();
            if (runtimeCompileForDev)
            {
                #if DEBUG
                var tMvcBuilder = builder.Services.AddRazorPages();
                tMvcBuilder.AddRazorRuntimeCompilation();
                builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(sOptions =>
                {
                    // GDFLogger.TraceAttention(string.Format(GDFLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, configurationType.Namespace, configurationType.Name));
                    var tLibraryPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, path, configurationType.Namespace));
                    sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                });
                #endif
            }
            else
            {
                //GDFLogger.TraceAttention(string.Format(GDFLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, configurationType.Namespace));
            }
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
        ///     The host application builder whose service collection is used to resolve string localizers.
        /// </param>
        /// <param name="dataAnnotationClass">
        ///     The resource marker type used for data annotation localization, or <see langword="null"/>
        ///     when no data annotation resource should be injected.
        /// </param>
        /// <param name="internalLocalizerClass">
        ///     The resource marker type used for internal localization, or <see langword="null"/> when
        ///     no internal resource should be injected.
        /// </param>
        /// <remarks>
        ///     The method builds a temporary service provider to resolve <c>IStringLocalizer&lt;TResource&gt;</c>
        ///     instances and registers them with <see cref="WebLocalizer"/> so MVC validation and internal
        ///     package text can share the same localization infrastructure.
        /// </remarks>
        public void AddAnnotationLocalization(
            IHostApplicationBuilder builder,
            Type? dataAnnotationClass,
            Type? internalLocalizerClass
        )
        {
            var serviceProvider = builder.Services.BuildServiceProvider();

            static IStringLocalizer ResolveLocalizer(IServiceProvider sp, Type resourceType) => (IStringLocalizer)sp.GetRequiredService(typeof(IStringLocalizer<>).MakeGenericType(resourceType));

            static ICombinedStringLocalizer CallGenericGetLocalizer(Type type)
            {
                var method = typeof(WebLocalizer)
                    .GetMethod(nameof(WebLocalizer.GetLocalizer))!
                    .MakeGenericMethod(type);

                return (ICombinedStringLocalizer)method.Invoke(null, null)!;
            }

            // DataAnnotation
            if (dataAnnotationClass is not null)
                WebLocalizer.DataAnnotation.InjectResource(
                    dataAnnotationClass.Name,
                    ResolveLocalizer(serviceProvider, dataAnnotationClass)
                );

            // Global
            if (internalLocalizerClass is not null)
                WebLocalizer.Internal.InjectResource(
                    internalLocalizerClass.Name,
                    ResolveLocalizer(serviceProvider, internalLocalizerClass)
                );

            // Internal
            if (internalLocalizerClass is not null)
            {
                var localizer = CallGenericGetLocalizer(internalLocalizerClass);
                localizer.InjectResource(
                    internalLocalizerClass.Name,
                    ResolveLocalizer(serviceProvider, internalLocalizerClass)
                );
                Localizer = localizer;
            }
        }

        #endregion
    }
}
