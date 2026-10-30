using System;
using System.Collections.Generic;
using DockerComposeFluent.Serialisation;
using DockerComposeFluent.Validation;

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
        /// Other Compose applications or sub-projects to include, as specified by the top-level <c>include</c>
        /// property. Included files are resolved before the rest of this file. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
        /// </summary>
        public IReadOnlyList<IncludeDefinition> Includes
        {
            get { return _includes; }
            init { _includes = Collections.Seal(value); }
        }

        private IReadOnlyList<IncludeDefinition> _includes = Collections.EmptyList<IncludeDefinition>();

        /// <summary>
        /// The services defined under the top-level <c>services</c> property, keyed by service name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, ServiceDefinition> Services
        {
            get { return _services; }
            init { _services = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, ServiceDefinition> _services = Collections.EmptyDictionary<ServiceDefinition>();

        /// <summary>
        /// The networks defined under the top-level <c>networks</c> property, keyed by network name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, NetworkDefinition> Networks
        {
            get { return _networks; }
            init { _networks = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, NetworkDefinition> _networks = Collections.EmptyDictionary<NetworkDefinition>();

        /// <summary>
        /// The volumes defined under the top-level <c>volumes</c> property, keyed by volume name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, VolumeDefinition> Volumes
        {
            get { return _volumes; }
            init { _volumes = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, VolumeDefinition> _volumes = Collections.EmptyDictionary<VolumeDefinition>();

        /// <summary>
        /// The secrets defined under the top-level <c>secrets</c> property, keyed by secret name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, SecretDefinition> Secrets
        {
            get { return _secrets; }
            init { _secrets = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, SecretDefinition> _secrets = Collections.EmptyDictionary<SecretDefinition>();

        /// <summary>
        /// The configs defined under the top-level <c>configs</c> property, keyed by config name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, ConfigDefinition> Configs
        {
            get { return _configs; }
            init { _configs = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, ConfigDefinition> _configs = Collections.EmptyDictionary<ConfigDefinition>();

        /// <summary>
        /// The extension fields defined at the top level of the file, keyed by their <c>x-</c> name. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions
        {
            get { return _extensions; }
            init { _extensions = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, object?> _extensions = Collections.EmptyDictionary<object?>();

        /// <summary>
        /// Checks the file as a whole for mistakes that individual builders cannot see, such as a service that
        /// depends on a service, network, volume, secret or config that is not defined, a dependency cycle, a
        /// <c>service_healthy</c> dependency on a service with no healthcheck, or two services publishing the same
        /// host port. It never throws and never changes the file; see <see cref="ValidationResult"/>.
        /// </summary>
        /// <remarks>
        /// A file that uses <c>include</c> may legitimately refer to names defined in the included files, so
        /// unknown names are then reported as warnings rather than errors.
        /// </remarks>
        /// <returns>Every problem found.</returns>
        public ValidationResult Validate()
        {
            return ComposeValidator.Validate(this);
        }

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
