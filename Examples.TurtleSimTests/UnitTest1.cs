using FluentAssertions;
using IntegROS;
using Xunit;


[assembly: TestFramework("IntegROS.XunitExtensions.IntegrosTestFramework", "IntegROS")]

namespace Examples.TurtleSimTests
{

    // [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
    // [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
    public class ForNewScenarioExample : ForNewScenario
    {
        [ExpectThat]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
        public void Test_fails()
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