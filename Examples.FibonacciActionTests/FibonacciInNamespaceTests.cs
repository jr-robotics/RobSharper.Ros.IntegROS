using System.Collections.Generic;
using System.Linq;
using Examples.FibonacciActionTests.Messages;
using FluentAssertions;
using RobSharper.Ros.IntegROS;

namespace Examples.FibonacciActionTests
{
    
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciIn2Namespaces)]
    public class FibonacciInNamespaceTests : ForScenario
    {
        [ExpectThat]
        public void Fooo()
        {
            /*
             * 1) Select all Messages
             * 2) Filter for any "fibonacci" action calls
             * 3) Select the result sequence of each action call
             * 4) Remove any null values or sequences without elements
             *    (may be the case if scenario has no GoalResult message for any reason)
             * 5) Select the first number of each result
             */
            var fibonacciActionMessages = Scenario
                .Messages
                .ForAction("**/fibonacci")
                .Calls();

            fibonacciActionMessages.Should().NotBeEmpty();

            foreach (var actionCall in fibonacciActionMessages)
            {
                var actionName = actionCall.ActionName;

                actionCall.GoalMessage.Topic.Should().StartWith(actionName);
                actionCall.ResultMessage?.Topic.Should().StartWith(actionName);
                actionCall.FeedbackMessages.Should().OnlyContain(x => x.Topic.StartsWith(actionName));
            }

            
            var firstNumbers = fibonacciActionMessages
                //.Calls()
                .Select(x => x.ResultMessage.Value.Result<FibonacciResult>()?.Sequence)
                .Where(x => x != null && x.Any())
                .Select(x => x.First());
            
            // Now there should be only one 0 value left
            foreach (var firstNumber in firstNumbers)
            {
                firstNumber.Should().Be(0);
            }
        }
        
        [ExpectThat]
        public void Fibonacci_is_a_valid_action()
        {
            Scenario.Messages.HasAction("**/fibonacci").Should().BeTrue();
        }
    }
}