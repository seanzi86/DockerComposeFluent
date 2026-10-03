using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ExtensionsTests
    {
        [Fact]
        public void DockerComposeBuilder_WithExtension_SetsIt()
        {
            DockerComposeFile file = new DockerComposeBuilder().WithExtension("x-project-notes", "internal use only").Build();

            Assert.Equal("internal use only", file.Extensions["x-project-notes"]);
        }

        [Fact]
        public void ServiceBuilder_WithExtension_SetsIt()
        {
            ServiceDefinition service = new ServiceBuilder().WithExtension("x-team", "platform").Build();

            Assert.Equal("platform", service.Extensions["x-team"]);
        }

        [Fact]
        public void WithExtension_SameKeyReplaces()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithExtension("x-note", "first")
                .WithExtension("x-note", "second")
                .Build();

            Assert.Equal("second", file.Extensions["x-note"]);
            Assert.Single(file.Extensions);
        }

        [Fact]
        public void WithExtension_NotXPrefixed_Throws()
        {
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithExtension("notes", "value"));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithExtension("notes", "value"));
        }

        [Fact]
        public void WithExtension_BlankKey_Throws()
        {
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithExtension(" ", "value"));
        }

        [Fact]
        public void WithExtension_NullValue_IsAllowed()
        {
            DockerComposeFile file = new DockerComposeBuilder().WithExtension("x-note", null).Build();

            Assert.Null(file.Extensions["x-note"]);
        }
    }
}
