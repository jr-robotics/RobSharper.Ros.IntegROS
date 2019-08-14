using System.Collections.Generic;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting.TestFrameworks
{
    public interface ITestFramework
    {
        bool IsAvailable { get; }

        void Throw(string errorMessage);
    }
}