using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ServiceSettingsBuilderTests
    {
        [Fact]
        public void StringSettings_AreSet()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithHostname("h").WithDomainName("d").WithUser("u").WithWorkingDir("/w")
                .WithMacAddress("m").WithStopSignal("SIGINT").WithRuntime("r").WithIsolation("i")
                .Build();

            Assert.Equal("h", service.Hostname);
            Assert.Equal("d", service.DomainName);
            Assert.Equal("u", service.User);
            Assert.Equal("/w", service.WorkingDir);
            Assert.Equal("m", service.MacAddress);
            Assert.Equal("SIGINT", service.StopSignal);
            Assert.Equal("r", service.Runtime);
            Assert.Equal("i", service.Isolation);
        }

        [Fact]
        public void BooleanAndIntegerSettings_AreSet()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithInit(true).WithPrivileged(true).WithReadOnly(true).WithStdinOpen(true)
                .WithTty(true).WithAttach(false).WithUseApiSocket(true).WithScale(0)
                .Build();

            Assert.True(service.Init);
            Assert.True(service.Privileged);
            Assert.True(service.ReadOnly);
            Assert.True(service.StdinOpen);
            Assert.True(service.Tty);
            Assert.False(service.Attach);
            Assert.True(service.UseApiSocket);
            Assert.Equal(0, service.Scale);
        }

        [Fact]
        public void Durations_AcceptTextAndTimeSpan()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithStopGracePeriod(TimeSpan.FromSeconds(90))
                .WithPullRefreshAfter("48h")
                .Build();

            Assert.Equal("1m30s", service.StopGracePeriod);
            Assert.Equal("48h", service.PullRefreshAfter);
        }

        [Theory]
        [InlineData("always")]
        [InlineData("never")]
        [InlineData("build")]
        [InlineData("if_not_present")]
        [InlineData("missing")]
        [InlineData("refresh")]
        [InlineData("daily")]
        [InlineData("weekly")]
        [InlineData("every_1d12h")]
        public void WithPullPolicy_ValidValue_IsSet(string policy)
        {
            Assert.Equal(policy, new ServiceBuilder().WithPullPolicy(policy).Build().PullPolicy);
        }

        [Theory]
        [InlineData("")]
        [InlineData("sometimes")]
        [InlineData("every_")]
        [InlineData("every_12")]
        public void WithPullPolicy_InvalidValue_Throws(string policy)
        {
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithPullPolicy(policy));
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithHostname(" "));
            Assert.Throws<ArgumentException>(() => builder.WithUser(" "));
            Assert.Throws<ArgumentException>(() => builder.WithStopGracePeriod("soon"));
            Assert.Throws<ArgumentException>(() => builder.WithPullRefreshAfter("soon"));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithStopGracePeriod(TimeSpan.Zero));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithScale(-1));
        }
    }
}
