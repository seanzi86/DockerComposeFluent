using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="SecretReference"/>: how a service accesses one secret.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
    /// </summary>
    public sealed class SecretReferenceBuilder
    {
        private SecretReference _reference = new SecretReference();

        /// <summary>
        /// Sets the name of the secret as defined in the top-level <c>secrets</c> section.
        /// </summary>
        /// <param name="source">The secret name.</param>
        /// <returns>This builder.</returns>
        public SecretReferenceBuilder WithSource(string source)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            _reference = _reference with { Source = source };
            return this;
        }

        /// <summary>
        /// Sets the name of the mounted file, or an absolute path for an alternate location. Defaults to the
        /// source name if not set.
        /// </summary>
        /// <param name="target">The target file name or path.</param>
        /// <returns>This builder.</returns>
        public SecretReferenceBuilder WithTarget(string target)
        {
            Guard.NotNullOrWhiteSpace(target, nameof(target));
            _reference = _reference with { Target = target };
            return this;
        }

        /// <summary>
        /// Sets the numeric user ID that owns the mounted file.
        /// </summary>
        /// <param name="uid">The user ID.</param>
        /// <returns>This builder.</returns>
        public SecretReferenceBuilder WithUid(string uid)
        {
            Guard.NotNullOrWhiteSpace(uid, nameof(uid));
            _reference = _reference with { Uid = uid };
            return this;
        }

        /// <summary>
        /// Sets the numeric group ID that owns the mounted file.
        /// </summary>
        /// <param name="gid">The group ID.</param>
        /// <returns>This builder.</returns>
        public SecretReferenceBuilder WithGid(string gid)
        {
            Guard.NotNullOrWhiteSpace(gid, nameof(gid));
            _reference = _reference with { Gid = gid };
            return this;
        }

        /// <summary>
        /// Sets the permissions of the mounted file, in octal notation, for example
        /// <c>Convert.ToInt32("0440", 8)</c>. The default is world-readable (<c>0444</c>).
        /// </summary>
        /// <param name="mode">The file mode.</param>
        /// <returns>This builder.</returns>
        public SecretReferenceBuilder WithMode(int mode)
        {
            _reference = _reference with { Mode = mode };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this reference. Setting the same key again replaces its value. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public SecretReferenceBuilder WithExtension(string key, object? value)
        {
            _reference = _reference with { Extensions = ExtensionsMutator.Set(_reference.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the reference from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="SecretReference"/>.</returns>
        public SecretReference Build()
        {
            Guard.NotNullOrWhiteSpace(_reference.Source, "source");
            return _reference;
        }
    }
}
