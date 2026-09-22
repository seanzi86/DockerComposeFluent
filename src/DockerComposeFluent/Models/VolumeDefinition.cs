namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single named volume (the <c>volumes.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// </summary>
    public sealed record VolumeDefinition
    {
        /// <summary>
        /// The volume driver to use, as specified by <c>driver</c>.
        /// </summary>
        public string? Driver { get; init; }
    }
}
