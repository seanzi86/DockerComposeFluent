using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="HealthcheckDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
    /// </summary>
    public sealed class HealthcheckBuilder
    {
        private static readonly string[] _testKinds = { "CMD", "CMD-SHELL", "NONE" };

        private HealthcheckDefinition _healthcheck = new HealthcheckDefinition();

        /// <summary>
        /// Sets the check as a shell-form string, written to YAML as a string. Compose runs it with
        /// <c>CMD-SHELL</c>, using the container's default shell.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="command">The command, for example <c>curl -f http://localhost || exit 1</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithTest(string command)
        {
            _healthcheck = _healthcheck with { Test = CommandLine.FromShell(command) };
            return this;
        }

        /// <summary>
        /// Sets the check as a list, written to YAML as a list. The first item must be <c>CMD</c>,
        /// <c>CMD-SHELL</c> or <c>NONE</c>. Use <see cref="WithCommand(IEnumerable{string})"/> to have <c>CMD</c>
        /// added for you.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="test">The list, for example <c>CMD</c>, <c>curl</c>, <c>-f</c>, <c>http://localhost</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithTest(IEnumerable<string> test)
        {
            Guard.NotNull(test, nameof(test));

            CommandLine command = CommandLine.FromArguments(test);
            if (command.Arguments!.Count == 0 || Array.IndexOf(_testKinds, command.Arguments[0]) < 0)
            {
                throw new ArgumentException("The first item of a healthcheck test must be CMD, CMD-SHELL or NONE.", nameof(test));
            }

            _healthcheck = _healthcheck with { Test = command };
            return this;
        }

        /// <summary>
        /// Sets the check to run a command directly, without a shell: <c>CMD</c> followed by the arguments.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="arguments">The command and its arguments, for example <c>curl</c>, <c>-f</c>,
        /// <c>http://localhost</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithCommand(IEnumerable<string> arguments)
        {
            Guard.NotNull(arguments, nameof(arguments));

            List<string> test = new List<string> { "CMD" };
            test.AddRange(arguments);
            if (test.Count == 1)
            {
                throw new ArgumentException("A healthcheck command needs at least one argument.", nameof(arguments));
            }

            return WithTest(test);
        }

        /// <summary>
        /// Sets the time between checks.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="interval">The interval, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithInterval(TimeSpan interval)
        {
            return WithInterval(Durations.Format(interval, nameof(interval)));
        }

        /// <summary>
        /// Sets the time between checks from a compose-spec duration.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="interval">The interval, such as <c>30s</c> or <c>1m30s</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithInterval(string interval)
        {
            Guard.Duration(interval, nameof(interval));
            _healthcheck = _healthcheck with { Interval = interval };
            return this;
        }

        /// <summary>
        /// Sets how long a single check may run before it counts as failed.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="timeout">The timeout, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithTimeout(TimeSpan timeout)
        {
            return WithTimeout(Durations.Format(timeout, nameof(timeout)));
        }

        /// <summary>
        /// Sets how long a single check may run before it counts as failed, from a compose-spec duration.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="timeout">The timeout, such as <c>10s</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithTimeout(string timeout)
        {
            Guard.Duration(timeout, nameof(timeout));
            _healthcheck = _healthcheck with { Timeout = timeout };
            return this;
        }

        /// <summary>
        /// Sets the number of consecutive failures before the container is considered unhealthy.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="retries">The number of retries, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithRetries(int retries)
        {
            if (retries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retries), retries, "The number of retries must not be negative.");
            }

            _healthcheck = _healthcheck with { Retries = retries };
            return this;
        }

        /// <summary>
        /// Sets how long the container has to start before failed checks count.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="startPeriod">The start period, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithStartPeriod(TimeSpan startPeriod)
        {
            return WithStartPeriod(Durations.Format(startPeriod, nameof(startPeriod)));
        }

        /// <summary>
        /// Sets how long the container has to start before failed checks count, from a compose-spec duration.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="startPeriod">The start period, such as <c>40s</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithStartPeriod(string startPeriod)
        {
            Guard.Duration(startPeriod, nameof(startPeriod));
            _healthcheck = _healthcheck with { StartPeriod = startPeriod };
            return this;
        }

        /// <summary>
        /// Sets the time between checks during the start period.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <remarks>Requires Compose 2.20.2 or later.</remarks>
        /// <param name="startInterval">The interval, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithStartInterval(TimeSpan startInterval)
        {
            return WithStartInterval(Durations.Format(startInterval, nameof(startInterval)));
        }

        /// <summary>
        /// Sets the time between checks during the start period, from a compose-spec duration.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <remarks>Requires Compose 2.20.2 or later.</remarks>
        /// <param name="startInterval">The interval, such as <c>5s</c>.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithStartInterval(string startInterval)
        {
            Guard.Duration(startInterval, nameof(startInterval));
            _healthcheck = _healthcheck with { StartInterval = startInterval };
            return this;
        }

        /// <summary>
        /// Sets whether the healthcheck set by the image is disabled.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="disable"><c>true</c> to disable the image's healthcheck.</param>
        /// <returns>This builder.</returns>
        public HealthcheckBuilder WithDisable(bool disable)
        {
            _healthcheck = _healthcheck with { Disable = disable };
            return this;
        }

        /// <summary>
        /// Creates the healthcheck from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="HealthcheckDefinition"/>.</returns>
        public HealthcheckDefinition Build()
        {
            return _healthcheck;
        }
    }
}
