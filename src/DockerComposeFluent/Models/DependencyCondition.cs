namespace DockerComposeFluent.Models
{
    /// <summary>
    /// When a dependency of a service is considered satisfied, as specified by <c>condition</c> under
    /// <c>depends_on</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
    /// </summary>
    public enum DependencyCondition
    {
        /// <summary>
        /// The dependency has been started (<c>service_started</c>), the default.
        /// </summary>
        ServiceStarted,

        /// <summary>
        /// The dependency is healthy (<c>service_healthy</c>), as reported by its <c>healthcheck</c>. The
        /// dependency must define a healthcheck for this to work.
        /// </summary>
        ServiceHealthy,

        /// <summary>
        /// The dependency has run to completion and exited successfully (<c>service_completed_successfully</c>).
        /// </summary>
        ServiceCompletedSuccessfully
    }
}
