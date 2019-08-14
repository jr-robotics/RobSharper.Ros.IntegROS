using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class ValidationError
    {
        public string Message { get; }
        
        public CallerReference Caller { get; }
        
        public ValidationError(string errorMessage, CallerReference caller)
        {
            if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));

            Message = RemoveTrailingNewLine(errorMessage);
            Caller = caller;
        }

        public override string ToString()
        {
            if (Caller == null)
            {
                return Message;
            }
            else
            {
                return $"{Message}{Environment.NewLine}  in {Caller}{Environment.NewLine}";
            }
        }

        private static string RemoveTrailingNewLine(string errorMessage)
        {
            return errorMessage.EndsWith(Environment.NewLine)
                ? errorMessage.Substring(0, errorMessage.Length - Environment.NewLine.Length)
                : errorMessage;
        }
    }
}