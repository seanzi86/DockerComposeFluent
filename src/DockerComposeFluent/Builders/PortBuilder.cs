using System;
using System.Globalization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a long-syntax <see cref="PortDefinition"/>. A target port is required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
    /// </summary>
    public sealed class PortBuilder
    {
        private int? _target;
        private string? _published;
        private string? _hostIp;
        private PortProtocol? _protocol;
        private string? _appProtocol;
        private PortMode? _mode;
        private string? _name;

        /// <summary>
        /// Sets the container port.
        /// </summary>
        /// <param name="target">The container port, from 1 to 65535.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithTarget(int target)
        {
            Guard.ValidPort(target, nameof(target));

            _target = target;
            return this;
        }

        /// <summary>
        /// Sets a single publicly exposed port.
        /// </summary>
        /// <param name="port">The published port, from 1 to 65535.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithPublished(int port)
        {
            Guard.ValidPort(port, nameof(port));

            _published = port.ToString(CultureInfo.InvariantCulture);
            return this;
        }

        /// <summary>
        /// Sets a range of publicly exposed ports, from which an available port is chosen.
        /// </summary>
        /// <param name="start">The first port in the range.</param>
        /// <param name="end">The last port in the range, which must not be lower than <paramref name="start"/>.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithPublished(int start, int end)
        {
            Guard.ValidPort(start, nameof(start));
            Guard.ValidPort(end, nameof(end));

            if (end < start)
            {
                throw new ArgumentException("The end of the range must not be lower than the start.", nameof(end));
            }

            _published = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", start, end);
            return this;
        }

        /// <summary>
        /// Sets the host IP address to bind to.
        /// </summary>
        /// <param name="hostIp">The host IP address.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithHostIp(string hostIp)
        {
            Guard.NotNullOrWhiteSpace(hostIp, nameof(hostIp));

            _hostIp = hostIp;
            return this;
        }

        /// <summary>
        /// Sets the port protocol.
        /// </summary>
        /// <param name="protocol">The protocol.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithProtocol(PortProtocol protocol)
        {
            _protocol = protocol;
            return this;
        }

        /// <summary>
        /// Sets the application protocol this port is used for, such as <c>http</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.26.0 or later.</remarks>
        /// <param name="appProtocol">The application protocol.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithAppProtocol(string appProtocol)
        {
            Guard.NotNullOrWhiteSpace(appProtocol, nameof(appProtocol));

            _appProtocol = appProtocol;
            return this;
        }

        /// <summary>
        /// Sets how the port is published.
        /// </summary>
        /// <param name="mode">The publishing mode.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithMode(PortMode mode)
        {
            _mode = mode;
            return this;
        }

        /// <summary>
        /// Sets a human-readable name documenting the port's use.
        /// </summary>
        /// <param name="name">The port name.</param>
        /// <returns>This builder.</returns>
        public PortBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));

            _name = name;
            return this;
        }

        /// <summary>
        /// Creates the port definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="PortDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">No target port has been set.</exception>
        public PortDefinition Build()
        {
            if (_target == null)
            {
                throw new InvalidOperationException("A target port is required. Call WithTarget before Build.");
            }

            return new PortDefinition(_target.Value)
            {
                AppProtocol = _appProtocol,
                HostIp = _hostIp,
                Mode = _mode,
                Name = _name,
                Protocol = _protocol,
                Published = _published
            };
        }
    }
}
