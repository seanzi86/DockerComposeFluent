using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="CredentialSpecDefinition"/>. Exactly one source is required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#credential_spec"/>
    /// </summary>
    public sealed class CredentialSpecBuilder
    {
        private CredentialSpecDefinition _definition = new CredentialSpecDefinition();

        /// <summary>
        /// Sets the name of the credential spec config to use.
        /// </summary>
        /// <param name="config">The config name.</param>
        /// <returns>This builder.</returns>
        public CredentialSpecBuilder WithConfig(string config)
        {
            Guard.NotNullOrWhiteSpace(config, nameof(config));
            _definition = _definition with { Config = config };
            return this;
        }

        /// <summary>
        /// Sets the path to a credential spec file.
        /// </summary>
        /// <param name="file">The file path.</param>
        /// <returns>This builder.</returns>
        public CredentialSpecBuilder WithFile(string file)
        {
            Guard.NotNullOrWhiteSpace(file, nameof(file));
            _definition = _definition with { File = file };
            return this;
        }

        /// <summary>
        /// Sets the path to a credential spec in the Windows registry.
        /// </summary>
        /// <param name="registry">The registry path.</param>
        /// <returns>This builder.</returns>
        public CredentialSpecBuilder WithRegistry(string registry)
        {
            Guard.NotNullOrWhiteSpace(registry, nameof(registry));
            _definition = _definition with { Registry = registry };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this credential spec. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public CredentialSpecBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the credential spec from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="CredentialSpecDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">Not exactly one of config, file and registry was set.</exception>
        public CredentialSpecDefinition Build()
        {
            int sources = (_definition.Config != null ? 1 : 0) + (_definition.File != null ? 1 : 0) + (_definition.Registry != null ? 1 : 0);
            if (sources != 1)
            {
                throw new InvalidOperationException("A credential spec must set exactly one of WithConfig, WithFile or WithRegistry.");
            }

            return _definition;
        }
    }
}
