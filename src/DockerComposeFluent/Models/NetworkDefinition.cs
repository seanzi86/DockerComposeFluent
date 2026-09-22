namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single network (the <c>networks.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/06-networks.md"/>
    /// </summary>
    public sealed record NetworkDefinition
    {
        /// <summary>
        /// The network driver to use, as specified by <c>driver</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/06-networks.md#driver"/>
        /// </summary>
        public string? Driver { get; init; }
    }
}
