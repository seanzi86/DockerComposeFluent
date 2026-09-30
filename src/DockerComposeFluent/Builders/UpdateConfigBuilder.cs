using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds an <see cref="UpdateConfigDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#update_config"/>
    /// </summary>
    public sealed class UpdateConfigBuilder
    {
        private UpdateConfigDefinition _definition = new UpdateConfigDefinition();

        /// <summary>
        /// Sets the number of containers to update at a time.
        /// </summary>
        /// <param name="parallelism">The count, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithParallelism(int parallelism)
        {
            if (parallelism < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(parallelism), parallelism, "The parallelism must not be negative.");
            }

            _definition = _definition with { Parallelism = parallelism };
            return this;
        }

        /// <summary>
        /// Sets the time to wait between updating each group of containers.
        /// </summary>
        /// <param name="delay">The delay, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithDelay(TimeSpan delay)
        {
            return WithDelay(Durations.Format(delay, nameof(delay)));
        }

        /// <summary>
        /// Sets the time to wait between updating each group of containers, from a compose-spec duration.
        /// </summary>
        /// <param name="delay">The delay, such as <c>10s</c>.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithDelay(string delay)
        {
            Guard.Duration(delay, nameof(delay));
            _definition = _definition with { Delay = delay };
            return this;
        }

        /// <summary>
        /// Sets what to do if the update fails.
        /// </summary>
        /// <param name="failureAction">The action.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithFailureAction(UpdateFailureAction failureAction)
        {
            if (!Enum.IsDefined(typeof(UpdateFailureAction), failureAction))
            {
                throw new ArgumentOutOfRangeException(nameof(failureAction), failureAction, "Unknown UpdateFailureAction value.");
            }

            _definition = _definition with { FailureAction = failureAction };
            return this;
        }

        /// <summary>
        /// Sets how long to monitor each task for failure after it is updated.
        /// </summary>
        /// <param name="monitor">The duration, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithMonitor(TimeSpan monitor)
        {
            return WithMonitor(Durations.Format(monitor, nameof(monitor)));
        }

        /// <summary>
        /// Sets how long to monitor each task for failure after it is updated, from a compose-spec duration.
        /// </summary>
        /// <param name="monitor">The duration, such as <c>30s</c>.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithMonitor(string monitor)
        {
            Guard.Duration(monitor, nameof(monitor));
            _definition = _definition with { Monitor = monitor };
            return this;
        }

        /// <summary>
        /// Sets the failure rate to tolerate during the update.
        /// </summary>
        /// <param name="maxFailureRatio">The ratio, from <c>0</c> to <c>1</c>.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithMaxFailureRatio(double maxFailureRatio)
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
        public UpdateConfigBuilder WithOrder(RolloutOrder order)
        {
            if (!Enum.IsDefined(typeof(RolloutOrder), order))
            {
                throw new ArgumentOutOfRangeException(nameof(order), order, "Unknown RolloutOrder value.");
            }

            _definition = _definition with { Order = order };
            return this;
        }

        /// <summary>
        /// Sets an extension field on <c>deploy.update_config</c>. Setting the same key again replaces its
        /// value. Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public UpdateConfigBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the update configuration from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="UpdateConfigDefinition"/>.</returns>
        public UpdateConfigDefinition Build()
        {
            return _definition;
        }
    }
}
