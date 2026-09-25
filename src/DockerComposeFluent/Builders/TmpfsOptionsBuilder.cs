using System;
using System.Globalization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds <see cref="TmpfsOptions"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed class TmpfsOptionsBuilder
    {
        private TmpfsOptions _options = new TmpfsOptions();

        /// <summary>
        /// Sets the size of the mount in bytes.
        /// </summary>
        /// <param name="bytes">The size in bytes, which must be greater than zero.</param>
        /// <returns>This builder.</returns>
        public TmpfsOptionsBuilder WithSize(long bytes)
        {
            if (bytes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), bytes, "The size must be greater than zero.");
            }

            _options = _options with { Size = bytes.ToString(CultureInfo.InvariantCulture) };
            return this;
        }

        /// <summary>
        /// Sets the size of the mount as a value with a unit, such as <c>100m</c>.
        /// </summary>
        /// <param name="size">The size, with a bytes unit.</param>
        /// <returns>This builder.</returns>
        public TmpfsOptionsBuilder WithSize(string size)
        {
            Guard.NotNullOrWhiteSpace(size, nameof(size));

            _options = _options with { Size = size };
            return this;
        }

        /// <summary>
        /// Sets the file mode as Unix permission bits.
        /// </summary>
        /// <remarks>Requires Compose 2.14.0 or later.</remarks>
        /// <param name="mode">The numeric mode, from 0 to 4095. The octal mode <c>1777</c> is <c>Convert.ToInt32("1777", 8)</c>.</param>
        /// <returns>This builder.</returns>
        public TmpfsOptionsBuilder WithMode(int mode)
        {
            if (mode < 0 || mode > 4095)
            {
                throw new ArgumentOutOfRangeException(nameof(mode), mode, "The mode must be between 0 and 4095 (octal 7777).");
            }

            _options = _options with { Mode = mode };
            return this;
        }

        /// <summary>
        /// Creates the options from the values set so far.
        /// </summary>
        /// <returns>Immutable <see cref="TmpfsOptions"/>.</returns>
        public TmpfsOptions Build()
        {
            return _options;
        }
    }
}
