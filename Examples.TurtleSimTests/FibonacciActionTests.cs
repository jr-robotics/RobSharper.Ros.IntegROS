using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IntegROS;

namespace Examples.TurtleSimTests
{
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
    }

    public static class RecordedMessageActionExtensions
    {
        public static IEnumerable<IRosActionCall> ForAction(this IEnumerable<IRecordedMessage> messages, string actionName)
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

        public IEnumerable<IRecordedMessage> StatusMessages
        {
            get
            {
                return _actionMessages.InTopic(ActionName + "/status");
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