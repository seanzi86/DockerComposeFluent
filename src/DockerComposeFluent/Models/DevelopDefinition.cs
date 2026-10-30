using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Settings that help while developing a service (the <c>develop</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md"/>
    /// </summary>
    public sealed record DevelopDefinition
    {
        /// <summary>
        /// The rules that update the service when local files change, as specified by <c>watch</c>. Empty
        /// when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#watch"/>
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        public IReadOnlyList<WatchRuleDefinition> Watch
        {
            get { return _watch; }
            init { _watch = Collections.Seal(value); }
        }

        private IReadOnlyList<WatchRuleDefinition> _watch = Collections.EmptyList<WatchRuleDefinition>();

        /// <summary>
        /// The extension fields defined on this develop section, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions
        {
            get { return _extensions; }
            init { _extensions = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, object?> _extensions = Collections.EmptyDictionary<object?>();
    }
}
