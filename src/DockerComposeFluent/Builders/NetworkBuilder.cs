using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="NetworkDefinition"/>.
    /// </summary>
    public sealed class NetworkBuilder
    {
        private NetworkDefinition definition = new NetworkDefinition();

        /// <summary>
        /// Sets the network driver.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver"/>
        /// </summary>
        /// <param name="driver">The driver name.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithDriver(string driver)
        {
            definition = definition with { Driver = Guard.NotNullOrWhiteSpace(driver, nameof(driver)) };
            return this;
        }

        /// <summary>
        /// Sets the actual Docker network name, overriding the name generated from the project name and key.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#name"/>
        /// </summary>
        /// <param name="name">The network name.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithName(string name)
        {
            definition = definition with { Name = Guard.NotNullOrWhiteSpace(name, nameof(name)) };
            return this;
        }

        /// <summary>
        /// Creates the network definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="NetworkDefinition"/>.</returns>
        public NetworkDefinition Build()
        {
            return definition;
        }
    }
}
