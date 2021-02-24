using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IntegROS.Ros.Messages;
using RobSharper.Ros.MessageEssentials;

namespace IntegROS.Test.Expectations
{
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted)]
    public class ActionCallCollectionTests : ForScenario
    {
        [ExpectThat]
        public void Can_get_all_action_calls()
        {
            var allMessages = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_goal_message_from_action_call()
        {
            var fibonacciCalls = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
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
        public void Can_get_gaol_message_content()
        {
            var allActionGoals = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.GoalMessage.Value.Goal<FibonacciGoal>())
                .ToList();

            foreach (var goal in allActionGoals)
            {
                goal.Order.Should().BePositive();
            }
        }

        [ExpectThat]
        public void Can_get_result_message_from_action_call()
        {
            var fibonacciCalls = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
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
        public void Can_get_result_message_content()
        {
            var actionResults = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.ResultMessage.Value.Result<FibonacciResult>())
                .ToList();

            foreach (var result in actionResults)
            {
                result.Sequence.Should().BeInAscendingOrder();
            }
        }
        
        [ExpectThat]
        public void Can_get_all_feedback_messages_from_action_call()
        {
            var fibonacciCalls = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
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
                .ForActionCalls("/fibonacci")
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
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel, Skip = "Should not hold for canceled scenario")]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted, Skip = "Should not hold for preempted scenario")]
        [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted, Skip = "Should not hold for preempted scenario")]
        public void All_action_calls_succeed()
        {
            Scenario.Messages
                .ForActionCalls("/fibonacci")
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
                .ForActionCalls("/fibonacci")
                .Select(x => x.GoalId)
                .ToList();

            actionCallGoals.Should().BeEquivalentTo(actionGoals);
        }
    }

    [RosMessage("actionlib_tutorials/FibonacciGoal")]
    public class FibonacciGoal
    {
        [RosMessageField("int32", "order", 1)]
        public int Order { get; set; }
    }

    [RosMessage("actionlib_tutorials/FibonacciResult")]
    public class FibonacciResult
    {
        [RosMessageField("int32[]", "sequence", 1)]
        public IEnumerable<int> Sequence { get; set; }
    }
}