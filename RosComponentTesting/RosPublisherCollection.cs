using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RosComponentTesting
{
    public class RosPublisherCollection : IRosPublisherResolver
    {
        private readonly IDictionary<TopicDescriptor, IRosPublisher> _publishers = new ConcurrentDictionary<TopicDescriptor, IRosPublisher>();
        
        public IRosPublisher GetPublisherFor(TopicDescriptor topic)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            
            return _publishers[topic];
        }

        public void Add(TopicDescriptor topic, IRosPublisher publisher)
        {
            _publishers.Add(topic, publisher);
        }
    }
}