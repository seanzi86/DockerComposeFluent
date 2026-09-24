using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class MountOptionsBuilderTests
    {
        [Fact]
        public void BindOptionsBuilder_WithNothingSet_IsEmpty()
        {
            Assert.Equal(new BindOptions(), new BindOptionsBuilder().Build());
        }

        [Fact]
        public void TmpfsOptionsBuilder_WithSizeInBytes_StoresDigits()
        {
            Assert.Equal("1048576", new TmpfsOptionsBuilder().WithSize(1048576L).Build().Size);
        }

        [Fact]
        public void TmpfsOptionsBuilder_ModeIsTheNumericValue()
        {
            Assert.Equal(Convert.ToInt32("1777", 8), new TmpfsOptionsBuilder().WithMode(Convert.ToInt32("1777", 8)).Build().Mode);
        }

        [Fact]
        public void TmpfsOptionsBuilder_InvalidValues_Throw()
        {
            TmpfsOptionsBuilder builder = new TmpfsOptionsBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithSize(0L));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithSize(-1L));
            Assert.Throws<ArgumentException>(() => builder.WithSize(" "));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithMode(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithMode(4096));
        }

        [Fact]
        public void BlankSubpaths_Throw()
        {
            Assert.Throws<ArgumentException>(() => new VolumeOptionsBuilder().WithSubpath(""));
            Assert.Throws<ArgumentException>(() => new ImageOptionsBuilder().WithSubpath(" "));
        }
    }
}
