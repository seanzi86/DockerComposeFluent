using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class MountModelTests
    {
        [Fact]
        public void MountDefinition_SetsTypeAndTarget()
        {
            MountDefinition mount = new MountDefinition(MountType.Bind, "/app");

            Assert.Equal(MountType.Bind, mount.Type);
            Assert.Equal("/app", mount.Target);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void MountDefinition_BlankTarget_Throws(string? target)
        {
            Assert.Throws<ArgumentException>(() => new MountDefinition(MountType.Volume, target!));
        }

        [Fact]
        public void MountDefinition_With_KeepsTypeAndTarget()
        {
            MountDefinition original = new MountDefinition(MountType.Volume, "/data") { Source = "a" };

            MountDefinition updated = original with { Source = "b" };

            Assert.Equal(MountType.Volume, updated.Type);
            Assert.Equal("/data", updated.Target);
            Assert.Equal("a", original.Source);
        }

        [Fact]
        public void MountMapping_FromShortSyntax_SetsShortSyntaxOnly()
        {
            MountMapping mapping = MountMapping.FromShortSyntax("db-data:/data:ro");

            Assert.Equal("db-data:/data:ro", mapping.ShortSyntax);
            Assert.Null(mapping.Definition);
        }

        [Fact]
        public void MountMapping_FromDefinition_SetsDefinitionOnly()
        {
            MountDefinition definition = new MountDefinition(MountType.Tmpfs, "/tmp");

            MountMapping mapping = MountMapping.FromDefinition(definition);

            Assert.Same(definition, mapping.Definition);
            Assert.Null(mapping.ShortSyntax);
        }

        [Fact]
        public void MountMapping_InvalidArguments_Throw()
        {
            Assert.Throws<ArgumentException>(() => MountMapping.FromShortSyntax(" "));
            Assert.Throws<ArgumentNullException>(() => MountMapping.FromDefinition(null!));
        }
    }
}
