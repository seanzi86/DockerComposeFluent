using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class BuildBuilderTests
    {
        [Fact]
        public void ContextOnly_IsContextOnly()
        {
            BuildDefinition build = new BuildBuilder().WithContext("./app").Build();

            Assert.True(build.IsContextOnly);
        }

        [Theory]
        [InlineData("dockerfile")]
        [InlineData("target")]
        [InlineData("arg")]
        [InlineData("label")]
        [InlineData("tags")]
        [InlineData("pull")]
        [InlineData("extension")]
        [InlineData("secret")]
        [InlineData("ulimit")]
        public void AnyOtherSetting_ForcesLongForm(string setting)
        {
            BuildBuilder builder = new BuildBuilder().WithContext("./app");

            switch (setting)
            {
                case "dockerfile": builder.WithDockerfile("D"); break;
                case "target": builder.WithTarget("t"); break;
                case "arg": builder.WithArg("a", "b"); break;
                case "label": builder.WithLabel("a", "b"); break;
                case "tags": builder.WithTags("t:1"); break;
                case "pull": builder.WithPull(false); break;
                case "extension": builder.WithExtension("x-a", 1); break;
                case "secret": builder.WithSecret("s"); break;
                default: builder.WithUlimit("nproc", 1); break;
            }

            Assert.False(builder.Build().IsContextOnly);
        }

        [Fact]
        public void NoContext_IsNeverContextOnly()
        {
            Assert.False(new BuildBuilder().WithDockerfileInline("FROM busybox").Build().IsContextOnly);
            Assert.False(new BuildBuilder().Build().IsContextOnly);
        }

        [Fact]
        public void Lists_AppendAndAcceptSeveral()
        {
            BuildDefinition build = new BuildBuilder()
                .WithCacheFrom("a").WithCacheFrom(new[] { "b", "c" })
                .WithPlatforms("linux/amd64")
                .Build();

            Assert.Equal(new[] { "a", "b", "c" }, build.CacheFrom);
            Assert.Equal(new[] { "linux/amd64" }, build.Platforms);
        }

        [Fact]
        public void Maps_SameKeyReplaces()
        {
            BuildDefinition build = new BuildBuilder()
                .WithArg("a", "1").WithArg("a", "2")
                .WithAdditionalContext("c", "x").WithExtraHost("h", "1.1.1.1").WithExtraHost("h", "2.2.2.2")
                .WithUlimit("nproc", 1).WithUlimit("nproc", 2)
                .Build();

            Assert.Equal("2", build.Args["a"]);
            Assert.Equal("2.2.2.2", build.ExtraHosts["h"]);
            Assert.Equal(2, build.Ulimits["nproc"].Single);
            Assert.Single(build.Args);
        }

        [Fact]
        public void BooleanOrString_AttestationsAcceptBoth()
        {
            Assert.Equal("true", new BuildBuilder().WithProvenance(true).Build().Provenance);
            Assert.Equal("false", new BuildBuilder().WithSbom(false).Build().Sbom);
            Assert.Equal("mode=max", new BuildBuilder().WithProvenance("mode=max").Build().Provenance);
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            BuildBuilder builder = new BuildBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithContext(" "));
            Assert.Throws<ArgumentException>(() => builder.WithArg(" ", "x"));
            Assert.Throws<ArgumentException>(() => builder.WithTags(" "));
            Assert.Throws<ArgumentException>(() => builder.WithProvenance(" "));
            Assert.Throws<ArgumentException>(() => builder.WithUlimit("NoFile", 1));
            Assert.Throws<ArgumentException>(() => builder.WithLabel("com.docker.compose.x", "1"));
            Assert.Throws<ArgumentNullException>(() => new ServiceBuilder().WithBuild((BuildDefinition)null!));
        }

        [Fact]
        public void ServiceBuilder_WithBuild_AcceptsEachForm()
        {
            Assert.True(new ServiceBuilder().WithBuild("./app").Build().Build!.IsContextOnly);
            Assert.Equal("t", new ServiceBuilder().WithBuild(b => b.WithContext(".").WithTarget("t")).Build().Build!.Target);
        }
    }
}
