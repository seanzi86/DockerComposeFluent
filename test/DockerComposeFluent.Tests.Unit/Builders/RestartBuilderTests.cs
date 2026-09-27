using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class RestartBuilderTests
    {
        [Fact]
        public void Build_NothingSet_DefaultsToNo()
        {
            RestartDefinition restart = new RestartBuilder().Build();

            Assert.Equal(RestartDefinition.FromPolicy(RestartPolicy.No), restart);
        }

        [Theory]
        [InlineData(RestartPolicy.No)]
        [InlineData(RestartPolicy.Always)]
        [InlineData(RestartPolicy.UnlessStopped)]
        public void WithPolicy_NoRetries_BuildsThatPolicy(RestartPolicy policy)
        {
            RestartDefinition restart = new RestartBuilder().WithPolicy(policy).Build();

            Assert.Equal(RestartDefinition.FromPolicy(policy), restart);
        }

        [Fact]
        public void WithPolicy_UnknownValue_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RestartBuilder().WithPolicy((RestartPolicy)99));
        }

        [Fact]
        public void WithMaxRetries_OnFailure_SetsTheLimit()
        {
            RestartDefinition restart = new RestartBuilder().WithPolicy(RestartPolicy.OnFailure).WithMaxRetries(5).Build();

            Assert.Equal(RestartDefinition.OnFailure(5), restart);
        }

        [Fact]
        public void WithMaxRetries_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RestartBuilder().WithMaxRetries(-1));
        }

        [Theory]
        [InlineData(RestartPolicy.No)]
        [InlineData(RestartPolicy.Always)]
        [InlineData(RestartPolicy.UnlessStopped)]
        public void Build_MaxRetriesWithOtherPolicy_Throws(RestartPolicy policy)
        {
            RestartBuilder builder = new RestartBuilder().WithPolicy(policy).WithMaxRetries(3);

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void WithMaxRetries_BeforePolicy_StillValidatesOnBuild()
        {
            RestartBuilder builder = new RestartBuilder().WithMaxRetries(3).WithPolicy(RestartPolicy.Always);

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }
    }
}
