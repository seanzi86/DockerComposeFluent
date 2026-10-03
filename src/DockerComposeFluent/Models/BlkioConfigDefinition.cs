using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The block I/O configuration of a service (the <c>blkio_config</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
    /// </summary>
    public sealed record BlkioConfigDefinition
    {
        /// <summary>
        /// The relative block I/O weight of the service, from 10 to 1000, as specified by <c>weight</c>.
        /// </summary>
        public int? Weight { get; init; }

        /// <summary>
        /// The block I/O weight for specific devices, as specified by <c>weight_device</c>. Empty when none
        /// are set.
        /// </summary>
        public IReadOnlyList<BlkioWeightDefinition> WeightDevice { get; init; } = Array.Empty<BlkioWeightDefinition>();

        /// <summary>
        /// The read rate limits in bytes per second, as specified by <c>device_read_bps</c>. Empty when none
        /// are set.
        /// </summary>
        public IReadOnlyList<BlkioLimitDefinition> DeviceReadBps { get; init; } = Array.Empty<BlkioLimitDefinition>();

        /// <summary>
        /// The read rate limits in operations per second, as specified by <c>device_read_iops</c>. Empty
        /// when none are set.
        /// </summary>
        public IReadOnlyList<BlkioLimitDefinition> DeviceReadIops { get; init; } = Array.Empty<BlkioLimitDefinition>();

        /// <summary>
        /// The write rate limits in bytes per second, as specified by <c>device_write_bps</c>. Empty when
        /// none are set.
        /// </summary>
        public IReadOnlyList<BlkioLimitDefinition> DeviceWriteBps { get; init; } = Array.Empty<BlkioLimitDefinition>();

        /// <summary>
        /// The write rate limits in operations per second, as specified by <c>device_write_iops</c>. Empty
        /// when none are set.
        /// </summary>
        public IReadOnlyList<BlkioLimitDefinition> DeviceWriteIops { get; init; } = Array.Empty<BlkioLimitDefinition>();
    }
}
