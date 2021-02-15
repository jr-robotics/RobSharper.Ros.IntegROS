using FluentAssertions;
using IntegROS;
using Xunit;

//[assembly: TestCaseOrderer("IntegROS.XunitExtensions.ExpectationTestCaseOrderer", "IntegROS")]

namespace Examples.TurtleSimTests
{

    [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
    [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
    public class ForClassScenariosExample : ForScenario
    {
        [ExpectThat]
        public void TestCase()
        {
            Scenario.Should().NotBeNull();
        }
    }
    
    public class ForScenarioExample : ForScenario
    {
        [ExpectThat(Skip = "Does not work without scenario attribute")]
        public void Expectation_without_scenario_should_not_run()
        {
            Scenario.Should().BeNull();
        }
        
        [ExpectThat(DisplayName = "Expect that")]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        public void Expectation_for_one_scenario()
        {
            Scenario.Should().NotBeNull();
        }
        
        [Theory(DisplayName = "Theory Name")]
        [InlineData(2)]
        public void My_single_data_theory(int input)
        {
            input.Should().BePositive();
        }
        
        [ExpectThat()]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
        public void Expectation_for_two_scenarios()
        {
            Scenario.Should().NotBeNull();
        }
    }
}