namespace DockerComposeFluent.Models
{
    /// <summary>
    /// What Compose does when a watched file changes, as specified by <c>action</c> in a <c>develop.watch</c>
    /// rule.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#action"/>
    /// </summary>
    public enum WatchAction
    {
        /// <summary>
        /// Rebuild the service image from its <c>build</c> section and recreate the service (<c>rebuild</c>).
        /// </summary>
        Rebuild,

        /// <summary>
        /// Keep the container running and synchronise the changed files into it (<c>sync</c>).
        /// </summary>
        Sync,

        /// <summary>
        /// Restart the service container (<c>restart</c>).
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        Restart,

        /// <summary>
        /// Synchronise the changed files, then restart the container (<c>sync+restart</c>).
        /// </summary>
        SyncAndRestart,

        /// <summary>
        /// Synchronise the changed files, then run a command in the container (<c>sync+exec</c>).
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        SyncAndExec
    }
}
