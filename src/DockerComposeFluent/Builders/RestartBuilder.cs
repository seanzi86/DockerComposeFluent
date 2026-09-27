using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="RestartDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
    /// </summary>
    public sealed class RestartBuilder
    {
        private RestartPolicy _policy = RestartPolicy.No;
        private int? _maxRetries;

        /// <summary>
        /// Sets the restart policy.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="policy">The restart policy.</param>
        /// <returns>This builder.</returns>
        public RestartBuilder WithPolicy(RestartPolicy policy)
        {
            if (!Enum.IsDefined(typeof(RestartPolicy), policy))
            {
                throw new ArgumentOutOfRangeException(nameof(policy), policy, "Unknown RestartPolicy value.");
            }

            _policy = policy;
            return this;
        }

        /// <summary>
        /// Sets the maximum number of restart attempts. Only meaningful when the policy is
        /// <see cref="RestartPolicy.OnFailure"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="maxRetries">The maximum number of restart attempts, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public RestartBuilder WithMaxRetries(int maxRetries)
        {
            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries), maxRetries, "The maximum number of retries must not be negative.");
            }

            _maxRetries = maxRetries;
            return this;
        }

        /// <summary>
        /// Creates the restart definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="RestartDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">A maximum number of retries was set for a policy other
        /// than <see cref="RestartPolicy.OnFailure"/>.</exception>
        public RestartDefinition Build()
        {
            if (_maxRetries != null)
            {
                if (_policy != RestartPolicy.OnFailure)
                {
                    throw new InvalidOperationException("WithMaxRetries only applies when the policy is RestartPolicy.OnFailure.");
                }

                return RestartDefinition.OnFailure(_maxRetries.Value);
            }

            return RestartDefinition.FromPolicy(_policy);
        }
    }
}
