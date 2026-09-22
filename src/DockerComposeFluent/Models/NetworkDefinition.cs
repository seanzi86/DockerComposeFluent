namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single network (the <c>networks.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// </summary>
    public sealed record NetworkDefinition
    {
        /// <summary>
        /// The network driver to use, as specified by <c>driver</c>.
        /// </summary>
        public string? Driver { get; init; }
    }
}
