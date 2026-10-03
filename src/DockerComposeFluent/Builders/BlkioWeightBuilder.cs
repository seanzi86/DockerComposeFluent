using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="BlkioWeightDefinition"/>. A path and a weight are required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
    /// </summary>
    public sealed class BlkioWeightBuilder
    {
        private BlkioWeightDefinition _definition = new BlkioWeightDefinition();

        /// <summary>
        /// Sets the path of the device.
        /// </summary>
        /// <param name="path">The device path, for example <c>/dev/sda</c>.</param>
        /// <returns>This builder.</returns>
        public BlkioWeightBuilder WithPath(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { Path = path };
            return this;
        }

        /// <summary>
        /// Sets the relative weight for the device.
        /// </summary>
        /// <param name="weight">The weight, from 10 to 1000.</param>
        /// <returns>This builder.</returns>
        public BlkioWeightBuilder WithWeight(int weight)
        {
            if (weight < 10 || weight > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(weight), weight, "The weight must be between 10 and 1000.");
            }

            _definition = _definition with { Weight = weight };
            return this;
        }

        /// <summary>
        /// Creates the weight from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="BlkioWeightDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">The path or weight was not set.</exception>
        public BlkioWeightDefinition Build()
        {
            if (_definition.Path == null || _definition.Weight == null)
            {
                throw new InvalidOperationException("A block I/O weight requires a path and a weight.");
            }

            return _definition;
        }
    }
}
