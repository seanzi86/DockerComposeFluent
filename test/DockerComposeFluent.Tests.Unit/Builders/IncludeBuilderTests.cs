using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class IncludeBuilderTests
    {
        [Fact]
        public void WithPath_Only_IsPathOnly()
        {
            IncludeDefinition include = new IncludeBuilder().WithPath("./common.yml").Build();

            Assert.Equal(new[] { "./common.yml" }, include.Path);
            Assert.True(include.IsPathOnly);
        }

        [Fact]
        public void WithPaths_AddsThem()
        {
            IncludeDefinition include = new IncludeBuilder().WithPaths(new[] { "./a.yml", "./b.yml" }).Build();

            Assert.Equal(new[] { "./a.yml", "./b.yml" }, include.Path);
            Assert.False(include.IsPathOnly);
        }

        [Fact]
        public void WithEnvFile_ForcesLongForm()
        {
            IncludeDefinition include = new IncludeBuilder().WithPath("./common.yml").WithEnvFile("./common.env").Build();

            Assert.False(include.IsPathOnly);
            Assert.Equal(new[] { "./common.env" }, include.EnvFile);
        }

        [Fact]
        public void WithProjectDirectory_ForcesLongForm()
        {
            IncludeDefinition include = new IncludeBuilder().WithPath("./common.yml").WithProjectDirectory(".").Build();

            Assert.False(include.IsPathOnly);
            Assert.Equal(".", include.ProjectDirectory);
        }

        [Fact]
        public void Build_NoPath_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new IncludeBuilder().Build());
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            IncludeBuilder builder = new IncludeBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithPath(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEnvFile(" "));
            Assert.Throws<ArgumentException>(() => builder.WithProjectDirectory(" "));
        }

        [Fact]
        public void DockerComposeBuilder_WithInclude_Path_AddsIt()
        {
            DockerComposeFile file = new DockerComposeBuilder().WithInclude("./common.yml").Build();

            Assert.Single(file.Includes);
            Assert.Equal(new[] { "./common.yml" }, file.Includes[0].Path);
        }

        [Fact]
        public void DockerComposeBuilder_WithIncludes_AddsOnePerPath()
        {
            DockerComposeFile file = new DockerComposeBuilder().WithIncludes(new[] { "./a.yml", "./b.yml" }).Build();

            Assert.Equal(2, file.Includes.Count);
            Assert.Equal(new[] { "./a.yml" }, file.Includes[0].Path);
            Assert.Equal(new[] { "./b.yml" }, file.Includes[1].Path);
        }

        [Fact]
        public void DockerComposeBuilder_WithInclude_FromBuilder_AddsIt()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithInclude(include => include.WithPath("./common.yml").WithProjectDirectory("."))
                .Build();

            Assert.Single(file.Includes);
            Assert.Equal(".", file.Includes[0].ProjectDirectory);
        }
    }
}
