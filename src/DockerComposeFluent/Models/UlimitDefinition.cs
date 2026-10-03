using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// One resource limit for processes in the container (an entry of <c>services.&lt;name&gt;.ulimits</c>):
    /// a single value for both limits, written as a plain number, or separate soft and hard limits.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ulimits"/>
    /// </summary>
    public sealed record UlimitDefinition
    {
        /// <summary>
        /// The single value used for both the soft and hard limit. Not used together with
        /// <see cref="Soft"/> and <see cref="Hard"/>.
        /// </summary>
        public int? Single { get; init; }

        /// <summary>
        /// The soft limit, the value actually enforced, as specified by <c>soft</c>.
        /// </summary>
        public int? Soft { get; init; }

        /// <summary>
        /// The hard limit, the maximum allowed value, as specified by <c>hard</c>.
        /// </summary>
        public int? Hard { get; init; }

        /// <summary>
        /// The extension fields defined on this {0}, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();

        /// <summary>
        /// Whether the limit is written as a plain number: only <see cref="Single"/> is set.
        /// </summary>
        public bool IsSingle
        {
            get { return Single != null && Extensions.Count == 0; }
        }
    }
}
