using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class HealthcheckBuilderTests
    {
        [Fact]
        public void Build_NothingSet_HasNoValues()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder().Build();

            Assert.Equal(new HealthcheckDefinition(), healthcheck);
        }

        [Fact]
        public void Build_SetsEveryField()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder()
                .WithTest("curl -f http://localhost || exit 1")
                .WithInterval("1m30s")
                .WithTimeout("10s")
                .WithRetries(3)
                .WithStartPeriod("40s")
                .WithStartInterval("5s")
                .WithDisable(false)
                .Build();

            Assert.Equal("curl -f http://localhost || exit 1", healthcheck.Test!.Shell);
            Assert.Equal("1m30s", healthcheck.Interval);
            Assert.Equal("10s", healthcheck.Timeout);
            Assert.Equal(3, healthcheck.Retries);
            Assert.Equal("40s", healthcheck.StartPeriod);
            Assert.Equal("5s", healthcheck.StartInterval);
            Assert.False(healthcheck.Disable);
        }

        [Fact]
        public void WithTest_List_KeepsTheList()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder().WithTest(new[] { "CMD-SHELL", "exit 0" }).Build();

            Assert.Null(healthcheck.Test!.Shell);
            Assert.Equal(new[] { "CMD-SHELL", "exit 0" }, healthcheck.Test.Arguments);
        }

        [Fact]
        public void WithCommand_AddsCmd()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder().WithCommand(new[] { "curl", "-f", "http://localhost" }).Build();

            Assert.Equal(new[] { "CMD", "curl", "-f", "http://localhost" }, healthcheck.Test!.Arguments);
        }

        [Fact]
        public void WithTest_None_IsAllowed()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder().WithTest(new[] { "NONE" }).Build();

            Assert.Equal(new[] { "NONE" }, healthcheck.Test!.Arguments);
        }

        [Fact]
        public void WithTest_CalledTwice_LastFormWins()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder().WithTest("a").WithTest(new[] { "CMD", "b" }).Build();

            Assert.Null(healthcheck.Test!.Shell);
            Assert.Equal(new[] { "CMD", "b" }, healthcheck.Test.Arguments);
        }

        [Fact]
        public void Durations_TimeSpan_AreWrittenAsComposeDurations()
        {
            HealthcheckDefinition healthcheck = new HealthcheckBuilder()
                .WithInterval(TimeSpan.FromSeconds(90))
                .WithTimeout(TimeSpan.FromSeconds(10))
                .WithStartPeriod(TimeSpan.FromMinutes(2))
                .WithStartInterval(TimeSpan.FromMilliseconds(500))
                .Build();

            Assert.Equal("1m30s", healthcheck.Interval);
            Assert.Equal("10s", healthcheck.Timeout);
            Assert.Equal("2m", healthcheck.StartPeriod);
            Assert.Equal("500ms", healthcheck.StartInterval);
        }

        [Fact]
        public void InvalidTest_Throws()
        {
            HealthcheckBuilder builder = new HealthcheckBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithTest(" "));
            Assert.Throws<ArgumentException>(() => builder.WithTest(new[] { "curl", "-f" }));
            Assert.Throws<ArgumentException>(() => builder.WithTest(Array.Empty<string>()));
            Assert.Throws<ArgumentException>(() => builder.WithTest(new[] { "cmd", "curl" }));
            Assert.Throws<ArgumentNullException>(() => builder.WithTest((string[])null!));
            Assert.Throws<ArgumentException>(() => builder.WithCommand(Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => builder.WithCommand(null!));
        }

        [Fact]
        public void InvalidDurations_Throw()
        {
            HealthcheckBuilder builder = new HealthcheckBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithInterval("soon"));
            Assert.Throws<ArgumentException>(() => builder.WithTimeout("10"));
            Assert.Throws<ArgumentException>(() => builder.WithStartPeriod(""));
            Assert.Throws<ArgumentException>(() => builder.WithStartInterval("1.5s"));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithInterval(TimeSpan.Zero));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithTimeout(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void WithRetries_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HealthcheckBuilder().WithRetries(-1));
            Assert.Equal(0, new HealthcheckBuilder().WithRetries(0).Build().Retries);
        }
    }
}
