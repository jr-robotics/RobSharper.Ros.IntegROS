﻿using System;
using System.Collections.Generic;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionMessages
    {
        public string ActionName { get; }

        public bool Exists => AllMessages.HasAction(ActionName);
        
        public IEnumerable<IRecordedMessage> AllMessages { get; }

        public IEnumerable<IRecordedMessage<GoalStatusArray>> StatusMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(AllMessages, ActionName + "/status")
                    .WithMessageType<GoalStatusArray>();
            }
        }
        
        public IEnumerable<IRecordedMessage<ActionGoal>> GoalMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(AllMessages, ActionName + "/goal")
                    .WithMessageType<ActionGoal>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionResult>> ResultMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(AllMessages, ActionName + "/result")
                    .WithMessageType<ActionResult>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionFeedback>> FeedbackMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(AllMessages, ActionName + "/feedback")
                    .WithMessageType<ActionFeedback>();
            }
        }

        public IEnumerable<IRecordedMessage<GoalID>> CancelMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(AllMessages, ActionName + "/cancel")
                    .WithMessageType<GoalID>();
            }
        }

        public ActionMessages(string actionName, IEnumerable<IRecordedMessage> messages)
        {
            ActionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            AllMessages = messages ?? throw new ArgumentNullException(nameof(messages));
        }
    }
}