using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Constraints and preferences for the platform's choice of node to run a service's containers (the
    /// <c>deploy.placement</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#placement"/>
    /// </summary>
    public sealed record PlacementDefinition
    {
        /// <summary>
        /// The properties a node must have to run the service, as specified by <c>constraints</c>, for example
        /// <c>disktype=ssd</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyList<string> Constraints { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The node label values to spread tasks evenly across, as specified by <c>preferences</c> (each
        /// written as a <c>spread</c> entry - currently the only supported strategy). Empty when none are set.
        /// </summary>
        public IReadOnlyList<string> Preferences { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The maximum number of replicas of the service on a single node, as specified by
        /// <c>max_replicas_per_node</c>.
        /// </summary>
        public int? MaxReplicasPerNode { get; init; }
    }
}
