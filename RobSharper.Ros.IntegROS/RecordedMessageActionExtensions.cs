using System;
using System.Collections.Generic;
using RobSharper.Ros.IntegROS.Ros.Actionlib;

namespace RobSharper.Ros.IntegROS
{
    public static class RecordedMessageActionExtensions
    {
        public static ActionMessagesCollection ForAction(this IEnumerable<IRecordedMessage> messages, string actionNamePattern)
        {
            return ActionMessagesCollection.Create(actionNamePattern, messages);
        }

        public static ActionCallCollection Calls(this ActionMessagesCollection actionMessages)
        {
            return new ActionCallCollection(actionMessages);
        }

        public static ActionCallCollection ForActionCalls(this IEnumerable<IRecordedMessage> messages,
            string actionNamePattern)
        {
            return messages
                .ForAction(actionNamePattern)
                .Calls();
        }
        
        public static bool HasAction(this IEnumerable<IRecordedMessage> messages, string actionNamePattern)
        {
            if (actionNamePattern == null) throw new ArgumentNullException(nameof(actionNamePattern));
            
            if (messages is ActionMessagesCollection action && action.ActionNamePattern.Equals(actionNamePattern))
            {
                return action.Exists;
            }
            else
            {
                return ActionMessagesCollection.Create(actionNamePattern, messages).Exists;
            }
        }
    }
}