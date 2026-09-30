using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds an <see cref="IncludeDefinition"/>. At least one path is required.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
    /// </summary>
    public sealed class IncludeBuilder
    {
        private IncludeDefinition _definition = new IncludeDefinition();

        /// <summary>
        /// Adds a path to a Compose file to include.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>This builder.</returns>
        public IncludeBuilder WithPath(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { Path = Collections.Append(_definition.Path, path) };
            return this;
        }

        /// <summary>
        /// Adds several paths to Compose files to include.
        /// </summary>
        /// <param name="paths">The file paths.</param>
        /// <returns>This builder.</returns>
        public IncludeBuilder WithPaths(IEnumerable<string> paths)
        {
            Guard.NotNull(paths, nameof(paths));

            foreach (string path in paths)
            {
                WithPath(path);
            }

            return this;
        }

        /// <summary>
        /// Adds a path to an environment file that provides default values when interpolating variables in
        /// the included files.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>This builder.</returns>
        public IncludeBuilder WithEnvFile(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { EnvFile = Collections.Append(_definition.EnvFile, path) };
            return this;
        }

        /// <summary>
        /// Adds several paths to environment files that provide default values when interpolating variables
        /// in the included files.
        /// </summary>
        /// <param name="paths">The file paths.</param>
        /// <returns>This builder.</returns>
        public IncludeBuilder WithEnvFiles(IEnumerable<string> paths)
        {
            Guard.NotNull(paths, nameof(paths));

            foreach (string path in paths)
            {
                WithEnvFile(path);
            }

            return this;
        }

        /// <summary>
        /// Sets the directory relative paths in the included file are resolved against. Defaults to the
        /// included file's own directory if not set.
        /// </summary>
        /// <param name="projectDirectory">The directory path.</param>
        /// <returns>This builder.</returns>
        public IncludeBuilder WithProjectDirectory(string projectDirectory)
        {
            Guard.NotNullOrWhiteSpace(projectDirectory, nameof(projectDirectory));
            _definition = _definition with { ProjectDirectory = projectDirectory };
            return this;
        }

        /// <summary>
        /// Creates the include definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="IncludeDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">No path was set.</exception>
        public IncludeDefinition Build()
        {
            if (_definition.Path.Count == 0)
            {
                throw new InvalidOperationException("An include must set at least one path. Call WithPath before Build.");
            }

            return _definition;
        }
    }
}
