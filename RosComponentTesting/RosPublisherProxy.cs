using System;
using System.Reflection;
using Uml.Robotics.Ros;

namespace RosComponentTesting
{
    internal class RosPublisherProxy : IRosPublisher
    {

        public static RosPublisherProxy Create(TopicDescriptor topic, object rosPublisher)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (rosPublisher == null) throw new ArgumentNullException(nameof(rosPublisher));
            
            var publishMethod = typeof(Publisher<>)
                .MakeGenericType(topic.Type)
                .GetMethod("Publish");

            return new RosPublisherProxy(topic, publishMethod, rosPublisher);
        }


        private readonly MethodInfo _publishMethod;
        private readonly object _rosPublisher;

        public TopicDescriptor Topic { get; }

        private RosPublisherProxy(TopicDescriptor topic, MethodInfo publishMethod, object rosPublisher)
        {
            Topic = topic;
            _publishMethod = publishMethod;
            _rosPublisher = rosPublisher;
        }

        public void Publish(object msg)
        {
            _publishMethod.Invoke(_rosPublisher, new[] {msg});
        }
    }
}