using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class NetworkModelTests
    {
        [Fact]
        public void NetworkAttachment_Default_IsEmpty()
        {
            Assert.True(new NetworkAttachment().IsEmpty);
        }

        [Fact]
        public void NetworkAttachment_AnySetting_IsNotEmpty()
        {
            Assert.False((new NetworkAttachment() with { Aliases = new[] { "a" } }).IsEmpty);
            Assert.False((new NetworkAttachment() with { Priority = 0 }).IsEmpty);
            Assert.False((new NetworkAttachment() with { GatewayPriority = 0 }).IsEmpty);
            Assert.False((new NetworkAttachment() with { Ipv4Address = "10.0.0.1" }).IsEmpty);
            Assert.False((new NetworkAttachment() with { MacAddress = "02:42:ac:11:00:02" }).IsEmpty);
        }

        [Fact]
        public void ModelsWithNoEntries_CompareAsEqual()
        {
            Assert.Equal(new ServiceDefinition(), new ServiceDefinition());
            Assert.Equal(new NetworkDefinition(), new NetworkDefinition());
            Assert.Equal(new DockerComposeFile(), new DockerComposeFile());
        }
    }
}
