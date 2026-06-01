#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using System.Globalization;
using DMBServerHelper;
using DMBServerWebHelper.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Provides the default ASP.NET Core web configuration for applications that consume
    ///     <c>DMBServerWebHelper</c>.
    /// </summary>
    /// <remarks>
    ///     The configuration registers MVC, session state, antiforgery, request localization,
    ///     embedded static assets, cookie definitions, validation localization, and the shared
    ///     <see cref="WebLocalizer" /> resources used by server-side PageBuilder applications.
    /// </remarks>
    [Serializable]
    public class ServerWebHelperConfiguration : WebGenericConfiguration<ServerWebHelperConfiguration>, IServerWebConfig
    {
        #region Static fields and properties

        /// <summary>
        ///     Defines the optional analytics consent cookie used by applications that enable analytics features.
        /// </summary>
        /// <remarks>
        ///     The field is reserved for host applications and remains <see langword="null" /> until a host
        ///     application assigns an analytics cookie definition.
        /// </remarks>
        public static CookieBool? CookieAnalytics;

        /// <summary>
        ///     Defines the session consent cookie registered by <see cref="AfterConfiguration" />.
        /// </summary>
        /// <remarks>
        ///     The value is <see langword="null" /> until <see cref="AfterConfiguration" /> registers the default
        ///     consent cookie definition.
        /// </remarks>
        public static CookieBool? CookieConsent;

        /// <summary>
        ///     Defines the culture cookie used to remember the selected language for the web layout.
        /// </summary>
        /// <remarks>
        ///     The value is <see langword="null" /> until <see cref="AfterConfiguration" /> registers the default
        ///     culture cookie definition.
        /// </remarks>
        public static CookieString? CookieLanguage;

        /// <summary>
        ///     Defines the optional advertising consent cookie used by applications that enable advertising features.
        /// </summary>
        /// <remarks>
        ///     The field is reserved for host applications and remains <see langword="null" /> until a host
        ///     application assigns an advertising cookie definition.
        /// </remarks>
        public static CookieBool? CookiePub;

        #endregion

        #region Static constructors and destructors

        static ServerWebHelperConfiguration()
        {
        }

        #endregion

        #region Static methods

        /// <summary>
        ///     Registers the runtime middleware pipeline required by <c>DMBServerWebHelper</c>.
        /// </summary>
        /// <param name="app">
        ///     The web application whose request pipeline will receive the shared server helper middleware.
        /// </param>
        /// <remarks>
        ///     This method increments <see cref="RequestCounter" /> for each request, delegates common
        ///     server setup to <see cref="ServerHelperConfiguration.UseApp" />, and then
        ///     registers cookie policy, session, <see cref="SessionGuardMiddleware" />, request localization,
        ///     exception handling, and status code re-execution middleware.
        /// </remarks>
        public static void UseApp(WebApplication app)
        {
            app.Use(async (ctx, next) =>
            {
                RequestCounter.Increment();
                await next();
            });

            ServerHelperConfiguration.UseApp(app);


            app.UseCookiePolicy();

            app.UseSession();
            app.UseSessionGuard();

            app.UseRequestLocalization();

            app.UseExceptionHandler("/Exception");

            string errorRedirection = "/Error/{0}";

            app.UseStatusCodePagesWithReExecute(errorRedirection);
        }

        #endregion

        #region Instance fields and properties

        /// <summary>
        ///     Gets or sets the captcha rendering parameters used by the configured web application.
        /// </summary>
        /// <value>
        ///     A <see cref="DMBServerWebHelper.CaptchaParameters" /> instance. The default value uses
        ///     the standard captcha rendering settings.
        /// </value>
        public CaptchaParameters CaptchaParameters { set; get; } = new CaptchaParameters();

        /// <summary>
        ///     Defines the session and application cookie duration, in seconds.
        /// </summary>
        /// <value>
        ///     The default value is <c>3600</c>, which corresponds to one hour.
        /// </value>
        public int WebSessionDurationInSeconds = 60 * 60; // => 1 hour

        #endregion

        #region Instance methods

        #region From interface IServerWebConfig

        /// <summary>
        ///     Registers the web services, options, localization resources, and cookie definitions
        ///     required after configuration files have been loaded.
        /// </summary>
        /// <param name="appBuilder">
        ///     The host application builder whose service collection receives the web helper registrations.
        /// </param>
        /// <param name="configBuilder">
        ///     The configuration builder supplied by the generic configuration pipeline.
        /// </param>
        /// <param name="configRoot">
        ///     The resolved configuration root supplied by the generic configuration pipeline.
        /// </param>
        /// <remarks>
        ///     This method configures Kestrel synchronous I/O, MVC, session state, antiforgery cookies,
        ///     request localization, Razor view localization, data annotation localization, embedded static
        ///     file options, and the default language and consent cookie definitions.
        /// </remarks>
        public override void AfterConfiguration(IHostApplicationBuilder appBuilder, IConfigurationBuilder configBuilder, IConfigurationRoot configRoot)
        {
            appBuilder.Services.ConfigureOptions<ServerWebHelperConfigureOptions>();
            appBuilder.Services.Configure<KestrelServerOptions>(sOptions =>
            {
                if (sOptions == null)
                {
                    throw new ArgumentNullException(nameof(sOptions));
                }

                sOptions.AllowSynchronousIO = true;
            });
            appBuilder.Services.AddControllersWithViews();
            appBuilder.Services.AddDistributedMemoryCache();
            appBuilder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromSeconds(Config.WebSessionDurationInSeconds);
                options.SlidingExpiration = true;
            });

            appBuilder.Services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ServerHelperConfiguration.Config.CookiePrefix + ServerHelperConfiguration.Config.SessionCookieName;
                options.IdleTimeout = TimeSpan.FromSeconds(Config.WebSessionDurationInSeconds);
                options.Cookie.IsEssential = true;
            });


            appBuilder.Services.AddMvc();
            appBuilder.Services.AddMvc().AddSessionStateTempDataProvider();
            appBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            appBuilder.Services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ServerHelperConfiguration.Config.CookiePrefix + "AntiForgery";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            appBuilder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedLanguages = new List<string>();
                if (ServerHelperConfiguration.Config.SupportLanguages != null)
                {
                    supportedLanguages.AddRange(ServerHelperConfiguration.Config.SupportLanguages);
                }

                var supportedCultures = supportedLanguages.Select(lang => new CultureInfo(lang)).ToList();

                var defaultCulture = new CultureInfo(ServerHelperConfiguration.Config.BaseLanguage);
                if (!supportedCultures.Any(c => c.Name.Equals(ServerHelperConfiguration.Config.BaseLanguage, StringComparison.OrdinalIgnoreCase)))
                {
                    supportedCultures.Insert(0, defaultCulture);
                }

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                var cookieProvider = new CookieRequestCultureProvider
                {
                    CookieName = ServerHelperConfiguration.Config.CookiePrefix + "Culture"
                };
                options.RequestCultureProviders.Insert(0, cookieProvider);
            });

            appBuilder.Services.AddLocalization();

            appBuilder.Services.AddControllersWithViews().AddRazorOptions(options => { options.ViewLocationExpanders.Add(new WebLocalizedViewLocationExpander()); });
            WebLocalizer.DataAnnotationLocalizer = WebLocalizer.GetLocalizer<DMBServerWebHelperDataAnnotationLocalization>();
            WebLocalizer.InternalLocalizer = WebLocalizer.GetLocalizer<DMBServerWebHelperInternalLocalization>();

            appBuilder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => { options.DataAnnotationLocalizerProvider = (type, factory) => WebLocalizer.DataAnnotationLocalizer; });

            appBuilder.Services.ConfigureOptions<ServerWebHelperConfigureOptions>();
            // appBuilder.Services.AddHostedService<ServerWebHelperStartupService>();

            AddAnnotationLocalization(appBuilder,
                typeof(DMBServerWebHelperDataAnnotationLocalization),
                typeof(DMBServerWebHelperInternalLocalization)
            );

            CookieLanguage = new CookieString(ServerHelperConfiguration.Config.CookiePrefix + "Culture", "Language", "Language selected for layout", CookieDefinitionGroup.Functional, ServerHelperConfiguration.Config.BaseLanguage, false, false);

            CookieConsent = new CookieBool(ServerHelperConfiguration.Config.CookiePrefix + "Consent", "Session Consent", "Consent to use session for this website", CookieDefinitionGroup.Consent, false);
        }

        /// <summary>
        ///     Indicates whether this package contributes an API description endpoint.
        /// </summary>
        /// <returns>
        ///     Always <see langword="false" /> because the package contributes web infrastructure rather
        ///     than an application API description surface.
        /// </returns>
        public override bool ApiDescription()
        {
            return false;
        }

        /// <summary>
        ///     Runs before configuration is loaded.
        /// </summary>
        /// <param name="appBuilder">
        ///     The host application builder supplied by the generic configuration pipeline.
        /// </param>
        /// <param name="configBuilder">
        ///     The configuration builder supplied by the generic configuration pipeline.
        /// </param>
        /// <param name="configRoot">
        ///     The resolved configuration root supplied by the generic configuration pipeline.
        /// </param>
        /// <remarks>
        ///     The default web helper configuration does not need pre-configuration work.
        /// </remarks>
        public override void BeforeConfiguration(IHostApplicationBuilder appBuilder, IConfigurationBuilder configBuilder, IConfigurationRoot configRoot)
        {
        }

        /// <summary>
        ///     Indicates whether this configuration requires a dedicated configuration file or app settings section.
        /// </summary>
        /// <returns>
        ///     Always <see langword="false" /> because this package can operate with the shared server helper defaults.
        /// </returns>
        public override bool NeedsConfigFileOrAppSettings()
        {
            return false;
        }

        /// <summary>
        ///     Populates the configuration with random fake values.
        /// </summary>
        /// <remarks>
        ///     The default web helper configuration does not generate fake values.
        /// </remarks>
        public override void RandomFake()
        {
        }

        #endregion

        #endregion
    }
}
