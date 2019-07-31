using System;

namespace RosComponentTesting.TestSteps
{
    public class WaitForExpectationStep<TTopic> : ITestStep, ITestStepExecutor, IExpectationStep
    {
        private readonly WaitForExpectation<TTopic> _expectation;
        public IExpectation Expectation => _expectation;

        public WaitForExpectationStep(WaitForExpectation<TTopic> expectation)
        {
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));

            _expectation = expectation;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            _expectation.Activate();
            _expectation.WaitHandle.WaitOne();
            _expectation.Deactivate();
        }
    }
}