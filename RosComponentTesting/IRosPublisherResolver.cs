namespace RosComponentTesting
{
    public interface IRosPublisherResolver
    {
        IRosPublisher GetPublisherFor(TopicDescriptor topic);
    }
}