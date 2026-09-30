using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds <see cref="ImageOptions"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    /// <remarks>Requires Compose 2.35.0 or later.</remarks>
    public sealed class ImageOptionsBuilder
    {
        private ImageOptions _options = new ImageOptions();

        /// <summary>
        /// Sets a path inside the image to mount instead of the image root.
        /// </summary>
        /// <remarks>Requires Compose 2.35.0 or later.</remarks>
        /// <param name="subpath">The path inside the image.</param>
        /// <returns>This builder.</returns>
        public ImageOptionsBuilder WithSubpath(string subpath)
        {
            Guard.NotNullOrWhiteSpace(subpath, nameof(subpath));

            _options = _options with { Subpath = subpath };
            return this;
        }

        /// <summary>
        /// Sets an extension field on these image options. Setting the same key again replaces its value.
        /// Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public ImageOptionsBuilder WithExtension(string key, object? value)
        {
            _options = _options with { Extensions = ExtensionsMutator.Set(_options.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the options from the values set so far.
        /// </summary>
        /// <returns>Immutable <see cref="ImageOptions"/>.</returns>
        public ImageOptions Build()
        {
            return _options;
        }
    }
}
