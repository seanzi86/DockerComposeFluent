using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class SmallGapBuilderTests
    {
        [Fact]
        public void Annotation_SameKeyReplaces()
        {
            ServiceDefinition service = new ServiceBuilder().WithAnnotation("a", "1").WithAnnotation("a", "2").Build();

            Assert.Equal("2", service.Annotations["a"]);
            Assert.Single(service.Annotations);
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithAnnotation(" ", "x"));
        }

        [Fact]
        public void Hooks_AppendInOrder_AndAcceptAShellStringShortcut()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithPostStart("a.sh")
                .WithPostStart(hook => hook.WithCommand(new[] { "b" }))
                .WithPreStop("c.sh")
                .Build();

            Assert.Equal(2, service.PostStart.Count);
            Assert.Single(service.PreStop);
        }

        [Fact]
        public void Hook_RequiresACommand_AndValidatesInput()
        {
            Assert.Throws<InvalidOperationException>(() => new ServiceHookBuilder().Build());
            Assert.Throws<ArgumentException>(() => new ServiceHookBuilder().WithEnvironment("A=B", "x"));
            Assert.Throws<ArgumentException>(() => new ServiceHookBuilder().WithUser(" "));
            Assert.Throws<ArgumentException>(() => new ServiceHookBuilder().WithWorkingDir(" "));
        }

        [Fact]
        public void GenericResource_NeedsAKindAndAPositiveValue()
        {
            Assert.Throws<InvalidOperationException>(() => new DiscreteResourceSpecBuilder().WithKind("GPU").Build());
            Assert.Throws<InvalidOperationException>(() => new DiscreteResourceSpecBuilder().WithValue(1).Build());
            Assert.Throws<ArgumentOutOfRangeException>(() => new DiscreteResourceSpecBuilder().WithValue(0));

            ResourceReservationsDefinition reservations = new ResourceReservationsBuilder().WithGenericResource("GPU", 2).Build();

            Assert.Equal("GPU", reservations.GenericResources[0].DiscreteResourceSpec!.Kind);
            Assert.Equal(2, reservations.GenericResources[0].DiscreteResourceSpec!.Value);
        }

        [Fact]
        public void Secret_SetsTheOptionalSettings()
        {
            SecretDefinition secret = new SecretBuilder()
                .WithFile("./s")
                .WithDriver("vault")
                .WithDriverOption("k", "v")
                .WithLabel("l", "1")
                .WithTemplateDriver("golang")
                .Build();

            Assert.Equal("vault", secret.Driver);
            Assert.Equal("v", secret.DriverOptions["k"]);
            Assert.Equal("1", secret.Labels.Values["l"]);
            Assert.Equal("golang", secret.TemplateDriver);
            Assert.Throws<ArgumentException>(() => new SecretBuilder().WithDriver(" "));
            Assert.Throws<ArgumentException>(() => new SecretBuilder().WithLabel("com.docker.compose.x", "1"));
        }

        [Fact]
        public void Config_SetsTheOptionalSettings()
        {
            ConfigDefinition config = new ConfigBuilder()
                .WithContent("c")
                .WithLabels(new[] { "a=1", "b" })
                .WithTemplateDriver("golang")
                .Build();

            Assert.True(config.Labels.UsesListSyntax);
            Assert.Equal("1", config.Labels.Values["a"]);
            Assert.Equal("golang", config.TemplateDriver);
            Assert.Throws<ArgumentException>(() => new ConfigBuilder().WithTemplateDriver(" "));
        }
    }
}
