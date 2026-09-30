using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="LoggingDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging"/>
    /// </summary>
    public sealed class LoggingBuilder
    {
        private LoggingDefinition _logging = new LoggingDefinition();

        /// <summary>
        /// Sets the logging driver. The default and the available drivers are platform-specific.
        /// </summary>
        /// <param name="driver">The driver name, for example <c>json-file</c> or <c>syslog</c>.</param>
        /// <returns>This builder.</returns>
        public LoggingBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _logging = _logging with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets a driver-specific option. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public LoggingBuilder WithOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _logging = _logging with { Options = Collections.With(_logging.Options, key, value) };
            return this;
        }

        /// <summary>
        /// Sets several driver-specific options at once. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public LoggingBuilder WithOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (KeyValuePair<string, string> option in options)
            {
                WithOption(option.Key, option.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets an extension field on this logging configuration. Setting the same key again replaces its
        /// value. Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public LoggingBuilder WithExtension(string key, object? value)
        {
            _logging = _logging with { Extensions = ExtensionsMutator.Set(_logging.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the logging definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="LoggingDefinition"/>.</returns>
        public LoggingDefinition Build()
        {
            return _logging;
        }
    }
}
