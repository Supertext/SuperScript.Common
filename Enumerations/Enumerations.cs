namespace SuperScript
{
    /// <summary>
    /// Determines for which modes (i.e., debug, live, or forced on or off) a property should be emitted.
    /// </summary>
    public enum EmitMode
    {
        /// <summary>
        /// Always emit, regardless of debug or live.
        /// </summary>
        Always,

        /// <summary>
        /// Emit only when debug="true" in the web.config.
        /// </summary>
        DebugOnly,

        /// <summary>
        /// Emit only when debug="false" in the web.config.
        /// </summary>
        LiveOnly,

        /// <summary>
        /// Never emit, regardless of debug or live.
        /// </summary>
        Never
    }
}