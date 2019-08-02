using System;
using System.Runtime.CompilerServices;
using RosComponentTesting.Debugging;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class WaitForExpectationBuilder<TTopicType>
    {
        private readonly WaitForExpectation<TTopicType> _expectation;

        public WaitForExpectationBuilder()
        {
            _expectation = new WaitForExpectation<TTopicType>();
        }

        public WaitForExpectation<TTopicType> Expectation => _expectation;

        public WaitForExpectationBuilder<TTopicType> Topic(string topicName)
        {
            Expectation.TopicName = topicName;
            return this;
        }

        public WaitForExpectationBuilder<TTopicType> Match(Func<TTopicType, bool> matchExpression)
        {
            return Match(new Match<TTopicType>(matchExpression));
        }

        public WaitForExpectationBuilder<TTopicType> Match(Match<TTopicType> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            
            Expectation.TopicType = typeof(TTopicType);

            var handler = new MatchMessageHandler<TTopicType>(match);
            _expectation.AddMessageHandler(handler, true);

            return this;
        }

        public WaitForExpectationBuilder<TTopicType> Timeout(TimeSpan timeout, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var handler = new ValidatingTimeoutMessageHandler<TTopicType>(timeout, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, true);
            
            return this;
        }

        public WaitForExpectationBuilder<TTopicType> Occurrences(Times times, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (times == null) throw new ArgumentNullException(nameof(times));
            if (times.Max < uint.MaxValue)
            {
                throw new ArgumentException("Upper bound for occurrences is not supported in a WaitFor step", nameof(times));
            }

            var handler = new OccurrenceMessageHandler<TTopicType>(times, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, true);
            
            return this;
        }
    }
}