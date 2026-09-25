using System;
using System.Globalization;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The restart behaviour of a service (the <c>restart</c> value): a <see cref="RestartPolicy"/> and, for
    /// <see cref="RestartPolicy.OnFailure"/>, an optional limit on the number of retries.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
    /// </summary>
    public sealed record RestartDefinition
    {
        private const string _onFailure = "on-failure";

        private RestartDefinition(RestartPolicy policy, int? maxRetries)
        {
            Policy = policy;
            MaxRetries = maxRetries;
        }

        /// <summary>
        /// The restart policy.
        /// </summary>
        public RestartPolicy Policy { get; }

        /// <summary>
        /// The maximum number of restart attempts, which is only set for <see cref="RestartPolicy.OnFailure"/>.
        /// <c>null</c> means no limit.
        /// </summary>
        public int? MaxRetries { get; }

        /// <summary>
        /// Creates a restart definition from a policy that has no retry limit.
        /// </summary>
        /// <param name="policy">The policy. Use <see cref="OnFailure"/> to limit the retries of
        /// <see cref="RestartPolicy.OnFailure"/>.</param>
        /// <returns>A new <see cref="RestartDefinition"/>.</returns>
        public static RestartDefinition FromPolicy(RestartPolicy policy)
        {
            if (!Enum.IsDefined(typeof(RestartPolicy), policy))
            {
                throw new ArgumentOutOfRangeException(nameof(policy), policy, "Unknown RestartPolicy value.");
            }

            return new RestartDefinition(policy, null);
        }

        /// <summary>
        /// Creates an <c>on-failure</c> definition that gives up after a number of retries.
        /// </summary>
        /// <param name="maxRetries">The maximum number of restart attempts, which must not be negative.</param>
        /// <returns>A new <see cref="RestartDefinition"/>.</returns>
        public static RestartDefinition OnFailure(int maxRetries)
        {
            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries), maxRetries, "The maximum number of retries must not be negative.");
            }

            return new RestartDefinition(RestartPolicy.OnFailure, maxRetries);
        }

        /// <summary>
        /// Creates a restart definition from its compose-spec text.
        /// </summary>
        /// <param name="value">One of <c>no</c>, <c>always</c>, <c>on-failure</c>, <c>on-failure:&lt;retries&gt;</c>
        /// or <c>unless-stopped</c>.</param>
        /// <returns>A new <see cref="RestartDefinition"/>.</returns>
        public static RestartDefinition Parse(string value)
        {
            Guard.NotNullOrWhiteSpace(value, nameof(value));

            switch (value)
            {
                case "no":
                    return FromPolicy(RestartPolicy.No);
                case "always":
                    return FromPolicy(RestartPolicy.Always);
                case "unless-stopped":
                    return FromPolicy(RestartPolicy.UnlessStopped);
                case _onFailure:
                    return FromPolicy(RestartPolicy.OnFailure);
            }

            string prefix = _onFailure + ":";
            if (value.StartsWith(prefix, StringComparison.Ordinal)
                && int.TryParse(value.Substring(prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out int maxRetries))
            {
                return OnFailure(maxRetries);
            }

            throw new ArgumentException(
                $"'{value}' is not a restart policy. Use no, always, on-failure, on-failure:<retries> or unless-stopped.",
                nameof(value));
        }
    }
}
