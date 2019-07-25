using System.Collections.Generic;

namespace RosComponentTesting
{
    public interface IExpectation
    {
        void Activate();
        void Deactivate();
        
        IEnumerable<string> GetValidationErrors();
    }
}