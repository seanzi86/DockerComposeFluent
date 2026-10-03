using System;
using System.Collections.Generic;
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
        /// Sets the driver that provides this secret.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="driver">The driver name.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _definition = _definition with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets a driver-specific option. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithDriverOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));
            _definition = _definition with { DriverOptions = Collections.With(_definition.DriverOptions, key, value) };
            return this;
        }

        /// <summary>
        /// Sets a label, keeping the mapping form. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="key">The label key, which must not start with the reserved <c>com.docker.compose</c> prefix.</param>
        /// <param name="value">The label value, which may be empty.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithLabel(string key, string value)
        {
            _definition = _definition with { Labels = LabelsMutator.Set(_definition.Labels, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Sets several labels from key/value pairs, keeping the mapping form.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithLabels(IEnumerable<KeyValuePair<string, string>> labels)
        {
            Guard.NotNull(labels, nameof(labels));

            foreach (KeyValuePair<string, string> label in labels)
            {
                WithLabel(label.Key, label.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets several labels from <c>KEY=value</c> strings, or a bare <c>KEY</c> for an empty value. Makes the
        /// YAML use the list form (<c>- KEY=value</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithLabels(IEnumerable<string> entries)
        {
            _definition = _definition with { Labels = LabelsMutator.SetFromEntries(_definition.Labels, entries, nameof(entries)) };
            return this;
        }

        /// <summary>
        /// Sets the driver used to template the secret's value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <param name="templateDriver">The template driver name.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithTemplateDriver(string templateDriver)
        {
            Guard.NotNullOrWhiteSpace(templateDriver, nameof(templateDriver));
            _definition = _definition with { TemplateDriver = templateDriver };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this secret. Setting the same key again replaces its value. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public SecretBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
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
