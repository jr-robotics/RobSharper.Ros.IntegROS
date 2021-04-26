using System;
using System.Collections;
using System.Collections.Generic;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionMessagesCollection : IEnumerable<IRecordedMessage>
    {
        private IEnumerable<IRecordedMessage> AllMessages { get; }
        
        public string ActionNamePattern { get; }

        public bool Exists => AllMessages.HasAction(ActionNamePattern);

        public IEnumerable<IRecordedMessage<GoalStatusArray>> StatusMessages
        {
            get
            {
                return RecordedMessageTopicsExtensions.InTopic(AllMessages, ActionNamePattern + "/status")
                    .WithMessageType<GoalStatusArray>();
            }
        }
        
        public IEnumerable<IRecordedMessage<ActionGoal>> GoalMessages
        {
            get
            {
                return RecordedMessageTopicsExtensions.InTopic(AllMessages, ActionNamePattern + "/goal")
                    .WithMessageType<ActionGoal>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionResult>> ResultMessages
        {
            get
            {
                return RecordedMessageTopicsExtensions.InTopic(AllMessages, ActionNamePattern + "/result")
                    .WithMessageType<ActionResult>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionFeedback>> FeedbackMessages
        {
            get
            {
                return RecordedMessageTopicsExtensions.InTopic(AllMessages, ActionNamePattern + "/feedback")
                    .WithMessageType<ActionFeedback>();
            }
        }

        public IEnumerable<IRecordedMessage<GoalID>> CancelMessages
        {
            get
            {
                return RecordedMessageTopicsExtensions.InTopic(AllMessages, ActionNamePattern + "/cancel")
                    .WithMessageType<GoalID>();
            }
        }

        public ActionMessagesCollection(string actionNamePattern, IEnumerable<IRecordedMessage> messages)
        {
            ActionNamePattern = actionNamePattern ?? throw new ArgumentNullException(nameof(actionNamePattern));
            AllMessages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        public IEnumerator<IRecordedMessage> GetEnumerator()
        {
            return AllMessages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}