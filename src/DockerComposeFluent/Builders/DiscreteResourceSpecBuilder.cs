using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DiscreteResourceSpecDefinition"/>. A kind and a value are required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed class DiscreteResourceSpecBuilder
    {
        private DiscreteResourceSpecDefinition _definition = new DiscreteResourceSpecDefinition();

        /// <summary>
        /// Sets the kind of resource.
        /// </summary>
        /// <param name="kind">The kind, for example <c>GPU</c> or <c>SSD</c>.</param>
        /// <returns>This builder.</returns>
        public DiscreteResourceSpecBuilder WithKind(string kind)
        {
            Guard.NotNullOrWhiteSpace(kind, nameof(kind));
            _definition = _definition with { Kind = kind };
            return this;
        }

        /// <summary>
        /// Sets the number of resources of this kind to reserve.
        /// </summary>
        /// <param name="value">The number, which must be positive.</param>
        /// <returns>This builder.</returns>
        public DiscreteResourceSpecBuilder WithValue(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The number of resources must be positive.");
            }

            _definition = _definition with { Value = value };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this discrete resource spec. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public DiscreteResourceSpecBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the specification from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DiscreteResourceSpecDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">The kind or value was not set.</exception>
        public DiscreteResourceSpecDefinition Build()
        {
            if (_definition.Kind == null || _definition.Value == null)
            {
                throw new InvalidOperationException("A discrete resource spec requires a kind and a value.");
            }

            return _definition;
        }
    }
}
