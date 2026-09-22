using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents the root of a Docker Compose file.
    /// </summary>
    public sealed record DockerComposeFile
    {
        /// <summary>
        /// The project name, as specified by the top-level <c>name</c> property.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The services defined under the top-level <c>services</c> property, keyed by service name.
        /// </summary>
        public IReadOnlyDictionary<string, ServiceDefinition> Services { get; init; } = new Dictionary<string, ServiceDefinition>();

        /// <summary>
        /// The networks defined under the top-level <c>networks</c> property, keyed by network name.
        /// </summary>
        public IReadOnlyDictionary<string, NetworkDefinition> Networks { get; init; } = new Dictionary<string, NetworkDefinition>();

        /// <summary>
        /// The volumes defined under the top-level <c>volumes</c> property, keyed by volume name.
        /// </summary>
        public IReadOnlyDictionary<string, VolumeDefinition> Volumes { get; init; } = new Dictionary<string, VolumeDefinition>();
    }
}
