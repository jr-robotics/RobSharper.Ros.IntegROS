using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionMessagesCollection : IEnumerable<IRecordedMessage>
    {
        private readonly Lazy<bool> _exists;
        
        private IEnumerable<IRecordedMessage> AllMessages { get; }
        
        public string ActionNamePattern { get; }

        public bool IsFullQualifiedActionNamePattern { get; }
        
        public bool Exists => _exists.Value;

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

        private ActionMessagesCollection(string actionNamePattern, IEnumerable<IRecordedMessage> messages)
        {
            ActionNamePattern = actionNamePattern;
            AllMessages = messages;

            IsFullQualifiedActionNamePattern = RosNameRegex.IsFullQualifiedPattern(actionNamePattern);
            
            _exists = new Lazy<bool>(ActionExists);
        }

        public IEnumerator<IRecordedMessage> GetEnumerator()
        {
            return AllMessages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private static readonly string[] ActionTopicNames = {
            "/status",
            "/cancel",
            "/goal",
            "/feedback",
            "/result"
        };
        
        private bool ActionExists()
        {
            if (!AllMessages.Any())
                return false;
            
            var topicNames = AllMessages
                .Select(x => x.Topic.Substring(x.Topic.LastIndexOf("/", StringComparison.InvariantCulture)))
                .Distinct()
                .ToList();
            
            if (topicNames.Count > 5)
                return false;

            return topicNames.All(x => ActionTopicNames.Contains(x));
        }
        
        public static ActionMessagesCollection Create(string actionNamePattern, IEnumerable<IRecordedMessage> messages)
        {
            if (actionNamePattern == null) throw new ArgumentNullException(nameof(actionNamePattern));
            if (messages == null) throw new ArgumentNullException(nameof(messages));
            
            actionNamePattern = actionNamePattern.Trim();
            
            if (string.Empty.Equals(actionNamePattern))
                throw new InvalidRosNamePatternException("ROS name pattern must not be empty", nameof(actionNamePattern));
            
            if (actionNamePattern.EndsWith("/"))
                throw new InvalidRosNamePatternException(
                    "ROS name pattern must not end with a namespace separator ('/')", nameof(actionNamePattern));

            
            var actionName = actionNamePattern.Substring(actionNamePattern.LastIndexOf("/", StringComparison.InvariantCulture) + 1);
            
            if (RosNameRegex.ContainsPlaceholders(actionName))
                throw new InvalidRosNamePatternException("ROS action name must not contain any placeholders", nameof(actionNamePattern));

            var actionTopicsPattern = actionNamePattern + "/*";
            var actionMessages = messages
                .InTopic(actionTopicsPattern);
            
            var actionMessagesCollection = new ActionMessagesCollection(actionNamePattern, actionMessages);
            return actionMessagesCollection;
        }
    }
}