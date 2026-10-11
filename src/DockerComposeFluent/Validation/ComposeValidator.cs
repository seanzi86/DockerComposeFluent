using DockerComposeFluent.Models;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Runs every check against a whole <see cref="DockerComposeFile"/>, which individual builders cannot do
    /// because a service builder knows nothing about the other services, networks or volumes.
    /// </summary>
    internal static class ComposeValidator
    {
        /// <summary>
        /// Validates a compose file.
        /// </summary>
        /// <param name="file">The file to check.</param>
        /// <returns>Every problem found.</returns>
        internal static ValidationResult Validate(DockerComposeFile file)
        {
            ProblemCollector problems = new ProblemCollector();

            ReferenceChecks.Run(file, problems);
            CycleChecks.Run(file, problems);
            HealthcheckChecks.Run(file, problems);
            PortChecks.Run(file, problems);

            return new ValidationResult(problems.Problems);
        }
    }
}
