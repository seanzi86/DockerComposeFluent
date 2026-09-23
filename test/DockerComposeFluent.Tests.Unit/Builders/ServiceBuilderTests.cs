using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ServiceBuilderTests
    {
        [Fact]
        public void Build_WithNothingSet_ProducesEmptyDefinition()
        {
            Assert.Equal(new ServiceDefinition(), new ServiceBuilder().Build());
        }

        [Fact]
        public void With_Methods_SetProperties()
        {
            ServiceDefinition service = new ServiceBuilder().WithImage("nginx").WithContainerName("web").Build();

            Assert.Equal("nginx", service.Image);
            Assert.Equal("web", service.ContainerName);
        }

        [Fact]
        public void Build_ReturnsSnapshot_UnaffectedByLaterChanges()
        {
            ServiceBuilder builder = new ServiceBuilder().WithImage("nginx");
            ServiceDefinition first = builder.Build();

            builder.WithImage("apache");

            Assert.Equal("nginx", first.Image);
        }

        [Fact]
        public void BlankValues_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithImage(" "));
            Assert.Throws<ArgumentException>(() => builder.WithContainerName(""));
        }

        [Fact]
        public void WithCommand_String_UsesShellForm()
        {
            ServiceDefinition service = new ServiceBuilder().WithCommand("npm start").Build();

            Assert.Equal("npm start", service.Command!.Shell);
            Assert.Null(service.Command.Arguments);
        }

        [Fact]
        public void WithCommand_List_UsesExecForm()
        {
            ServiceDefinition service = new ServiceBuilder().WithCommand(new[] { "npm", "start" }).Build();

            Assert.Null(service.Command!.Shell);
            Assert.Equal(new[] { "npm", "start" }, service.Command.Arguments);
        }

        [Fact]
        public void WithEntrypoint_BothForms_SetEntrypoint()
        {
            ServiceDefinition shell = new ServiceBuilder().WithEntrypoint("/start.sh").Build();
            ServiceDefinition exec = new ServiceBuilder().WithEntrypoint(new[] { "/start.sh", "--fast" }).Build();

            Assert.Equal("/start.sh", shell.Entrypoint!.Shell);
            Assert.Equal(new[] { "/start.sh", "--fast" }, exec.Entrypoint!.Arguments);
        }

        [Fact]
        public void WithCommand_CalledTwice_LastFormWins()
        {
            ServiceDefinition service = new ServiceBuilder().WithCommand("a").WithCommand(new[] { "b" }).Build();

            Assert.Null(service.Command!.Shell);
            Assert.Equal(new[] { "b" }, service.Command.Arguments);
        }

        [Fact]
        public void BlankCommandAndNullArguments_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithCommand(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEntrypoint(""));
            Assert.Throws<ArgumentNullException>(() => builder.WithCommand((string[])null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithEntrypoint((string[])null!));
        }
    }
}
