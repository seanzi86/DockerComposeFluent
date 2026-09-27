using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The labels attached to a service, network or volume, as specified by <c>labels</c>. Compose allows a
    /// mapping (<c>KEY: value</c>) or a list (<c>- KEY=value</c>); the list form is used when it was asked for.
    /// Unlike <see cref="EnvironmentVariables"/>, every label has a value (which may be empty).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels"/>
    /// </summary>
    public sealed record Labels
    {
        /// <summary>
        /// The labels in the order they were added.
        /// </summary>
        public IReadOnlyDictionary<string, string> Values { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// Whether the labels are written as a list of <c>KEY=value</c> strings rather than a mapping.
        /// </summary>
        public bool UsesListSyntax { get; init; }
    }
}
