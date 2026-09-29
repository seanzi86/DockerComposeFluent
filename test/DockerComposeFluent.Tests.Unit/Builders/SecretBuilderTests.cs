using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class SecretBuilderTests
    {
        [Fact]
        public void WithFile_SetsIt()
        {
            SecretDefinition secret = new SecretBuilder().WithFile("./server.cert").Build();

            Assert.Equal("./server.cert", secret.File);
            Assert.Null(secret.Environment);
        }

        [Fact]
        public void WithEnvironment_SetsIt()
        {
            SecretDefinition secret = new SecretBuilder().WithEnvironment("OAUTH_TOKEN").Build();

            Assert.Equal("OAUTH_TOKEN", secret.Environment);
        }

        [Fact]
        public void WithExternal_NoOtherSettings_IsAllowed()
        {
            SecretDefinition secret = new SecretBuilder().WithExternal(true).Build();

            Assert.True(secret.External);
        }

        [Fact]
        public void WithExternal_AndName_IsAllowed()
        {
            SecretDefinition secret = new SecretBuilder().WithExternal(true).WithName("CERTIFICATE_KEY").Build();

            Assert.True(secret.External);
            Assert.Equal("CERTIFICATE_KEY", secret.Name);
        }

        [Fact]
        public void Build_NoSource_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new SecretBuilder().Build());
        }

        [Fact]
        public void Build_FileAndEnvironment_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new SecretBuilder().WithFile("./x").WithEnvironment("Y").Build());
        }

        [Fact]
        public void Build_ExternalWithFile_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new SecretBuilder().WithExternal(true).WithFile("./x").Build());
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            SecretBuilder builder = new SecretBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithFile(" "));
            Assert.Throws<ArgumentException>(() => builder.WithEnvironment(" "));
            Assert.Throws<ArgumentException>(() => builder.WithName(" "));
        }
    }
}
