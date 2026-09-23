using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DockerComposeFile"/>. Registering the same name more than once
    /// replaces the earlier definition.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/03-compose-file.md"/>
    /// </summary>
    public sealed class DockerComposeBuilder
    {
        private readonly Dictionary<string, ServiceDefinition> _services = new Dictionary<string, ServiceDefinition>();
        private readonly Dictionary<string, NetworkDefinition> _networks = new Dictionary<string, NetworkDefinition>();
        private readonly Dictionary<string, VolumeDefinition> _volumes = new Dictionary<string, VolumeDefinition>();
        private string? _name;

        /// <summary>
        /// Sets the project name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/04-version-and-name.md#name-top-level-element"/>
        /// </summary>
        /// <param name="name">The project name.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));

            _name = name;
            return this;
        }

        /// <summary>
        /// Adds a service from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md"/>
        /// </summary>
        /// <param name="name">The service name.</param>
        /// <param name="service">The service definition.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithService(string name, ServiceDefinition service)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(service, nameof(service));

            _services[name] = service;
            return this;
        }

        /// <summary>
        /// Adds a service configured through a <see cref="ServiceBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md"/>
        /// </summary>
        /// <param name="name">The service name.</param>
        /// <param name="configure">Configures the service.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithService(string name, Action<ServiceBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ServiceBuilder builder = new ServiceBuilder();
            configure(builder);
            return WithService(name, builder.Build());
        }

        /// <summary>
        /// Adds a network from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md"/>
        /// </summary>
        /// <param name="name">The network name.</param>
        /// <param name="network">The network definition.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithNetwork(string name, NetworkDefinition network)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(network, nameof(network));

            _networks[name] = network;
            return this;
        }

        /// <summary>
        /// Adds a network configured through a <see cref="NetworkBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md"/>
        /// </summary>
        /// <param name="name">The network name.</param>
        /// <param name="configure">Configures the network.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithNetwork(string name, Action<NetworkBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            NetworkBuilder builder = new NetworkBuilder();
            configure(builder);
            return WithNetwork(name, builder.Build());
        }

        /// <summary>
        /// Adds a volume from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md"/>
        /// </summary>
        /// <param name="name">The volume name.</param>
        /// <param name="volume">The volume definition.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithVolume(string name, VolumeDefinition volume)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(volume, nameof(volume));

            _volumes[name] = volume;
            return this;
        }

        /// <summary>
        /// Adds a volume configured through a <see cref="VolumeBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md"/>
        /// </summary>
        /// <param name="name">The volume name.</param>
        /// <param name="configure">Configures the volume.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithVolume(string name, Action<VolumeBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            VolumeBuilder builder = new VolumeBuilder();
            configure(builder);
            return WithVolume(name, builder.Build());
        }

        /// <summary>
        /// Creates the compose file from the values set so far. Later changes to this builder do not affect the returned file.
        /// </summary>
        /// <returns>An immutable <see cref="DockerComposeFile"/>.</returns>
        public DockerComposeFile Build()
        {
            return new DockerComposeFile
            {
                Name = _name,
                Services = new Dictionary<string, ServiceDefinition>(_services),
                Networks = new Dictionary<string, NetworkDefinition>(_networks),
                Volumes = new Dictionary<string, VolumeDefinition>(_volumes)
            };
        }
    }
}
