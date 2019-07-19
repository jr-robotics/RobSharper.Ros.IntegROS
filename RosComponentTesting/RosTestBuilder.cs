using System;
using System.Collections.Generic;
using System.Linq;

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

        public RosTestBuilder Expect<T>(string subscribeTopic, Func<T, bool> func)
        {
            return Expect(subscribeTopic, new Match<T>(func));
        }

        public RosTestBuilder Expect<T>(string subscriberTopic, Match<T> match)
        {
            return Expect<T>(x => x
                .Topic(subscriberTopic)
                .Match(match));
        }

        public RosTestBuilder Expect<T>(Action<ExpectationBuilder<T>> builderAction)
        {
            var builder = new ExpectationBuilder<T>();
            builderAction(builder);

            return Expect(builder.ToExpectation());
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