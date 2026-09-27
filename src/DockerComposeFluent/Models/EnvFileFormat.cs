namespace DockerComposeFluent.Models
{
    /// <summary>
    /// An alternative parsing format for an <c>env_file</c> entry, as specified by <c>format</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#format"/>
    /// </summary>
    public enum EnvFileFormat
    {
        /// <summary>
        /// Key-value pairs are used as-is, including quotes and <c>$</c> signs, without Compose's usual
        /// interpolation (<c>raw</c>).
        /// </summary>
        Raw
    }
}
