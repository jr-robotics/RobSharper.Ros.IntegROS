using System;
using System.Collections.Generic;

namespace RosComponentTesting
{
    public class RosPublisherCollection : IRosPublisherResolver
    {
        private readonly Dictionary<TopicDescriptor, IRosPublisher> _publishers = new Dictionary<TopicDescriptor, IRosPublisher>();
        
        public IRosPublisher GetPublisherFor(TopicDescriptor topic)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            
            return _publishers[topic];
        }

        public void Add(TopicDescriptor topic, IRosPublisher publisher)
        {
            lock (_publishers)
            {
                _publishers.Add(topic, publisher);
            }
        }
    }
}