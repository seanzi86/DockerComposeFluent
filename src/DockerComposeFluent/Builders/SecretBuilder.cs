using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="SecretDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
    /// </summary>
    public sealed class SecretBuilder
    {
        private SecretDefinition _definition = new SecretDefinition();

        /// <summary>
        /// Sets the file whose contents become the secret.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithFile(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { File = path };
            return this;
        }

        /// <summary>
        /// Sets the environment variable whose value becomes the secret.
        /// </summary>
        /// <remarks>Requires Compose 2.6.0 or later.</remarks>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithEnvironment(string variable)
        {
            Guard.NotNullOrWhiteSpace(variable, nameof(variable));
            _definition = _definition with { Environment = variable };
            return this;
        }

        /// <summary>
        /// Marks this secret as already created outside Compose.
        /// </summary>
        /// <param name="external"><c>true</c> if the secret already exists.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithExternal(bool external)
        {
            _definition = _definition with { External = external };
            return this;
        }

        /// <summary>
        /// Sets the actual name of the secret object to look up.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            _definition = _definition with { Name = name };
            return this;
        }

        /// <summary>
        /// Creates the secret from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="SecretDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">Neither a source nor <c>external</c> is set, more than
        /// one source is set, or <c>external</c> is combined with a source.</exception>
        public SecretDefinition Build()
        {
            int sources = (_definition.File != null ? 1 : 0) + (_definition.Environment != null ? 1 : 0);

            if (_definition.External == true)
            {
                if (sources > 0)
                {
                    throw new InvalidOperationException("An external secret must not also set a file or environment source.");
                }
            }
            else if (sources != 1)
            {
                throw new InvalidOperationException("A secret must set exactly one of WithFile or WithEnvironment, or be marked WithExternal(true).");
            }

            return _definition;
        }
    }
}
