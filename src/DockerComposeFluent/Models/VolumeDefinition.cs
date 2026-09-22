namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single named volume (the <c>volumes.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md"/>
    /// </summary>
    public sealed record VolumeDefinition
    {
        /// <summary>
        /// The volume driver to use, as specified by <c>driver</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver"/>
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// The actual Docker volume name to use, overriding the default name generated from the
        /// project name and the key this volume is defined under, as specified by <c>name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#name"/>
        /// </summary>
        public string? Name { get; init; }
    }
}
