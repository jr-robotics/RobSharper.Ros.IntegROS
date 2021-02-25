using System;
using System.Runtime.Serialization;

namespace RobSharper.Ros.IntegROS
{
    public class InvalidTopicPatternException : ArgumentException
    {
        public InvalidTopicPatternException()
        {
        }

        protected InvalidTopicPatternException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidTopicPatternException(string message) : base(message)
        {
        }

        public InvalidTopicPatternException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidTopicPatternException(string message, string paramName) : base(message, paramName)
        {
        }

        public InvalidTopicPatternException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }
    }
}