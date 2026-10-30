using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class ValueEqualityTests
    {
        private static DockerComposeFile Build()
        {
            return new DockerComposeBuilder()
                .WithName("app")
                .WithService("web", service => service
                    .WithImage("nginx")
                    .WithPort(80, 8080)
                    .WithEnvironment("A", "1")
                    .WithCommand(new[] { "nginx", "-g" })
                    .WithExtension("x-meta", new Dictionary<string, object?> { ["tags"] = new List<object?> { "a", 1 } }))
                .WithVolume("data", volume => { })
                .Build();
        }

        [Fact]
        public void FilesBuiltTheSameWay_AreEqualWithTheSameHashCode()
        {
            DockerComposeFile first = Build();
            DockerComposeFile second = Build();

            Assert.Equal(first, second);
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
            Assert.Equal(first.Services["web"], second.Services["web"]);
        }

        [Fact]
        public void FilesThatDiffer_AreNotEqual()
        {
            DockerComposeFile other = new DockerComposeBuilder()
                .WithName("app")
                .WithService("web", service => service.WithImage("nginx").WithPort(80, 8081))
                .Build();

            Assert.NotEqual(Build(), other);
        }

        [Fact]
        public void ListAndDictionaryOrder_Matters()
        {
            ServiceDefinition ab = new ServiceDefinition { Dns = new[] { "a", "b" } };
            ServiceDefinition ba = new ServiceDefinition { Dns = new[] { "b", "a" } };

            Assert.NotEqual(ab, ba);
            Assert.NotEqual(
                new DockerComposeFile { Networks = new Dictionary<string, NetworkDefinition> { ["a"] = new NetworkDefinition(), ["b"] = new NetworkDefinition() } },
                new DockerComposeFile { Networks = new Dictionary<string, NetworkDefinition> { ["b"] = new NetworkDefinition(), ["a"] = new NetworkDefinition() } });
        }

        [Fact]
        public void HandAssignedCollections_AreCopied()
        {
            List<string> dns = new List<string> { "1.1.1.1" };
            ServiceDefinition service = new ServiceDefinition { Dns = dns };

            dns.Add("8.8.8.8");

            Assert.Single(service.Dns);
            Assert.Equal(new ServiceDefinition { Dns = new[] { "1.1.1.1" } }, service);
        }

        [Fact]
        public void EmptyCollections_CompareEqualWhicheverWayTheyWereMade()
        {
            Assert.Equal(new ServiceDefinition(), new ServiceDefinition { Dns = new string[0], Labels = new Labels { Values = new Dictionary<string, string>() } });
        }

        [Fact]
        public void CommandLinesWithTheSameArguments_AreEqual()
        {
            Assert.Equal(CommandLine.FromArguments(new[] { "a", "b" }), CommandLine.FromArguments(new[] { "a", "b" }));
        }
    }
}
