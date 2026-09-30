using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="RollbackConfigDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#rollback_config"/>
    /// </summary>
    public sealed class RollbackConfigBuilder
    {
        private RollbackConfigDefinition _definition = new RollbackConfigDefinition();

        /// <summary>
        /// Sets the number of containers to roll back at a time.
        /// </summary>
        /// <param name="parallelism">The count. <c>0</c> rolls every container back at once.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithParallelism(int parallelism)
        {
            if (parallelism < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(parallelism), parallelism, "The parallelism must not be negative.");
            }

            _definition = _definition with { Parallelism = parallelism };
            return this;
        }

        /// <summary>
        /// Sets the time to wait between each group's rollback.
        /// </summary>
        /// <param name="delay">The delay, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithDelay(TimeSpan delay)
        {
            return WithDelay(Durations.Format(delay, nameof(delay)));
        }

        /// <summary>
        /// Sets the time to wait between each group's rollback, from a compose-spec duration.
        /// </summary>
        /// <param name="delay">The delay, such as <c>10s</c>.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithDelay(string delay)
        {
            Guard.Duration(delay, nameof(delay));
            _definition = _definition with { Delay = delay };
            return this;
        }

        /// <summary>
        /// Sets what to do if the rollback itself fails.
        /// </summary>
        /// <param name="failureAction">The action.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithFailureAction(RollbackFailureAction failureAction)
        {
            if (!Enum.IsDefined(typeof(RollbackFailureAction), failureAction))
            {
                throw new ArgumentOutOfRangeException(nameof(failureAction), failureAction, "Unknown RollbackFailureAction value.");
            }

            _definition = _definition with { FailureAction = failureAction };
            return this;
        }

        /// <summary>
        /// Sets how long to monitor each task for failure after it is updated.
        /// </summary>
        /// <param name="monitor">The duration, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithMonitor(TimeSpan monitor)
        {
            return WithMonitor(Durations.Format(monitor, nameof(monitor)));
        }

        /// <summary>
        /// Sets how long to monitor each task for failure after it is updated, from a compose-spec duration.
        /// </summary>
        /// <param name="monitor">The duration, such as <c>30s</c>.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithMonitor(string monitor)
        {
            Guard.Duration(monitor, nameof(monitor));
            _definition = _definition with { Monitor = monitor };
            return this;
        }

        /// <summary>
        /// Sets the failure rate to tolerate during the rollback.
        /// </summary>
        /// <param name="maxFailureRatio">The ratio, from <c>0</c> to <c>1</c>.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithMaxFailureRatio(double maxFailureRatio)
        {
            if (maxFailureRatio < 0 || maxFailureRatio > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxFailureRatio), maxFailureRatio, "The failure ratio must be between 0 and 1.");
            }

            _definition = _definition with { MaxFailureRatio = maxFailureRatio };
            return this;
        }

        /// <summary>
        /// Sets the order containers are stopped and started in.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithOrder(RolloutOrder order)
        {
            if (!Enum.IsDefined(typeof(RolloutOrder), order))
            {
                throw new ArgumentOutOfRangeException(nameof(order), order, "Unknown RolloutOrder value.");
            }

            _definition = _definition with { Order = order };
            return this;
        }

        /// <summary>
        /// Sets an extension field on <c>deploy.rollback_config</c>. Setting the same key again replaces its
        /// value. Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public RollbackConfigBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the rollback configuration from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="RollbackConfigDefinition"/>.</returns>
        public RollbackConfigDefinition Build()
        {
            return _definition;
        }
    }
}
