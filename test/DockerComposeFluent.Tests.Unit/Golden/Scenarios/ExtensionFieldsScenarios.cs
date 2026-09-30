using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>x-</c> extension field golden scenarios, spanning the top level, a service and its nested
    /// settings, and the other top-level sections.
    /// </summary>
    internal static class ExtensionFieldsScenarios
    {
        /// <summary>
        /// An extension field at the top level, on a service, and on several of a service's nested settings
        /// (deploy and its sub-sections, healthcheck, logging, a port, a mount and its bind options, a
        /// dependency, a network attachment, and a secret/config reference - each forcing the long syntax even
        /// though it is the only setting). Also one on each other top-level section (networks, including IPAM,
        /// volumes, secrets, configs).
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithExtension("x-project-notes", "internal use only")
                .WithService("web", service => service
                    .WithImage("example/web")
                    .WithExtension("x-team", "platform")
                    .WithDependsOn("db")
                    .WithHealthcheck(healthcheck => healthcheck
                        .WithTest("curl -f http://localhost || exit 1")
                        .WithExtension("x-healthcheck-note", "tuned for slow disks"))
                    .WithLogging(logging => logging
                        .WithDriver("json-file")
                        .WithExtension("x-logging-note", "rotated hourly"))
                    .WithPort(port => port
                        .WithTarget(80)
                        .WithExtension("x-port-note", "public"))
                    .WithVolume(mount => mount
                        .WithType(MountType.Bind)
                        .WithSource("./conf")
                        .WithTarget("/etc/app")
                        .WithExtension("x-mount-note", "read-only config")
                        .WithBindOptions(options => options
                            .WithCreateHostPath(false)
                            .WithExtension("x-bind-note", "must already exist")))
                    .WithNetwork("front", attachment => attachment
                        .WithExtension("x-network-note", "public facing"))
                    .WithSecret(secretReference => secretReference
                        .WithSource("api_key")
                        .WithExtension("x-secret-note", "rotated monthly"))
                    .WithConfig(configReference => configReference
                        .WithSource("app_config")
                        .WithExtension("x-config-note", "generated at deploy time"))
                    .WithDeploy(deploy => deploy
                        .WithReplicas(2)
                        .WithExtension("x-deploy-note", "scaled for peak traffic")
                        .WithPlacement(placement => placement
                            .WithConstraint("node.role==worker")
                            .WithExtension("x-placement-note", "worker only"))
                        .WithResources(resources => resources
                            .WithExtension("x-resources-note", "tuned after load test")
                            .WithLimits(limits => limits
                                .WithCpus("0.50")
                                .WithExtension("x-limits-note", "ceiling from load test"))
                            .WithReservations(reservations => reservations
                                .WithCpus("0.25")
                                .WithExtension("x-reservations-note", "floor from load test")
                                .WithDevice(device => device
                                    .WithCapability("gpu")
                                    .WithExtension("x-device-note", "inference workload"))))
                        .WithRestartPolicy(restartPolicy => restartPolicy
                            .WithCondition(DeployRestartCondition.OnFailure)
                            .WithExtension("x-restart-note", "tuned after incident"))
                        .WithRollbackConfig(rollbackConfig => rollbackConfig
                            .WithParallelism(1)
                            .WithExtension("x-rollback-note", "one at a time"))
                        .WithUpdateConfig(updateConfig => updateConfig
                            .WithParallelism(1)
                            .WithExtension("x-update-note", "one at a time"))))
                .WithService("db", service => service.WithImage("postgres"))
                .WithNetwork("front", network => network
                    .WithDriver("bridge")
                    .WithExtension("x-network-top-note", "shared ingress")
                    .WithIpam(ipam => ipam
                        .WithExtension("x-ipam-note", "static plan")
                        .WithConfig(config => config
                            .WithSubnet("10.0.0.0/24")
                            .WithExtension("x-ipam-config-note", "reserved for prod"))))
                .WithVolume("data", volume => volume
                    .WithDriver("local")
                    .WithExtension("x-volume-note", "backed up nightly"))
                .WithSecret("api_key", secret => secret
                    .WithEnvironment("API_KEY")
                    .WithExtension("x-secret-top-note", "rotated monthly"))
                .WithConfig("app_config", config => config
                    .WithContent("debug=false")
                    .WithExtension("x-config-top-note", "generated at deploy time"))
                .Build();
        }
    }
}
