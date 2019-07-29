using System.Collections.Generic;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public interface IExpectation
    {
        void Activate();
        void Deactivate();
        
        IEnumerable<ValidationError> GetValidationErrors();
    }
}