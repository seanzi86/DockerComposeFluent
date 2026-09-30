using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="VolumeDefinition"/>.
    /// </summary>
    public sealed class VolumeBuilder
    {
        private VolumeDefinition _definition = new VolumeDefinition();

        /// <summary>
        /// Sets a label, keeping the mapping form. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#labels"/>
        /// </summary>
        /// <param name="key">The label key, which must not start with the reserved <c>com.docker.compose</c>
        /// prefix.</param>
        /// <param name="value">The label value, which may be empty.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithLabel(string key, string value)
        {
            _definition = _definition with { Labels = LabelsMutator.Set(_definition.Labels, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Sets several labels from key/value pairs, keeping the mapping form.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#labels"/>
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithLabels(IEnumerable<KeyValuePair<string, string>> labels)
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
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#labels"/>
        /// </summary>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithLabels(IEnumerable<string> entries)
        {
            _definition = _definition with { Labels = LabelsMutator.SetFromEntries(_definition.Labels, entries, nameof(entries)) };
            return this;
        }

        /// <summary>
        /// Sets the volume driver.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver"/>
        /// </summary>
        /// <param name="driver">The driver name.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _definition = _definition with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets a driver-specific option. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver_opts"/>
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithDriverOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _definition = _definition with { DriverOptions = Collections.With(_definition.DriverOptions, key, value) };
            return this;
        }

        /// <summary>
        /// Sets several driver-specific options at once. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver_opts"/>
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithDriverOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (KeyValuePair<string, string> option in options)
            {
                WithDriverOption(option.Key, option.Value);
            }

            return this;
        }

        /// <summary>
        /// Marks this volume as already created outside Compose.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#external"/>
        /// </summary>
        /// <param name="external"><c>true</c> if the volume already exists.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithExternal(bool external)
        {
            _definition = _definition with { External = external };
            return this;
        }

        /// <summary>
        /// Sets the actual Docker volume name, overriding the name generated from the project name and key.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#name"/>
        /// </summary>
        /// <param name="name">The volume name.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            _definition = _definition with { Name = name };
            return this;
        }

        /// <summary>
        /// Creates the volume definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="VolumeDefinition"/>.</returns>
        public VolumeDefinition Build()
        {
            return _definition;
        }
    }
}
