using System.Collections.Generic;
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
        /// Creates the service definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ServiceDefinition"/>.</returns>
        public ServiceDefinition Build()
        {
            return _definition;
        }
    }
}
