using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ServiceBuilderTests
    {
        [Fact]
        public void Build_WithNothingSet_ProducesEmptyDefinition()
        {
            Assert.Equal(new ServiceDefinition(), new ServiceBuilder().Build());
        }

        [Fact]
        public void With_Methods_SetProperties()
        {
            ServiceDefinition service = new ServiceBuilder().WithImage("nginx").WithContainerName("web").Build();

            Assert.Equal("nginx", service.Image);
            Assert.Equal("web", service.ContainerName);
        }

        [Fact]
        public void Build_ReturnsSnapshot_UnaffectedByLaterChanges()
        {
            ServiceBuilder builder = new ServiceBuilder().WithImage("nginx");
            ServiceDefinition first = builder.Build();

            builder.WithImage("apache");

            Assert.Equal("nginx", first.Image);
        }

        [Fact]
        public void BlankValues_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithImage(" "));
            Assert.Throws<ArgumentException>(() => builder.WithContainerName(""));
        }

        [Fact]
        public void WithCommand_String_UsesShellForm()
        {
            ServiceDefinition service = new ServiceBuilder().WithCommand("npm start").Build();

            Assert.Equal("npm start", service.Command!.Shell);
            Assert.Null(service.Command.Arguments);
        }

        [Fact]
        public void WithCommand_List_UsesExecForm()
        {
            ServiceDefinition service = new ServiceBuilder().WithCommand(new[] { "npm", "start" }).Build();

            Assert.Null(service.Command!.Shell);
            Assert.Equal(new[] { "npm", "start" }, service.Command.Arguments);
        }

        [Fact]
        public void WithEntrypoint_BothForms_SetEntrypoint()
        {
            ServiceDefinition shell = new ServiceBuilder().WithEntrypoint("/start.sh").Build();
            ServiceDefinition exec = new ServiceBuilder().WithEntrypoint(new[] { "/start.sh", "--fast" }).Build();

            Assert.Equal("/start.sh", shell.Entrypoint!.Shell);
            Assert.Equal(new[] { "/start.sh", "--fast" }, exec.Entrypoint!.Arguments);
        }

        [Fact]
        public void WithCommand_CalledTwice_LastFormWins()
        {
            ServiceDefinition service = new ServiceBuilder().WithCommand("a").WithCommand(new[] { "b" }).Build();

            Assert.Null(service.Command!.Shell);
            Assert.Equal(new[] { "b" }, service.Command.Arguments);
        }

        [Fact]
        public void BlankCommandAndNullArguments_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithCommand(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEntrypoint(""));
            Assert.Throws<ArgumentNullException>(() => builder.WithCommand((string[])null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithEntrypoint((string[])null!));
        }

        [Fact]
        public void WithPort_String_UsesShortSyntax()
        {
            ServiceDefinition service = new ServiceBuilder().WithPort("8080:80").Build();

            Assert.Equal("8080:80", service.Ports[0].ShortSyntax);
            Assert.Null(service.Ports[0].Definition);
        }

        [Fact]
        public void WithPort_TargetOnly_UsesLongSyntaxWithoutPublishedPort()
        {
            ServiceDefinition service = new ServiceBuilder().WithPort(80).Build();

            PortDefinition port = service.Ports[0].Definition!;
            Assert.Null(service.Ports[0].ShortSyntax);
            Assert.Equal(80, port.Target);
            Assert.Null(port.Published);
            Assert.Null(port.Protocol);
        }

        [Fact]
        public void WithPort_PublishedAndTarget_UsesLongSyntax()
        {
            ServiceDefinition service = new ServiceBuilder().WithPort(8080, 80, PortProtocol.Udp).Build();

            PortDefinition port = service.Ports[0].Definition!;
            Assert.Null(service.Ports[0].ShortSyntax);
            Assert.Equal(80, port.Target);
            Assert.Equal("8080", port.Published);
            Assert.Equal(PortProtocol.Udp, port.Protocol);
        }

        [Fact]
        public void WithPort_PublishedAndTarget_ProtocolIsOptional()
        {
            ServiceDefinition service = new ServiceBuilder().WithPort(8080, 80).Build();

            Assert.Null(service.Ports[0].Definition!.Protocol);
        }

        [Fact]
        public void WithPort_Definition_AddsIt()
        {
            PortDefinition definition = new PortDefinition(80);

            ServiceDefinition service = new ServiceBuilder().WithPort(definition).Build();

            Assert.Same(definition, service.Ports[0].Definition);
        }

        [Fact]
        public void WithPort_Configure_BuildsLongSyntax()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithPort(port => port.WithTarget(80).WithPublished(8000, 8010).WithHostIp("127.0.0.1"))
                .Build();

            PortDefinition port = service.Ports[0].Definition!;
            Assert.Equal(80, port.Target);
            Assert.Equal("8000-8010", port.Published);
            Assert.Equal("127.0.0.1", port.HostIp);
        }

        [Fact]
        public void WithPort_CalledRepeatedly_AppendsInOrder()
        {
            ServiceDefinition service = new ServiceBuilder().WithPort("80").WithPort(8443, 443).WithPort("53/udp").Build();

            Assert.Equal(3, service.Ports.Count);
            Assert.Equal("80", service.Ports[0].ShortSyntax);
            Assert.Equal("8443", service.Ports[1].Definition!.Published);
            Assert.Equal("53/udp", service.Ports[2].ShortSyntax);
        }

        [Fact]
        public void WithPorts_BothForms_AddEachPort()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithPorts(new[] { "80", "443" })
                .WithPorts(new[] { new PortDefinition(8080), new PortDefinition(9090) })
                .Build();

            Assert.Equal(4, service.Ports.Count);
            Assert.Equal("443", service.Ports[1].ShortSyntax);
            Assert.Equal(9090, service.Ports[3].Definition!.Target);
        }

        [Fact]
        public void Build_ReturnsPortsSnapshot_UnaffectedByLaterChanges()
        {
            ServiceBuilder builder = new ServiceBuilder().WithPort("80");
            ServiceDefinition first = builder.Build();

            builder.WithPort("443");

            Assert.Single(first.Ports);
        }

        [Fact]
        public void InvalidPortArguments_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithPort(" "));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPort(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPort(0, 80));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPort(8080, 70000));
            Assert.Throws<ArgumentNullException>(() => builder.WithPort((PortDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithPort((Action<PortBuilder>)null!));
            Assert.Throws<InvalidOperationException>(() => builder.WithPort(port => port.WithPublished(80)));
            Assert.Throws<ArgumentNullException>(() => builder.WithPorts((string[])null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithPorts((PortDefinition[])null!));
        }

        [Fact]
        public void WithVolume_String_UsesShortSyntax()
        {
            ServiceDefinition service = new ServiceBuilder().WithVolume("db-data:/var/lib/db:ro").Build();

            Assert.Equal("db-data:/var/lib/db:ro", service.Volumes[0].ShortSyntax);
            Assert.Null(service.Volumes[0].Definition);
        }

        [Fact]
        public void WithVolume_SourceAndTarget_WritesShortSyntax()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithVolume("db-data", "/data")
                .WithVolume("./src", "/app", readOnly: true)
                .Build();

            Assert.Equal("db-data:/data", service.Volumes[0].ShortSyntax);
            Assert.Equal("./src:/app:ro", service.Volumes[1].ShortSyntax);
        }

        [Fact]
        public void WithVolume_Definition_AddsIt()
        {
            MountDefinition definition = new MountDefinition(MountType.Tmpfs, "/tmp");

            ServiceDefinition service = new ServiceBuilder().WithVolume(definition).Build();

            Assert.Same(definition, service.Volumes[0].Definition);
        }

        [Fact]
        public void WithVolume_Configure_BuildsLongSyntax()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithVolume(mount => mount.WithType(MountType.Bind).WithSource("./conf").WithTarget("/etc/app"))
                .Build();

            MountDefinition mount = service.Volumes[0].Definition!;
            Assert.Equal(MountType.Bind, mount.Type);
            Assert.Equal("./conf", mount.Source);
            Assert.Equal("/etc/app", mount.Target);
        }

        [Fact]
        public void WithVolume_CalledRepeatedly_AppendsInOrder()
        {
            ServiceDefinition service = new ServiceBuilder().WithVolume("a:/a").WithVolume("b", "/b").WithVolume(mount => mount.WithType(MountType.Tmpfs).WithTarget("/t")).Build();

            Assert.Equal(3, service.Volumes.Count);
            Assert.Equal("a:/a", service.Volumes[0].ShortSyntax);
            Assert.Equal("b:/b", service.Volumes[1].ShortSyntax);
            Assert.Equal(MountType.Tmpfs, service.Volumes[2].Definition!.Type);
        }

        [Fact]
        public void WithVolumes_BothForms_AddEachMount()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithVolumes(new[] { "a:/a", "b:/b" })
                .WithVolumes(new[] { new MountDefinition(MountType.Tmpfs, "/t") })
                .Build();

            Assert.Equal(3, service.Volumes.Count);
            Assert.Equal("b:/b", service.Volumes[1].ShortSyntax);
            Assert.Equal("/t", service.Volumes[2].Definition!.Target);
        }

        [Fact]
        public void InvalidVolumeArguments_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithVolume(" "));
            Assert.Throws<ArgumentException>(() => builder.WithVolume("", "/x"));
            Assert.Throws<ArgumentException>(() => builder.WithVolume("x", " "));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolume((MountDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolume((Action<MountBuilder>)null!));
            Assert.Throws<InvalidOperationException>(() => builder.WithVolume(mount => mount.WithTarget("/x")));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolumes((string[])null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolumes((MountDefinition[])null!));
        }
    }
}
