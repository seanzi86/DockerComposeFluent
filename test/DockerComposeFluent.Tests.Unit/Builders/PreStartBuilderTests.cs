using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class PreStartBuilderTests
    {
        [Fact]
        public void EverySetting_IsSet()
        {
            PreStartDefinition step = new PreStartBuilder()
                .WithCommand(new[] { "a", "b" })
                .WithImage("busybox")
                .WithUser("root")
                .WithPrivileged(true)
                .WithWorkingDir("/data")
                .WithEnvironment("MODE", "init")
                .WithPerReplica(true)
                .WithExtension("x-note", "n")
                .Build();

            Assert.Equal(new[] { "a", "b" }, step.Command!.Arguments);
            Assert.Equal("busybox", step.Image);
            Assert.Equal("root", step.User);
            Assert.True(step.Privileged);
            Assert.Equal("/data", step.WorkingDir);
            Assert.Equal("init", step.Environment["MODE"]);
            Assert.True(step.PerReplica);
            Assert.Equal("n", step.Extensions["x-note"]);
        }

        [Fact]
        public void NothingSet_IsAllowed_BecauseTheImageEntrypointMayDoTheWork()
        {
            Assert.Equal(new PreStartDefinition(), new PreStartBuilder().Build());
        }

        [Fact]
        public void Environment_SameNameReplaces()
        {
            PreStartDefinition step = new PreStartBuilder().WithEnvironment("A", "1").WithEnvironment("A", "2").Build();

            Assert.Equal("2", step.Environment["A"]);
            Assert.Single(step.Environment);
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            PreStartBuilder builder = new PreStartBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithImage(" "));
            Assert.Throws<ArgumentException>(() => builder.WithUser(" "));
            Assert.Throws<ArgumentException>(() => builder.WithWorkingDir(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment("A=B", "x"));
            Assert.Throws<ArgumentException>(() => builder.WithExtension("note", "x"));
        }

        [Fact]
        public void Service_StepsAccumulateInOrder_AcrossAllThreeOverloads()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithPreStart("one.sh")
                .WithPreStart(new PreStartDefinition { Image = "two" })
                .WithPreStart(step => step.WithImage("three"))
                .Build();

            Assert.Equal(3, service.PreStart.Count);
            Assert.Equal("two", service.PreStart[1].Image);
            Assert.Equal("three", service.PreStart[2].Image);
            Assert.Throws<ArgumentNullException>(() => new ServiceBuilder().WithPreStart((PreStartDefinition)null!));
        }
    }
}
