using System;

namespace RosComponentTesting
{
    public interface IExpectation
    {
        
    }

    public interface ITopicExpectation : IExpectation
    {
        string TopicName { get; }
        Type TopicType { get; }

        void OnReceiveMessage(object message);
    }

    public interface ICallbackHandler<T>
    {
        void Callback(T item);
    }
}