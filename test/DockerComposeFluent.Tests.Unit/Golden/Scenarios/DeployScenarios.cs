using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>deploy</c> golden scenarios.
    /// </summary>
    internal static class DeployScenarios
    {
        /// <summary>
        /// Every top-level <c>deploy</c> setting other than <c>resources</c>, built through
        /// <see cref="DeployBuilder"/>: mode, endpoint mode, replicas, labels, placement, restart policy,
        /// rollback configuration and update configuration.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("web", service => service
                    .WithImage("example/web")
                    .WithDeploy(deploy => deploy
                        .WithMode(DeployMode.Replicated)
                        .WithEndpointMode(EndpointMode.Vip)
                        .WithReplicas(6)
                        .WithLabel("com.example.description", "Front-end web service")
                        .WithPlacement(placement => placement
                            .WithConstraint("node.role==worker")
                            .WithConstraint("disktype=ssd")
                            .WithPreference("node.labels.zone")
                            .WithMaxReplicasPerNode(2))
                        .WithRestartPolicy(restartPolicy => restartPolicy
                            .WithCondition(DeployRestartCondition.OnFailure)
                            .WithDelay(TimeSpan.FromSeconds(5))
                            .WithMaxAttempts(3)
                            .WithWindow("120s"))
                        .WithRollbackConfig(rollbackConfig => rollbackConfig
                            .WithParallelism(2)
                            .WithDelay("10s")
                            .WithFailureAction(RollbackFailureAction.Continue)
                            .WithMonitor(TimeSpan.FromSeconds(30))
                            .WithMaxFailureRatio(0.3)
                            .WithOrder(RolloutOrder.StartFirst))
                        .WithUpdateConfig(updateConfig => updateConfig
                            .WithParallelism(1)
                            .WithDelay(TimeSpan.FromSeconds(10))
                            .WithFailureAction(UpdateFailureAction.Rollback)
                            .WithMonitor("15s")
                            .WithMaxFailureRatio(0.1)
                            .WithOrder(RolloutOrder.StopFirst))))
                .WithService("worker", service => service
                    .WithImage("example/worker")
                    .WithDeploy(new DeployDefinition
                    {
                        Mode = DeployMode.Global,
                        EndpointMode = EndpointMode.Dnsrr
                    }))
                .Build();
        }

        /// <summary>
        /// Resource limits and reservations, including a device reservation, and the device-reservation forms
        /// (capabilities only, a driver, a device count, and explicit device IDs).
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Resources()
        {
            return new DockerComposeBuilder()
                .WithService("api", service => service
                    .WithImage("example/api")
                    .WithDeploy(deploy => deploy
                        .WithResources(resources => resources
                            .WithLimits(limits => limits
                                .WithCpus("0.50")
                                .WithMemory("50M")
                                .WithPids(100))
                            .WithReservations(reservations => reservations
                                .WithCpus("0.25")
                                .WithMemory("20M")
                                .WithDevice(device => device
                                    .WithCapability("gpu")
                                    .WithDriver("nvidia")
                                    .WithCount(2))))))
                .WithService("accelerated", service => service
                    .WithImage("example/accelerated")
                    .WithDeploy(deploy => deploy
                        .WithResources(resources => resources
                            .WithReservations(reservations => reservations
                                .WithDevice(device => device
                                    .WithCapability("gpu")
                                    .WithDeviceIds(new[] { "GPU-f123d1c9-26bb-df9b-1c23-4a731f61d8c7" })
                                    .WithOption("virtualization", "false"))))))
                .Build();
        }
    }
}
