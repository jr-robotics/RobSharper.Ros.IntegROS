using FluentAssertions;
using IntegROS;
using IntegROS.Scenarios;
using Xunit;


//[assembly: TestFramework("IntegROS.XunitExtensions.IntegrosTestFramework", "IntegROS")]

namespace Examples.TurtleSimTests
{

    // [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
    // [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
    public class ForNewScenarioExample : ForNewScenario
    {
        [ExpectThat()]
        public void Expectation_without_scenario_should_not_run(IScenario scenario)
        {
            Assert.True(true);
        }
        
        [ExpectThat()]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        public void Expectation_for_one_scenario(IScenario scenario)
        {
            Assert.True(true);
        }
        
        [ExpectThat()]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
        public void Expectation_for_two_scenarios(IScenario scenario)
        {
            Assert.True(true);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void My_theory(int input)
        {
            input.Should().BePositive();
        }
    }
}