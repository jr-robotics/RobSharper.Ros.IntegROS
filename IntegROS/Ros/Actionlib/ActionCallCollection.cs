using System.Collections.Generic;
using System.Linq;
using IntegROS;
using IntegROS.Ros.Messages;

namespace IntegROS.Ros.Actionlib
{
    public class ActionCallCollection
    {
        private readonly IEnumerable<IRecordedMessage> _allActionMessages;
        public string ActionName { get; }

        public IEnumerable<IRecordedMessage> AllActionMessages => _allActionMessages;

        public IEnumerable<IRecordedMessage<GoalStatusArray>> StatusMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(_allActionMessages, ActionName + "/status")
                    .WithMessageType<GoalStatusArray>();
            }
        }
        
        public IEnumerable<IRecordedMessage<ActionGoal>> GoalMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(_allActionMessages, ActionName + "/goal")
                    .WithMessageType<ActionGoal>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionResult>> ResultMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(_allActionMessages, ActionName + "/result")
                    .WithMessageType<ActionResult>();
            }
        }

        public IEnumerable<IRecordedMessage<ActionFeedback>> FeedbackMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(_allActionMessages, ActionName + "/feedback")
                    .WithMessageType<ActionFeedback>();
            }
        }

        public IEnumerable<IRecordedMessage<GoalID>> CancelMessages
        {
            get
            {
                return RecordedMessageExtensions.InTopic(_allActionMessages, ActionName + "/cancel")
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
}