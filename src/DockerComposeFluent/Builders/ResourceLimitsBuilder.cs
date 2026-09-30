using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ResourceLimitsDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed class ResourceLimitsBuilder
    {
        private ResourceLimitsDefinition _definition = new ResourceLimitsDefinition();

        /// <summary>
        /// Sets the number of CPU cores the container can use.
        /// </summary>
        /// <param name="cpus">The core count, for example <c>"0.50"</c>.</param>
        /// <returns>This builder.</returns>
        public ResourceLimitsBuilder WithCpus(string cpus)
        {
            Guard.NotNullOrWhiteSpace(cpus, nameof(cpus));
            _definition = _definition with { Cpus = cpus };
            return this;
        }

        /// <summary>
        /// Sets the amount of memory the container can allocate.
        /// </summary>
        /// <param name="memory">A compose-spec byte value, such as <c>50M</c>.</param>
        /// <returns>This builder.</returns>
        public ResourceLimitsBuilder WithMemory(string memory)
        {
            Guard.NotNullOrWhiteSpace(memory, nameof(memory));
            _definition = _definition with { Memory = memory };
            return this;
        }

        /// <summary>
        /// Sets the container's PID limit.
        /// </summary>
        /// <param name="pids">The limit, which must be positive.</param>
        /// <returns>This builder.</returns>
        public ResourceLimitsBuilder WithPids(int pids)
        {
            if (pids < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pids), pids, "The PID limit must be positive.");
            }

            _definition = _definition with { Pids = pids };
            return this;
        }

        /// <summary>
        /// Creates the resource limits from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ResourceLimitsDefinition"/>.</returns>
        public ResourceLimitsDefinition Build()
        {
            return _definition;
        }
    }
}
