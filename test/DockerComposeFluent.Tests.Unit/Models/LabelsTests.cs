using System;
using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class LabelsTests
    {
        [Fact]
        public void WithLabel_SetsIt_AndReplacesOnSameKey()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithImage("nginx")
                .WithLabel("com.example.department", "Finance")
                .WithLabel("com.example.department", "IT")
                .Build();

            Assert.Equal("IT", service.Labels.Values["com.example.department"]);
            Assert.False(service.Labels.UsesListSyntax);
        }

        [Fact]
        public void WithLabels_Dictionary_AddsEachOne()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithLabels(new Dictionary<string, string> { ["a"] = "1", ["b"] = "" })
                .Build();

            Assert.Equal("1", service.Labels.Values["a"]);
            Assert.Equal("", service.Labels.Values["b"]);
        }

        [Fact]
        public void WithLabels_Entries_UsesListSyntax_AndAllowsBareKey()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithLabels(new[] { "com.example.description=Accounting webapp", "com.example.label-with-empty-value" })
                .Build();

            Assert.True(service.Labels.UsesListSyntax);
            Assert.Equal("Accounting webapp", service.Labels.Values["com.example.description"]);
            Assert.Equal("", service.Labels.Values["com.example.label-with-empty-value"]);
        }

        [Fact]
        public void WithLabel_ReservedPrefix_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithLabel("com.docker.compose.project", "x"));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithLabel("COM.DOCKER.COMPOSE.project", "x"));
        }

        [Fact]
        public void WithLabel_InvalidInput_Throws()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithLabel(" ", "x"));
            Assert.Throws<ArgumentNullException>(() => builder.WithLabel("key", null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithLabels((IEnumerable<KeyValuePair<string, string>>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithLabels((IEnumerable<string>)null!));
        }

        [Fact]
        public void NetworkBuilder_SupportsLabels()
        {
            NetworkDefinition network = new NetworkBuilder().WithLabel("a", "1").Build();

            Assert.Equal("1", network.Labels.Values["a"]);
        }

        [Fact]
        public void VolumeBuilder_SupportsLabels()
        {
            VolumeDefinition volume = new VolumeBuilder().WithLabel("a", "1").Build();

            Assert.Equal("1", volume.Labels.Values["a"]);
        }

        [Fact]
        public void WithLabelFile_AddsIt()
        {
            ServiceDefinition service = new ServiceBuilder().WithLabelFile("./app.labels").Build();

            Assert.Equal(new[] { "./app.labels" }, service.LabelFiles);
        }

        [Fact]
        public void WithLabelFiles_AddsEachOneInOrder()
        {
            ServiceDefinition service = new ServiceBuilder().WithLabelFiles(new[] { "./app.labels", "./more.labels" }).Build();

            Assert.Equal(new[] { "./app.labels", "./more.labels" }, service.LabelFiles);
        }

        [Fact]
        public void WithLabelFile_InvalidInput_Throws()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithLabelFile(" "));
            Assert.Throws<ArgumentNullException>(() => builder.WithLabelFiles(null!));
        }
    }
}
