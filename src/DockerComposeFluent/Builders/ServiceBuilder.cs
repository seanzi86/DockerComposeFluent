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
    }
}
