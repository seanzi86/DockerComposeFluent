using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>volumes</c> golden scenarios.
    /// </summary>
    internal static class VolumesScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("nginx")
                    .WithVolume("db-data:/var/lib/db")
                    .WithVolume("./src", "/app", readOnly: true)
                    .WithVolume(mount => mount
                        .WithType(MountType.Bind)
                        .WithSource("./conf")
                        .WithTarget("/etc/app")
                        .WithBindOptions(options => options
                            .WithCreateHostPath(false)
                            .WithPropagation(BindPropagation.RShared)
                            .WithSelinux(SelinuxLabel.Shared)))
                    .WithVolume(mount => mount
                        .WithType(MountType.Volume)
                        .WithSource("cache")
                        .WithTarget("/cache")
                        .WithReadOnly(true)
                        .WithVolumeOptions(options => options.WithNoCopy(true).WithSubpath("sub")))
                    .WithVolume(mount => mount
                        .WithType(MountType.Tmpfs)
                        .WithTarget("/tmp")
                        .WithTmpfsOptions(options => options.WithSize("100m").WithMode(Convert.ToInt32("1777", 8))))
                    .WithVolume(mount => mount
                        .WithType(MountType.Image)
                        .WithSource("example/assets:1")
                        .WithTarget("/assets")
                        .WithImageOptions(options => options.WithSubpath("static"))))
                .WithVolume("db-data", volume => volume.WithDriver("local"))
                .WithVolume("cache", volume => volume.WithName("app_cache"))
                .Build();
        }
    }
}
