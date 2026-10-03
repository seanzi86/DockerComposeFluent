using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ConfigDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
    /// </summary>
    public sealed class ConfigBuilder
    {
        private ConfigDefinition _definition = new ConfigDefinition();

        /// <summary>
        /// Sets the file whose contents become the config.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>This builder.</returns>
        public ConfigBuilder WithFile(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { File = path };
            return this;
        }

        /// <summary>
        /// Sets the environment variable whose value becomes the config.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>This builder.</returns>
        public ConfigBuilder WithEnvironment(string variable)
        {
            Guard.NotNullOrWhiteSpace(variable, nameof(variable));
            _definition = _definition with { Environment = variable };
            return this;
        }

        /// <summary>
        /// Sets the inline text that becomes the config.
        /// </summary>
        /// <remarks>Requires Compose 2.23.1 or later.</remarks>
        /// <param name="content">The inline text.</param>
        /// <returns>This builder.</returns>
        public ConfigBuilder WithContent(string content)
        {
            Guard.NotNull(content, nameof(content));
            _definition = _definition with { Content = content };
            return this;
        }

        /// <summary>
        /// Marks this config as already created outside Compose.
        /// </summary>
        /// <param name="external"><c>true</c> if the config already exists.</param>
        /// <returns>This builder.</returns>
        public ConfigBuilder WithExternal(bool external)
        {
            _definition = _definition with { External = external };
            return this;
        }

        /// <summary>
        /// Sets the actual name of the config object to look up.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>This builder.</returns>
        public ConfigBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            _definition = _definition with { Name = name };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this config. Setting the same key again replaces its value. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public ConfigBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the config from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ConfigDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">Neither a source nor <c>external</c> is set, more than
        /// one source is set, or <c>external</c> is combined with a source.</exception>
        public ConfigDefinition Build()
        {
            int sources = (_definition.File != null ? 1 : 0)
                + (_definition.Environment != null ? 1 : 0)
                + (_definition.Content != null ? 1 : 0);

            if (_definition.External == true)
            {
                if (sources > 0)
                {
                    throw new InvalidOperationException("An external config must not also set a file, environment or content source.");
                }
            }
            else if (sources != 1)
            {
                throw new InvalidOperationException("A config must set exactly one of WithFile, WithEnvironment or WithContent, or be marked WithExternal(true).");
            }

            return _definition;
        }
    }
}
