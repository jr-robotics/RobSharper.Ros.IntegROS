using System;
using System.Runtime.CompilerServices;
using RosComponentTesting.Debugging;
using RosComponentTesting.MessageHandling;

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

            var handler = new MatchMessageHandler<TTopicType>(match);
            _expectation.AddMessageHandler(handler, 100, true);

            return this;
        }

        public WaitForExpectationBuilder<TTopicType> Timeout(TimeSpan timeout, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var handler = new ValidatingTimeoutMessageHandler<TTopicType>(timeout, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, 100,true);
            
            return this;
        }

        public WaitForExpectationBuilder<TTopicType> Occurrences(Times times, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (times == null) throw new ArgumentNullException(nameof(times));
            if (times.Min == 0)
            {
                throw new ArgumentException("Lower bound for occurrences must be defined in a WaitFor step", nameof(times));
            }

            var handler = new OccurrenceMessageHandler<TTopicType>(times, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, 50, true);
            
            return this;
        }

        public WaitForExpectationBuilder<TTopicType> Callback(Action<TTopicType> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var handler = new CallbackMessageHandler<TTopicType>(callback);
            _expectation.AddMessageHandler(handler, 50);
            
            return this;
        }

    }
}