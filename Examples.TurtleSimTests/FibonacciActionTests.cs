using System.Linq;
using FluentAssertions;
using IntegROS;
using IntegROS.Ros.Messages;

namespace Examples.TurtleSimTests
{
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted)]
    public class FibonacciActionTests : ForScenario
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
        public void Can_get_all_action_calls()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_goal_message_from_action_call()
        {
            var fibonacciCalls = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .ToList();

            fibonacciCalls.Should().NotBeNull();
            fibonacciCalls.Should().NotBeEmpty();

            foreach (var fibonacciCall in fibonacciCalls)
            {
                fibonacciCall.Should().NotBeNull();
                
                fibonacciCall.GoalMessage.Should().NotBeNull();
                fibonacciCall.GoalMessage.Value.Should().NotBeNull();
                fibonacciCall.GoalMessage.Value.GoalId.Id.Should().BeEquivalentTo(fibonacciCall.GoalId);
            }
        }

        [ExpectThat]
        public void Can_get_result_message_from_action_call()
        {
            var fibonacciCalls = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .ToList();

            fibonacciCalls.Should().NotBeNull();
            fibonacciCalls.Should().NotBeEmpty();

            foreach (var fibonacciCall in fibonacciCalls)
            {
                fibonacciCall.Should().NotBeNull();
                
                fibonacciCall.ResultMessage.Should().NotBeNull();
                fibonacciCall.ResultMessage.Value.Should().NotBeNull();
                fibonacciCall.ResultMessage.Value.GoalStatus.GoalId.Id.Should().BeEquivalentTo(fibonacciCall.GoalId);
            }
        }

        [ExpectThat]
        public void Can_get_all_feedback_messages_from_action_call()
        {
            var fibonacciCalls = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .ToList();

            fibonacciCalls.Should().NotBeNull();
            fibonacciCalls.Should().NotBeEmpty();

            foreach (var fibonacciCall in fibonacciCalls)
            {
                fibonacciCall.Should().NotBeNull();

                var callGoalId = fibonacciCall.GoalId;
                var feedbackMessages = fibonacciCall
                    .FeedbackMessages
                    .ToList();
                
                feedbackMessages.Should().NotBeNull();
                feedbackMessages.Should().NotBeEmpty();

                feedbackMessages
                    .Select(x => x.Value.GoalStatus.GoalId.Id)
                    .Distinct()
                    .Should()
                    .BeEquivalentTo(callGoalId);
            
                feedbackMessages
                    .Select(x => x.Value)
                    .ToList()
                    .Should().NotBeEmpty();
            }
        }

        [ExpectThat]
        public void Can_get_all_status_changes_from_action_call()
        {
            var callStatusChanges = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .Select(x => x.StatusChanges)
                .ToList();

            callStatusChanges.Should().NotBeNull();
            callStatusChanges.Should().NotBeEmpty();

            foreach (var statusChanges in callStatusChanges)
            {
                statusChanges.Should().NotBeNull();
                statusChanges.Should().NotBeEmpty();

                statusChanges
                    .Select(x => x.Item1)
                    .ToList()
                    .Should().BeInAscendingOrder();
            }
        }

        [ExpectThat]
        public void Fibonacci_is_a_valid_action()
        {
            Messages.HasAction("/fibonacci").Should().BeTrue();
        }
        
        [ExpectThat]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel, Skip = "Should not hold for canceled scenario")]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted, Skip = "Should not hold for preempted scenario")]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted, Skip = "Should not hold for preempted scenario")]
        public void All_actions_succeed()
        {
            Scenario.Messages
                .ForAction("/fibonacci")
                .Calls
                .ToList()
                .Should()
                .OnlyContain(x => x.FinalState == GoalStatusValue.Succeeded);
        }

        [ExpectThat]
        public void For_every_goal_message_an_action_call_object_exists()
        {
            var actionGoals = Messages
                .ForAction("/fibonacci")
                .GoalMessages
                .Select(x => x.Value.GoalId.Id)
                .ToList();

            var actionCallGoals = Messages
                .ForAction("/fibonacci")
                .Calls
                .Select(x => x.GoalId)
                .ToList();

            actionCallGoals.Should().BeEquivalentTo(actionGoals);
        }
    }
}