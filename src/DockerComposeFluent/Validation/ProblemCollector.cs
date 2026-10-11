using System.Collections.Generic;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Collects the problems found by the checks, in the order they are found.
    /// </summary>
    internal sealed class ProblemCollector
    {
        private readonly List<ValidationProblem> _problems = new List<ValidationProblem>();

        /// <summary>
        /// Gets the problems collected so far.
        /// </summary>
        internal IReadOnlyList<ValidationProblem> Problems
        {
            get { return _problems; }
        }

        /// <summary>
        /// Adds a problem.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="code">The code of the check.</param>
        /// <param name="path">Where the problem is.</param>
        /// <param name="message">The description.</param>
        internal void Add(ValidationSeverity severity, string code, string path, string message)
        {
            _problems.Add(new ValidationProblem(severity, code, path, message));
        }
    }
}
