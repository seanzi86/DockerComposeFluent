using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A command or entrypoint, given either as a single shell string or as a list of arguments.
    /// The form chosen is preserved in the generated YAML.
    /// </summary>
    public sealed record CommandLine
    {
        private CommandLine(string? shell, IReadOnlyList<string>? arguments)
        {
            Shell = shell;
            Arguments = arguments;
        }

        /// <summary>
        /// The command as a single shell-form string, or <c>null</c> when it was given as a list of arguments.
        /// </summary>
        public string? Shell { get; }

        /// <summary>
        /// The command as an exec-form list of arguments, or <c>null</c> when it was given as a single string.
        /// An empty list is valid and clears any command or entrypoint defined by the image.
        /// </summary>
        public IReadOnlyList<string>? Arguments { get; }

        /// <summary>
        /// Creates a shell-form command from a single string.
        /// </summary>
        /// <param name="command">The command, for example <c>nginx -g "daemon off;"</c>.</param>
        /// <returns>A new <see cref="CommandLine"/>.</returns>
        public static CommandLine FromShell(string command)
        {
            Guard.NotNullOrWhiteSpace(command, nameof(command));

            return new CommandLine(command, null);
        }

        /// <summary>
        /// Creates an exec-form command from a list of arguments.
        /// </summary>
        /// <param name="arguments">The arguments. May be empty, but no argument may be <c>null</c>.</param>
        /// <returns>A new <see cref="CommandLine"/>.</returns>
        public static CommandLine FromArguments(IEnumerable<string> arguments)
        {
            Guard.NotNull(arguments, nameof(arguments));

            List<string> copy = new List<string>(arguments);
            foreach (string argument in copy)
            {
                Guard.NotNull(argument, nameof(arguments));
            }

            return new CommandLine(null, copy.AsReadOnly());
        }
    }
}
