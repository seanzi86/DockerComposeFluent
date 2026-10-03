using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="UlimitDefinition"/>: either a single value or both a soft and a hard limit.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ulimits"/>
    /// </summary>
    public sealed class UlimitBuilder
    {
        private UlimitDefinition _definition = new UlimitDefinition();

        /// <summary>
        /// Sets one value used for both the soft and the hard limit.
        /// </summary>
        /// <param name="limit">The limit. <c>-1</c> means unlimited for most limits.</param>
        /// <returns>This builder.</returns>
        public UlimitBuilder WithLimit(int limit)
        {
            _definition = _definition with { Single = limit };
            return this;
        }

        /// <summary>
        /// Sets the soft limit, the value actually enforced.
        /// </summary>
        /// <param name="soft">The soft limit.</param>
        /// <returns>This builder.</returns>
        public UlimitBuilder WithSoft(int soft)
        {
            _definition = _definition with { Soft = soft };
            return this;
        }

        /// <summary>
        /// Sets the hard limit, the maximum allowed value.
        /// </summary>
        /// <param name="hard">The hard limit.</param>
        /// <returns>This builder.</returns>
        public UlimitBuilder WithHard(int hard)
        {
            _definition = _definition with { Hard = hard };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this ulimit. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public UlimitBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the ulimit from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="UlimitDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">Neither a single limit nor both soft and hard limits were set, or both forms were mixed.</exception>
        public UlimitDefinition Build()
        {
            bool single = _definition.Single != null && _definition.Soft == null && _definition.Hard == null;
            bool pair = _definition.Single == null && _definition.Soft != null && _definition.Hard != null;

            if (!single && !pair)
            {
                throw new InvalidOperationException("A ulimit must set either WithLimit, or both WithSoft and WithHard.");
            }

            return _definition;
        }
    }
}
