using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="WatchRuleDefinition"/>. A path and an action are required, and the
    /// settings must suit the action.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md#watch"/>
    /// </summary>
    public sealed class WatchRuleBuilder
    {
        private WatchRuleDefinition _definition = new WatchRuleDefinition();

        /// <summary>
        /// Sets the path to watch.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="path">The path, relative to the project directory.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithPath(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { Path = path };
            return this;
        }

        /// <summary>
        /// Sets what to do when a change is detected.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="action">The action.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithAction(WatchAction action)
        {
            if (!Enum.IsDefined(typeof(WatchAction), action))
            {
                throw new ArgumentOutOfRangeException(nameof(action), action, "Unknown WatchAction value.");
            }

            _definition = _definition with { Action = action };
            return this;
        }

        /// <summary>
        /// Sets the path in the container that changed files are synchronised to.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="target">The container path.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithTarget(string target)
        {
            Guard.NotNullOrWhiteSpace(target, nameof(target));
            _definition = _definition with { Target = target };
            return this;
        }

        /// <summary>
        /// Adds a pattern of paths that do not trigger the action, using <c>.dockerignore</c> syntax.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="pattern">The pattern, for example <c>node_modules/</c>.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithIgnore(string pattern)
        {
            Guard.NotNullOrWhiteSpace(pattern, nameof(pattern));
            _definition = _definition with { Ignore = Collections.Append(_definition.Ignore, pattern) };
            return this;
        }

        /// <summary>
        /// Adds several patterns of paths that do not trigger the action.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="patterns">The patterns.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithIgnore(IEnumerable<string> patterns)
        {
            Guard.NotNull(patterns, nameof(patterns));

            foreach (string pattern in patterns)
            {
                WithIgnore(pattern);
            }

            return this;
        }

        /// <summary>
        /// Adds a pattern of paths to watch; once any is set, only matching files trigger the action.
        /// </summary>
        /// <remarks>Requires Compose 2.34.0 or later.</remarks>
        /// <param name="pattern">The pattern, for example <c>*.go</c>.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithInclude(string pattern)
        {
            Guard.NotNullOrWhiteSpace(pattern, nameof(pattern));
            _definition = _definition with { Include = Collections.Append(_definition.Include, pattern) };
            return this;
        }

        /// <summary>
        /// Adds several patterns of paths to watch.
        /// </summary>
        /// <remarks>Requires Compose 2.34.0 or later.</remarks>
        /// <param name="patterns">The patterns.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithInclude(IEnumerable<string> patterns)
        {
            Guard.NotNull(patterns, nameof(patterns));

            foreach (string pattern in patterns)
            {
                WithInclude(pattern);
            }

            return this;
        }

        /// <summary>
        /// Sets the command to run in the container after synchronising, as a shell-form string. Only used
        /// with <see cref="WatchAction.SyncAndExec"/>.
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        /// <param name="command">The command, for example <c>app reload</c>.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithExec(string command)
        {
            return WithExec(new ServiceHookBuilder().WithCommand(command).Build());
        }

        /// <summary>
        /// Sets the command to run in the container after synchronising, from an existing definition.
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        /// <param name="exec">The command definition.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithExec(ServiceHookDefinition exec)
        {
            Guard.NotNull(exec, nameof(exec));
            _definition = _definition with { Exec = exec };
            return this;
        }

        /// <summary>
        /// Sets the command to run in the container after synchronising, configured through a
        /// <see cref="ServiceHookBuilder"/>.
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        /// <param name="configure">Configures the command.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithExec(Action<ServiceHookBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ServiceHookBuilder builder = new ServiceHookBuilder();
            configure(builder);
            return WithExec(builder.Build());
        }

        /// <summary>
        /// Sets whether to check that files in existing containers are up to date when a watch session starts.
        /// </summary>
        /// <remarks>Requires Compose 2.39.4 or later.</remarks>
        /// <param name="initialSync"><c>true</c> to check at the start of a session.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithInitialSync(bool initialSync)
        {
            _definition = _definition with { InitialSync = initialSync };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this watch rule. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public WatchRuleBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the watch rule from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="WatchRuleDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">The path or action is missing, a <c>sync</c> action has
        /// no target, <see cref="WatchAction.SyncAndExec"/> has no command, or a command is set for another action.</exception>
        public WatchRuleDefinition Build()
        {
            if (_definition.Path == null)
            {
                throw new InvalidOperationException("A watch rule requires a path. Call WithPath before Build.");
            }

            if (_definition.Action == null)
            {
                throw new InvalidOperationException("A watch rule requires an action. Call WithAction before Build.");
            }

            WatchAction action = _definition.Action.Value;
            bool syncs = action == WatchAction.Sync || action == WatchAction.SyncAndRestart || action == WatchAction.SyncAndExec;

            if (syncs && _definition.Target == null)
            {
                throw new InvalidOperationException($"A {action} watch rule synchronises files, so it requires a target. Call WithTarget before Build.");
            }

            if (action == WatchAction.SyncAndExec && _definition.Exec == null)
            {
                throw new InvalidOperationException("A SyncAndExec watch rule requires a command. Call WithExec before Build.");
            }

            if (action != WatchAction.SyncAndExec && _definition.Exec != null)
            {
                throw new InvalidOperationException("A command (WithExec) only applies to a SyncAndExec watch rule.");
            }

            return _definition;
        }
    }
}
