using System.Linq;
using Examples.FibonacciActionTests.Messages;
using FluentAssertions;
using IntegROS;
using IntegROS.Ros.Messages;

namespace Examples.FibonacciActionTests
{
    public class FibonacciActionAllInOneTests : ForScenario
    {
        [ExpectThat]
        [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
        [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
        [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
        public void Fibonacci_calculation_is_valid()
        {
            var actionCalls = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => new
                {
                    GoalOrder = x.GoalMessage.Value.Goal<FibonacciGoal>().Order,
                    Result = x.ResultMessage.Value.Result<FibonacciResult>().Sequence,
                    FinalState = x.FinalState
                });

            foreach (var actionCall in actionCalls)
            {
                actionCall.FinalState.Should().Be(GoalStatus.Succeeded);
                actionCall.Result.Count().Should().Be(actionCall.GoalOrder + 2);

                for (var i = 0; i < actionCall.Result.Count(); i++)
                {
                    if (i == 0 || i == 1)
                        actionCall.Result.Skip(i).First().Should().Be(i, "first two fibonaccis should be 0 and 1");
                    else
                    {
                        var items = actionCall.Result
                            .Skip(i - 2)
                            .Take(3)
                            .ToList();

                        items[2].Should().Be(items[0] + items[1]);
                    }
                }
            }
        }
    }
}