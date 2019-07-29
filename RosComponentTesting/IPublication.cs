namespace RosComponentTesting
{
    public interface IPublication
    {
        TopicDescriptor Topic { get; }
        
        object Message { get; }
    }
}