using System;

namespace RosComponentTesting
{
    public class Publication : IPublication
    {
        public TopicDescriptor Topic { get; }
        public object Message { get; }

        public Publication(TopicDescriptor topic, object message)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (!message.GetType().IsAssignableFrom(topic.Type))
            {
                throw new InvalidOperationException($"Expected type {topic.Type} but found ${message.GetType()}");
            }

            Topic = topic;
            Message = message;
        }
    }
}