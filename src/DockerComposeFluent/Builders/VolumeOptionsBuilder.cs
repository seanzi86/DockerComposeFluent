using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds <see cref="VolumeOptions"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed class VolumeOptionsBuilder
    {
        private VolumeOptions _options = new VolumeOptions();

        /// <summary>
        /// Sets whether copying data from the container is disabled when the volume is created.
        /// </summary>
        /// <param name="noCopy"><c>true</c> to disable copying.</param>
        /// <returns>This builder.</returns>
        public VolumeOptionsBuilder WithNoCopy(bool noCopy)
        {
            _options = _options with { NoCopy = noCopy };
            return this;
        }

        /// <summary>
        /// Sets a path inside the volume to mount instead of the volume root.
        /// </summary>
        /// <param name="subpath">The path inside the volume.</param>
        /// <returns>This builder.</returns>
        public VolumeOptionsBuilder WithSubpath(string subpath)
        {
            Guard.NotNullOrWhiteSpace(subpath, nameof(subpath));

            _options = _options with { Subpath = subpath };
            return this;
        }

        /// <summary>
        /// Creates the options from the values set so far.
        /// </summary>
        /// <returns>Immutable <see cref="VolumeOptions"/>.</returns>
        public VolumeOptions Build()
        {
            return _options;
        }
    }
}
