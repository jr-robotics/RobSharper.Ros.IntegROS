using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public class NamespaceScopedRecordedMessage : INamespaceScopedRecordedMessage
    {
        public NamespaceScope NamespaceScope { get; }
        public IRecordedMessage InnerMessage { get; }

        private NamespaceScopedRecordedMessage(IRecordedMessage recordedMessage, NamespaceScope namespaceScope)
        {
            NamespaceScope = namespaceScope ?? throw new ArgumentNullException(nameof(namespaceScope));
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
            return Create(recordedMessage, new NamespaceScope(namespaceScope));
        }

        public static NamespaceScopedRecordedMessage Create(IRecordedMessage recordedMessage, NamespaceScope namespaceScope)
        {
            if (recordedMessage == null)
                return null;

            if (recordedMessage is INamespaceScopedRecordedMessage namespaceScopedMessage)
            {
                namespaceScope = namespaceScopedMessage.NamespaceScope.Concat(namespaceScope);
                recordedMessage = namespaceScopedMessage.Unwrap();
            }
            
            return new NamespaceScopedRecordedMessage(recordedMessage, namespaceScope);
        }
    }
}