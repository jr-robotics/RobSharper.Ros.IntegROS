using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IntegROS;
using IntegROS.Ros.Actionlib;
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
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .Select(x => x.GoalMessage)
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();
            
            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_result_message_from_action_call()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .Select(x => x.ResultMessage)
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();
            
            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Can_get_all_feedback_messages_from_action_call()
        {
            var callFeedbackMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Calls
                .Select(x => x.FeedbackMessages)
                .ToList();

            callFeedbackMessages.Should().NotBeNull();
            callFeedbackMessages.Should().NotBeEmpty();

            foreach (var feedbackMessages in callFeedbackMessages)
            {
                feedbackMessages.Should().NotBeNull();
                feedbackMessages.Should().NotBeEmpty();
            
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

    public static class RecordedMessageActionExtensions
    {
        public static ActionCallCollection ForAction(this IEnumerable<IRecordedMessage> messages, string actionName)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            
            var actionFilter = actionName + "/*";
            var actionMessages = messages.Where(m => m.IsInTopic(actionFilter));

            return new ActionCallCollection(actionName, actionMessages);
        }
    }

    public class ActionCallCollection
    {
        private readonly IEnumerable<IRecordedMessage> _allActionMessages;
        public string ActionName { get; }

        public IEnumerable<IRecordedMessage> AllActionMessages => _allActionMessages;

        public IEnumerable<IRecordedMessage<GoalStatusArray>> StatusMessages
        {
            get
            {
                return _allActionMessages
                    .InTopic(ActionName + "/status")
                    .WithMessageType<GoalStatusArray>();
            }
        }
        
        public IEnumerable<IRecordedMessage<ActionGoal>> GoalMessages
        {
            get
            {
                return _allActionMessages
                    .InTopic(ActionName + "/goal")
                    .WithMessageType<ActionGoal>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionResult>> ResultMessages
        {
            get
            {
                return _allActionMessages
                    .InTopic(ActionName + "/result")
                    .WithMessageType<ActionResult>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionFeedback>> FeedbackMessages
        {
            get
            {
                return _allActionMessages
                    .InTopic(ActionName + "/feedback")
                    .WithMessageType<ActionFeedback>();
            }
        }

        public IEnumerable<IRecordedMessage<GoalID>> CancelMessages
        {
            get
            {
                return _allActionMessages
                    .InTopic(ActionName + "/cancel")
                    .WithMessageType<GoalID>();
            }
        }

        public IEnumerable<RosActionCall> Calls
        {
            get
            {
                var goals = GoalMessages.ToList();

                foreach (var goal in goals)
                {
                    yield return new RosActionCall(goal, this);
                }
            }
        }

        public ActionCallCollection(string actionName, IEnumerable<IRecordedMessage> allActionMessages)
        {
            ActionName = actionName;
            _allActionMessages = allActionMessages;
        }
    }

    public class RosActionCall
    {
        private readonly IRecordedMessage<ActionGoal> _goal;
        private IRecordedMessage<ActionResult> _result;
        private readonly ActionCallCollection _actionCallCollection;
        private GoalStatusValue? _finalState;
        private List<Tuple<DateTime,GoalStatusValue>> _statusChanges;

        public string ActionName => _actionCallCollection.ActionName;
        public string GoalId => _goal.Value.GoalId.Id;

        public GoalStatusValue FinalState
        {
            get
            {
                if (_finalState == null)
                {
                    _finalState = StatusChanges
                        .Last()
                        .Item2;
                }
                
                return _finalState.Value;
            }
        }

        public IRecordedMessage<ActionGoal> GoalMessage => _goal;

        public IRecordedMessage<ActionResult> ResultMessage
        {
            get
            {
                if (_result == null)
                {
                    _result = _actionCallCollection
                        .ResultMessages
                        .FirstOrDefault(x => x.Value.GoalStatus.GoalId.Id == GoalId);
                }

                return _result;
            }
        }

        public IEnumerable<IRecordedMessage<ActionFeedback>> FeedbackMessages
        {
            get
            {
                return _actionCallCollection
                    .FeedbackMessages
                    .Where(x => x.Value.GoalStatus.GoalId.Id == GoalId);
            }
        }
        
        public IReadOnlyCollection<Tuple<DateTime, GoalStatusValue>> StatusChanges
        {
            get
            {
                if (_statusChanges == null)
                {
                    // Select all published statuses of the goal:
                    // 1) Select all timestamps and status of the goal (or null if the status message has no entry for the goal)
                    // 2) Select only those items where the status is not null (item belongs to the goal)
                    // 3) Group by Status
                    // 4) Select the first element of each group

                    // ReSharper disable once PossibleInvalidOperationException
                    _statusChanges = _actionCallCollection.StatusMessages
                        .Select(x => new
                        {
                            Stamp = x.Value.Header.Stamp,
                            Status = x.Value.StatusList.FirstOrDefault((y => y.GoalId.Id == GoalId))?.Status
                        })
                        .Where(x => x.Status.HasValue)
                        .GroupBy(x => x.Status)
                        .Select(g => g.First())
                        .Select(x => new Tuple<DateTime, GoalStatusValue>(x.Stamp, x.Status.Value))
                        .ToList();
                }
                
                return _statusChanges;
            }
        }

        public RosActionCall(IRecordedMessage<ActionGoal> goal, ActionCallCollection actionCallCollection)
        {
            _goal = goal ?? throw new ArgumentNullException(nameof(goal));
            _actionCallCollection = actionCallCollection ?? throw new ArgumentNullException(nameof(actionCallCollection));
        }
    }
}