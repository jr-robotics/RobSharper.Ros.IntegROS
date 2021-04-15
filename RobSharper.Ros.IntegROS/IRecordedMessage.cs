using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public interface IRecordedMessage : ITimestampMessage
    {
        string Topic { get; }
        
        RosType Type { get; }

        object GetMessage(Type type);
        TType GetMessage<TType>();
    }

    public interface IRecordedMessage<out TMessage> : ITimestampMessage
    {
        string Topic { get; }
        
        RosType Type { get; }
        
        TMessage Value { get; }
    }
}