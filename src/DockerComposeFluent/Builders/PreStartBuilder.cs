using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="PreStartDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_start"/>
    /// </summary>
    public sealed class PreStartBuilder
    {
        private PreStartDefinition _definition = new PreStartDefinition();

        /// <summary>
        /// Sets the command as a single shell-form string, written to YAML as a string.
        /// </summary>
        /// <param name="command">The command, for example <c>./manage.py migrate</c>.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithCommand(string command)
        {
            _definition = _definition with { Command = CommandLine.FromShell(command) };
            return this;
        }

        /// <summary>
        /// Sets the command as an exec-form list of arguments, written to YAML as a list.
        /// </summary>
        /// <param name="arguments">The command and its arguments.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithCommand(IEnumerable<string> arguments)
        {
            _definition = _definition with { Command = CommandLine.FromArguments(arguments) };
            return this;
        }

        /// <summary>
        /// Sets the image for the step's container. The service's own image is used when this is not set.
        /// </summary>
        /// <param name="image">The image name, optionally including a tag or digest.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithImage(string image)
        {
            Guard.NotNullOrWhiteSpace(image, nameof(image));
            _definition = _definition with { Image = image };
            return this;
        }

        /// <summary>
        /// Sets the user to run the command as.
        /// </summary>
        /// <param name="user">The user name or ID.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithUser(string user)
        {
            Guard.NotNullOrWhiteSpace(user, nameof(user));
            _definition = _definition with { User = user };
            return this;
        }

        /// <summary>
        /// Sets whether the command runs with extended privileges.
        /// </summary>
        /// <param name="privileged"><c>true</c> to run it privileged.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithPrivileged(bool privileged)
        {
            _definition = _definition with { Privileged = privileged };
            return this;
        }

        /// <summary>
        /// Sets the working directory for the command.
        /// </summary>
        /// <param name="workingDir">The directory.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithWorkingDir(string workingDir)
        {
            Guard.NotNullOrWhiteSpace(workingDir, nameof(workingDir));
            _definition = _definition with { WorkingDir = workingDir };
            return this;
        }

        /// <summary>
        /// Sets an environment variable for this step, added to or overriding the service's own. Setting the
        /// same name again replaces it.
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithEnvironment(string key, string value)
        {
            Guard.EnvironmentKey(key, nameof(key));
            Guard.NotNull(value, nameof(value));
            _definition = _definition with { Environment = Collections.With(_definition.Environment, key, value) };
            return this;
        }

        /// <summary>
        /// Sets whether the step runs once for each replica of the service instead of once for the whole service.
        /// </summary>
        /// <param name="perReplica"><c>true</c> to run it for every replica.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithPerReplica(bool perReplica)
        {
            _definition = _definition with { PerReplica = perReplica };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this step. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public PreStartBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the step from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="PreStartDefinition"/>.</returns>
        public PreStartDefinition Build()
        {
            return _definition;
        }
    }
}
