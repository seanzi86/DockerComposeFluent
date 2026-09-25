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

        /// <summary>
        /// Every way of writing a mount in one file: short syntax, helper overloads and each long-syntax type,
        /// with the top-level volumes they refer to.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile KitchenSink()
        {
            return new DockerComposeBuilder()
                .WithService("short", service => service
                    .WithImage("nginx")
                    .WithVolumes(new[] { "/opt/data:/var/lib/mysql", "./cache:/tmp/cache", "~/configs:/etc/configs/:ro", "datavolume:/var/lib/mysql" })
                    .WithVolume("logs", "/var/log")
                    .WithVolume("./site", "/usr/share/nginx/html", readOnly: true))
                .WithService("long", service => service
                    .WithImage("nginx")
                    .WithVolume(mount => mount.WithType(MountType.Volume).WithSource("datavolume").WithTarget("/data"))
                    .WithVolume(mount => mount
                        .WithType(MountType.Volume)
                        .WithSource("datavolume")
                        .WithTarget("/data-nocopy")
                        .WithReadOnly(true)
                        .WithVolumeOptions(options => options.WithNoCopy(true).WithSubpath("nested")))
                    .WithVolume(mount => mount.WithType(MountType.Bind).WithSource("./static").WithTarget("/opt/app/static"))
                    .WithVolume(mount => mount
                        .WithType(MountType.Bind)
                        .WithSource("./conf")
                        .WithTarget("/etc/app")
                        .WithReadOnly(true)
                        .WithConsistency("cached")
                        .WithBindOptions(options => options
                            .WithCreateHostPath(false)
                            .WithPropagation(BindPropagation.RSlave)
                            .WithSelinux(SelinuxLabel.Private)))
                    .WithVolume(mount => mount.WithType(MountType.Tmpfs).WithTarget("/run"))
                    .WithVolume(mount => mount
                        .WithType(MountType.Tmpfs)
                        .WithTarget("/tmp")
                        .WithTmpfsOptions(options => options.WithSize("64m").WithMode(Convert.ToInt32("1777", 8))))
                    .WithVolume(mount => mount
                        .WithType(MountType.Image)
                        .WithSource("example/assets:1")
                        .WithTarget("/assets")
                        .WithImageOptions(options => options.WithSubpath("static"))))
                .WithVolume("datavolume", volume => volume.WithDriver("local"))
                .WithVolume("logs", volume => volume.WithName("shared_logs"))
                .Build();
        }
    }
}
