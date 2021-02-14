using FluentAssertions;
using IntegROS;
using Xunit;

//[assembly: TestCaseOrderer("IntegROS.XunitExtensions.ExpectationTestCaseOrderer", "IntegROS")]
//[assembly: TestFramework("IntegROS.XunitExtensions.IntegrosTestFramework", "IntegROS")]

namespace Examples.TurtleSimTests
{

    // [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
    // [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
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
        
        //
        // [Theory]
        // // [InlineData(-1, Skip = "Invalid data")]
        // // [InlineData(0, Skip = "Invalid data")]
        // [InlineData(1)]
        // [InlineData(2)]
        // public void My_theory(int input)
        // {
        //     input.Should().BePositive();
        // }
        
        //
        // [Fact()]
        // public void My_fact()
        // {
        //     true.Should().BeTrue();
        // }

        // [Fact]
        // public void FailIfJetBrainsRider()
        // {
        //     var executing = Assembly.GetEntryAssembly();
        //     var isJetBrainsExecutor = executing.FullName.StartsWith("resharper", StringComparison.OrdinalIgnoreCase);
        //
        //     isJetBrainsExecutor.Should().BeFalse("JetBrains Resharper Test executor is not supported");
        // }
        //
        // [Fact]
        // public void TestExecutorName()
        // {
        //     var executing = Assembly.GetEntryAssembly();
        //     executing.FullName.Should().BeNullOrEmpty();
        // }
    }
}