namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Extra options for a tmpfs mount, as specified by <c>tmpfs</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed record TmpfsOptions
    {
        /// <summary>
        /// The file mode as Unix permission bits, as specified by <c>mode</c>. This is the numeric value, so
        /// the octal mode <c>1777</c> is <c>Convert.ToInt32("1777", 8)</c>.
        /// </summary>
        public int? Mode { get; init; }

        /// <summary>
        /// The size of the mount, either a number of bytes or a value with a unit such as <c>100m</c>,
        /// as specified by <c>size</c>.
        /// </summary>
        public string? Size { get; init; }
    }
}
