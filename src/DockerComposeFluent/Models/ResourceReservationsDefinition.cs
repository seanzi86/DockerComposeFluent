using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The physical resources the platform must guarantee a container can allocate (the
    /// <c>deploy.resources.reservations</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed record ResourceReservationsDefinition
    {
        /// <summary>
        /// The number of CPU cores the container can use, as specified by <c>cpus</c>, for example
        /// <c>"0.25"</c>.
        /// </summary>
        public string? Cpus { get; init; }

        /// <summary>
        /// The amount of memory the container can allocate, as a compose-spec byte value such as <c>20M</c>,
        /// as specified by <c>memory</c>.
        /// </summary>
        public string? Memory { get; init; }

        /// <summary>
        /// The container's PID limit, as specified by <c>pids</c>.
        /// </summary>
        public int? Pids { get; init; }

        /// <summary>
        /// The devices to reserve, as specified by <c>devices</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyList<DeviceDefinition> Devices { get; init; } = Array.Empty<DeviceDefinition>();

        /// <summary>
        /// The extension fields defined on <c>deploy.resources.reservations</c>, keyed by their <c>x-</c>
        /// name. Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
