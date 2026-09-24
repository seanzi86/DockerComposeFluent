using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds <see cref="BindOptions"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed class BindOptionsBuilder
    {
        private BindOptions _options = new BindOptions();

        /// <summary>
        /// Sets whether a directory is created at the source path on the host if it does not exist.
        /// </summary>
        /// <param name="createHostPath"><c>true</c> to create it (the default when unset), <c>false</c> to require it to exist.</param>
        /// <returns>This builder.</returns>
        public BindOptionsBuilder WithCreateHostPath(bool createHostPath)
        {
            _options = _options with { CreateHostPath = createHostPath };
            return this;
        }

        /// <summary>
        /// Sets the propagation mode used for the bind.
        /// </summary>
        /// <param name="propagation">The propagation mode.</param>
        /// <returns>This builder.</returns>
        public BindOptionsBuilder WithPropagation(BindPropagation propagation)
        {
            _options = _options with { Propagation = propagation };
            return this;
        }

        /// <summary>
        /// Sets the SELinux re-labelling option.
        /// </summary>
        /// <param name="selinux">The SELinux option.</param>
        /// <returns>This builder.</returns>
        public BindOptionsBuilder WithSelinux(SelinuxLabel selinux)
        {
            _options = _options with { Selinux = selinux };
            return this;
        }

        /// <summary>
        /// Creates the options from the values set so far.
        /// </summary>
        /// <returns>Immutable <see cref="BindOptions"/>.</returns>
        public BindOptions Build()
        {
            return _options;
        }
    }
}
