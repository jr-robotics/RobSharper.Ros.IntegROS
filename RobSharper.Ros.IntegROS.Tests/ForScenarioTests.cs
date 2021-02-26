using FluentAssertions;
using Moq;
using RobSharper.Ros.IntegROS.Scenarios;
using Xunit;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.Tests
{
    public class ForScenarioTests
    {
        private class TestableForScenario : ForScenario
        {
            public TestableForScenario() : base() { }
            
            public TestableForScenario(ITestOutputHelper outputHelper) : base(outputHelper) {}

            public void SetScenario(IScenario scenario)
            {
                Scenario = scenario;
            }
        }
        
        [Fact]
        public void Can_set_and_get_scenario()
        {
            var target = new TestableForScenario();

            var scenario = new Mock<IScenario>().Object;

            target.SetScenario(scenario);

            target.Scenario.Should().BeSameAs(scenario);
        }
        
        [Fact]
        public void Can_pass_output_helper_in_constructor()
        {
            var outputHelper = new Mock<ITestOutputHelper>().Object;
            var target = new TestableForScenario(outputHelper);

            target.OutputHelper.Should().BeSameAs(outputHelper);
        }
    }
}