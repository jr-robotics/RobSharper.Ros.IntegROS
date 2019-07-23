using System;
using System.Collections.Generic;

namespace RosComponentTesting.ExpectationProcessing
{
    public class ValidationContext
    {
        private readonly List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors.AsReadOnly();

        public void AddError(string errorMessage)
        {
            if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));

            _errors.Add(errorMessage);
        }
    }
}