using System;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public interface ITopicExpectation : IExpectation
    {
        string TopicName { get; }
        
        Type TopicType { get; }

        void HandleMessage(object message);
    }
}