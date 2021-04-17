namespace RobSharper.Ros.IntegROS
{
    public static class INamespaceScopedRecordedMessageExtensions
    {
        public static IRecordedMessage Unwrap(this INamespaceScopedRecordedMessage message)
        {
            if (message == null)
                return null;

            var unwrappedMessage = message.InnerMessage;

            while (unwrappedMessage is INamespaceScopedRecordedMessage unwrappedNamespaceMessage)
            {
                unwrappedMessage = unwrappedNamespaceMessage.InnerMessage;
            }

            return unwrappedMessage;
        }
    }
}