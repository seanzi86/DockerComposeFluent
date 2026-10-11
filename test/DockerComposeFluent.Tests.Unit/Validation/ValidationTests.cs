using System;
using System.Linq;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;
using DockerComposeFluent.Validation;

namespace DockerComposeFluent.Tests.Unit.Validation
{
    public class ValidationTests
    {
        private static ValidationProblem Single(ValidationResult result)
        {
            return Assert.Single(result.Problems);
        }

        [Fact]
        public void EmptyFile_IsValid()
        {
            ValidationResult result = new DockerComposeBuilder().Build().Validate();

            Assert.True(result.IsValid);
            Assert.Empty(result.Problems);
        }

        [Fact]
        public void ConsistentFile_HasNoProblems()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("web", s => s.WithImage("nginx").WithDependsOn("db").WithNetwork("front").WithVolume("data:/d")
                    .WithSecret("s").WithConfig("c").WithPort("8080:80"))
                .WithService("db", s => s.WithImage("postgres").WithPort("5432:5432"))
                .WithNetwork("front", n => n.WithDriver("bridge"))
                .WithVolume("data", v => v.WithDriver("local"))
                .WithSecret("s", s => s.WithFile("./s"))
                .WithConfig("c", c => c.WithFile("./c"))
                .Build().Validate();

            Assert.Empty(result.Problems);
        }

        // --- DCF001 undefined service ---

        [Fact]
        public void DependsOn_UndefinedService_IsAnErrorNamingThePath()
        {
            ValidationProblem problem = Single(new DockerComposeBuilder()
                .WithService("web", s => s.WithImage("nginx").WithDependsOn("db")).Build().Validate());

            Assert.Equal(ValidationSeverity.Error, problem.Severity);
            Assert.Equal(ValidationCodes.UndefinedService, problem.Code);
            Assert.Equal("services.web.depends_on.db", problem.Path);
            Assert.Equal("Service \"web\" depends on service \"db\", which is not defined.", problem.Message);
        }

