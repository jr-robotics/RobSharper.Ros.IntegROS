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

        protected bool Equals(TopicDescriptor other)
        {
            return string.Equals(Topic, other.Topic) && Equals(Type, other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TopicDescriptor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Topic != null ? Topic.GetHashCode() : 0) * 397) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }
    }
}