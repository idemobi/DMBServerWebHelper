#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

#region

using DMBServerHelper;

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Defines the marker contract for server web helper configurations.
    /// </summary>
    /// <remarks>
    ///     Implementations extend <see cref="IServerConfig" /> with ASP.NET Core web infrastructure
    ///     behavior such as MVC, session, localization, cookies, and static asset configuration.
    /// </remarks>
    public interface IServerWebConfig : IServerConfig
    {
    }
}