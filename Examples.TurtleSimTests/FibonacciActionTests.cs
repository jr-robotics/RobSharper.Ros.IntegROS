using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IntegROS;
using Messages.actionlib_msgs;

namespace Examples.TurtleSimTests
{
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
    public class FibonacciActionTests : ForScenario
    {
        [ExpectThat]
        public void All_actions_succeed()
        {
            Scenario.Messages
                .ForAction("/fibonacci")
                .Should()
                .OnlyContain(x => x.ResultState == ActionStatus.Succeeded);
        }
        
        [ExpectThat]
        public void Can_get_status_messages()
        {
            var statusMessages = Scenario.Messages
                .ForAction("/fibonacci")
                .StatusMessages
                .ToList();

            statusMessages.Should().NotBeNull();

            statusMessages.First().Value.Should().NotBeNull();
        }
    }

    public static class RecordedMessageActionExtensions
    {
        public static ActionCallCollection ForAction(this IEnumerable<IRecordedMessage> messages, string actionName)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            
            var actionFilter = actionName + "/*";
            // var goalTopic = actionName + "/goal";
            // var feedbackTopic = actionName + "/feedback";
            // var resultTopic = actionName + "/result";
            // var statusTopic = actionName + "/status";
            // var cancelTopic = actionName + "/cancel";
            
            var actionMessages = messages.Where(m => m.IsInTopic(actionFilter));

            return new ActionCallCollection(actionName, actionMessages);
        }
    }

    public class ActionCallCollection : IEnumerable<IRosActionCall>
    {
        private IEnumerable<IRecordedMessage> _actionMessages;
        public string ActionName { get; }

        public IEnumerable<IRecordedMessage<GoalStatusArray>> StatusMessages
        {
            get
            {
                return _actionMessages
                    .InTopic(ActionName + "/status")
                    .WithMessageType<GoalStatusArray>();
            }
        }
        
        public ActionCallCollection(string actionName, IEnumerable<IRecordedMessage> actionMessages)
        {
            ActionName = actionName;
            _actionMessages = actionMessages;
        }

        public IEnumerator<IRosActionCall> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IRosActionCall
    {
        ActionStatus ResultState { get; }
    }

    public enum ActionStatus
    {
        Succeeded
    }
}