namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// How serious a <see cref="ValidationProblem"/> is.
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// Worth knowing about, but Compose accepts the file. It typically only goes wrong at
        /// <c>docker compose up</c>, or may be intentional.
        /// </summary>
        Warning,

        /// <summary>
        /// Compose rejects the file as written.
        /// </summary>
        Error
    }
}
