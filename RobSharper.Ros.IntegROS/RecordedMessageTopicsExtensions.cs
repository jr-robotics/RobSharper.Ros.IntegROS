using System;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.IntegROS
{
    public static class RecordedMessageTopicsExtensions
    {
        public static bool IsInTopic(this IRecordedMessage message, string topicNamePattern)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (topicNamePattern == null) throw new ArgumentNullException(nameof(topicNamePattern));

            var regex = TopicRegx.Create(topicNamePattern);
            return regex.IsMatch(message.Topic);
        }
        
        public static IEnumerable<IRecordedMessage> InTopic(this IEnumerable<IRecordedMessage> messages, string topicNamePattern)
        {
            return messages.Where(m => IsInTopic(m, topicNamePattern));
        }
        
        public static IRecordedMessage<TType> SetMessageType<TType>(this IRecordedMessage message) where TType : class
        {
            return new RecordedMessage<TType>(message);
        }

        public static IEnumerable<IRecordedMessage<TType>> WithMessageType<TType>(
            this IEnumerable<IRecordedMessage> messages) where TType : class
        {
            return messages.Select(m => m.SetMessageType<TType>());
        }
        
        public static IEnumerable<TType> SelectMessages<TType>(
            this IEnumerable<IRecordedMessage> messages) where TType : class
        {
            return messages.Select(m => m.SetMessageType<TType>().Value);
        }
    }
}