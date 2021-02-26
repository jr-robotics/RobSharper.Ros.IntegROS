using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public interface ITimestampMessage
    {
        public DateTime TimeStamp { get; }
    }
    
    public interface IRecordedMessage : ITimestampMessage
    {
        public string Topic { get; }
        
        public RosType Type { get; }
        
        public DateTime TimeStamp { get; }

        public object GetMessage(Type type);
        public TType GetMessage<TType>();
    }

    public interface IRecordedMessage<out TMessage> : ITimestampMessage
    {
        public string Topic { get; }
        
        public RosType Type { get; }
        
        public DateTime TimeStamp { get; }
        
        TMessage Value { get; }
    }
}