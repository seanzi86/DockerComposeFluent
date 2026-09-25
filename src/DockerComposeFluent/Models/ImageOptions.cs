namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Extra options for an image mount, as specified by <c>image</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    /// <remarks>Requires Compose 2.35.0 or later.</remarks>
    public sealed record ImageOptions
    {
        /// <summary>
        /// A path inside the image to mount instead of the image root, as specified by <c>subpath</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.35.0 or later.</remarks>
        public string? Subpath { get; init; }
    }
}
