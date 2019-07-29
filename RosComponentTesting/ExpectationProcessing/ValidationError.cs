using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public class ValidationError
    {
        public string Message { get; }
        
        public CallerReference Caller { get; }
        
        public ValidationError(string errorMessage, CallerReference caller)
        {
            if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
            
            Message = errorMessage;
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
    }
}