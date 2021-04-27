namespace RobSharper.Ros.IntegROS
{
    public interface INamespaceScopedTopicMessage : ITopicMessage
    {
        NamespacePattern NamespacePattern { get; }
    }
    
    public interface INamespaceScopedTopicMessage<out TMessage> : INamespaceScopedTopicMessage where TMessage : ITopicMessage
    {
        TMessage InnerMessage { get; }
    }
}