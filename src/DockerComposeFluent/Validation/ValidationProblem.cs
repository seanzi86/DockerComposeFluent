namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// One problem found by <c>DockerComposeFile.Validate</c>.
    /// </summary>
    /// <param name="Severity">Whether Compose rejects the file (an error) or merely may misbehave (a warning).</param>
    /// <param name="Code">The stable code of the check, such as <c>DCF001</c>; see <see cref="ValidationCodes"/>.</param>
    /// <param name="Path">Where the problem is, for example <c>services.web.depends_on.db</c>.</param>
    /// <param name="Message">A description of the problem, naming the services and properties involved.</param>
    public sealed record ValidationProblem(ValidationSeverity Severity, string Code, string Path, string Message)
    {
        /// <summary>
        /// Formats the problem on one line, for example
        /// <c>Error DCF001 services.web.depends_on.db: Service "web" depends on service "db", which is not defined.</c>.
        /// </summary>
        /// <returns>The formatted problem.</returns>
        public override string ToString()
        {
            return Severity + " " + Code + " " + Path + ": " + Message;
        }
    }
}
