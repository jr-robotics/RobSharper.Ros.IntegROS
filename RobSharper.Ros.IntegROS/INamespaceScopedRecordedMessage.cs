namespace RobSharper.Ros.IntegROS
{
    public interface INamespaceScopedRecordedMessage : IRecordedMessage
    {
        NamespacePattern NamespacePattern { get; }
        
        IRecordedMessage InnerMessage { get; }
    }
}