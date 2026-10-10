using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class DevelopBuilderTests
    {
        [Fact]
        public void Rebuild_NeedsOnlyAPathAndAction()
        {
            WatchRuleDefinition rule = new WatchRuleBuilder().WithPath("./src").WithAction(WatchAction.Rebuild).Build();

            Assert.Equal("./src", rule.Path);
            Assert.Equal(WatchAction.Rebuild, rule.Action);
        }

        [Fact]
        public void PathAndAction_AreRequired()
        {
            Assert.Throws<InvalidOperationException>(() => new WatchRuleBuilder().WithAction(WatchAction.Rebuild).Build());
            Assert.Throws<InvalidOperationException>(() => new WatchRuleBuilder().WithPath("./src").Build());
        }

        [Theory]
        [InlineData(WatchAction.Sync)]
        [InlineData(WatchAction.SyncAndRestart)]
        [InlineData(WatchAction.SyncAndExec)]
        public void SyncActions_NeedATarget(WatchAction action)
        {
            WatchRuleBuilder builder = new WatchRuleBuilder().WithPath("./src").WithAction(action).WithExec("x");

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void SyncAndExec_NeedsACommand_AndOnlyThatActionAllowsOne()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new WatchRuleBuilder().WithPath("./src").WithAction(WatchAction.SyncAndExec).WithTarget("/t").Build());
            Assert.Throws<InvalidOperationException>(() =>
                new WatchRuleBuilder().WithPath("./src").WithAction(WatchAction.Sync).WithTarget("/t").WithExec("x").Build());
        }

        [Fact]
        public void Lists_AppendAndAcceptSeveral()
        {
            WatchRuleDefinition rule = new WatchRuleBuilder()
                .WithPath("./src").WithAction(WatchAction.Rebuild)
                .WithIgnore("a").WithIgnore(new[] { "b", "c" })
                .WithInclude(new[] { "*.go" })
                .Build();

            Assert.Equal(new[] { "a", "b", "c" }, rule.Ignore);
            Assert.Equal(new[] { "*.go" }, rule.Include);
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            WatchRuleBuilder builder = new WatchRuleBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithPath(" "));
            Assert.Throws<ArgumentException>(() => builder.WithTarget(" "));
            Assert.Throws<ArgumentException>(() => builder.WithIgnore(" "));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithAction((WatchAction)99));
        }

        [Fact]
        public void Develop_AccumulatesRulesInOrder()
        {
            DevelopDefinition develop = new DevelopBuilder()
                .WithSyncWatch("./a", "/a")
                .WithWatch("./b", WatchAction.Restart)
                .WithWatch(rule => rule.WithPath("./c").WithAction(WatchAction.Rebuild))
                .Build();

            Assert.Equal(new[] { "./a", "./b", "./c" }, new[] { develop.Watch[0].Path, develop.Watch[1].Path, develop.Watch[2].Path });
        }

        [Fact]
        public void ServiceBuilder_WithDevelop_AcceptsBothForms()
        {
            DevelopDefinition develop = new DevelopBuilder().WithWatch("./a", WatchAction.Rebuild).Build();

            Assert.Same(develop, new ServiceBuilder().WithDevelop(develop).Build().Develop);
            Assert.Single(new ServiceBuilder().WithDevelop(d => d.WithWatch("./a", WatchAction.Rebuild)).Build().Develop!.Watch);
            Assert.Throws<ArgumentNullException>(() => new ServiceBuilder().WithDevelop((DevelopDefinition)null!));
        }
    }
}
