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
            
            var actionFilter = actionName + "/*";
            var actionMessages = messages.Where(m => RecordedMessageExtensions.IsInTopic(m, actionFilter));

            return new ActionCallCollection(actionName, actionMessages);
        }
    }
}