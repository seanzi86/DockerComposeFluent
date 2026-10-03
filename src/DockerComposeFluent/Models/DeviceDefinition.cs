using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A device reservation for a container (an entry of <c>deploy.resources.reservations.devices</c>).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#devices"/>
    /// </summary>
    public sealed record DeviceDefinition
    {
        /// <summary>
        /// The capabilities the device must satisfy, as specified by <c>capabilities</c>, for example
        /// <c>gpu</c> or a driver-specific value such as <c>nvidia-compute</c>. Never empty.
        /// </summary>
        public IReadOnlyList<string> Capabilities { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The driver to reserve the device through, as specified by <c>driver</c>.
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// The number of devices to reserve, as specified by <c>count</c>. Mutually exclusive with
        /// <c>DeviceIds</c>.
        /// </summary>
        public int? Count { get; init; }

        /// <summary>
        /// The IDs of the specific devices to reserve, as specified by <c>device_ids</c>. Mutually exclusive
        /// with <c>Count</c>. Empty when not set.
        /// </summary>
        public IReadOnlyList<string> DeviceIds { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>options</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyDictionary<string, string> Options { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The extension fields defined on this device reservation, keyed by their <c>x-</c> name. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
