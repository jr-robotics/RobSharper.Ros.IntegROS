using System;
using RobSharper.Ros.MessageEssentials;

namespace IntegROS
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
        
        TMessage Message { get; }
    }
}