using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The capability, security and device service setting golden scenarios.
    /// </summary>
    internal static class ServiceSecurityScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("secure", service => service
                    .WithImage("example/app")
                    .WithCapAdd(new[] { "NET_ADMIN", "SYS_TIME" })
                    .WithCapDrop("ALL")
                    .WithSecurityOpt("no-new-privileges:true")
                    .WithGroupAdd("mail")
                    .WithGroupAdd("1001")
                    .WithDeviceCgroupRules(new[] { "c 1:3 mr", "a 7:* rmw" })
                    .WithDevice("/dev/ttyUSB0")
                    .WithDevice("/dev/sda", "/dev/xvda")
                    .WithDevice(device => device
                        .WithSource("/dev/nvme0")
                        .WithTarget("/dev/nvme0")
                        .WithPermissions("rw"))
                    .WithDevice(device => device
                        .WithSource("/dev/video0")
                        .WithPermissions("r")
                        .WithExtension("x-note", "camera"))
                    .WithCredentialSpec(spec => spec.WithConfig("my_credential_spec")))
                .WithService("file-spec", service => service
                    .WithImage("example/app")
                    .WithCredentialSpec(spec => spec.WithFile("my-credential-spec.json")))
                .WithService("registry-spec", service => service
                    .WithImage("example/app")
                    .WithCredentialSpec(spec => spec.WithRegistry("HKLM\\SOFTWARE\\Spec")))
                .WithService("all-gpus", service => service
                    .WithImage("example/gpu")
                    .WithAllGpus())
                .WithService("some-gpus", service => service
                    .WithImage("example/gpu")
                    .WithGpu(gpu => gpu
                        .WithDriver("nvidia")
                        .WithCapability("compute")
                        .WithCapability("utility")
                        .WithCount(2))
                    .WithGpu(gpu => gpu
                        .WithCapability("gpu")
                        .WithDeviceId("GPU-3a23c669")
                        .WithOption("virtualization", "false")))
                .Build();
        }
    }
}
