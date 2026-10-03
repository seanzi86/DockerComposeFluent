using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A countable user-defined resource to reserve (the <c>discrete_resource_spec</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed record DiscreteResourceSpecDefinition
    {
        /// <summary>
        /// The kind of resource, for example <c>GPU</c> or <c>SSD</c>, as specified by <c>kind</c>.
        /// </summary>
        public string? Kind { get; init; }

        /// <summary>
        /// The number of resources of this kind to reserve, as specified by <c>value</c>.
        /// </summary>
        public int? Value { get; init; }

        /// <summary>
        /// The extension fields defined on this discrete resource spec, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
