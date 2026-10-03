using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ConfigBuilderTests
    {
        [Fact]
        public void WithFile_SetsIt()
        {
            ConfigDefinition config = new ConfigBuilder().WithFile("./httpd.conf").Build();

            Assert.Equal("./httpd.conf", config.File);
        }

        [Fact]
        public void WithEnvironment_SetsIt()
        {
            ConfigDefinition config = new ConfigBuilder().WithEnvironment("HTTP_CONFIG").Build();

            Assert.Equal("HTTP_CONFIG", config.Environment);
        }

        [Fact]
        public void WithContent_SetsIt()
        {
            ConfigDefinition config = new ConfigBuilder().WithContent("debug=true").Build();

            Assert.Equal("debug=true", config.Content);
        }

        [Fact]
        public void WithContent_EmptyString_IsAllowed()
        {
            ConfigDefinition config = new ConfigBuilder().WithContent("").Build();

            Assert.Equal("", config.Content);
        }

        [Fact]
        public void WithExternal_NoOtherSettings_IsAllowed()
        {
            ConfigDefinition config = new ConfigBuilder().WithExternal(true).Build();

            Assert.True(config.External);
        }

        [Fact]
        public void WithExternal_AndName_IsAllowed()
        {
            ConfigDefinition config = new ConfigBuilder().WithExternal(true).WithName("HTTP_CONFIG_KEY").Build();

            Assert.True(config.External);
            Assert.Equal("HTTP_CONFIG_KEY", config.Name);
        }

        [Fact]
        public void Build_NoSource_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new ConfigBuilder().Build());
        }

        [Fact]
        public void Build_MultipleSources_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new ConfigBuilder().WithFile("./x").WithContent("y").Build());
        }

        [Fact]
        public void Build_ExternalWithSource_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new ConfigBuilder().WithExternal(true).WithContent("y").Build());
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            ConfigBuilder builder = new ConfigBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithFile(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment(" "));
            Assert.Throws<ArgumentException>(() => builder.WithName(" "));
            Assert.Throws<ArgumentNullException>(() => builder.WithContent(null!));
        }
    }
}
