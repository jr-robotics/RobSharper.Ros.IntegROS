using System.Collections.Generic;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public class ValidationContext
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        public bool IsValid => _errors.Count == 0;
        
        public IEnumerable<ValidationError> Errors => _errors.AsReadOnly();

        public void AddError(string errorMessage)
        {
            AddError(errorMessage, null);
        }

        public void AddError(string errorMessage, CallerReference callerInfo)
        {
            var error = new ValidationError(errorMessage, callerInfo);            
            _errors.Add(error);
        }
    }
}