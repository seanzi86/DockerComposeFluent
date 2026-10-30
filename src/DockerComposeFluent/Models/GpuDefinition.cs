using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A request for specific GPUs (an entry of <c>services.&lt;name&gt;.gpus</c> when not using all of them).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#gpus"/>
    /// </summary>
    public sealed record GpuDefinition
    {
        /// <summary>
        /// The capabilities the GPU must have, as specified by <c>capabilities</c>, for example
        /// <c>compute</c> or <c>utility</c>. Empty when none are set.
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public IReadOnlyList<string> Capabilities
        {
            get { return _capabilities; }
            init { _capabilities = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _capabilities = Collections.EmptyList<string>();

        /// <summary>
        /// The number of GPUs to use, as specified by <c>count</c>. Mutually exclusive with
        /// <c>DeviceIds</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public int? Count { get; init; }

        /// <summary>
        /// The IDs of the specific GPUs to use, as specified by <c>device_ids</c>. Mutually exclusive with
        /// <c>Count</c>. Empty when none are set.
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public IReadOnlyList<string> DeviceIds
        {
            get { return _deviceIds; }
            init { _deviceIds = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _deviceIds = Collections.EmptyList<string>();

        /// <summary>
        /// The GPU driver to use, as specified by <c>driver</c>, for example <c>nvidia</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public string? Driver { get; init; }

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>options</c>. Empty when none are
        /// set.
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public IReadOnlyDictionary<string, string> Options
        {
            get { return _options; }
            init { _options = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, string> _options = Collections.EmptyDictionary<string>();
    }
}
