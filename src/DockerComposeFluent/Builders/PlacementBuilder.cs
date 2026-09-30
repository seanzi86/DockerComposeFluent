using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="PlacementDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#placement"/>
    /// </summary>
    public sealed class PlacementBuilder
    {
        private PlacementDefinition _definition = new PlacementDefinition();

        /// <summary>
        /// Adds a property a node must have to run the service.
        /// </summary>
        /// <param name="constraint">The constraint, for example <c>disktype=ssd</c>.</param>
        /// <returns>This builder.</returns>
        public PlacementBuilder WithConstraint(string constraint)
        {
            Guard.NotNullOrWhiteSpace(constraint, nameof(constraint));
            _definition = _definition with { Constraints = Collections.Append(_definition.Constraints, constraint) };
            return this;
        }

        /// <summary>
        /// Adds several properties a node must have to run the service.
        /// </summary>
        /// <param name="constraints">The constraints.</param>
        /// <returns>This builder.</returns>
        public PlacementBuilder WithConstraints(IEnumerable<string> constraints)
        {
            Guard.NotNull(constraints, nameof(constraints));

            foreach (string constraint in constraints)
            {
                WithConstraint(constraint);
            }

            return this;
        }

        /// <summary>
        /// Adds a node label to spread tasks evenly across, written as a <c>spread</c> preference.
        /// </summary>
        /// <param name="nodeLabel">The node label, for example <c>node.labels.zone</c>.</param>
        /// <returns>This builder.</returns>
        public PlacementBuilder WithPreference(string nodeLabel)
        {
            Guard.NotNullOrWhiteSpace(nodeLabel, nameof(nodeLabel));
            _definition = _definition with { Preferences = Collections.Append(_definition.Preferences, nodeLabel) };
            return this;
        }

        /// <summary>
        /// Sets the maximum number of replicas of the service on a single node.
        /// </summary>
        /// <param name="maxReplicasPerNode">The maximum, which must be positive.</param>
        /// <returns>This builder.</returns>
        public PlacementBuilder WithMaxReplicasPerNode(int maxReplicasPerNode)
        {
            if (maxReplicasPerNode < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxReplicasPerNode), maxReplicasPerNode, "The maximum replicas per node must be positive.");
            }

            _definition = _definition with { MaxReplicasPerNode = maxReplicasPerNode };
            return this;
        }

        /// <summary>
        /// Creates the placement definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="PlacementDefinition"/>.</returns>
        public PlacementDefinition Build()
        {
            return _definition;
        }
    }
}
