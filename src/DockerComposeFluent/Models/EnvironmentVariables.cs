using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The environment variables of a service, as specified by <c>environment</c>. Compose allows a mapping
    /// (<c>KEY: value</c>) or a list (<c>- KEY=value</c>); the list form is used when it was asked for.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
    /// </summary>
    public sealed record EnvironmentVariables
    {
        /// <summary>
        /// The variables in the order they were added. A <c>null</c> value means the variable has no value
        /// here, so Compose takes it from the environment of the machine running it and removes it from the
        /// container when it is not set there.
        /// </summary>
        public IReadOnlyDictionary<string, string?> Variables { get; init; } = Collections.EmptyDictionary<string?>();

        /// <summary>
        /// Whether the variables are written as a list of <c>KEY=value</c> strings rather than a mapping.
        /// </summary>
        public bool UsesListSyntax { get; init; }
    }
}
