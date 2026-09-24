using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class MountBuilderTests
    {
        [Fact]
        public void Build_SetsCommonFields()
        {
            MountDefinition mount = new MountBuilder()
                .WithType(MountType.Volume)
                .WithSource("data")
                .WithTarget("/data")
                .WithReadOnly(true)
                .WithConsistency("cached")
                .Build();

            Assert.Equal(MountType.Volume, mount.Type);
            Assert.Equal("data", mount.Source);
            Assert.Equal("/data", mount.Target);
            Assert.True(mount.ReadOnly);
            Assert.Equal("cached", mount.Consistency);
        }

        [Fact]
        public void Build_WithOnlyTypeAndTarget_LeavesOtherFieldsUnset()
        {
            MountDefinition mount = new MountBuilder().WithType(MountType.Tmpfs).WithTarget("/tmp").Build();

            Assert.Null(mount.Source);
            Assert.Null(mount.ReadOnly);
            Assert.Null(mount.Consistency);
            Assert.Null(mount.Bind);
            Assert.Null(mount.Volume);
            Assert.Null(mount.Tmpfs);
            Assert.Null(mount.Image);
        }

        [Fact]
        public void Build_WithoutTypeOrTarget_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithTarget("/x").Build());
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithType(MountType.Bind).Build());
        }

        [Fact]
        public void OptionGroups_BuiltThroughTheirBuilders()
        {
            MountDefinition bind = new MountBuilder()
                .WithType(MountType.Bind).WithTarget("/x")
                .WithBindOptions(o => o.WithCreateHostPath(false).WithPropagation(BindPropagation.RShared).WithSelinux(SelinuxLabel.Private))
                .Build();
            MountDefinition volume = new MountBuilder()
                .WithType(MountType.Volume).WithTarget("/x")
                .WithVolumeOptions(o => o.WithNoCopy(true).WithSubpath("sub"))
                .Build();
            MountDefinition tmpfs = new MountBuilder()
                .WithType(MountType.Tmpfs).WithTarget("/x")
                .WithTmpfsOptions(o => o.WithSize("100m").WithMode(1023))
                .Build();
            MountDefinition image = new MountBuilder()
                .WithType(MountType.Image).WithTarget("/x")
                .WithImageOptions(o => o.WithSubpath("assets"))
                .Build();

            Assert.Equal(false, bind.Bind!.CreateHostPath);
            Assert.Equal(BindPropagation.RShared, bind.Bind.Propagation);
            Assert.Equal(SelinuxLabel.Private, bind.Bind.Selinux);
            Assert.Equal(true, volume.Volume!.NoCopy);
            Assert.Equal("sub", volume.Volume.Subpath);
            Assert.Equal("100m", tmpfs.Tmpfs!.Size);
            Assert.Equal(1023, tmpfs.Tmpfs.Mode);
            Assert.Equal("assets", image.Image!.Subpath);
        }

        [Fact]
        public void OptionGroups_RawModelOverloads_AreUsedAsGiven()
        {
            BindOptions options = new BindOptions { Propagation = BindPropagation.Slave };

            MountDefinition mount = new MountBuilder().WithType(MountType.Bind).WithTarget("/x").WithBindOptions(options).Build();

            Assert.Same(options, mount.Bind);
        }

        [Fact]
        public void OptionGroupsThatDoNotMatchTheType_Throw()
        {
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithType(MountType.Volume).WithTarget("/x").WithBindOptions(o => o.WithCreateHostPath(true)).Build());
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithType(MountType.Bind).WithTarget("/x").WithVolumeOptions(o => o.WithNoCopy(true)).Build());
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithType(MountType.Bind).WithTarget("/x").WithTmpfsOptions(o => o.WithSize("1m")).Build());
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithType(MountType.Volume).WithTarget("/x").WithImageOptions(o => o.WithSubpath("a")).Build());
        }

        [Fact]
        public void TmpfsMountWithSource_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new MountBuilder().WithType(MountType.Tmpfs).WithTarget("/tmp").WithSource("x").Build());
        }

        [Fact]
        public void InvalidArguments_Throw()
        {
            MountBuilder builder = new MountBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithSource(" "));
            Assert.Throws<ArgumentException>(() => builder.WithTarget(""));
            Assert.Throws<ArgumentException>(() => builder.WithConsistency(" "));
            Assert.Throws<ArgumentNullException>(() => builder.WithBindOptions((BindOptions)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithBindOptions((Action<BindOptionsBuilder>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolumeOptions((Action<VolumeOptionsBuilder>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithTmpfsOptions((TmpfsOptions)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithImageOptions((Action<ImageOptionsBuilder>)null!));
        }
    }
}
