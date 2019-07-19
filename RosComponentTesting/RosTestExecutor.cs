using System;
using System.Collections.Generic;
using System.Linq;

namespace RosComponentTesting
{
    public class RosTestExecutor
    {
        private readonly IEnumerable<IExpectation> _expectations;

        public RosTestExecutor(IEnumerable<IExpectation> expectations)
        {
            if (expectations == null) throw new ArgumentNullException(nameof(expectations));

            _expectations = expectations;
        }

        public void Execute(TestExecutionOptions options = null)
        {
            if (options == null)
            {
                options = TestExecutionOptions.Default;
            }
            
            
            // TODO Register Publishers
            // TODO Register Subscribers
            // TODO Publish Messages
            
            
            throw new NotImplementedException();
        }
    }
}