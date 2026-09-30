using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DeployRestartPolicyDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#restart_policy"/>
    /// </summary>
    public sealed class DeployRestartPolicyBuilder
    {
        private DeployRestartPolicyDefinition _definition = new DeployRestartPolicyDefinition();

        /// <summary>
        /// Sets when a container is restarted.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>This builder.</returns>
        public DeployRestartPolicyBuilder WithCondition(DeployRestartCondition condition)
        {
            if (!Enum.IsDefined(typeof(DeployRestartCondition), condition))
            {
                throw new ArgumentOutOfRangeException(nameof(condition), condition, "Unknown DeployRestartCondition value.");
            }

            _definition = _definition with { Condition = condition };
            return this;
        }

        /// <summary>
        /// Sets how long to wait between restart attempts.
        /// </summary>
        /// <param name="delay">The delay, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public DeployRestartPolicyBuilder WithDelay(TimeSpan delay)
        {
            return WithDelay(Durations.Format(delay, nameof(delay)));
        }

        /// <summary>
        /// Sets how long to wait between restart attempts, from a compose-spec duration.
        /// </summary>
        /// <param name="delay">The delay, such as <c>5s</c>.</param>
        /// <returns>This builder.</returns>
        public DeployRestartPolicyBuilder WithDelay(string delay)
        {
            Guard.Duration(delay, nameof(delay));
            _definition = _definition with { Delay = delay };
            return this;
        }

        /// <summary>
        /// Sets the maximum number of restart attempts before giving up.
        /// </summary>
        /// <param name="maxAttempts">The maximum, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public DeployRestartPolicyBuilder WithMaxAttempts(int maxAttempts)
        {
            if (maxAttempts < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxAttempts), maxAttempts, "The maximum number of attempts must not be negative.");
            }

            _definition = _definition with { MaxAttempts = maxAttempts };
            return this;
        }

        /// <summary>
        /// Sets how long to wait before deciding a restart has succeeded.
        /// </summary>
        /// <param name="window">The window, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public DeployRestartPolicyBuilder WithWindow(TimeSpan window)
        {
            return WithWindow(Durations.Format(window, nameof(window)));
        }

        /// <summary>
        /// Sets how long to wait before deciding a restart has succeeded, from a compose-spec duration.
        /// </summary>
        /// <param name="window">The window, such as <c>120s</c>.</param>
        /// <returns>This builder.</returns>
        public DeployRestartPolicyBuilder WithWindow(string window)
        {
            Guard.Duration(window, nameof(window));
            _definition = _definition with { Window = window };
            return this;
        }

        /// <summary>
        /// Creates the restart policy from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DeployRestartPolicyDefinition"/>.</returns>
        public DeployRestartPolicyDefinition Build()
        {
            return _definition;
        }
    }
}
