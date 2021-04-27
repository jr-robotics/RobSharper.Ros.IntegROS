using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public class NamespaceScopedRecordedMessage
    {
        class NamespaceScopedRecordedMessageWrapper : IRecordedMessage, INamespaceScopedTopicMessage<IRecordedMessage>
        {
            public NamespacePattern NamespacePattern { get; }
            public IRecordedMessage InnerMessage { get; }

            public NamespaceScopedRecordedMessageWrapper(IRecordedMessage recordedMessage, NamespacePattern namespacePattern)
            {
                NamespacePattern = namespacePattern ?? throw new ArgumentNullException(nameof(namespacePattern));
                InnerMessage = recordedMessage ?? throw new ArgumentNullException(nameof(recordedMessage));
            }

            public DateTime TimeStamp => InnerMessage.TimeStamp;
            public string Topic => InnerMessage.Topic;
            public RosType Type => InnerMessage.Type;
            public object GetMessage(Type type)
            {
                return InnerMessage.GetMessage(type);
            }

            public TType GetMessage<TType>()
            {
                return InnerMessage.GetMessage<TType>();
            }
        }
        

        class NamespaceScopedRecordedMessageWrapper<TType> : IRecordedMessage<TType>, INamespaceScopedTopicMessage<IRecordedMessage<TType>>
        {
            public NamespacePattern NamespacePattern { get; }
            public IRecordedMessage<TType> InnerMessage { get; }

            public NamespaceScopedRecordedMessageWrapper(IRecordedMessage<TType> recordedMessage, NamespacePattern namespacePattern)
            {
                NamespacePattern = namespacePattern ?? throw new ArgumentNullException(nameof(namespacePattern));
                InnerMessage = recordedMessage ?? throw new ArgumentNullException(nameof(recordedMessage));
            }

            public DateTime TimeStamp => InnerMessage.TimeStamp;
            public string Topic => InnerMessage.Topic;
            public RosType Type => InnerMessage.Type;
            public TType Value => InnerMessage.Value;
        }

        
        public static IRecordedMessage Create(IRecordedMessage recordedMessage,
            string namespaceScope)
        {
            return Create(recordedMessage, new NamespacePattern(namespaceScope));
        }

        public static IRecordedMessage Create(IRecordedMessage recordedMessage, NamespacePattern namespacePattern)
        {
            if (recordedMessage == null)
                return null;

            if (recordedMessage is INamespaceScopedTopicMessage<IRecordedMessage> namespaceScopedMessage)
            {
                namespacePattern = namespaceScopedMessage.NamespacePattern.Concat(namespacePattern);
                recordedMessage = namespaceScopedMessage.InnerMessage;
            }
            
            return new NamespaceScopedRecordedMessageWrapper(recordedMessage, namespacePattern);
        }
        
        public static IRecordedMessage<TType> Create<TType>(IRecordedMessage<TType> recordedMessage,
            string namespaceScope)
        {
            return Create(recordedMessage, new NamespacePattern(namespaceScope));
        }

        public static IRecordedMessage<TType> Create<TType>(IRecordedMessage<TType> recordedMessage, NamespacePattern namespacePattern)
        {
            if (recordedMessage == null)
                return null;

            if (recordedMessage is INamespaceScopedTopicMessage<IRecordedMessage<TType>> namespaceScopedMessage)
            {
                namespacePattern = namespaceScopedMessage.NamespacePattern.Concat(namespacePattern);
                recordedMessage = namespaceScopedMessage.InnerMessage;
            }
            
            return new NamespaceScopedRecordedMessageWrapper<TType>(recordedMessage, namespacePattern);
        }
    }
}