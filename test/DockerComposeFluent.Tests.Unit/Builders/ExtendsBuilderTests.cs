using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ExtendsBuilderTests
    {
        [Fact]
        public void WithService_SetsIt()
        {
            ExtendsDefinition extends = new ExtendsBuilder().WithService("common").Build();

            Assert.Equal("common", extends.Service);
            Assert.True(extends.IsServiceOnly);
        }

        [Fact]
        public void WithFile_SetsIt()
        {
            ExtendsDefinition extends = new ExtendsBuilder().WithService("webapp").WithFile("common.yml").Build();

            Assert.Equal("common.yml", extends.File);
            Assert.False(extends.IsServiceOnly);
        }

        [Fact]
        public void Build_NoService_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ExtendsBuilder().Build());
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            ExtendsBuilder builder = new ExtendsBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithService(" "));
            Assert.Throws<ArgumentException>(() => builder.WithFile(" "));
        }

        [Fact]
        public void ServiceBuilder_WithExtends_Service_SetsIt()
        {
            ServiceDefinition service = new ServiceBuilder().WithExtends("common").Build();

            Assert.Equal("common", service.Extends!.Service);
            Assert.True(service.Extends.IsServiceOnly);
        }

        [Fact]
        public void ServiceBuilder_WithExtends_ServiceAndFile_SetsIt()
        {
            ServiceDefinition service = new ServiceBuilder().WithExtends("webapp", "common.yml").Build();

            Assert.Equal("webapp", service.Extends!.Service);
            Assert.Equal("common.yml", service.Extends.File);
        }

        [Fact]
        public void ServiceBuilder_WithExtends_FromBuilder_SetsIt()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithExtends(extends => extends.WithService("webapp").WithFile("common.yml"))
                .Build();

            Assert.Equal("webapp", service.Extends!.Service);
            Assert.Equal("common.yml", service.Extends.File);
        }
    }
}
