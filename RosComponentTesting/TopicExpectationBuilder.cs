using System;
using System.Runtime.CompilerServices;
using RosComponentTesting.Debugging;
using RosComponentTesting.MessageHandling;

namespace RosComponentTesting
{
    public class TopicExpectationBuilder<TTopicType>
    {
        private readonly TopicExpectation<TTopicType> _expectation;

        public TopicExpectationBuilder()
        {
            _expectation = new TopicExpectation<TTopicType>();
        }

        public TopicExpectation<TTopicType> Expectation => _expectation;

        public TopicExpectationBuilder<TTopicType> Topic(string topicName)
        {
            Expectation.TopicName = topicName;
            return this;
        }

        public TopicExpectationBuilder<TTopicType> Match(Func<TTopicType, bool> matchExpression)
        {
            return Match(new Match<TTopicType>(matchExpression));
        }

        public TopicExpectationBuilder<TTopicType> Match(Match<TTopicType> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            
            Expectation.TopicType = typeof(TTopicType);

            var handler = new MatchMessageHandler<TTopicType>(match);
            _expectation.AddMessageHandler(handler, true);

            return this;
        }

        public TopicExpectationBuilder<TTopicType> Callback(Action<TTopicType> callback, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var handler = new CallbackMessageHandler<TTopicType>(callback, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler);
            
            return this;
        }

        public TopicExpectationBuilder<TTopicType> Timeout(TimeSpan timeout, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var handler = new TimeoutMessageHandler<TTopicType>(timeout);
            _expectation.AddMessageHandler(handler, true);
            
            return this;
        }

        public TopicExpectationBuilder<TTopicType> Occurrences(Times times, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (times == null) throw new ArgumentNullException(nameof(times));

            var handler = new OccurrenceMessageHandler<TTopicType>(times, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, true);
            
            return this;
        }

        public TopicExpectationBuilder<TTopicType> Name(string expectationName)
        {
            // TODO
            return this;
        }

        public TopicExpectationBuilder<TTopicType> DependsOn(string referencedExpectationName)
        {
            // TODO
            return this;
        }
    }
}