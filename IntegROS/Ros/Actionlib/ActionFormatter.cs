using System;
using System.Collections.Generic;
using IntegROS.Ros.MessageEssentials;
using IntegROS.Ros.Messages;
using RobSharper.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace IntegROS.Ros.Actionlib
{
    public class ActionFormatter : IRosMessageFormatter
    {
        public RosMessageSerializer Serializer { get; }

        public ActionFormatter(RosMessageSerializer serializer)
        {
            Serializer = serializer;
        }
        
        public bool CanSerialize(IRosMessageTypeInfo typeInfo)
        {
            return typeInfo is IntegrosActionTypeInfo;
        }

        public void Serialize(SerializationContext context, RosBinaryWriter writer, IRosMessageTypeInfo messageTypeInfo, object o)
        {
            throw new NotSupportedException("This formatter only supports deserialization");
        }

        public object Deserialize(SerializationContext context, RosBinaryReader reader, IRosMessageTypeInfo messageTypeInfo)
        {
            var deserializer = Deserializers[messageTypeInfo.Type];
            return deserializer(context, reader, messageTypeInfo, Serializer);
        }

        private static readonly
            IDictionary<Type, Func<SerializationContext, RosBinaryReader, IRosMessageTypeInfo, RosMessageSerializer, object>> Deserializers =
                new Dictionary<Type, Func<SerializationContext, RosBinaryReader, IRosMessageTypeInfo, RosMessageSerializer, object>>()
                {
                    {typeof(ActionGoal), DeserializeActionGoal}
                };
        
        private static ActionGoal DeserializeActionGoal(SerializationContext context, RosBinaryReader reader, IRosMessageTypeInfo messageTypeInfo, RosMessageSerializer serializer)
        {
            var header = Deserialize<Header>(context, reader);
            var goalId = Deserialize<GoalID>(context, reader);
            var goalMessageDeserializer = PartialMessageDeserializer.CreateFromStream(context.Stream, serializer);
            
            return new ActionGoal(header, goalId, goalMessageDeserializer);
        }
        
        private static T Deserialize<T>(SerializationContext context, RosBinaryReader reader)
        {
            var typeInfo = context.MessageTypeRegistry.GetOrCreateMessageTypeInfo(typeof(T));
            var formatter = context.MessageFormatters.FindFormatterFor(typeInfo);

            if (formatter == null)
                throw new NotSupportedException($"No formatter for message {typeInfo} found.");

            var header = formatter.Deserialize(context, reader, typeInfo);
            return (T) header;
        }
    }
}