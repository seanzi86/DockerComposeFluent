using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="BlkioLimitDefinition"/>. A path and a rate are required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
    /// </summary>
    public sealed class BlkioLimitBuilder
    {
        private BlkioLimitDefinition _definition = new BlkioLimitDefinition();

        /// <summary>
        /// Sets the path of the device.
        /// </summary>
        /// <param name="path">The device path, for example <c>/dev/sda</c>.</param>
        /// <returns>This builder.</returns>
        public BlkioLimitBuilder WithPath(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { Path = path };
            return this;
        }

        /// <summary>
        /// Sets the rate limit.
        /// </summary>
        /// <param name="rate">Bytes per second (for example <c>12mb</c>) or operations per second.</param>
        /// <returns>This builder.</returns>
        public BlkioLimitBuilder WithRate(string rate)
        {
            Guard.NotNullOrWhiteSpace(rate, nameof(rate));
            _definition = _definition with { Rate = rate };
            return this;
        }

        /// <summary>
        /// Creates the limit from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="BlkioLimitDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">The path or rate was not set.</exception>
        public BlkioLimitDefinition Build()
        {
            if (_definition.Path == null || _definition.Rate == null)
            {
                throw new InvalidOperationException("A block I/O limit requires a path and a rate.");
            }

            return _definition;
        }
    }
}
