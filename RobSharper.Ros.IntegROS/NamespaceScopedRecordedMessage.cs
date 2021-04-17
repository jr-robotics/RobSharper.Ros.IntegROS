using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public class NamespaceScopedRecordedMessage : INamespaceScopedRecordedMessage
    {
        public NamespacePattern NamespacePattern { get; }
        public IRecordedMessage InnerMessage { get; }

        private NamespaceScopedRecordedMessage(IRecordedMessage recordedMessage, NamespacePattern namespacePattern)
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

        public static NamespaceScopedRecordedMessage Create(IRecordedMessage recordedMessage,
            string namespaceScope)
        {
            return Create(recordedMessage, new NamespacePattern(namespaceScope));
        }

        public static NamespaceScopedRecordedMessage Create(IRecordedMessage recordedMessage, NamespacePattern namespacePattern)
        {
            if (recordedMessage == null)
                return null;

            if (recordedMessage is INamespaceScopedRecordedMessage namespaceScopedMessage)
            {
                namespacePattern = namespaceScopedMessage.NamespacePattern.Concat(namespacePattern);
                recordedMessage = namespaceScopedMessage.Unwrap();
            }
            
            return new NamespaceScopedRecordedMessage(recordedMessage, namespacePattern);
        }
    }
}