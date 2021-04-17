using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public interface IRecordedMessage : ITopicMessage, ITimestampMessage
    {
        object GetMessage(Type type);
        
        TType GetMessage<TType>();
    }

    public interface IRecordedMessage<out TMessage> : ITopicMessage, ITimestampMessage
    {
        TMessage Value { get; }
    }
}