using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RosComponentTesting
{
    public class RosTestBuilder
    {
        private readonly ICollection<IExpectation> _expectations = new List<IExpectation>();

        public RosTestBuilder Publish(string advertiseTopic, object message)
        {
            // TODO add publisher
            return this;
        }

        public RosTestBuilder Expect<T>(string subscribeTopic, Func<T, bool> func, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Expect(subscribeTopic, new Match<T>(func), callerFilePath, lineNumber);
        }

        public RosTestBuilder Expect<T>(string subscriberTopic, Match<T> match, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Expect<T>(x => x
                .Topic(subscriberTopic)
                .Match(match), 
                callerFilePath, lineNumber);
        }

        public RosTestBuilder Expect<T>(Action<ExpectationBuilder<T>> builderAction, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var builder = new ExpectationBuilder<T>()
                .Occurrences(Times.AtLeast(1), callerFilePath, lineNumber);
            
            builderAction(builder);

            return Expect(builder.Expectation);
        }

        public RosTestBuilder Expect(IExpectation expectation)
        {
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));

            _expectations.Add(expectation);
            return this;
        }

        public RosTestExecutor ToTestExecutor()
        {
            if (!_expectations.Any()) throw new InvalidOperationException("No expectations defined");
            
            return new RosTestExecutor(_expectations);
        }
        
        public void Execute(TestExecutionOptions options = null)
        {
            ToTestExecutor().Execute(options);
        }
    }
}