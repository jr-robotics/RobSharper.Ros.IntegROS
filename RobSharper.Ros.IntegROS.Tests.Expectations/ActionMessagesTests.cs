using System.Linq;
using FluentAssertions;

namespace RobSharper.Ros.IntegROS.Tests.Expectations
{
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted)]
    public class ActionMessagesTests : ForScenario
    {
        [ExpectThat]
        public void Can_get_action_name()
        {
            var actionName = Scenario
                .Messages
                .ForAction("/fibonacci")
                .ActionName;

            actionName.Should().NotBeNull();
            actionName.Should().Be("/fibonacci");
        }
        
        [ExpectThat]
        public void Can_get_all_status_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .StatusMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();

            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_all_goal_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .GoalMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();

            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_all_feedback_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .FeedbackMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();

            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_all_result_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .ResultMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();

            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_all_cancel_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .CancelMessages
                .ToList();

            allMessages.Should().NotBeNull();
            
            // Cancel may be empty
            if (allMessages.Any())
            {
                allMessages
                    .Select(x => x.Value)
                    .ToList()
                    .Should().NotBeEmpty();
            }
        }

        [ExpectThat]
        public void Fibonacci_is_a_valid_action()
        {
            Scenario.Messages.HasAction("/fibonacci").Should().BeTrue();
        }
    }
}