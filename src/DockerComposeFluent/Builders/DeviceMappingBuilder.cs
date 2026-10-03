using System;
using System.Text.RegularExpressions;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DeviceMappingDefinition"/>. A source is required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#devices"/>
    /// </summary>
    public sealed class DeviceMappingBuilder
    {
        private DeviceMappingDefinition _definition = new DeviceMappingDefinition();

        /// <summary>
        /// Sets the path of the device on the host.
        /// </summary>
        /// <param name="source">The host path, for example <c>/dev/ttyUSB0</c>.</param>
        /// <returns>This builder.</returns>
        public DeviceMappingBuilder WithSource(string source)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            _definition = _definition with { Source = source };
            return this;
        }

        /// <summary>
        /// Sets the path the device is mapped to in the container. Defaults to the source path.
        /// </summary>
        /// <param name="target">The container path.</param>
        /// <returns>This builder.</returns>
        public DeviceMappingBuilder WithTarget(string target)
        {
            Guard.NotNullOrWhiteSpace(target, nameof(target));
            _definition = _definition with { Target = target };
            return this;
        }

        /// <summary>
        /// Sets the cgroup permissions for the device.
        /// </summary>
        /// <param name="permissions">A combination of <c>r</c>, <c>w</c> and <c>m</c>, such as <c>rwm</c>.</param>
        /// <returns>This builder.</returns>
        public DeviceMappingBuilder WithPermissions(string permissions)
        {
            if (permissions == null || !Regex.IsMatch(permissions, "^[rwm]{1,3}$"))
            {
                throw new ArgumentException($"'{permissions}' is not a set of device permissions. Use a combination of r, w and m.", nameof(permissions));
            }

            _definition = _definition with { Permissions = permissions };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this device mapping. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public DeviceMappingBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the device mapping from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DeviceMappingDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">No source was set.</exception>
        public DeviceMappingDefinition Build()
        {
            if (_definition.Source == null)
            {
                throw new InvalidOperationException("A device mapping requires a source. Call WithSource before Build.");
            }

            bool needsLongForm = _definition.Permissions != null || _definition.Extensions.Count > 0;
            if (needsLongForm && _definition.Target == null)
            {
                // Compose rejects a long-form device with no target, though the source is what it means by default.
                return _definition with { Target = _definition.Source };
            }

            return _definition;
        }
    }
}
