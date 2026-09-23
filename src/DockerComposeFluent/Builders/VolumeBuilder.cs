using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="VolumeDefinition"/>.
    /// </summary>
    public sealed class VolumeBuilder
    {
        private VolumeDefinition _definition = new VolumeDefinition();

        /// <summary>
        /// Sets the volume driver.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver"/>
        /// </summary>
        /// <param name="driver">The driver name.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _definition = _definition with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets the actual Docker volume name, overriding the name generated from the project name and key.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#name"/>
        /// </summary>
        /// <param name="name">The volume name.</param>
        /// <returns>This builder.</returns>
        public VolumeBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            _definition = _definition with { Name = name };
            return this;
        }

        /// <summary>
        /// Creates the volume definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="VolumeDefinition"/>.</returns>
        public VolumeDefinition Build()
        {
            return _definition;
        }
    }
}
