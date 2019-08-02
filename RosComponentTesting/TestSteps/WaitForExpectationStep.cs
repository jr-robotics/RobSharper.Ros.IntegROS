using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RosComponentTesting.Debugging;
using RosComponentTesting.ExpectationProcessing;
using RosComponentTesting.TestFrameworks;

namespace RosComponentTesting.TestSteps
{
    public class WaitForExpectationStep<TTopic> : ITestStep, ITestStepExecutor, IExpectationStep
    {
        private CallerReference _caller;
        private readonly WaitForExpectation<TTopic> _expectation;
        
        public IExpectation Expectation => _expectation;

        public WaitForExpectationStep(WaitForExpectation<TTopic> expectation) : this(expectation, null)
        {
        }

        public WaitForExpectationStep(WaitForExpectation<TTopic> expectation, CallerReference callerInfo)
        {
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));

            _expectation = expectation;
            _caller = callerInfo;
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

        private static string BuildErrorMessage(IEnumerable<ValidationError> errors)
        {
            var m = new StringBuilder();

            m.AppendLine($"Wait for step failed");
            m.AppendLine();

            if (errors != null)
            {
                foreach (var error in errors)
                {
                    m.AppendLine(error.ToString());
                }
            }

            return m.ToString();
        }
        
    }
}