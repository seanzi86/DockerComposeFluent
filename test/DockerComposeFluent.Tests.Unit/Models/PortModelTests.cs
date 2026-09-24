using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class PortModelTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(80)]
        [InlineData(65535)]
        public void PortDefinition_ValidTarget_IsAccepted(int target)
        {
            Assert.Equal(target, new PortDefinition(target).Target);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(65536)]
        public void PortDefinition_InvalidTarget_Throws(int target)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PortDefinition(target));
        }

        [Fact]
        public void PortDefinition_With_KeepsTarget()
        {
            PortDefinition original = new PortDefinition(80) { Name = "web" };

            PortDefinition updated = original with { Name = "api" };

            Assert.Equal(80, updated.Target);
            Assert.Equal("api", updated.Name);
            Assert.Equal("web", original.Name);
        }

        [Fact]
        public void PortMapping_FromShortSyntax_SetsShortSyntaxOnly()
        {
            PortMapping mapping = PortMapping.FromShortSyntax("8080:80");

            Assert.Equal("8080:80", mapping.ShortSyntax);
            Assert.Null(mapping.Definition);
        }

        [Fact]
        public void PortMapping_FromDefinition_SetsDefinitionOnly()
        {
            PortDefinition definition = new PortDefinition(80);

            PortMapping mapping = PortMapping.FromDefinition(definition);

            Assert.Same(definition, mapping.Definition);
            Assert.Null(mapping.ShortSyntax);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void PortMapping_BlankShortSyntax_Throws(string? port)
        {
            Assert.Throws<ArgumentException>(() => PortMapping.FromShortSyntax(port!));
        }

        [Fact]
        public void PortMapping_NullDefinition_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => PortMapping.FromDefinition(null!));
        }
    }
}
