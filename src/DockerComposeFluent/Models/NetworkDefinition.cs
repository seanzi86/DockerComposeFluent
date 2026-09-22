namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single network (the <c>networks.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md"/>
    /// </summary>
    public sealed record NetworkDefinition
    {
        /// <summary>
        /// The network driver to use, as specified by <c>driver</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver"/>
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// The actual Docker network name to use, overriding the default name generated from the
        /// project name and the key this network is defined under, as specified by <c>name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#name"/>
        /// </summary>
        public string? Name { get; init; }
    }
}
