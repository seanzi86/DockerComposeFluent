using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DependencyDefinition"/>: the settings for one dependency of a service.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#depends_on"/>
    /// </summary>
    public sealed class DependencyBuilder
    {
        private DependencyDefinition _dependency = new DependencyDefinition();

        /// <summary>
        /// Sets when the dependency is considered satisfied.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>This builder.</returns>
        public DependencyBuilder WithCondition(DependencyCondition condition)
        {
            _dependency = _dependency with { Condition = condition };
            return this;
        }

        /// <summary>
        /// Sets whether this service is restarted when the dependency is restarted by a Compose operation.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <remarks>Requires Compose 2.17.0 or later.</remarks>
        /// <param name="restart"><c>true</c> to restart this service with the dependency.</param>
        /// <returns>This builder.</returns>
        public DependencyBuilder WithRestart(bool restart)
        {
            _dependency = _dependency with { Restart = restart };
            return this;
        }

        /// <summary>
        /// Sets whether the dependency must be available. When <c>false</c>, Compose only warns if it is missing.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <remarks>Requires Compose 2.20.0 or later.</remarks>
        /// <param name="required"><c>false</c> to make the dependency optional.</param>
        /// <returns>This builder.</returns>
        public DependencyBuilder WithRequired(bool required)
        {
            _dependency = _dependency with { Required = required };
            return this;
        }

        /// <summary>
        /// Creates the dependency from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DependencyDefinition"/>.</returns>
        public DependencyDefinition Build()
        {
            return _dependency;
        }
    }
}
