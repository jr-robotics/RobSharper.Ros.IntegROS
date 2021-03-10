using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public interface ITimestampMessage
    {
        DateTime TimeStamp { get; }
    }
    
    public interface IRecordedMessage : ITimestampMessage
    {
        string Topic { get; }
        
        RosType Type { get; }
        
        DateTime TimeStamp { get; }

        object GetMessage(Type type);
        TType GetMessage<TType>();
    }

    public interface IRecordedMessage<out TMessage> : ITimestampMessage
    {
        string Topic { get; }
        
        RosType Type { get; }
        
        DateTime TimeStamp { get; }
        
        TMessage Value { get; }
    }
}