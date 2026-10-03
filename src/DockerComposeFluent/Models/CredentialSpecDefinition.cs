using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The credential spec for a managed service account (the <c>credential_spec</c> entry). Exactly one of
    /// <see cref="Config"/>, <see cref="File"/> or <see cref="Registry"/> is set.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#credential_spec"/>
    /// </summary>
    public sealed record CredentialSpecDefinition
    {
        /// <summary>
        /// The name of the credential spec config to use, as specified by <c>config</c>.
        /// </summary>
        public string? Config { get; init; }

        /// <summary>
        /// The path to a credential spec file, as specified by <c>file</c>.
        /// </summary>
        public string? File { get; init; }

        /// <summary>
        /// The path to a credential spec in the Windows registry, as specified by <c>registry</c>.
        /// </summary>
        public string? Registry { get; init; }

        /// <summary>
        /// The extension fields defined on this {0}, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
