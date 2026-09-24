using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a long-syntax <see cref="MountDefinition"/>. A type and a target are required, and
    /// the option groups must match the type.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed class MountBuilder
    {
        private MountType? _type;
        private string? _target;
        private string? _source;
        private bool? _readOnly;
        private string? _consistency;
        private BindOptions? _bind;
        private VolumeOptions? _volume;
        private TmpfsOptions? _tmpfs;
        private ImageOptions? _image;

        /// <summary>
        /// Sets the kind of mount.
        /// </summary>
        /// <param name="type">The mount type.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithType(MountType type)
        {
            _type = type;
            return this;
        }

        /// <summary>
        /// Sets the source: a host path for a bind mount, a volume name for a volume, or an image reference
        /// for an image mount. Not applicable to tmpfs.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithSource(string source)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));

            _source = source;
            return this;
        }

        /// <summary>
        /// Sets the path in the container where the mount appears.
        /// </summary>
        /// <param name="target">The container path.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithTarget(string target)
        {
            Guard.NotNullOrWhiteSpace(target, nameof(target));

            _target = target;
            return this;
        }

        /// <summary>
        /// Sets whether the mount is read-only.
        /// </summary>
        /// <param name="readOnly"><c>true</c> for a read-only mount.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithReadOnly(bool readOnly)
        {
            _readOnly = readOnly;
            return this;
        }

        /// <summary>
        /// Sets the consistency requirements of the mount. The available values are platform specific.
        /// </summary>
        /// <param name="consistency">The consistency value.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithConsistency(string consistency)
        {
            Guard.NotNullOrWhiteSpace(consistency, nameof(consistency));

            _consistency = consistency;
            return this;
        }

        /// <summary>
        /// Sets the bind options from an existing definition.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithBindOptions(BindOptions options)
        {
            Guard.NotNull(options, nameof(options));

            _bind = options;
            return this;
        }

        /// <summary>
        /// Sets the bind options through a <see cref="BindOptionsBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithBindOptions(Action<BindOptionsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            BindOptionsBuilder builder = new BindOptionsBuilder();
            configure(builder);
            return WithBindOptions(builder.Build());
        }

        /// <summary>
        /// Sets the volume options from an existing definition.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithVolumeOptions(VolumeOptions options)
        {
            Guard.NotNull(options, nameof(options));

            _volume = options;
            return this;
        }

        /// <summary>
        /// Sets the volume options through a <see cref="VolumeOptionsBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithVolumeOptions(Action<VolumeOptionsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            VolumeOptionsBuilder builder = new VolumeOptionsBuilder();
            configure(builder);
            return WithVolumeOptions(builder.Build());
        }

        /// <summary>
        /// Sets the tmpfs options from an existing definition.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithTmpfsOptions(TmpfsOptions options)
        {
            Guard.NotNull(options, nameof(options));

            _tmpfs = options;
            return this;
        }

        /// <summary>
        /// Sets the tmpfs options through a <see cref="TmpfsOptionsBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithTmpfsOptions(Action<TmpfsOptionsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            TmpfsOptionsBuilder builder = new TmpfsOptionsBuilder();
            configure(builder);
            return WithTmpfsOptions(builder.Build());
        }

        /// <summary>
        /// Sets the image options from an existing definition.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithImageOptions(ImageOptions options)
        {
            Guard.NotNull(options, nameof(options));

            _image = options;
            return this;
        }

        /// <summary>
        /// Sets the image options through a <see cref="ImageOptionsBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the options.</param>
        /// <returns>This builder.</returns>
        public MountBuilder WithImageOptions(Action<ImageOptionsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ImageOptionsBuilder builder = new ImageOptionsBuilder();
            configure(builder);
            return WithImageOptions(builder.Build());
        }

        /// <summary>
        /// Creates the mount definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="MountDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// The type or target is missing, a tmpfs mount has a source, or an option group does not match the type.
        /// </exception>
        public MountDefinition Build()
        {
            if (_type == null)
            {
                throw new InvalidOperationException("A mount type is required. Call WithType before Build.");
            }

            if (_target == null)
            {
                throw new InvalidOperationException("A target path is required. Call WithTarget before Build.");
            }

            MountType type = _type.Value;

            if (type == MountType.Tmpfs && _source != null)
            {
                throw new InvalidOperationException("A tmpfs mount has no source.");
            }

            RequireType(_bind != null, type, MountType.Bind, "Bind");
            RequireType(_volume != null, type, MountType.Volume, "Volume");
            RequireType(_tmpfs != null, type, MountType.Tmpfs, "Tmpfs");
            RequireType(_image != null, type, MountType.Image, "Image");

            return new MountDefinition(type, _target)
            {
                Bind = _bind,
                Consistency = _consistency,
                Image = _image,
                ReadOnly = _readOnly,
                Source = _source,
                Tmpfs = _tmpfs,
                Volume = _volume
            };
        }

        private static void RequireType(bool optionsSet, MountType actual, MountType expected, string optionsName)
        {
            if (optionsSet && actual != expected)
            {
                throw new InvalidOperationException(
                    $"{optionsName} options only apply to {expected} mounts, but the mount type is {actual}.");
            }
        }
    }
}
