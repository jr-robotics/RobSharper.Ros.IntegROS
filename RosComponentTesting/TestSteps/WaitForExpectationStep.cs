using System;
using System.Collections.Generic;
using System.Text;
using RosComponentTesting.ExpectationProcessing;
using RosComponentTesting.TestFrameworks;

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

            if (!_expectation.IsValid)
            {
                var errorMessage = BuildErrorMessage(_expectation.GetValidationErrors());
                TestFrameworkProvider.Framework.Throw(errorMessage);
            }
        }

        public void Cancel()
        {
            Expectation.Cancel();
        }

        private static string BuildErrorMessage(IEnumerable<ValidationError> errors)
        {
            var m = new StringBuilder();

            m.AppendLine($"Wait for step failed");

            if (errors != null)
            {
                m.AppendLine();
                
                foreach (var error in errors)
                {
                    m.AppendLine(error.ToString());
                }
            }

            return m.ToString();
        }
        
    }
}