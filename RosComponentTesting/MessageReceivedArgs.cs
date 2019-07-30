using System;

namespace RosComponentTesting
{
    public class MessageReceivedArgs : EventArgs
    {
        public object Message { get; }

        public MessageReceivedArgs(object message)
        {
            Message = message;
        }
    }
}