using System.Collections.Generic;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public interface IExpectation
    {
        bool Active { get; }
        
        void Activate();
        void Deactivate();
        
        bool IsValid { get; }

        IEnumerable<ValidationError> GetValidationErrors();

        void Cancel();
    }
}