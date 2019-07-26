using System;

namespace RosComponentTesting.TestFrameworks
{
    public class FallbackTestFramework : ITestFramework
    {
        public bool IsLoaded
        {
            get { return true; }
        }

        public void Throw(string errorMessage)
        {
            throw new ValidationException(errorMessage);
        }
    }
}