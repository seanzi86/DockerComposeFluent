using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class SecretReferenceBuilderTests
    {
        [Fact]
        public void WithSource_Only_IsSourceOnly()
        {
            SecretReference reference = new SecretReferenceBuilder().WithSource("server-certificate").Build();

            Assert.Equal("server-certificate", reference.Source);
            Assert.True(reference.IsSourceOnly);
        }

        [Fact]
        public void Build_EverySetting_IsSet()
        {
            SecretReference reference = new SecretReferenceBuilder()
                .WithSource("server-certificate")
                .WithTarget("server.cert")
                .WithUid("103")
                .WithGid("103")
                .WithMode(Convert.ToInt32("440", 8))
                .Build();

            Assert.False(reference.IsSourceOnly);
            Assert.Equal("server.cert", reference.Target);
            Assert.Equal("103", reference.Uid);
            Assert.Equal("103", reference.Gid);
            Assert.Equal(Convert.ToInt32("440", 8), reference.Mode);
        }

        [Fact]
        public void WithExtension_ForcesLongForm()
        {
            SecretReference reference = new SecretReferenceBuilder()
                .WithSource("server-certificate")
                .WithExtension("x-note", "rotated monthly")
                .Build();

            Assert.False(reference.IsSourceOnly);
            Assert.Equal("rotated monthly", reference.Extensions["x-note"]);
        }

        [Fact]
        public void Build_NoSource_Throws()
        {
            Assert.Throws<ArgumentException>(() => new SecretReferenceBuilder().Build());
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            SecretReferenceBuilder builder = new SecretReferenceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithSource(" "));
            Assert.Throws<ArgumentException>(() => builder.WithTarget(" "));
            Assert.Throws<ArgumentException>(() => builder.WithUid(" "));
            Assert.Throws<ArgumentException>(() => builder.WithGid(" "));
        }
    }
}
