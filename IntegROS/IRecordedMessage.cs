using System;

namespace IntegROS
{
    public interface IRecordedMessage
    {
        public string Topic { get; }

        public object GetMessage(Type type);
        public TType GetMessage<TType>();
    }

    public interface IRecordedMessage<out TMessage>
    {
        public string Topic { get; }
        
        TMessage Message { get; }
    }
}