using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A user-defined resource to reserve (an entry of <c>deploy.resources.reservations.generic_resources</c>).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed record GenericResourceDefinition
    {
        /// <summary>
        /// The countable resource to reserve, as specified by <c>discrete_resource_spec</c>.
        /// </summary>
        public DiscreteResourceSpecDefinition? DiscreteResourceSpec { get; init; }

        /// <summary>
        /// The extension fields defined on this generic resource, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
