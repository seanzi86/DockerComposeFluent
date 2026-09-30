using System.Collections.Generic;
using DockerComposeFluent.Serialisation;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents the root of a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/03-compose-file.md"/>
    /// </summary>
    public sealed record DockerComposeFile
    {
        /// <summary>
        /// The project name, as specified by the top-level <c>name</c> property.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/04-version-and-name.md#name-top-level-element"/>
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The services defined under the top-level <c>services</c> property, keyed by service name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, ServiceDefinition> Services { get; init; } = Collections.EmptyDictionary<ServiceDefinition>();

        /// <summary>
        /// The networks defined under the top-level <c>networks</c> property, keyed by network name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, NetworkDefinition> Networks { get; init; } = Collections.EmptyDictionary<NetworkDefinition>();

        /// <summary>
        /// The volumes defined under the top-level <c>volumes</c> property, keyed by volume name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, VolumeDefinition> Volumes { get; init; } = Collections.EmptyDictionary<VolumeDefinition>();

        /// <summary>
        /// The secrets defined under the top-level <c>secrets</c> property, keyed by secret name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, SecretDefinition> Secrets { get; init; } = Collections.EmptyDictionary<SecretDefinition>();

        /// <summary>
        /// The configs defined under the top-level <c>configs</c> property, keyed by config name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, ConfigDefinition> Configs { get; init; } = Collections.EmptyDictionary<ConfigDefinition>();

        /// <summary>
        /// The extension fields defined at the top level of the file, keyed by their <c>x-</c> name. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();

        /// <summary>
        /// Serialises this compose file to YAML.
        /// </summary>
        /// <returns>The compose file as a YAML string.</returns>
        public string ToYaml()
        {
            return ComposeYamlSerialiser.Serialise(this);
        }
    }
}
