using System;
using DockerComposeFluent.Builders;

namespace DockerComposeFluent.Tests.Unit.Models
{
    /// <summary>
    /// Tests of the compose-spec duration rules, through the public healthcheck builder that uses them.
    /// </summary>
    public class DurationTests
    {
        [Theory]
        [InlineData("10ms")]
        [InlineData("40s")]
        [InlineData("1m30s")]
        [InlineData("1h5m30s20ms")]
        [InlineData("5us")]
        [InlineData("0s")]
        [InlineData("90m")]
        public void Text_SpecExamples_AreAccepted(string text)
        {
            Assert.Equal(text, new HealthcheckBuilder().WithInterval(text).Build().Interval);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("s")]
        [InlineData("30")]
        [InlineData("30 s")]
        [InlineData("1.5s")]
        [InlineData("-5s")]
        [InlineData("30sec")]
        [InlineData("1d")]
        [InlineData("5S")]
        [InlineData("1m30")]
        [InlineData("m30s")]
        public void Text_OtherValues_AreRejected(string text)
        {
            Assert.Throws<ArgumentException>(() => new HealthcheckBuilder().WithInterval(text));
        }

        [Fact]
        public void Text_Null_IsRejected()
        {
            Assert.Throws<ArgumentException>(() => new HealthcheckBuilder().WithInterval((string)null!));
        }

        [Theory]
        [InlineData(10, "10ms")]
        [InlineData(40000, "40s")]
        [InlineData(90000, "1m30s")]
        [InlineData(3600000, "1h")]
        [InlineData(3930020, "1h5m30s20ms")]
        [InlineData(1, "1ms")]
        public void TimeSpan_Milliseconds_IsWrittenInTheShortestForm(int milliseconds, string expected)
        {
            Assert.Equal(expected, new HealthcheckBuilder().WithInterval(TimeSpan.FromMilliseconds(milliseconds)).Build().Interval);
        }

        [Fact]
        public void TimeSpan_Microseconds_AreKept()
        {
            Assert.Equal("1s500us", new HealthcheckBuilder().WithInterval(TimeSpan.FromTicks(10_005_000)).Build().Interval);
            Assert.Equal("7us", new HealthcheckBuilder().WithInterval(TimeSpan.FromTicks(70)).Build().Interval);
        }

        [Fact]
        public void TimeSpan_WithEveryUnit_IsWrittenAsAValidDuration()
        {
            string text = new HealthcheckBuilder().WithInterval(new TimeSpan(1, 2, 3, 4, 5)).Build().Interval!;

            Assert.Equal("26h3m4s5ms", text);
            Assert.Equal(text, new HealthcheckBuilder().WithTimeout(text).Build().Timeout);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10000)]
        [InlineData(5)]
        public void TimeSpan_ZeroNegativeOrTooPrecise_Throws(long ticks)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HealthcheckBuilder().WithInterval(TimeSpan.FromTicks(ticks)));
        }
    }
}
