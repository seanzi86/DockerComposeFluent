using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models
{
    public class RestartModelTests
    {
        [Theory]
        [InlineData(RestartPolicy.No)]
        [InlineData(RestartPolicy.Always)]
        [InlineData(RestartPolicy.OnFailure)]
        [InlineData(RestartPolicy.UnlessStopped)]
        public void FromPolicy_SetsPolicyWithoutRetryLimit(RestartPolicy policy)
        {
            RestartDefinition restart = RestartDefinition.FromPolicy(policy);

            Assert.Equal(policy, restart.Policy);
            Assert.Null(restart.MaxRetries);
        }

        [Fact]
        public void FromPolicy_UnknownValue_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RestartDefinition.FromPolicy((RestartPolicy)99));
        }

        [Fact]
        public void OnFailure_SetsPolicyAndRetryLimit()
        {
            RestartDefinition restart = RestartDefinition.OnFailure(5);

            Assert.Equal(RestartPolicy.OnFailure, restart.Policy);
            Assert.Equal(5, restart.MaxRetries);
        }

        [Fact]
        public void OnFailure_NegativeRetries_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RestartDefinition.OnFailure(-1));
        }

        [Theory]
        [InlineData("no", RestartPolicy.No, null)]
        [InlineData("always", RestartPolicy.Always, null)]
        [InlineData("unless-stopped", RestartPolicy.UnlessStopped, null)]
        [InlineData("on-failure", RestartPolicy.OnFailure, null)]
        [InlineData("on-failure:5", RestartPolicy.OnFailure, 5)]
        [InlineData("on-failure:0", RestartPolicy.OnFailure, 0)]
        public void Parse_ValidText_SetsPolicyAndRetries(string text, RestartPolicy policy, int? maxRetries)
        {
            RestartDefinition restart = RestartDefinition.Parse(text);

            Assert.Equal(policy, restart.Policy);
            Assert.Equal(maxRetries, restart.MaxRetries);
        }

        [Theory]
        [InlineData("sometimes")]
        [InlineData("Always")]
        [InlineData("on-failure:")]
        [InlineData("on-failure:x")]
        [InlineData("on-failure:-1")]
        [InlineData("on-failure:1.5")]
        [InlineData("always:3")]
        public void Parse_InvalidText_Throws(string text)
        {
            Assert.Throws<ArgumentException>(() => RestartDefinition.Parse(text));
        }

        [Fact]
        public void Parse_BlankOrNull_Throws()
        {
            Assert.Throws<ArgumentException>(() => RestartDefinition.Parse(" "));
            Assert.Throws<ArgumentException>(() => RestartDefinition.Parse(null!));
        }

        [Fact]
        public void Equality_SameValues_AreEqual()
        {
            Assert.Equal(RestartDefinition.OnFailure(3), RestartDefinition.Parse("on-failure:3"));
            Assert.NotEqual(RestartDefinition.OnFailure(3), RestartDefinition.FromPolicy(RestartPolicy.OnFailure));
        }
    }
}
