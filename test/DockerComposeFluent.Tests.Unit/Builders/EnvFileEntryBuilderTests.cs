using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class EnvFileEntryBuilderTests
    {
        [Fact]
        public void Build_PathOnly_IsPathOnly()
        {
            EnvFileEntry entry = new EnvFileEntryBuilder().WithPath("./.env").Build();

            Assert.Equal("./.env", entry.Path);
            Assert.True(entry.IsPathOnly);
            Assert.Null(entry.Required);
            Assert.Null(entry.Format);
        }

        [Fact]
        public void Build_EverySetting_IsSet()
        {
            EnvFileEntry entry = new EnvFileEntryBuilder()
                .WithPath("./.env")
                .WithRequired(false)
                .WithFormat(EnvFileFormat.Raw)
                .Build();

            Assert.False(entry.IsPathOnly);
            Assert.False(entry.Required);
            Assert.Equal(EnvFileFormat.Raw, entry.Format);
        }

        [Fact]
        public void Build_NoPath_Throws()
        {
            Assert.Throws<ArgumentException>(() => new EnvFileEntryBuilder().WithRequired(true).Build());
        }

        [Fact]
        public void WithPath_InvalidInput_Throws()
        {
            Assert.Throws<ArgumentException>(() => new EnvFileEntryBuilder().WithPath(" "));
        }
    }
}
