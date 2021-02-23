using System;
using System.Collections.Generic;
using System.Linq;
using IntegROS.Ros.Actionlib;

namespace IntegROS
{
    public static class RecordedMessageActionExtensions
    {
        public static ActionCallCollection ForAction(this IEnumerable<IRecordedMessage> messages, string actionName)
        {
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            var actionMessages = FilterActionMessages(actionName, messages);
            
            return new ActionCallCollection(actionName, actionMessages);
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

        private static IEnumerable<IRecordedMessage> FilterActionMessages(string actionName, IEnumerable<IRecordedMessage> messages)
        {
            var actionFilter = actionName + "/*";
            var actionMessages = messages.Where(m => RecordedMessageExtensions.IsInTopic(m, actionFilter));
            return actionMessages;
        }
    }
}