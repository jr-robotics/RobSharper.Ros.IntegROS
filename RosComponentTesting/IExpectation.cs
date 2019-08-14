using System.Collections.Generic;
using RosComponentTesting.MessageHandling;

namespace RosComponentTesting
{
    public interface IExpectation
    {
        bool IsActive { get; }
        void Activate();
        void Deactivate();
        
        bool IsValid { get; }
        IEnumerable<ValidationError> GetValidationErrors();

        void Cancel();
    }
}