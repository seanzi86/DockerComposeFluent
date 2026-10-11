using System;
using System.Collections.Generic;
using System.Linq;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// The outcome of <c>DockerComposeFile.Validate</c>: every problem found, in a stable order.
    /// </summary>
    public sealed class ValidationResult
    {
        internal ValidationResult(IReadOnlyList<ValidationProblem> problems)
        {
            Problems = problems;
        }

        /// <summary>
        /// Every problem found, errors and warnings, in a stable order.
        /// </summary>
        public IReadOnlyList<ValidationProblem> Problems { get; }

        /// <summary>
        /// The problems Compose would reject the file for.
        /// </summary>
        public IReadOnlyList<ValidationProblem> Errors
        {
            get { return Problems.Where(problem => problem.Severity == ValidationSeverity.Error).ToList(); }
        }

        /// <summary>
        /// The problems that may be worth fixing but that Compose accepts.
        /// </summary>
        public IReadOnlyList<ValidationProblem> Warnings
        {
            get { return Problems.Where(problem => problem.Severity == ValidationSeverity.Warning).ToList(); }
        }

        /// <summary>
        /// Whether there are no errors. Warnings do not make a file invalid.
        /// </summary>
        public bool IsValid
        {
            get { return !Problems.Any(problem => problem.Severity == ValidationSeverity.Error); }
        }

        /// <summary>
        /// Returns a copy of this result without the problems of the given checks, so that
        /// <see cref="IsValid"/> and <see cref="ThrowIfInvalid"/> leave them out too.
        /// </summary>
        /// <param name="codes">The codes to ignore, for example <see cref="ValidationCodes.DuplicatePublishedPort"/>.
        /// Codes that never occurred are fine.</param>
        /// <returns>A new result.</returns>
        public ValidationResult Ignoring(params string[] codes)
        {
            Guard.NotNull(codes, nameof(codes));

            return new ValidationResult(Problems.Where(problem => Array.IndexOf(codes, problem.Code) < 0).ToList());
        }

        /// <summary>
        /// Throws if there are any errors. Warnings never throw.
        /// </summary>
        /// <exception cref="ComposeValidationException">The file has at least one error.</exception>
        public void ThrowIfInvalid()
        {
            if (!IsValid)
            {
                throw new ComposeValidationException(Problems);
            }
        }
    }
}
