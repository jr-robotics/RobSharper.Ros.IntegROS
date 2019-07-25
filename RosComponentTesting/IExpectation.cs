using System;
using System.Collections.Generic;

namespace RosComponentTesting
{
    public interface IExpectation
    {
        void Activate();
        void Deactivate();
        
        IEnumerable<string> GetValidationErrors();
    }

    public interface ITopicExpectation : IExpectation
    {
        string TopicName { get; }
        Type TopicType { get; }

        void OnReceiveMessage(object message);
    }
}