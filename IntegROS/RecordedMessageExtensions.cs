using System;
using System.Collections.Generic;
using System.Linq;
using IntegROS.Rosbag;

namespace IntegROS
{
    public static class RecordedMessageExtensions
    {
        public static bool IsInTopic(this IRecordedMessage message, string topicNamePattern)
        {
            if (topicNamePattern == null) throw new ArgumentNullException(nameof(topicNamePattern));

            if (message == null)
                return false;

            var regex = TopicRegx.Create(topicNamePattern);
            return regex.IsMatch(message.Topic);
        }
        
        public static IEnumerable<IRecordedMessage> InTopic(this IEnumerable<IRecordedMessage> messages, string topicNamePattern)
        {
            return messages.Where(m => IsInTopic(m, topicNamePattern));
        }
        
        public static IRecordedMessage<TType> SetMessageType<TType>(this IRecordedMessage message) where TType : class
        {
            if (message is IRecordedMessage<TType>)
            {
                return (IRecordedMessage<TType>) message;
            }
            
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