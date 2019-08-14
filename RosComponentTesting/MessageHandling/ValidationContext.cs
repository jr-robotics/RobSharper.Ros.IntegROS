using System.Collections.Generic;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class ValidationContext
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        public bool IsValid => _errors.Count == 0;
        
        public IEnumerable<ValidationError> Errors => _errors.AsReadOnly();

        public void AddError(string errorMessage, CallerReference callerInfo = null)
        {
            var error = new ValidationError(errorMessage, callerInfo);            
            _errors.Add(error);
        }
    }
}