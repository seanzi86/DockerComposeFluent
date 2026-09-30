using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class DeployBuilderTests
    {
        [Fact]
        public void WithMode_SetsIt()
        {
            DeployDefinition deploy = new DeployBuilder().WithMode(DeployMode.Global).Build();

            Assert.Equal(DeployMode.Global, deploy.Mode);
        }

        [Fact]
        public void WithReplicas_SetsIt()
        {
            DeployDefinition deploy = new DeployBuilder().WithReplicas(4).Build();

            Assert.Equal(4, deploy.Replicas);
        }

        [Fact]
        public void WithReplicas_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DeployBuilder().WithReplicas(-1));
        }

        [Fact]
        public void WithLabel_SetsIt()
        {
            DeployDefinition deploy = new DeployBuilder().WithLabel("com.example.team", "platform").Build();

            Assert.Equal("platform", deploy.Labels.Values["com.example.team"]);
        }

        [Fact]
        public void WithPlacement_FromDefinition_SetsIt()
        {
            PlacementDefinition placement = new PlacementBuilder().WithConstraint("disktype=ssd").Build();

            DeployDefinition deploy = new DeployBuilder().WithPlacement(placement).Build();

            Assert.Same(placement, deploy.Placement);
        }

        [Fact]
        public void WithResources_FromBuilder_SetsIt()
        {
            DeployDefinition deploy = new DeployBuilder()
                .WithResources(resources => resources.WithLimits(limits => limits.WithCpus("0.5")))
                .Build();

            Assert.Equal("0.5", deploy.Resources!.Limits!.Cpus);
        }

        [Fact]
        public void WithRestartPolicy_FromBuilder_SetsIt()
        {
            DeployDefinition deploy = new DeployBuilder()
                .WithRestartPolicy(restartPolicy => restartPolicy.WithCondition(DeployRestartCondition.Any))
                .Build();

            Assert.Equal(DeployRestartCondition.Any, deploy.RestartPolicy!.Condition);
        }

        [Fact]
        public void WithRollbackConfig_MaxFailureRatioOutOfRange_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RollbackConfigBuilder().WithMaxFailureRatio(1.1));
        }

        [Fact]
        public void WithUpdateConfig_FromBuilder_SetsIt()
        {
            DeployDefinition deploy = new DeployBuilder()
                .WithUpdateConfig(updateConfig => updateConfig.WithOrder(RolloutOrder.StartFirst))
                .Build();

            Assert.Equal(RolloutOrder.StartFirst, deploy.UpdateConfig!.Order);
        }

        [Fact]
        public void ServiceBuilder_WithDeploy_SetsIt()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithDeploy(deploy => deploy.WithReplicas(3))
                .Build();

            Assert.Equal(3, service.Deploy!.Replicas);
        }
    }
}
