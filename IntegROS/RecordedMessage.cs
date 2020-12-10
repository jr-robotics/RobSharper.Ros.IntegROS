using System;

namespace IntegROS
{
    public class RecordedMessage
    {
        public TimeSpan Timestamp { get; }
        public string Topic { get; }
        public Type Type { get; }
        public object Data { get; }
    }
}