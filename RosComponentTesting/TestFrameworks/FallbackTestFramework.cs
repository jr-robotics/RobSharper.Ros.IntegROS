using System;

namespace RosComponentTesting.TestFrameworks
{
    internal class FallbackTestFramework : ITestFramework
    {
        public bool IsLoaded
        {
            get { return true; }
        }

        public bool IsTestException(Exception e)
        {
            return e is ValidationException;
        }

        public void Throw(string errorMessage)
        {
            throw new ValidationException(errorMessage);
        }
    }
}