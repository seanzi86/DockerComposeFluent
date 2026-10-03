namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A block I/O weight for one device (an entry of <c>weight_device</c> in <c>blkio_config</c>).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
    /// </summary>
    public sealed record BlkioWeightDefinition
    {
        /// <summary>
        /// The path of the device, for example <c>/dev/sda</c>, as specified by <c>path</c>.
        /// </summary>
        public string? Path { get; init; }

        /// <summary>
        /// The relative weight for the device, from 10 to 1000, as specified by <c>weight</c>.
        /// </summary>
        public int? Weight { get; init; }
    }
}
