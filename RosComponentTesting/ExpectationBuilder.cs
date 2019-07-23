using System;
using System.Runtime.CompilerServices;
using RosComponentTesting.Debugging;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class ExpectationBuilder<TTopicType>
    {
        private readonly TopicExpectation<TTopicType> _expectation = new TopicExpectation<TTopicType>();

        public TopicExpectation<TTopicType> Expectation => _expectation;

        public ExpectationBuilder<TTopicType> Topic(string topicName)
        {
            Expectation.TopicName = topicName;
            return this;
        }

        public ExpectationBuilder<TTopicType> Match(Func<TTopicType, bool> matchExpression)
        {
            return Match(new Match<TTopicType>(matchExpression));
        }

        public ExpectationBuilder<TTopicType> Match(Match<TTopicType> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            
            Expectation.TopicType = typeof(TTopicType);

            var validator = new MatchRule<TTopicType>(match);
            _expectation.AddExpectationRule(validator, true);

            return this;
        }

        public ExpectationBuilder<TTopicType> Callback(Action<TTopicType> func)
        {
            // TODO
            return this;
        }

        public ExpectationBuilder<TTopicType> Timeout(TimeSpan timeout)
        {
            var validator = new TimeoutRule<TTopicType>(timeout);
            _expectation.AddExpectationRule(validator, true);
            
            return this;
        }

        public ExpectationBuilder<TTopicType> Occurrences(Times times, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (times == null) throw new ArgumentNullException(nameof(times));

            var validator = new OccurrenceRule<TTopicType>(times, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddExpectationRule(validator, true);
            
            return this;
        }

        public ExpectationBuilder<TTopicType> Name(string expectationName)
        {
            // TODO
            return this;
        }

        public ExpectationBuilder<TTopicType> DependsOn(string referencedExpectationName)
        {
            // TODO
            return this;
        }
    }
}