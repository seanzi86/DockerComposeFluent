using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ConfigReference"/>: how a service accesses one config.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
    /// </summary>
    public sealed class ConfigReferenceBuilder
    {
        private ConfigReference _reference = new ConfigReference();

        /// <summary>
        /// Sets the name of the config as defined in the top-level <c>configs</c> section.
        /// </summary>
        /// <param name="source">The config name.</param>
        /// <returns>This builder.</returns>
        public ConfigReferenceBuilder WithSource(string source)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            _reference = _reference with { Source = source };
            return this;
        }

        /// <summary>
        /// Sets the path and name of the mounted file. Defaults to <c>/&lt;source&gt;</c> if not set.
        /// </summary>
        /// <param name="target">The target file path.</param>
        /// <returns>This builder.</returns>
        public ConfigReferenceBuilder WithTarget(string target)
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
        public ConfigReferenceBuilder WithUid(string uid)
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
        public ConfigReferenceBuilder WithGid(string gid)
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
        public ConfigReferenceBuilder WithMode(int mode)
        {
            _reference = _reference with { Mode = mode };
            return this;
        }

        /// <summary>
        /// Creates the reference from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ConfigReference"/>.</returns>
        public ConfigReference Build()
        {
            Guard.NotNullOrWhiteSpace(_reference.Source, "source");
            return _reference;
        }
    }
}
