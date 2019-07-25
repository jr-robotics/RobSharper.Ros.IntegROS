using System;

namespace RosComponentTesting.TestFrameworks
{
    public interface ITestFramework
    {
        bool IsLoaded { get; }
        
        bool IsTestException(Exception e);
        
        void Throw(string errorMessage);
    }
}