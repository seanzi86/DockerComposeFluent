namespace DockerComposeFluent.Models
{
    /// <summary>
    /// What to do if an update fails, as specified by <c>failure_action</c> under <c>deploy.update_config</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#update_config"/>
    /// </summary>
    public enum UpdateFailureAction
    {
        /// <summary>
        /// Keep going despite the failure (<c>continue</c>).
        /// </summary>
        Continue,

        /// <summary>
        /// Stop the update (<c>pause</c>), the default.
        /// </summary>
        Pause,

        /// <summary>
        /// Roll back to the previous version (<c>rollback</c>).
        /// </summary>
        Rollback
    }
}
