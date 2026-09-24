namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Extra options for an image mount, as specified by <c>image</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed record ImageOptions
    {
        /// <summary>
        /// A path inside the image to mount instead of the image root, as specified by <c>subpath</c>.
        /// </summary>
        public string? Subpath { get; init; }
    }
}
