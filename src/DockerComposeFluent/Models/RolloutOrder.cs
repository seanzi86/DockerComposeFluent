namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The order of operations when replacing a container, as specified by <c>order</c> under
    /// <c>deploy.rollback_config</c> and <c>deploy.update_config</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#update_config"/>
    /// </summary>
    public enum RolloutOrder
    {
        /// <summary>
        /// The old task is stopped before the new one starts (<c>stop-first</c>), the default.
        /// </summary>
        StopFirst,

        /// <summary>
        /// The new task starts first, briefly overlapping the old one (<c>start-first</c>).
        /// </summary>
        StartFirst
    }
}
