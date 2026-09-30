using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds an <see cref="ExtendsDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
    /// </summary>
    public sealed class ExtendsBuilder
    {
        private ExtendsDefinition _definition = new ExtendsDefinition();

        /// <summary>
        /// Sets the name of the service being referenced as a base.
        /// </summary>
        /// <param name="service">The service name.</param>
        /// <returns>This builder.</returns>
        public ExtendsBuilder WithService(string service)
        {
            Guard.NotNullOrWhiteSpace(service, nameof(service));
            _definition = _definition with { Service = service };
            return this;
        }

        /// <summary>
        /// Sets the Compose file the referenced service is defined in. Relative paths are resolved against the
        /// main Compose file's location. Defaults to the current file if not set.
        /// </summary>
        /// <param name="file">The file path.</param>
        /// <returns>This builder.</returns>
        public ExtendsBuilder WithFile(string file)
        {
            Guard.NotNullOrWhiteSpace(file, nameof(file));
            _definition = _definition with { File = file };
            return this;
        }

        /// <summary>
        /// Creates the extends definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ExtendsDefinition"/>.</returns>
        public ExtendsDefinition Build()
        {
            Guard.NotNullOrWhiteSpace(_definition.Service, "service");
            return _definition;
        }
    }
}
