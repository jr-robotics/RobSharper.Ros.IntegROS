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

            var regex = RosNameRegex.Create(topicNamePattern);
            return regex.IsMatch(message.Topic);
        }
        
        public static IRecordedMessage<TType> SetMessageType<TType>(this IRecordedMessage message) where TType : class
        {
            if (message is INamespaceScopedTopicMessage<IRecordedMessage> namespaceScopedMessage)
            {
                var innerMessage = namespaceScopedMessage.InnerMessage;
                var typedMessage = new RecordedMessage<TType>(innerMessage);

                return NamespaceScopedRecordedMessage.Create(typedMessage, namespaceScopedMessage.NamespacePattern);
            }
            else
            {
                return new RecordedMessage<TType>(message);
            }
        }
        
        public static IEnumerable<IRecordedMessage> InTopic(this IEnumerable<IRecordedMessage> messages, string topicNamePattern)
        {
            if (RosNameRegex.IsGlobalPattern(topicNamePattern))
            {
                return InGlobalTopic(messages, topicNamePattern);
            }
            else
            {
                return InRelativeTopic(messages, topicNamePattern);
            }
        }

        private static IEnumerable<IRecordedMessage> InGlobalTopic(IEnumerable<IRecordedMessage> messages, string globalTopicPattern)
        {
            var regex = RosNameRegex.Create(globalTopicPattern);
            return messages.Where(m => regex.IsMatch(m.Topic));
        }

        private static IEnumerable<IRecordedMessage> InRelativeTopic(IEnumerable<IRecordedMessage> messages, string relativeTopicNamePattern)
        {
            using (var regexCache = new RosNameRegexCache())
            {
                foreach (var message in messages)
                {
                    var scopedMessage = message as INamespaceScopedTopicMessage<IRecordedMessage>;
                    if (scopedMessage == null)
                        throw new InvalidRosNamePatternException(
                            "Relative topic name patterns are only supported if messages were filtered by namespace before.");

                    var regex = regexCache.GetOrCreate(scopedMessage.NamespacePattern + "/" + relativeTopicNamePattern);

                    if (regex.IsMatch(message.Topic))
                        yield return message;
                }
            }
        }

        public static IEnumerable<IRecordedMessage<TType>> InTopic<TType>(
            this IEnumerable<IRecordedMessage> messages, string topicNamePattern) where TType : class
        {
            return messages
                .InTopic(topicNamePattern)
                .WithMessageType<TType>();
        }

        public static IEnumerable<IRecordedMessage<TType>> WithMessageType<TType>(
            this IEnumerable<IRecordedMessage> messages) where TType : class
        {
            return messages.Select(m => m.SetMessageType<TType>());
        }

        public static IEnumerable<TType> SelectMessages<TType>(
            this IEnumerable<IRecordedMessage<TType>> messages)
        {
            return messages.Select(m => m.Value);
        }
        
        public static IEnumerable<TType> SelectMessages<TType>(
            this IEnumerable<IRecordedMessage> messages) where TType : class
        {
            return messages.Select(m => m.SetMessageType<TType>().Value);
        }
    }
}