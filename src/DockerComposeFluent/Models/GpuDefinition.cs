using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A request for specific GPUs (an entry of <c>services.&lt;name&gt;.gpus</c> when not using all of them).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#gpus"/>
    /// </summary>
    public sealed record GpuDefinition
    {
        /// <summary>
        /// The capabilities the GPU must have, as specified by <c>capabilities</c>, for example
        /// <c>compute</c> or <c>utility</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyList<string> Capabilities { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The number of GPUs to use, as specified by <c>count</c>. Mutually exclusive with
        /// <c>DeviceIds</c>.
        /// </summary>
        public int? Count { get; init; }

        /// <summary>
        /// The IDs of the specific GPUs to use, as specified by <c>device_ids</c>. Mutually exclusive with
        /// <c>Count</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyList<string> DeviceIds { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The GPU driver to use, as specified by <c>driver</c>, for example <c>nvidia</c>.
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>options</c>. Empty when none are
        /// set.
        /// </summary>
        public IReadOnlyDictionary<string, string> Options { get; init; } = Collections.EmptyDictionary<string>();
    }
}