        [Fact]
        public void OtherServiceReferences_AreChecked()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x")
                    .WithLinks("ghost1:alias")
                    .WithVolumesFrom("ghost2:ro")
                    .WithNetworkMode("service:ghost3")
                    .WithIpc("service:ghost4")
                    .WithPid("service:ghost5")
                    .WithExtends("ghost6"))
                .Build().Validate();

            Assert.All(result.Problems, problem => Assert.Equal(ValidationCodes.UndefinedService, problem.Code));
            Assert.Equal(
                new[]
                {
                    "services.a.links[0]",
                    "services.a.volumes_from[0]",
                    "services.a.network_mode",
                    "services.a.ipc",
                    "services.a.pid",
                    "services.a.extends",
                },
                result.Problems.Select(problem => problem.Path).ToArray());
        }

        [Fact]
        public void ReferencesToThingsOutsideTheFile_AreNotErrors()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x")
                    .WithVolumesFrom("container:external")
                    .WithNetworkMode("host")
                    .WithExternalLinks("legacy:old"))
                .Build().Validate();

            Assert.Empty(result.Problems);
        }

        [Fact]
        public void ServiceReferences_ToDefinedServices_AreFine()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithLinks("b").WithVolumesFrom("b").WithNetworkMode("service:b").WithExtends("b"))
                .WithService("b", s => s.WithImage("y"))
                .Build().Validate();

            Assert.Empty(result.Problems);
        }

        [Fact]
        public void ExtendsWithAFile_IsNotChecked()
        {
            Assert.Empty(new DockerComposeBuilder()
                .WithService("a", s => s.WithExtends("webapp", "common.yml")).Build().Validate().Problems);
        }

        [Fact]
        public void NamesContainingAVariable_AreNotChecked()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithDependsOn("${DB_SERVICE}").WithVolume("${DATA}:/d"))
                .Build().Validate();

            Assert.Empty(result.Problems);
        }

        // --- DCF002 .. DCF005 ---

        [Fact]
        public void Network_UndefinedIsAnError_ButDefaultIsFine()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithNetwork("default").WithNetwork("ghost"))
                .Build().Validate();

            ValidationProblem problem = Single(result);
            Assert.Equal(ValidationCodes.UndefinedNetwork, problem.Code);
            Assert.Equal("services.a.networks.ghost", problem.Path);
        }

        [Fact]
        public void Network_DefinedExternalIsFine()
        {
            Assert.Empty(new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithNetwork("shared"))
                .WithNetwork("shared", n => n.WithExternal(true))
                .Build().Validate().Problems);
        }

        [Theory]
        [InlineData("ghost:/data", true)]
        [InlineData("ghost:/data:ro", true)]
        [InlineData("./src:/app", false)]
        [InlineData("../src:/app", false)]
        [InlineData("/abs:/app", false)]
        [InlineData("~/home:/app", false)]
        [InlineData("C:\\data:/app", false)]
        [InlineData("/anonymous", false)]
        [InlineData("defined:/data", false)]
        public void Volume_ShortSyntax_OnlyNamedVolumesAreChecked(string mount, bool expectProblem)
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithVolume(mount))
                .WithVolume("defined", v => v.WithDriver("local"))
                .Build().Validate();

            Assert.Equal(expectProblem, result.Problems.Count == 1);
            if (expectProblem)
            {
                Assert.Equal(ValidationCodes.UndefinedVolume, result.Problems[0].Code);
                Assert.Equal("services.a.volumes[0]", result.Problems[0].Path);
            }
        }

        [Fact]
        public void Volume_LongSyntax_OnlyVolumeTypeWithASourceIsChecked()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x")
                    .WithVolume(m => m.WithType(MountType.Volume).WithSource("ghost").WithTarget("/a"))
                    .WithVolume(m => m.WithType(MountType.Volume).WithTarget("/anonymous"))
                    .WithVolume(m => m.WithType(MountType.Bind).WithSource("ghost").WithTarget("/b"))
                    .WithVolume(m => m.WithType(MountType.Tmpfs).WithTarget("/t")))
                .Build().Validate();

            ValidationProblem problem = Single(result);
            Assert.Equal("services.a.volumes[0]", problem.Path);
        }

        [Fact]
        public void SecretsAndConfigs_Undefined_AreErrors_IncludingBuildSecrets()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithSecret("s1").WithConfig("c1").WithBuild(b => b.WithContext(".").WithSecret("s2")))
                .Build().Validate();

            Assert.Equal(
                new[] { ValidationCodes.UndefinedSecret + " services.a.secrets[0]", ValidationCodes.UndefinedSecret + " services.a.build.secrets[0]", ValidationCodes.UndefinedConfig + " services.a.configs[0]" }.OrderBy(x => x),
                result.Problems.Select(p => p.Code + " " + p.Path).OrderBy(x => x));
            Assert.All(result.Problems, p => Assert.Equal(ValidationSeverity.Error, p.Severity));
        }

        // --- include downgrades ---

        [Fact]
        public void WithAnInclude_UndefinedNamesAreWarnings_WithANote()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithInclude("./common.yml")
                .WithService("a", s => s.WithImage("x").WithDependsOn("db").WithNetwork("ghost"))
                .Build().Validate();

            Assert.True(result.IsValid);
            Assert.Equal(2, result.Warnings.Count);
            Assert.All(result.Warnings, w => Assert.EndsWith("It may be defined in an included file.", w.Message));
        }

        // --- DCF006 cycles ---

        [Fact]
        public void DependsOn_Cycle_IsReportedOnceFromTheFirstService()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("b", s => s.WithImage("x").WithDependsOn("c"))
                .WithService("c", s => s.WithImage("x").WithDependsOn("a"))
                .WithService("a", s => s.WithImage("x").WithDependsOn("b"))
                .Build().Validate();

            ValidationProblem problem = Single(result);
            Assert.Equal(ValidationCodes.DependencyCycle, problem.Code);
            Assert.Equal("services.a.depends_on.b", problem.Path);
            Assert.Equal("Services depend on each other in a cycle: a -> b -> c -> a.", problem.Message);
        }

        [Fact]
        public void DependsOn_Itself_IsACycle()
        {
            ValidationProblem problem = Single(new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithDependsOn("a")).Build().Validate());

            Assert.Equal("Services depend on each other in a cycle: a -> a.", problem.Message);
        }

        [Fact]
        public void DependsOn_DiamondWithoutACycle_IsFine()
        {
            Assert.Empty(new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithDependsOn(new[] { "b", "c" }))
                .WithService("b", s => s.WithImage("x").WithDependsOn("d"))
                .WithService("c", s => s.WithImage("x").WithDependsOn("d"))
                .WithService("d", s => s.WithImage("x"))
                .Build().Validate().Problems);
        }

        [Fact]
        public void Extends_Cycle_IsReported()
        {
            ValidationProblem problem = Single(new DockerComposeBuilder()
                .WithService("a", s => s.WithExtends("b"))
                .WithService("b", s => s.WithExtends("a"))
                .Build().Validate());

            Assert.Equal("services.a.extends", problem.Path);
            Assert.Equal("Services extend each other in a cycle: a -> b -> a.", problem.Message);
        }

        [Fact]
        public void TwoSeparateCycles_AreBothReported()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithDependsOn("b"))
                .WithService("b", s => s.WithImage("x").WithDependsOn("a"))
                .WithService("c", s => s.WithImage("x").WithDependsOn("d"))
                .WithService("d", s => s.WithImage("x").WithDependsOn("c"))
                .Build().Validate();

            Assert.Equal(2, result.Problems.Count(p => p.Code == ValidationCodes.DependencyCycle));
        }

        // --- DCF007 healthcheck ---

        [Fact]
        public void ServiceHealthy_WithoutAHealthcheck_IsAWarning()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("web", s => s.WithImage("x").WithDependsOn("db", DependencyCondition.ServiceHealthy))
                .WithService("db", s => s.WithImage("postgres"))
                .Build().Validate();

            ValidationProblem problem = Single(result);
            Assert.Equal(ValidationSeverity.Warning, problem.Severity);
            Assert.Equal(ValidationCodes.MissingHealthcheck, problem.Code);
            Assert.Equal("services.web.depends_on.db", problem.Path);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ServiceHealthy_WithADisabledHealthcheck_IsAWarning_AndWithOneIsFine()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("web", s => s.WithImage("x")
                    .WithDependsOn("ok", DependencyCondition.ServiceHealthy)
                    .WithDependsOn("off", DependencyCondition.ServiceHealthy)
                    .WithDependsOn("started", DependencyCondition.ServiceStarted))
                .WithService("ok", s => s.WithImage("x").WithHealthcheck("true"))
                .WithService("off", s => s.WithImage("x").WithoutHealthcheck())
                .WithService("started", s => s.WithImage("x"))
                .Build();

            Assert.Equal(new[] { "services.web.depends_on.off" }, file.Validate().Problems.Select(p => p.Path).ToArray());
        }

        // --- DCF008 ports ---

        [Theory]
        [InlineData("8080:80", "8080:81", true)]
        [InlineData("8080:80", "8080:80/udp", false)]
        [InlineData("127.0.0.1:8080:80", "127.0.0.1:8080:81", true)]
        [InlineData("127.0.0.1:8080:80", "127.0.0.2:8080:81", false)]
        [InlineData("127.0.0.1:8080:80", "8080:81", true)]
        [InlineData("0.0.0.0:8080:80", "127.0.0.1:8080:81", true)]
        [InlineData("[::1]:8080:80", "[::1]:8080:81", true)]
        [InlineData("8000-8010:80", "8005:81", true)]
        [InlineData("8000-8010:80", "9000:81", false)]
        [InlineData("80", "80", false)]
        [InlineData("${PORT}:80", "${PORT}:80", false)]
        public void PublishedPorts_ClashOnlyWhenTheySameBind(string first, string second, bool expectWarning)
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithPort(first))
                .WithService("b", s => s.WithImage("x").WithPort(second))
                .Build().Validate();

            Assert.Equal(expectWarning, result.Warnings.Count == 1);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void PublishedPorts_Clash_NamesBothServices()
        {
            ValidationProblem problem = Single(new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithPort("8080:80"))
                .WithService("b", s => s.WithImage("x").WithPort(8080, 81))
                .Build().Validate());

            Assert.Equal(ValidationCodes.DuplicatePublishedPort, problem.Code);
            Assert.Equal("services.b.ports[0]", problem.Path);
            Assert.Equal("Service \"b\" publishes host port 8080/tcp, which service \"a\" also publishes (services.a.ports[0]).", problem.Message);
        }

        [Fact]
        public void PublishedPorts_SameServiceTwice_IsReportedOnItsOwn()
        {
            ValidationProblem problem = Single(new DockerComposeBuilder()
                .WithService("a", s => s.WithImage("x").WithPort("8080:80").WithPort("8080:81"))
                .Build().Validate());

            Assert.Equal("Service \"a\" publishes host port 8080/tcp more than once (also at services.a.ports[0]).", problem.Message);
        }

        // --- result API ---

        [Fact]
        public void Ignoring_RemovesCodes_AndValidityFollows()
        {
            ValidationResult result = new DockerComposeBuilder()
                .WithService("web", s => s.WithImage("x").WithDependsOn("db"))
                .Build().Validate();

            Assert.False(result.IsValid);
            Assert.True(result.Ignoring(ValidationCodes.UndefinedService).IsValid);
            Assert.False(result.Ignoring(ValidationCodes.DuplicatePublishedPort).IsValid);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ThrowIfInvalid_ThrowsForErrorsOnly()
        {
            ValidationResult errors = new DockerComposeBuilder().WithService("web", s => s.WithDependsOn("db")).Build().Validate();
            ValidationResult warnings = new DockerComposeBuilder()
                .WithService("a", s => s.WithPort("1:1"))
                .WithService("b", s => s.WithPort("1:2"))
                .Build().Validate();

            ComposeValidationException exception = Assert.Throws<ComposeValidationException>(() => errors.ThrowIfInvalid());
            warnings.ThrowIfInvalid();

            Assert.Contains("1 error", exception.Message);
            Assert.Contains("services.web.depends_on.db", exception.Message);
            Assert.Single(exception.Problems);
            Assert.False(warnings.Warnings.Count == 0);
        }

        [Fact]
        public void ThrowIfInvalid_AfterIgnoring_DoesNotThrow()
        {
            new DockerComposeBuilder().WithService("web", s => s.WithDependsOn("db")).Build()
                .Validate().Ignoring(ValidationCodes.UndefinedService).ThrowIfInvalid();
        }

        [Fact]
        public void Problem_FormatsOnOneLine()
        {
            ValidationProblem problem = new ValidationProblem(ValidationSeverity.Error, "DCF001", "services.a", "Oops.");

            Assert.Equal("Error DCF001 services.a: Oops.", problem.ToString());
        }

        [Fact]
        public void Ignoring_NullCodes_Throws()
        {
            ValidationResult result = new DockerComposeBuilder().Build().Validate();

            Assert.Throws<ArgumentNullException>(() => result.Ignoring(null!));
        }

        [Fact]
        public void Validate_DoesNotChangeTheFile()
        {
            DockerComposeFile file = new DockerComposeBuilder().WithService("web", s => s.WithImage("x").WithDependsOn("db")).Build();
            string before = file.ToYaml();

            file.Validate();

            Assert.Equal(before, file.ToYaml());
        }
    }
}
