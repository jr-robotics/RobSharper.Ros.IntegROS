using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RosComponentTesting.TestFrameworks;

namespace RosComponentTesting
{
    public class ExpectationErrorHandler
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IEnumerable<IExpectation> _expectations;
        private readonly List<ExpectationError> _errors;

        public IEnumerable<ExpectationError> Errors => _errors.AsReadOnly();
        
        public bool HasErrors => _errors.Count > 0;

        public ExpectationErrorHandler(CancellationTokenSource cancellationTokenSource,
            IEnumerable<IExpectation> expectations)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _expectations = expectations;
            _errors = new List<ExpectationError>();
        }

        public void AddError(ExpectationError error)
        {
            _errors.Add(error);
        }

        public void Cancel()
        {
            foreach (var expectation in _expectations)
            {
                expectation.Deactivate();
            }
            
            _cancellationTokenSource.Cancel();
        }

        public void Throw()
        {
            var innerExceptions = _errors.Select(e => e.Exception).ToList();

            if (!innerExceptions.Any())
            {
                return;
            }

            if (innerExceptions.Count == 1)
            {
                throw innerExceptions[0];
            }
            
            throw new AggregateException("Execution was canceled", innerExceptions);
        }
    }
}