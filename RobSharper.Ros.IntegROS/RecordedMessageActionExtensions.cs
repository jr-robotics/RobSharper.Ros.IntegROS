using System;
using System.Collections.Generic;
using System.Linq;
using RobSharper.Ros.IntegROS.Ros.Actionlib;

namespace RobSharper.Ros.IntegROS
{
    public static class RecordedMessageActionExtensions
    {
        public static ActionMessagesCollection ForAction(this IEnumerable<IRecordedMessage> messages, string actionNamePattern)
        {
            if (actionNamePattern == null) throw new ArgumentNullException(nameof(actionNamePattern));
            var actionMessages = FilterActionMessages(actionNamePattern, messages);
            
            return new ActionMessagesCollection(actionNamePattern, actionMessages);
        }

        public static ActionCallCollection Calls(this ActionMessagesCollection actionMessages)
        {
            return new ActionCallCollection(actionMessages);
        }

        public static ActionCallCollection ForActionCalls(this IEnumerable<IRecordedMessage> messages,
            string actionName)
        {
            return messages
                .ForAction(actionName)
                .Calls();
        }

        public static bool HasAction(this IEnumerable<IRecordedMessage> messages, string actionName)
        {
            var actionMessages = FilterActionMessages(actionName, messages);
            
            if (!actionMessages.Any())
                return false;
            
            var topicNames = actionMessages
                .Select(x => x.Topic)
                .Distinct()
                .ToList();
            
            // TODO: Support action name with patterns
            //   action name pattern can contain placeholders => topics.Count > 5 && expected topics is not as stated below
            if (topicNames.Count > 5)
                return false;

            string[] expectedTopics = {
                actionName + "/status",
                actionName + "/cancel",
                actionName + "/goal",
                actionName + "/feedback",
                actionName + "/result"
            };
            
            return topicNames.All(x => expectedTopics.Contains(x));
        }

        private static IEnumerable<IRecordedMessage> FilterActionMessages(string actionNamePattern, IEnumerable<IRecordedMessage> messages)
        {
            var actionFilter = actionNamePattern + "/*";
            var actionMessages = messages.Where(m => RecordedMessageTopicsExtensions.IsInTopic(m, actionFilter));
            return actionMessages;
        }
    }
}