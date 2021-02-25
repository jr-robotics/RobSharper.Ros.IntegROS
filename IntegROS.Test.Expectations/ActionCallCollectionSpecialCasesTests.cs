using FluentAssertions;
using IntegROS.Ros.Messages;

namespace IntegROS.Test.Expectations
{
    public class ActionCallCollectionSpecialCasesTests : ForScenario
    {
        [ExpectThat]
        [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
        [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
        [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
        public void All_action_calls_succeed()
        {
            Scenario.Messages
                .ForActionCalls("/fibonacci")
                .Should()
                .OnlyContain(x => x.FinalState == GoalStatusValue.Succeeded);
        }

        [ExpectThat]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciWithoutCalls)]
        public void Collection_is_empty_if_action_was_not_called()
        {
            Scenario.Messages
                .ForActionCalls("/fibonacci")
                .Should()
                .BeEmpty();
        }
        
        [ExpectThat]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciWithoutCalls)]
        public void Collection_is_empty_if_action_does_not_exists()
        {
            Scenario.Messages
                .ForActionCalls("/unknownaction")
                .Should()
                .BeEmpty();
        }
    }
}