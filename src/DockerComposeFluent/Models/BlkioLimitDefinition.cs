namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A block I/O rate limit for one device (an entry of the <c>device_*_bps</c> and <c>device_*_iops</c>
    /// lists of <c>blkio_config</c>).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
    /// </summary>
    public sealed record BlkioLimitDefinition
    {
        /// <summary>
        /// The path of the device, for example <c>/dev/sda</c>, as specified by <c>path</c>.
        /// </summary>
        public string? Path { get; init; }

        /// <summary>
        /// The rate limit, in bytes per second (for example <c>12mb</c>) or operations per second, as
        /// specified by <c>rate</c>.
        /// </summary>
        public string? Rate { get; init; }
    }
}
