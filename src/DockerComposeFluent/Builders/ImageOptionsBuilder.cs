using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds <see cref="ImageOptions"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed class ImageOptionsBuilder
    {
        private ImageOptions _options = new ImageOptions();

        /// <summary>
        /// Sets a path inside the image to mount instead of the image root.
        /// </summary>
        /// <param name="subpath">The path inside the image.</param>
        /// <returns>This builder.</returns>
        public ImageOptionsBuilder WithSubpath(string subpath)
        {
            Guard.NotNullOrWhiteSpace(subpath, nameof(subpath));

            _options = _options with { Subpath = subpath };
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
