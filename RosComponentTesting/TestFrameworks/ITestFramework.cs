using System.Collections.Generic;

namespace RosComponentTesting.TestFrameworks
{
    public interface ITestFramework
    {
        bool IsAvailable { get; }

        void Throw(string errorMessage);
    }
}