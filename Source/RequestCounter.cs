#region Copyright

// ©2002-2026 idéMobi
// www.idemobi.com

#endregion

namespace DMBServerWebHelper
{
    /// <summary>
    ///     Tracks the number of requests observed by the shared server web middleware pipeline.
    /// </summary>
    /// <remarks>
    ///     <see cref="ServerWebHelperConfiguration.UseApp" /> increments this counter for each request
    ///     that passes through the registered pipeline.
    /// </remarks>
    public static class RequestCounter
    {
        #region Static fields and properties

        private static long _total;

        /// <summary>
        ///     Gets the current total request count.
        /// </summary>
        /// <value>
        ///     The value is read using <c>Interlocked.Read</c> so concurrent updates are observed safely.
        /// </value>
        public static long Total => Interlocked.Read(ref _total);

        #endregion

        #region Static methods

        /// <summary>
        ///     Increments the total request count by one.
        /// </summary>
        public static void Increment() => Interlocked.Increment(ref _total);

        #endregion
    }
}