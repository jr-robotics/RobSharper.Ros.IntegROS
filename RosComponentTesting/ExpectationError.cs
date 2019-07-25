using System;

namespace RosComponentTesting
{
    public class ExpectationError
    {
        public IExpectation Expectation { get; }
        public Exception Exception { get; }

        public ExpectationError(IExpectation expectation, Exception exception)
        {
            Expectation = expectation;
            Exception = exception;
        }
    }
}