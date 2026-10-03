namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The replication model for a service, as specified by <c>mode</c> under <c>deploy</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#mode"/>
    /// </summary>
    public enum DeployMode
    {
        /// <summary>
        /// A specified number of containers (<c>replicated</c>), the default.
        /// </summary>
        Replicated,

        /// <summary>
        /// Exactly one container per physical node (<c>global</c>).
        /// </summary>
        Global
    }
}
