using System;
using System.Runtime.Serialization;

namespace RobSharper.Ros.IntegROS
{
    public class InvalidRosNamePatternException : ArgumentException
    {
        public InvalidRosNamePatternException()
        {
        }

        protected InvalidRosNamePatternException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidRosNamePatternException(string message) : base(message)
        {
        }

        public InvalidRosNamePatternException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidRosNamePatternException(string message, string paramName) : base(message, paramName)
        {
        }

        public InvalidRosNamePatternException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }
    }
}