using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegROS
{
    public class RecordedMessage
    {
        public TimeSpan Timestamp { get; }
        public string Topic { get; }
        
        public Type Type { get; }
        public object Data { get; }
    }

    public static class RecordedMessageExtensions
    {
        public static bool IsInTopic(this RecordedMessage message, string topicNamePattern)
        {
            if (topicNamePattern == null) throw new ArgumentNullException(nameof(topicNamePattern));

            if (message == null)
                return false;

            var regex = TopicRegx.Create(topicNamePattern);
            return regex.IsMatch(message.Topic);
        }
        
        public static IEnumerable<RecordedMessage> InTopic(this IEnumerable<RecordedMessage> messages, string topicNamePattern)
        {
            if (messages == null)
                return null;

            return messages.Where(m => m.IsInTopic(topicNamePattern));
        }
    }
}