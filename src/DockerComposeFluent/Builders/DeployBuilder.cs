using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DeployDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md"/>
    /// </summary>
    public sealed class DeployBuilder
    {
        private DeployDefinition _definition = new DeployDefinition();

        /// <summary>
        /// Sets the replication model.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithMode(DeployMode mode)
        {
            if (!Enum.IsDefined(typeof(DeployMode), mode))
            {
                throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown DeployMode value.");
            }

            _definition = _definition with { Mode = mode };
            return this;
        }

        /// <summary>
        /// Sets the service discovery method for external clients.
        /// </summary>
        /// <param name="endpointMode">The mode.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithEndpointMode(EndpointMode endpointMode)
        {
            if (!Enum.IsDefined(typeof(EndpointMode), endpointMode))
            {
                throw new ArgumentOutOfRangeException(nameof(endpointMode), endpointMode, "Unknown EndpointMode value.");
            }

            _definition = _definition with { EndpointMode = endpointMode };
            return this;
        }

        /// <summary>
        /// Sets the number of containers to run, when the mode is <see cref="DeployMode.Replicated"/> (the
        /// default).
        /// </summary>
        /// <param name="replicas">The count, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithReplicas(int replicas)
        {
            if (replicas < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(replicas), replicas, "The number of replicas must not be negative.");
            }

            _definition = _definition with { Replicas = replicas };
            return this;
        }

        /// <summary>
        /// Sets a label attached to the service on the platform, not to any of its containers. Setting the
        /// same key again replaces its value.
        /// </summary>
        /// <param name="key">The label key.</param>
        /// <param name="value">The label value.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithLabel(string key, string value)
        {
            _definition = _definition with { Labels = LabelsMutator.Set(_definition.Labels, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Sets several labels from key/value pairs.
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithLabels(IEnumerable<KeyValuePair<string, string>> labels)
        {
            Guard.NotNull(labels, nameof(labels));

            foreach (KeyValuePair<string, string> label in labels)
            {
                WithLabel(label.Key, label.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets constraints and preferences for the platform's choice of node, from an existing definition.
        /// </summary>
        /// <param name="placement">The placement definition.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithPlacement(PlacementDefinition placement)
        {
            Guard.NotNull(placement, nameof(placement));
            _definition = _definition with { Placement = placement };
            return this;
        }

        /// <summary>
        /// Sets constraints and preferences for the platform's choice of node, configured through a
        /// <see cref="PlacementBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the placement.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithPlacement(Action<PlacementBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            PlacementBuilder builder = new PlacementBuilder();
            configure(builder);
            return WithPlacement(builder.Build());
        }

        /// <summary>
        /// Sets the physical resource constraints and reservations for the containers, from an existing
        /// definition.
        /// </summary>
        /// <param name="resources">The resources definition.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithResources(ResourcesDefinition resources)
        {
            Guard.NotNull(resources, nameof(resources));
            _definition = _definition with { Resources = resources };
            return this;
        }

        /// <summary>
        /// Sets the physical resource constraints and reservations for the containers, configured through a
        /// <see cref="ResourcesBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the resources.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithResources(Action<ResourcesBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ResourcesBuilder builder = new ResourcesBuilder();
            configure(builder);
            return WithResources(builder.Build());
        }

        /// <summary>
        /// Sets how and whether the containers are restarted when they exit, from an existing definition.
        /// </summary>
        /// <param name="restartPolicy">The restart policy definition.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithRestartPolicy(DeployRestartPolicyDefinition restartPolicy)
        {
            Guard.NotNull(restartPolicy, nameof(restartPolicy));
            _definition = _definition with { RestartPolicy = restartPolicy };
            return this;
        }

        /// <summary>
        /// Sets how and whether the containers are restarted when they exit, configured through a
        /// <see cref="DeployRestartPolicyBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the restart policy.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithRestartPolicy(Action<DeployRestartPolicyBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            DeployRestartPolicyBuilder builder = new DeployRestartPolicyBuilder();
            configure(builder);
            return WithRestartPolicy(builder.Build());
        }

        /// <summary>
        /// Sets how to roll the service back after a failed update, from an existing definition.
        /// </summary>
        /// <param name="rollbackConfig">The rollback configuration.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithRollbackConfig(RollbackConfigDefinition rollbackConfig)
        {
            Guard.NotNull(rollbackConfig, nameof(rollbackConfig));
            _definition = _definition with { RollbackConfig = rollbackConfig };
            return this;
        }

        /// <summary>
        /// Sets how to roll the service back after a failed update, configured through a
        /// <see cref="RollbackConfigBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the rollback configuration.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithRollbackConfig(Action<RollbackConfigBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            RollbackConfigBuilder builder = new RollbackConfigBuilder();
            configure(builder);
            return WithRollbackConfig(builder.Build());
        }

        /// <summary>
        /// Sets how to roll out an update to the service, from an existing definition.
        /// </summary>
        /// <param name="updateConfig">The update configuration.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithUpdateConfig(UpdateConfigDefinition updateConfig)
        {
            Guard.NotNull(updateConfig, nameof(updateConfig));
            _definition = _definition with { UpdateConfig = updateConfig };
            return this;
        }

        /// <summary>
        /// Sets how to roll out an update to the service, configured through an
        /// <see cref="UpdateConfigBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the update configuration.</param>
        /// <returns>This builder.</returns>
        public DeployBuilder WithUpdateConfig(Action<UpdateConfigBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            UpdateConfigBuilder builder = new UpdateConfigBuilder();
            configure(builder);
            return WithUpdateConfig(builder.Build());
        }

        /// <summary>
        /// Creates the deploy definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DeployDefinition"/>.</returns>
        public DeployDefinition Build()
        {
            return _definition;
        }
    }
}
