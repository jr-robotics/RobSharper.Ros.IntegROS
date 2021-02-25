using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public interface IRecordedMessage
    {
        public string Topic { get; }
        
        public RosType Type { get; }

        public object GetMessage(Type type);
        public TType GetMessage<TType>();
    }

    public interface IRecordedMessage<out TMessage>
    {
        public string Topic { get; }
        
        public RosType Type { get; }
        
        TMessage Value { get; }
    }
}