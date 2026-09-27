using System;
using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class LoggingBuilderTests
    {
        [Fact]
        public void Build_NothingSet_HasNoDriverOrOptions()
        {
            LoggingDefinition logging = new LoggingBuilder().Build();

            Assert.Equal(new LoggingDefinition(), logging);
        }

        [Fact]
        public void WithDriver_SetsIt()
        {
            LoggingDefinition logging = new LoggingBuilder().WithDriver("syslog").Build();

            Assert.Equal("syslog", logging.Driver);
        }

        [Fact]
        public void WithOption_AddsOne_AndReplacesOnSameKey()
        {
            LoggingDefinition logging = new LoggingBuilder()
                .WithOption("syslog-address", "tcp://192.168.0.42:123")
                .WithOption("syslog-address", "tcp://192.168.0.43:123")
                .Build();

            Assert.Equal("tcp://192.168.0.43:123", logging.Options["syslog-address"]);
        }

        [Fact]
        public void WithOptions_AddsEachOne()
        {
            LoggingDefinition logging = new LoggingBuilder()
                .WithOptions(new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" })
                .Build();

            Assert.Equal("1", logging.Options["a"]);
            Assert.Equal("2", logging.Options["b"]);
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            LoggingBuilder builder = new LoggingBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithDriver(" "));
            Assert.Throws<ArgumentException>(() => builder.WithOption(" ", "x"));
            Assert.Throws<ArgumentNullException>(() => builder.WithOption("key", null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithOptions(null!));
        }
    }
}
