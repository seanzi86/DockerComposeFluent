using System;
using System.Collections.Generic;
using System.Globalization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ServiceDefinition"/>.
    /// </summary>
    public sealed class ServiceBuilder
    {
        private ServiceDefinition _definition = new ServiceDefinition();

        /// <summary>
        /// Sets the image to start the container from.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#image"/>
        /// </summary>
        /// <param name="image">The image name, optionally including a tag or digest.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithImage(string image)
        {
            Guard.NotNullOrWhiteSpace(image, nameof(image));
            _definition = _definition with { Image = image };
            return this;
        }

        /// <summary>
        /// Sets the command as a single shell-form string, written to YAML as a string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#command"/>
        /// </summary>
        /// <param name="command">The command, for example <c>nginx -g "daemon off;"</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCommand(string command)
        {
            _definition = _definition with { Command = CommandLine.FromShell(command) };
            return this;
        }

        /// <summary>
        /// Sets the command as an exec-form list of arguments, written to YAML as a list.
        /// An empty list clears the command defined by the image.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#command"/>
        /// </summary>
        /// <param name="arguments">The arguments, for example <c>nginx</c>, <c>-g</c>, <c>daemon off;</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCommand(IEnumerable<string> arguments)
        {
            _definition = _definition with { Command = CommandLine.FromArguments(arguments) };
            return this;
        }

        /// <summary>
        /// Sets a custom container name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#container_name"/>
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithContainerName(string containerName)
        {
            Guard.NotNullOrWhiteSpace(containerName, nameof(containerName));
            _definition = _definition with { ContainerName = containerName };
            return this;
        }

        /// <summary>
        /// Sets the entrypoint as a single shell-form string, written to YAML as a string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint"/>
        /// </summary>
        /// <param name="entrypoint">The entrypoint, for example <c>nginx -g "daemon off;"</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEntrypoint(string entrypoint)
        {
            _definition = _definition with { Entrypoint = CommandLine.FromShell(entrypoint) };
            return this;
        }

        /// <summary>
        /// Sets the entrypoint as an exec-form list of arguments, written to YAML as a list.
        /// An empty list clears the entrypoint defined by the image.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint"/>
        /// </summary>
        /// <param name="arguments">The arguments, for example <c>nginx</c>, <c>-g</c>, <c>daemon off;</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEntrypoint(IEnumerable<string> arguments)
        {
            _definition = _definition with { Entrypoint = CommandLine.FromArguments(arguments) };
            return this;
        }

        /// <summary>
        /// Adds a port mapping in short syntax, written to YAML as a string. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="port">The mapping, for example <c>8080:80</c> or <c>127.0.0.1:8080:80/udp</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(string port)
        {
            return AddPort(PortMapping.FromShortSyntax(port));
        }

        /// <summary>
        /// Adds a container port with no published port, so the runtime assigns a host port, written to YAML
        /// in long syntax. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="target">The container port, from 1 to 65535.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(int target)
        {
            return AddPort(PortMapping.FromDefinition(new PortDefinition(target)));
        }

        /// <summary>
        /// Adds a port mapping from a published and a target port, written to YAML in long syntax.
        /// Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="published">The publicly exposed port, from 1 to 65535.</param>
        /// <param name="target">The container port, from 1 to 65535.</param>
        /// <param name="protocol">The protocol, or <c>null</c> to leave it unset.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(int published, int target, PortProtocol? protocol = null)
        {
            Guard.ValidPort(published, nameof(published));

            PortDefinition definition = new PortDefinition(target)
            {
                Protocol = protocol,
                Published = published.ToString(CultureInfo.InvariantCulture)
            };

            return AddPort(PortMapping.FromDefinition(definition));
        }

        /// <summary>
        /// Adds a port mapping from an existing long-syntax definition. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="port">The port definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(PortDefinition port)
        {
            return AddPort(PortMapping.FromDefinition(port));
        }

        /// <summary>
        /// Adds a port mapping configured through a <see cref="PortBuilder"/>. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="configure">Configures the port.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(Action<PortBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            PortBuilder builder = new PortBuilder();
            configure(builder);
            return AddPort(PortMapping.FromDefinition(builder.Build()));
        }

        /// <summary>
        /// Adds several port mappings in short syntax.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="ports">The mappings, for example <c>8080:80</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPorts(IEnumerable<string> ports)
        {
            Guard.NotNull(ports, nameof(ports));

            foreach (string port in ports)
            {
                WithPort(port);
            }

            return this;
        }

        /// <summary>
        /// Adds several port mappings from existing long-syntax definitions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="ports">The port definitions.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPorts(IEnumerable<PortDefinition> ports)
        {
            Guard.NotNull(ports, nameof(ports));

            foreach (PortDefinition port in ports)
            {
                WithPort(port);
            }

            return this;
        }

        /// <summary>
        /// Adds a mount in short syntax, written to YAML as a string. Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mount">The mount, for example <c>db-data:/var/lib/db</c> or <c>./src:/app:ro</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(string mount)
        {
            return AddVolume(MountMapping.FromShortSyntax(mount));
        }

        /// <summary>
        /// Adds a mount from a source and a target, written to YAML in short syntax
        /// (<c>source:target</c> or <c>source:target:ro</c>) so that Compose decides whether the source is a
        /// named volume or a host path. Use <see cref="WithVolume(Action{MountBuilder})"/> to state the type.
        /// Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="source">A volume name or a host path.</param>
        /// <param name="target">The path in the container.</param>
        /// <param name="readOnly"><c>true</c> to mount read-only.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(string source, string target, bool readOnly = false)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            Guard.NotNullOrWhiteSpace(target, nameof(target));

            string mount = readOnly ? source + ":" + target + ":ro" : source + ":" + target;
            return AddVolume(MountMapping.FromShortSyntax(mount));
        }

        /// <summary>
        /// Adds a mount from an existing long-syntax definition. Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mount">The mount definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(MountDefinition mount)
        {
            return AddVolume(MountMapping.FromDefinition(mount));
        }

        /// <summary>
        /// Adds a mount configured through a <see cref="MountBuilder"/>. Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="configure">Configures the mount.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(Action<MountBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            MountBuilder builder = new MountBuilder();
            configure(builder);
            return AddVolume(MountMapping.FromDefinition(builder.Build()));
        }

        /// <summary>
        /// Adds several mounts in short syntax.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mounts">The mounts, for example <c>db-data:/var/lib/db</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolumes(IEnumerable<string> mounts)
        {
            Guard.NotNull(mounts, nameof(mounts));

            foreach (string mount in mounts)
            {
                WithVolume(mount);
            }

            return this;
        }

        /// <summary>
        /// Adds several mounts from existing long-syntax definitions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mounts">The mount definitions.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolumes(IEnumerable<MountDefinition> mounts)
        {
            Guard.NotNull(mounts, nameof(mounts));

            foreach (MountDefinition mount in mounts)
            {
                WithVolume(mount);
            }

            return this;
        }

        /// <summary>
        /// Attaches the service to a network with no settings. When no attached network has settings, the
        /// networks are written to YAML as a plain list of names. Attaching the same network again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="name">The name of a network defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetwork(string name)
        {
            return AddNetwork(name, new NetworkAttachment());
        }

        /// <summary>
        /// Attaches the service to a network with existing settings. Any network with settings makes the
        /// networks be written to YAML as a mapping. Attaching the same network again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="name">The name of a network defined at the top level.</param>
        /// <param name="attachment">The settings for this network.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetwork(string name, NetworkAttachment attachment)
        {
            Guard.NotNull(attachment, nameof(attachment));

            return AddNetwork(name, attachment);
        }

        /// <summary>
        /// Attaches the service to a network with settings configured through a
        /// <see cref="NetworkAttachmentBuilder"/>. Attaching the same network again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="name">The name of a network defined at the top level.</param>
        /// <param name="configure">Configures the settings for this network.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetwork(string name, Action<NetworkAttachmentBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            NetworkAttachmentBuilder builder = new NetworkAttachmentBuilder();
            configure(builder);
            return AddNetwork(name, builder.Build());
        }

        /// <summary>
        /// Attaches the service to several networks with no settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="names">The names of networks defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetworks(IEnumerable<string> names)
        {
            Guard.NotNull(names, nameof(names));

            foreach (string name in names)
            {
                WithNetwork(name);
            }

            return this;
        }

        /// <summary>
        /// Sets an environment variable. Setting the same name again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value, which may be empty.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, string value)
        {
            Guard.NotNull(value, nameof(value));

            return SetEnvironment(key, value, nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to <c>true</c> or <c>false</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, bool value)
        {
            return SetEnvironment(key, value ? "true" : "false", nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to a number.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, int value)
        {
            return SetEnvironment(key, value.ToString(CultureInfo.InvariantCulture), nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to a number.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, long value)
        {
            return SetEnvironment(key, value.ToString(CultureInfo.InvariantCulture), nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to a number, written with a <c>.</c> as the decimal separator.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, double value)
        {
            return SetEnvironment(key, value.ToString("R", CultureInfo.InvariantCulture), nameof(key));
        }

        /// <summary>
        /// Declares an environment variable with no value, so Compose takes it from the environment of the
        /// machine running it. If it is not set there, the variable is removed from the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key)
        {
            return SetEnvironment(key, null, nameof(key));
        }

        /// <summary>
        /// Sets several environment variables from name and value pairs.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="variables">The variables.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(IEnumerable<KeyValuePair<string, string>> variables)
        {
            Guard.NotNull(variables, nameof(variables));

            foreach (KeyValuePair<string, string> variable in variables)
            {
                WithEnvironment(variable.Key, variable.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets several environment variables from <c>KEY=value</c> strings, or a bare <c>KEY</c> for a
        /// variable with no value. Giving a list makes the YAML use the list form (<c>- KEY=value</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(IEnumerable<string> entries)
        {
            Guard.NotNull(entries, nameof(entries));

            foreach (string entry in entries)
            {
                Guard.NotNullOrWhiteSpace(entry, nameof(entries));

                int separator = entry.IndexOf('=');
                if (separator < 0)
                {
                    SetEnvironment(entry, null, nameof(entries));
                }
                else
                {
                    SetEnvironment(entry.Substring(0, separator), entry.Substring(separator + 1), nameof(entries));
                }
            }

            _definition = _definition with { Environment = _definition.Environment with { UsesListSyntax = true } };
            return this;
        }

        /// <summary>
        /// Creates the service definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ServiceDefinition"/>.</returns>
        public ServiceDefinition Build()
        {
            return _definition;
        }

        private ServiceBuilder AddPort(PortMapping port)
        {
            List<PortMapping> ports = new List<PortMapping>(_definition.Ports) { port };
            _definition = _definition with { Ports = ports.AsReadOnly() };
            return this;
        }

        private ServiceBuilder AddVolume(MountMapping mount)
        {
            List<MountMapping> volumes = new List<MountMapping>(_definition.Volumes) { mount };
            _definition = _definition with { Volumes = volumes.AsReadOnly() };
            return this;
        }

        private ServiceBuilder AddNetwork(string name, NetworkAttachment attachment)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));

            _definition = _definition with { Networks = Collections.With(_definition.Networks, name, attachment) };
            return this;
        }

        private ServiceBuilder SetEnvironment(string key, string? value, string parameterName)
        {
            Guard.EnvironmentKey(key, parameterName);

            EnvironmentVariables environment = _definition.Environment;
            _definition = _definition with
            {
                Environment = environment with { Variables = Collections.With(environment.Variables, key, value) }
            };
            return this;
        }
    }
}
