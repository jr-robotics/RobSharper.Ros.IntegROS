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

        private static readonly string[] ActionTopicNames = {
            "/status",
            "/cancel",
            "/goal",
            "/feedback",
            "/result"
        };
        
        public static bool HasAction(this IEnumerable<IRecordedMessage> messages, string actionNamePattern)
        {
            var actionMessages = FilterActionMessages(actionNamePattern, messages);
            
            if (!actionMessages.Any())
                return false;
            
            var topicNames = actionMessages
                .Select(x => x.Topic.Substring(x.Topic.LastIndexOf("/", StringComparison.InvariantCulture)))
                .Distinct()
                .ToList();
            
            if (topicNames.Count > 5)
                return false;

            return topicNames.All(x => ActionTopicNames.Contains(x));
        }

        private static IEnumerable<IRecordedMessage> FilterActionMessages(string actionNamePattern, IEnumerable<IRecordedMessage> messages)
        {
            if (actionNamePattern == null) throw new ArgumentNullException(nameof(actionNamePattern));

            // TODO: check name
            // 1) Topic name without namespace must not have a placeholder
            // 2) Topic name must not end with "/"
            
            
            actionNamePattern += "/*";
            
            var actionMessages = messages.Where(m => RecordedMessageTopicsExtensions.IsInTopic(m, actionNamePattern));
            return actionMessages;
        }
    }
}