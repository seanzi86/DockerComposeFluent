using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="GenericResourceDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed class GenericResourceBuilder
    {
        private GenericResourceDefinition _definition = new GenericResourceDefinition();

        /// <summary>
        /// Sets the countable resource to reserve.
        /// </summary>
        /// <param name="kind">The kind of resource, for example <c>GPU</c>.</param>
        /// <param name="value">The number to reserve, which must be positive.</param>
        /// <returns>This builder.</returns>
        public GenericResourceBuilder WithDiscreteResourceSpec(string kind, int value)
        {
            return WithDiscreteResourceSpec(new DiscreteResourceSpecBuilder().WithKind(kind).WithValue(value).Build());
        }

        /// <summary>
        /// Sets the countable resource to reserve, from an existing definition.
        /// </summary>
        /// <param name="spec">The specification.</param>
        /// <returns>This builder.</returns>
        public GenericResourceBuilder WithDiscreteResourceSpec(DiscreteResourceSpecDefinition spec)
        {
            Guard.NotNull(spec, nameof(spec));
            _definition = _definition with { DiscreteResourceSpec = spec };
            return this;
        }

        /// <summary>
        /// Sets the countable resource to reserve, configured through a <see cref="DiscreteResourceSpecBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the specification.</param>
        /// <returns>This builder.</returns>
        public GenericResourceBuilder WithDiscreteResourceSpec(Action<DiscreteResourceSpecBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            DiscreteResourceSpecBuilder builder = new DiscreteResourceSpecBuilder();
            configure(builder);
            return WithDiscreteResourceSpec(builder.Build());
        }

        /// <summary>
        /// Sets an extension field on this generic resource. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public GenericResourceBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the generic resource from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="GenericResourceDefinition"/>.</returns>
        public GenericResourceDefinition Build()
        {
            return _definition;
        }
    }
}
