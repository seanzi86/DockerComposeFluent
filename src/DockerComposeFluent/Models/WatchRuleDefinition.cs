using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// One rule of a service's <c>develop.watch</c> list: what to watch and what to do when it changes.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#watch"/>
    /// </summary>
    public sealed record WatchRuleDefinition
    {
        /// <summary>
        /// The path to watch, relative to the project directory, as specified by <c>path</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#path"/>
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        public string? Path { get; init; }

        /// <summary>
        /// What to do when a change is detected, as specified by <c>action</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#action"/>
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        public WatchAction? Action { get; init; }

        /// <summary>
        /// The path in the container that changed files are synchronised to, as specified by <c>target</c>.
        /// Used by the <c>sync</c> actions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#target"/>
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        public string? Target { get; init; }

        /// <summary>
        /// The patterns of paths that do not trigger the action, as specified by <c>ignore</c>. Empty when none
        /// are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#ignore"/>
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        public IReadOnlyList<string> Ignore
        {
            get { return _ignore; }
            init { _ignore = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _ignore = Collections.EmptyList<string>();

        /// <summary>
        /// The patterns of paths that are watched, as specified by <c>include</c>; only matching files trigger
        /// the action. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#include"/>
        /// </summary>
        /// <remarks>Requires Compose 2.34.0 or later.</remarks>
        public IReadOnlyList<string> Include
        {
            get { return _include; }
            init { _include = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _include = Collections.EmptyList<string>();

        /// <summary>
        /// The command to run in the container after synchronising, as specified by <c>exec</c>. Only used
        /// with <see cref="WatchAction.SyncAndExec"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#exec"/>
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        public ServiceHookDefinition? Exec { get; init; }

        /// <summary>
        /// Whether to check that files in existing containers are up to date when a watch session starts, as
        /// specified by <c>initial_sync</c>. Used by the <c>sync</c> actions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#initial_sync"/>
        /// </summary>
        /// <remarks>Requires Compose 2.39.4 or later.</remarks>
        public bool? InitialSync { get; init; }

        /// <summary>
        /// The extension fields defined on this watch rule, keyed by their <c>x-</c> name. Compose ignores these;
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
