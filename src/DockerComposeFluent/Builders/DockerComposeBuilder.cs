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
        private readonly Dictionary<string, SecretDefinition> _secrets = new Dictionary<string, SecretDefinition>();
        private readonly Dictionary<string, ConfigDefinition> _configs = new Dictionary<string, ConfigDefinition>();
        private readonly List<IncludeDefinition> _includes = new List<IncludeDefinition>();
        private string? _name;
        private IReadOnlyDictionary<string, object?> _extensions = Collections.EmptyDictionary<object?>();

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
        /// Sets an extension field at the top level of the file. Setting the same key again replaces its
        /// value. Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithExtension(string key, object? value)
        {
            _extensions = ExtensionsMutator.Set(_extensions, key, value, nameof(key));
            return this;
        }

        /// <summary>
        /// Adds a Compose file to include, referenced by its path, written to YAML as a plain string.
        /// Included files are resolved before the rest of this file.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithInclude(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            return WithInclude(new IncludeDefinition { Path = new[] { path } });
        }

        /// <summary>
        /// Adds several Compose files to include, each referenced by its path. Each call to this method adds
        /// one <c>include</c> entry per path; use <see cref="WithInclude(Action{IncludeBuilder})"/> for an
        /// entry that includes several paths together.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
        /// </summary>
        /// <param name="paths">The file paths.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithIncludes(IEnumerable<string> paths)
        {
            Guard.NotNull(paths, nameof(paths));

            foreach (string path in paths)
            {
                WithInclude(path);
            }

            return this;
        }

        /// <summary>
        /// Adds a Compose file to include, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
        /// </summary>
        /// <param name="include">The include definition.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithInclude(IncludeDefinition include)
        {
            Guard.NotNull(include, nameof(include));
            _includes.Add(include);
            return this;
        }

        /// <summary>
        /// Adds a Compose file to include, configured through an <see cref="IncludeBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
        /// </summary>
        /// <param name="configure">Configures the include.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithInclude(Action<IncludeBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            IncludeBuilder builder = new IncludeBuilder();
            configure(builder);
            return WithInclude(builder.Build());
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
        /// Adds a secret from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="name">The secret name.</param>
        /// <param name="secret">The secret definition.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithSecret(string name, SecretDefinition secret)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(secret, nameof(secret));

            _secrets[name] = secret;
            return this;
        }

        /// <summary>
        /// Adds a secret configured through a <see cref="SecretBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="name">The secret name.</param>
        /// <param name="configure">Configures the secret.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithSecret(string name, Action<SecretBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            SecretBuilder builder = new SecretBuilder();
            configure(builder);
            return WithSecret(name, builder.Build());
        }

        /// <summary>
        /// Adds a config from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        /// <param name="name">The config name.</param>
        /// <param name="config">The config definition.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithConfig(string name, ConfigDefinition config)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(config, nameof(config));

            _configs[name] = config;
            return this;
        }

        /// <summary>
        /// Adds a config configured through a <see cref="ConfigBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        /// <param name="name">The config name.</param>
        /// <param name="configure">Configures the config.</param>
        /// <returns>This builder.</returns>
        public DockerComposeBuilder WithConfig(string name, Action<ConfigBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ConfigBuilder builder = new ConfigBuilder();
            configure(builder);
            return WithConfig(name, builder.Build());
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
                Includes = new List<IncludeDefinition>(_includes),
                Services = new Dictionary<string, ServiceDefinition>(_services),
                Networks = new Dictionary<string, NetworkDefinition>(_networks),
                Volumes = new Dictionary<string, VolumeDefinition>(_volumes),
                Secrets = new Dictionary<string, SecretDefinition>(_secrets),
                Configs = new Dictionary<string, ConfigDefinition>(_configs),
                Extensions = _extensions
            };
        }
    }
}
