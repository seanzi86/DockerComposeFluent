using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DevelopDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md"/>
    /// </summary>
    public sealed class DevelopBuilder
    {
        private DevelopDefinition _definition = new DevelopDefinition();

        /// <summary>
        /// Adds a watch rule that does not need a target or command: <see cref="WatchAction.Rebuild"/> or
        /// <see cref="WatchAction.Restart"/>.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="path">The path to watch.</param>
        /// <param name="action">The action to take on a change.</param>
        /// <returns>This builder.</returns>
        public DevelopBuilder WithWatch(string path, WatchAction action)
        {
            return WithWatch(new WatchRuleBuilder().WithPath(path).WithAction(action).Build());
        }

        /// <summary>
        /// Adds a synchronising watch rule: changed files are copied into the container at <paramref name="target"/>.
        /// </summary>
        /// <param name="path">The path to watch.</param>
        /// <param name="target">The path in the container.</param>
        /// <returns>This builder.</returns>
        public DevelopBuilder WithSyncWatch(string path, string target)
        {
            return WithWatch(new WatchRuleBuilder().WithPath(path).WithAction(WatchAction.Sync).WithTarget(target).Build());
        }

        /// <summary>
        /// Adds a watch rule, from an existing definition.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="rule">The watch rule.</param>
        /// <returns>This builder.</returns>
        public DevelopBuilder WithWatch(WatchRuleDefinition rule)
        {
            Guard.NotNull(rule, nameof(rule));
            _definition = _definition with { Watch = Collections.Append(_definition.Watch, rule) };
            return this;
        }

        /// <summary>
        /// Adds a watch rule, configured through a <see cref="WatchRuleBuilder"/>.
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        /// <param name="configure">Configures the rule.</param>
        /// <returns>This builder.</returns>
        public DevelopBuilder WithWatch(Action<WatchRuleBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            WatchRuleBuilder builder = new WatchRuleBuilder();
            configure(builder);
            return WithWatch(builder.Build());
        }

        /// <summary>
        /// Sets an extension field on this develop section. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public DevelopBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the develop section from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DevelopDefinition"/>.</returns>
        public DevelopDefinition Build()
        {
            return _definition;
        }
    }
}
