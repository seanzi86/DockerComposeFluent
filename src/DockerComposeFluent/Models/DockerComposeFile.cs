using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents the root of a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/03-compose-file.md"/>
    /// </summary>
    public sealed record DockerComposeFile
    {
        /// <summary>
        /// The project name, as specified by the top-level <c>name</c> property.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/04-version-and-name.md#name-top-level-element"/>
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The services defined under the top-level <c>services</c> property, keyed by service name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/05-services.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, ServiceDefinition> Services { get; init; } = new Dictionary<string, ServiceDefinition>();

        /// <summary>
        /// The networks defined under the top-level <c>networks</c> property, keyed by network name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/06-networks.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, NetworkDefinition> Networks { get; init; } = new Dictionary<string, NetworkDefinition>();

        /// <summary>
        /// The volumes defined under the top-level <c>volumes</c> property, keyed by volume name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/914ec15d1fa498969c0df5c1d672306db3256089/07-volumes.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, VolumeDefinition> Volumes { get; init; } = new Dictionary<string, VolumeDefinition>();
    }
}
