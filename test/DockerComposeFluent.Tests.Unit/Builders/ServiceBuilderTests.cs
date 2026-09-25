using System;
using System.Globalization;
using System.Collections.Generic;
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

        [Fact]
        public void WithNetwork_NameOnly_AttachesWithNoSettings()
        {
            ServiceDefinition service = new ServiceBuilder().WithNetwork("front").Build();

            Assert.True(service.Networks["front"].IsEmpty);
        }

        [Fact]
        public void WithNetwork_Configure_AttachesWithSettings()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithNetwork("back", network => network.WithAlias("db").WithIpv4Address("172.16.238.10"))
                .Build();

            Assert.Equal(new[] { "db" }, service.Networks["back"].Aliases);
            Assert.Equal("172.16.238.10", service.Networks["back"].Ipv4Address);
        }

        [Fact]
        public void WithNetwork_RawAttachment_IsUsedAsGiven()
        {
            NetworkAttachment attachment = new NetworkAttachment { Priority = 5 };

            ServiceDefinition service = new ServiceBuilder().WithNetwork("back", attachment).Build();

            Assert.Same(attachment, service.Networks["back"]);
        }

        [Fact]
        public void WithNetwork_SameNameAgain_ReplacesAndKeepsPosition()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithNetwork("a")
                .WithNetwork("b")
                .WithNetwork("a", network => network.WithPriority(1))
                .Build();

            Assert.Equal(new[] { "a", "b" }, new List<string>(service.Networks.Keys));
            Assert.Equal(1, service.Networks["a"].Priority);
        }

        [Fact]
        public void WithNetworks_AttachesEachName()
        {
            ServiceDefinition service = new ServiceBuilder().WithNetworks(new[] { "front", "admin" }).Build();

            Assert.Equal(new[] { "front", "admin" }, new List<string>(service.Networks.Keys));
        }

        [Fact]
        public void InvalidNetworkArguments_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithNetwork(" "));
            Assert.Throws<ArgumentException>(() => builder.WithNetwork("", new NetworkAttachment()));
            Assert.Throws<ArgumentNullException>(() => builder.WithNetwork("a", (NetworkAttachment)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithNetwork("a", (Action<NetworkAttachmentBuilder>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithNetworks(null!));
        }

        [Fact]
        public void WithEnvironment_KeyAndValue_SetsAVariable()
        {
            EnvironmentVariables environment = new ServiceBuilder().WithEnvironment("RACK_ENV", "development").Build().Environment;

            Assert.Equal("development", environment.Variables["RACK_ENV"]);
            Assert.False(environment.UsesListSyntax);
        }

        [Fact]
        public void WithEnvironment_KeyOnly_DeclaresAPassThroughVariable()
        {
            EnvironmentVariables environment = new ServiceBuilder().WithEnvironment("USER_INPUT").Build().Environment;

            Assert.True(environment.Variables.ContainsKey("USER_INPUT"));
            Assert.Null(environment.Variables["USER_INPUT"]);
        }

        [Fact]
        public void WithEnvironment_EmptyValue_IsNotPassThrough()
        {
            EnvironmentVariables environment = new ServiceBuilder().WithEnvironment("EMPTY", "").Build().Environment;

            Assert.Equal(string.Empty, environment.Variables["EMPTY"]);
        }

        [Fact]
        public void WithEnvironment_TypedValues_AreWrittenAsStrings()
        {
            EnvironmentVariables environment = new ServiceBuilder()
                .WithEnvironment("A", true)
                .WithEnvironment("B", false)
                .WithEnvironment("C", 8080)
                .WithEnvironment("D", 5000000000L)
                .WithEnvironment("E", 1.5)
                .Build().Environment;

            Assert.Equal("true", environment.Variables["A"]);
            Assert.Equal("false", environment.Variables["B"]);
            Assert.Equal("8080", environment.Variables["C"]);
            Assert.Equal("5000000000", environment.Variables["D"]);
            Assert.Equal("1.5", environment.Variables["E"]);
        }

        [Fact]
        public void WithEnvironment_Double_UsesInvariantCulture()
        {
            CultureInfo original = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("de-DE");

                EnvironmentVariables environment = new ServiceBuilder().WithEnvironment("RATIO", 1.5).WithEnvironment("N", 1234567).Build().Environment;

                Assert.Equal("1.5", environment.Variables["RATIO"]);
                Assert.Equal("1234567", environment.Variables["N"]);
            }
            finally
            {
                CultureInfo.CurrentCulture = original;
            }
        }

        [Fact]
        public void WithEnvironment_Pairs_SetsEachVariableInMapForm()
        {
            EnvironmentVariables environment = new ServiceBuilder()
                .WithEnvironment(new Dictionary<string, string> { ["A"] = "1", ["B"] = "2" })
                .Build().Environment;

            Assert.Equal(new[] { "A", "B" }, new List<string>(environment.Variables.Keys));
            Assert.False(environment.UsesListSyntax);
        }

        [Fact]
        public void WithEnvironment_Entries_AreSplitAtTheFirstEqualsAndUseListForm()
        {
            EnvironmentVariables environment = new ServiceBuilder()
                .WithEnvironment(new[] { "A=1", "URL=http://x?a=b", "EMPTY=", "USER_INPUT" })
                .Build().Environment;

            Assert.Equal("1", environment.Variables["A"]);
            Assert.Equal("http://x?a=b", environment.Variables["URL"]);
            Assert.Equal(string.Empty, environment.Variables["EMPTY"]);
            Assert.Null(environment.Variables["USER_INPUT"]);
            Assert.True(environment.UsesListSyntax);
        }

        [Fact]
        public void WithEnvironment_MixingForms_KeepsEveryVariableAndUsesListForm()
        {
            EnvironmentVariables environment = new ServiceBuilder()
                .WithEnvironment("A", "1")
                .WithEnvironment(new[] { "B=2" })
                .WithEnvironment("C", "3")
                .Build().Environment;

            Assert.Equal(new[] { "A", "B", "C" }, new List<string>(environment.Variables.Keys));
            Assert.True(environment.UsesListSyntax);
        }

        [Fact]
        public void WithEnvironment_SameKeyAgain_ReplacesAndKeepsPosition()
        {
            EnvironmentVariables environment = new ServiceBuilder()
                .WithEnvironment("A", "1")
                .WithEnvironment("B", "2")
                .WithEnvironment("A", "3")
                .Build().Environment;

            Assert.Equal(new[] { "A", "B" }, new List<string>(environment.Variables.Keys));
            Assert.Equal("3", environment.Variables["A"]);
        }

        [Fact]
        public void WithEnvironment_InvalidArguments_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithEnvironment(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment("", "x"));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment("A=B", "x"));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment("A=B"));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment("A=B", 1));
            Assert.Throws<ArgumentNullException>(() => builder.WithEnvironment("A", (string)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithEnvironment((IEnumerable<string>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithEnvironment((IEnumerable<KeyValuePair<string, string>>)null!));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment(new[] { "=x" }));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment(new[] { " " }));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment(new string[] { null! }));
        }

        [Fact]
        public void WithRestart_Policy_SetsRestart()
        {
            ServiceDefinition service = new ServiceBuilder().WithRestart(RestartPolicy.UnlessStopped).Build();

            Assert.Equal(RestartDefinition.FromPolicy(RestartPolicy.UnlessStopped), service.Restart);
        }

        [Fact]
        public void WithRestart_Text_ParsesIt()
        {
            ServiceDefinition service = new ServiceBuilder().WithRestart("on-failure:5").Build();

            Assert.Equal(RestartDefinition.OnFailure(5), service.Restart);
        }

        [Fact]
        public void WithRestart_Definition_SetsIt()
        {
            RestartDefinition restart = RestartDefinition.OnFailure(2);

            Assert.Same(restart, new ServiceBuilder().WithRestart(restart).Build().Restart);
        }

        [Fact]
        public void WithRestartOnFailure_SetsRetryLimit()
        {
            ServiceDefinition service = new ServiceBuilder().WithRestartOnFailure(3).Build();

            Assert.Equal(RestartPolicy.OnFailure, service.Restart!.Policy);
            Assert.Equal(3, service.Restart.MaxRetries);
        }

        [Fact]
        public void WithRestart_CalledTwice_LastWins()
        {
            ServiceDefinition service = new ServiceBuilder().WithRestartOnFailure(3).WithRestart(RestartPolicy.Always).Build();

            Assert.Equal(RestartDefinition.FromPolicy(RestartPolicy.Always), service.Restart);
        }

        [Fact]
        public void WithRestart_InvalidInput_Throws()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithRestart("sometimes"));
            Assert.Throws<ArgumentNullException>(() => builder.WithRestart((RestartDefinition)null!));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithRestartOnFailure(-1));
        }

        [Fact]
        public void WithDependsOn_Name_AddsADependencyWithNoSettings()
        {
            ServiceDefinition service = new ServiceBuilder().WithDependsOn("db").Build();

            Assert.True(service.DependsOn["db"].IsEmpty);
        }

        [Fact]
        public void WithDependsOn_Names_AddsEachOne()
        {
            ServiceDefinition service = new ServiceBuilder().WithDependsOn(new[] { "db", "cache" }).Build();

            Assert.Equal(new[] { "db", "cache" }, service.DependsOn.Keys);
            Assert.All(service.DependsOn.Values, dependency => Assert.True(dependency.IsEmpty));
        }

        [Fact]
        public void WithDependsOn_Condition_SetsItEvenForTheDefault()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithDependsOn("db", DependencyCondition.ServiceHealthy)
                .WithDependsOn("cache", DependencyCondition.ServiceStarted)
                .Build();

            Assert.Equal(DependencyCondition.ServiceHealthy, service.DependsOn["db"].Condition);
            Assert.Equal(DependencyCondition.ServiceStarted, service.DependsOn["cache"].Condition);
            Assert.False(service.DependsOn["cache"].IsEmpty);
        }

        [Fact]
        public void WithDependsOn_Definition_SetsIt()
        {
            DependencyDefinition dependency = new DependencyDefinition { Required = false };

            ServiceDefinition service = new ServiceBuilder().WithDependsOn("db", dependency).Build();

            Assert.Same(dependency, service.DependsOn["db"]);
        }

        [Fact]
        public void WithDependsOn_Action_ConfiguresTheDependency()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithDependsOn("db", dependency => dependency.WithCondition(DependencyCondition.ServiceCompletedSuccessfully).WithRestart(true))
                .Build();

            Assert.Equal(DependencyCondition.ServiceCompletedSuccessfully, service.DependsOn["db"].Condition);
            Assert.True(service.DependsOn["db"].Restart);
        }

        [Fact]
        public void WithDependsOn_SameServiceTwice_ReplacesIt()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithDependsOn("db", DependencyCondition.ServiceHealthy)
                .WithDependsOn("db")
                .Build();

            Assert.Single(service.DependsOn);
            Assert.True(service.DependsOn["db"].IsEmpty);
        }

        [Fact]
        public void WithDependsOn_InvalidInput_Throws()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithDependsOn(" "));
            Assert.Throws<ArgumentException>(() => builder.WithDependsOn("", DependencyCondition.ServiceStarted));
            Assert.Throws<ArgumentNullException>(() => builder.WithDependsOn((string[])null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithDependsOn("db", (DependencyDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithDependsOn("db", (Action<DependencyBuilder>)null!));
        }

        [Fact]
        public void WithHealthcheck_String_SetsAShellTest()
        {
            ServiceDefinition service = new ServiceBuilder().WithHealthcheck("curl -f http://localhost || exit 1").Build();

            Assert.Equal("curl -f http://localhost || exit 1", service.Healthcheck!.Test!.Shell);
        }

        [Fact]
        public void WithHealthcheck_Action_ConfiguresIt()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithHealthcheck(healthcheck => healthcheck.WithCommand(new[] { "pg_isready" }).WithRetries(5))
                .Build();

            Assert.Equal(new[] { "CMD", "pg_isready" }, service.Healthcheck!.Test!.Arguments);
            Assert.Equal(5, service.Healthcheck.Retries);
        }

        [Fact]
        public void WithHealthcheck_Definition_SetsIt()
        {
            HealthcheckDefinition healthcheck = new HealthcheckDefinition { Interval = "5s" };

            Assert.Same(healthcheck, new ServiceBuilder().WithHealthcheck(healthcheck).Build().Healthcheck);
        }

        [Fact]
        public void WithoutHealthcheck_Disables()
        {
            ServiceDefinition service = new ServiceBuilder().WithoutHealthcheck().Build();

            Assert.True(service.Healthcheck!.Disable);
            Assert.Null(service.Healthcheck.Test);
        }

        [Fact]
        public void WithHealthcheck_InvalidInput_Throws()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithHealthcheck(" "));
            Assert.Throws<ArgumentNullException>(() => builder.WithHealthcheck((HealthcheckDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithHealthcheck((Action<HealthcheckBuilder>)null!));
        }
    }
}
