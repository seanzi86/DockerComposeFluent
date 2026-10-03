namespace DockerComposeFluent.Models
{
    /// <summary>
    /// What to do if a rollback fails, as specified by <c>failure_action</c> under
    /// <c>deploy.rollback_config</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#rollback_config"/>
    /// </summary>
    public enum RollbackFailureAction
    {
        /// <summary>
        /// Keep going despite the failure (<c>continue</c>).
        /// </summary>
        Continue,

        /// <summary>
        /// Stop the rollback (<c>pause</c>), the default.
        /// </summary>
        Pause
    }
}
