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
        public IReadOnlyList<DeviceReservationDefinition> Devices
        {
            get { return _devices; }
            init { _devices = Collections.Seal(value); }
        }

        private IReadOnlyList<DeviceReservationDefinition> _devices = Collections.EmptyList<DeviceReservationDefinition>();

        /// <summary>
        /// The user-defined resources to reserve, as specified by <c>generic_resources</c>. Empty when none are
        /// set.
        /// </summary>
        public IReadOnlyList<GenericResourceDefinition> GenericResources
        {
            get { return _genericResources; }
            init { _genericResources = Collections.Seal(value); }
        }

        private IReadOnlyList<GenericResourceDefinition> _genericResources = Collections.EmptyList<GenericResourceDefinition>();

        /// <summary>
        /// The extension fields defined on <c>deploy.resources.reservations</c>, keyed by their <c>x-</c>
        /// name. Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions
        {
            get { return _extensions; }
            init { _extensions = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, object?> _extensions = Collections.EmptyDictionary<object?>();
    }
}
