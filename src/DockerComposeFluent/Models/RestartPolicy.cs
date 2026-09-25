namespace DockerComposeFluent.Models
{
    /// <summary>
    /// When Compose restarts a service's container, as specified by <c>restart</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
    /// </summary>
    public enum RestartPolicy
    {
        /// <summary>
        /// Never restart the container (<c>no</c>), the default.
        /// </summary>
        No,

        /// <summary>
        /// Always restart the container until it is removed (<c>always</c>).
        /// </summary>
        Always,

        /// <summary>
        /// Restart the container if it exits with a non-zero exit code (<c>on-failure</c>), optionally limited to
        /// a number of retries.
        /// </summary>
        OnFailure,

        /// <summary>
        /// Always restart the container, unless it was stopped explicitly or the Docker daemon was stopped
        /// (<c>unless-stopped</c>).
        /// </summary>
        UnlessStopped
    }
}
