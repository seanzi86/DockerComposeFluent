using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A command run at a point in a container's lifecycle (an entry of <c>post_start</c> or <c>pre_stop</c>).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#post_start"/>
    /// </summary>
    public sealed record ServiceHookDefinition
    {
        /// <summary>
        /// The command to run, as specified by <c>command</c>: a shell-form string or an exec-form list.
        /// </summary>
        public CommandLine? Command { get; init; }

        /// <summary>
        /// The user to run the command as, as specified by <c>user</c>.
        /// </summary>
        public string? User { get; init; }

        /// <summary>
        /// Whether the command runs with extended privileges, as specified by <c>privileged</c>.
        /// </summary>
        public bool? Privileged { get; init; }

        /// <summary>
        /// The working directory for the command, as specified by <c>working_dir</c>.
        /// </summary>
        public string? WorkingDir { get; init; }

        /// <summary>
        /// The environment variables for the command, as specified by <c>environment</c>. Empty when none are
        /// set.
        /// </summary>
        public IReadOnlyDictionary<string, string> Environment { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The extension fields defined on this hook, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
