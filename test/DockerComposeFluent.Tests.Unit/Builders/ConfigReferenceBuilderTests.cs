using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ConfigReferenceBuilderTests
    {
        [Fact]
        public void WithSource_Only_IsSourceOnly()
        {
            ConfigReference reference = new ConfigReferenceBuilder().WithSource("my_config").Build();

            Assert.Equal("my_config", reference.Source);
            Assert.True(reference.IsSourceOnly);
        }

        [Fact]
        public void Build_EverySetting_IsSet()
        {
            ConfigReference reference = new ConfigReferenceBuilder()
                .WithSource("my_config")
                .WithTarget("/redis_config")
                .WithUid("103")
                .WithGid("103")
                .WithMode(Convert.ToInt32("440", 8))
                .Build();

            Assert.False(reference.IsSourceOnly);
            Assert.Equal("/redis_config", reference.Target);
            Assert.Equal("103", reference.Uid);
            Assert.Equal("103", reference.Gid);
            Assert.Equal(Convert.ToInt32("440", 8), reference.Mode);
        }

        [Fact]
        public void WithExtension_ForcesLongForm()
        {
            ConfigReference reference = new ConfigReferenceBuilder()
                .WithSource("my_config")
                .WithExtension("x-note", "generated at deploy time")
                .Build();

            Assert.False(reference.IsSourceOnly);
            Assert.Equal("generated at deploy time", reference.Extensions["x-note"]);
        }

        [Fact]
        public void Build_NoSource_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ConfigReferenceBuilder().Build());
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            ConfigReferenceBuilder builder = new ConfigReferenceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithSource(" "));
            Assert.Throws<ArgumentException>(() => builder.WithTarget(" "));
            Assert.Throws<ArgumentException>(() => builder.WithUid(" "));
            Assert.Throws<ArgumentException>(() => builder.WithGid(" "));
        }
    }
}
