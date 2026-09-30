using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Deployment metadata for a service, so a platform such as Docker Swarm can allocate and configure
    /// resources for it (the <c>deploy</c> entry). Compose itself only observes <see cref="Resources"/> and
    /// <see cref="RestartPolicy"/> outside a Swarm context.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md"/>
    /// </summary>
    public sealed record DeployDefinition
    {
        /// <summary>
        /// The replication model, as specified by <c>mode</c>.
        /// </summary>
        public DeployMode? Mode { get; init; }

        /// <summary>
        /// The service discovery method for external clients, as specified by <c>endpoint_mode</c>.
        /// </summary>
        public EndpointMode? EndpointMode { get; init; }

        /// <summary>
        /// The number of containers to run, when <c>Mode</c> is <c>DeployMode.Replicated</c> (the default), as
        /// specified by <c>replicas</c>.
        /// </summary>
        public int? Replicas { get; init; }

        /// <summary>
        /// The labels attached to the service on the platform, not to any of its containers, as specified by
        /// <c>labels</c>.
        /// </summary>
        public Labels Labels { get; init; } = new Labels();

        /// <summary>
        /// Constraints and preferences for the platform's choice of node, as specified by <c>placement</c>.
        /// </summary>
        public PlacementDefinition? Placement { get; init; }

        /// <summary>
        /// The physical resource constraints and reservations for the containers, as specified by
        /// <c>resources</c>.
        /// </summary>
        public ResourcesDefinition? Resources { get; init; }

        /// <summary>
        /// How and whether to restart the containers when they exit, as specified by <c>restart_policy</c>.
        /// </summary>
        public DeployRestartPolicyDefinition? RestartPolicy { get; init; }

        /// <summary>
        /// How to roll the service back after a failed update, as specified by <c>rollback_config</c>.
        /// </summary>
        public RollbackConfigDefinition? RollbackConfig { get; init; }

        /// <summary>
        /// How to roll out an update to the service, as specified by <c>update_config</c>.
        /// </summary>
        public UpdateConfigDefinition? UpdateConfig { get; init; }

        /// <summary>
        /// The extension fields defined on <c>deploy</c>, keyed by their <c>x-</c> name. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
