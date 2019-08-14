using System;

namespace RosComponentTesting.TestFrameworks
{
    public class FallbackTestFramework : ITestFramework
    {
        public bool IsAvailable
        {
            get { return true; }
        }

        public void Throw(string errorMessage)
        {
            throw new ValidationException(errorMessage);
        }
    }
}