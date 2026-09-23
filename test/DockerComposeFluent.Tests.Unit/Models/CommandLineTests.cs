using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class CommandLineTests
    {
        [Fact]
        public void FromShell_SetsShellOnly()
        {
            CommandLine command = CommandLine.FromShell("nginx -g 'daemon off;'");

            Assert.Equal("nginx -g 'daemon off;'", command.Shell);
            Assert.Null(command.Arguments);
        }

        [Fact]
        public void FromArguments_SetsArgumentsOnly()
        {
            CommandLine command = CommandLine.FromArguments(new[] { "nginx", "-g", "daemon off;" });

            Assert.Null(command.Shell);
            Assert.Equal(new[] { "nginx", "-g", "daemon off;" }, command.Arguments);
        }

        [Fact]
        public void FromArguments_AllowsEmptyList()
        {
            CommandLine command = CommandLine.FromArguments(Array.Empty<string>());

            Assert.Empty(command.Arguments!);
        }

        [Fact]
        public void FromArguments_CopiesInput_SoLaterChangesDoNotAffectIt()
        {
            List<string> arguments = new List<string> { "a" };
            CommandLine command = CommandLine.FromArguments(arguments);

            arguments.Add("b");

            Assert.Single(command.Arguments!);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void FromShell_BlankValue_Throws(string? value)
        {
            Assert.Throws<ArgumentException>(() => CommandLine.FromShell(value!));
        }

        [Fact]
        public void FromArguments_NullListOrElement_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CommandLine.FromArguments(null!));
            Assert.Throws<ArgumentNullException>(() => CommandLine.FromArguments(new string[] { "a", null! }));
        }
    }
}
