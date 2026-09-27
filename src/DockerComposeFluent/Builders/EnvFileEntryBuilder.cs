using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds an <see cref="EnvFileEntry"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
    /// </summary>
    public sealed class EnvFileEntryBuilder
    {
        private EnvFileEntry _entry = new EnvFileEntry();

        /// <summary>
        /// Sets the file path.
        /// </summary>
        /// <param name="path">The path, resolved relative to the Compose file's folder.</param>
        /// <returns>This builder.</returns>
        public EnvFileEntryBuilder WithPath(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _entry = _entry with { Path = path };
            return this;
        }

        /// <summary>
        /// Sets whether the file must exist. When <c>false</c>, Compose silently ignores a missing file.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#required"/>
        /// </summary>
        /// <remarks>Requires Compose 2.24.0 or later.</remarks>
        /// <param name="required"><c>false</c> to make the file optional.</param>
        /// <returns>This builder.</returns>
        public EnvFileEntryBuilder WithRequired(bool required)
        {
            _entry = _entry with { Required = required };
            return this;
        }

        /// <summary>
        /// Sets the alternative parsing format for the file.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#format"/>
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        /// <param name="format">The format.</param>
        /// <returns>This builder.</returns>
        public EnvFileEntryBuilder WithFormat(EnvFileFormat format)
        {
            _entry = _entry with { Format = format };
            return this;
        }

        /// <summary>
        /// Creates the entry from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="EnvFileEntry"/>.</returns>
        public EnvFileEntry Build()
        {
            Guard.NotNullOrWhiteSpace(_entry.Path, "path");
            return _entry;
        }
    }
}
