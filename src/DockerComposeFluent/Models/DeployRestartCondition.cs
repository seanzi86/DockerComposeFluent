namespace DockerComposeFluent.Models
{
    /// <summary>
    /// When a container is restarted under <c>deploy.restart_policy</c>, as specified by <c>condition</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#restart_policy"/>
    /// </summary>
    public enum DeployRestartCondition
    {
        /// <summary>
        /// Containers are not automatically restarted regardless of the exit status (<c>none</c>).
        /// </summary>
        None,

        /// <summary>
        /// The container is restarted if it exits with a non-zero status (<c>on-failure</c>).
        /// </summary>
        OnFailure,

        /// <summary>
        /// Containers are restarted regardless of the exit status (<c>any</c>), the default.
        /// </summary>
        Any
    }
}
