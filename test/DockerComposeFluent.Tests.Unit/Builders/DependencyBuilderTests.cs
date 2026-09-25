using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class DependencyBuilderTests
    {
        [Fact]
        public void Build_NothingSet_IsEmpty()
        {
            DependencyDefinition dependency = new DependencyBuilder().Build();

            Assert.True(dependency.IsEmpty);
            Assert.Null(dependency.Condition);
            Assert.Null(dependency.Restart);
            Assert.Null(dependency.Required);
        }

        [Fact]
        public void Build_AllSettings_AreSet()
        {
            DependencyDefinition dependency = new DependencyBuilder()
                .WithCondition(DependencyCondition.ServiceHealthy)
                .WithRestart(true)
                .WithRequired(false)
                .Build();

            Assert.False(dependency.IsEmpty);
            Assert.Equal(DependencyCondition.ServiceHealthy, dependency.Condition);
            Assert.True(dependency.Restart);
            Assert.False(dependency.Required);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void IsEmpty_AnySingleSetting_IsNotEmpty(bool condition, bool restart, bool required)
        {
            DependencyDefinition dependency = new DependencyDefinition
            {
                Condition = condition ? DependencyCondition.ServiceStarted : null,
                Restart = restart ? false : null,
                Required = required ? false : null
            };

            Assert.False(dependency.IsEmpty);
        }
    }
}
