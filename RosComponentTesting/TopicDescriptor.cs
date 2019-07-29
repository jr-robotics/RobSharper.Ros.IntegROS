using System;

namespace RosComponentTesting
{
    public class TopicDescriptor
    {
        public string Topic { get; }
        public Type Type { get; }

        public TopicDescriptor(string topic, Type type)
        {
            Topic = topic;
            Type = type;
        }
    }
}