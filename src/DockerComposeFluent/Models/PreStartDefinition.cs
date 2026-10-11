using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// One init-container step of a service's <c>pre_start</c> list. Each step runs to completion in its own
    /// short-lived container, in the order declared, and the service only starts once every step has exited
    /// successfully. Unlike <c>post_start</c> and <c>pre_stop</c>, which run a command inside the running
    /// container, a step can use a different image.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_start"/>
    /// </summary>
    /// <remarks>
    /// Compose accepts any container attribute here and inherits the ones left out from the service; this type
    /// models the seven the specification documents.
    /// </remarks>
    public sealed record PreStartDefinition
    {
        /// <summary>
        /// The command to run, as specified by <c>command</c>: a shell-form string or an exec-form list. It can
        /// be left out when the image's entrypoint already runs the intended command.
        /// </summary>
        public CommandLine? Command { get; init; }

        /// <summary>
        /// The image for the step's container, as specified by <c>image</c>. The service's own image is used
        /// when this is not set.
        /// </summary>
        public string? Image { get; init; }

        /// <summary>
        /// The user to run the command as, as specified by <c>user</c>. Defaults to the image's user, or the
        /// service's when no image is set.
        /// </summary>
        public string? User { get; init; }

        /// <summary>
        /// Whether the command runs with extended privileges, as specified by <c>privileged</c>.
        /// </summary>
        public bool? Privileged { get; init; }

        /// <summary>
        /// The working directory for the command, as specified by <c>working_dir</c>. Defaults to the service's.
        /// </summary>
        public string? WorkingDir { get; init; }

        /// <summary>
        /// Environment variables added to, or overriding, the service's own for this step, as specified by
        /// <c>environment</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyDictionary<string, string> Environment { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// Whether the step runs once for each replica of the service instead of once for the whole service, as
        /// specified by <c>per_replica</c>. Use it for per-instance work such as initialising a replica's
        /// anonymous volumes.
        /// </summary>
        public bool? PerReplica { get; init; }

        /// <summary>
        /// The extension fields defined on this step, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
