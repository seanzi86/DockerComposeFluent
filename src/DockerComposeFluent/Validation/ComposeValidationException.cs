using System;
using System.Collections.Generic;
using System.Linq;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Thrown by <see cref="ValidationResult.ThrowIfInvalid"/> when the compose file has errors.
    /// </summary>
    public sealed class ComposeValidationException : Exception
    {
        /// <summary>
        /// Creates the exception for the problems found.
        /// </summary>
        /// <param name="problems">Every problem found, including warnings.</param>
        public ComposeValidationException(IReadOnlyList<ValidationProblem> problems)
            : base(BuildMessage(problems))
        {
            Problems = problems;
        }

        /// <summary>
        /// Every problem found, including warnings.
        /// </summary>
        public IReadOnlyList<ValidationProblem> Problems { get; }

        private static string BuildMessage(IReadOnlyList<ValidationProblem> problems)
        {
            List<ValidationProblem> errors = problems.Where(problem => problem.Severity == ValidationSeverity.Error).ToList();
            string header = errors.Count == 1
                ? "The compose file has 1 error:"
                : "The compose file has " + errors.Count + " errors:";

            return header + string.Concat(errors.Select(error => Environment.NewLine + "  " + error));
        }
    }
}
