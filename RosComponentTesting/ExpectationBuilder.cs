using System;
using System.Runtime.CompilerServices;
using RosComponentTesting.Debugging;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class ExpectationBuilder<TTopicType>
    {
        private readonly TopicExpectation<TTopicType> _expectation;

        public ExpectationBuilder()
        {
            _expectation = new TopicExpectation<TTopicType>();
        }
        
        public ExpectationBuilder(TopicExpectation<TTopicType> expectation)
        {
            _expectation = expectation;
        }

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

            var handler = new MatchMessageHandler<TTopicType>(match);
            _expectation.AddMessageHandler(handler, true);

            return this;
        }

        public ExpectationBuilder<TTopicType> Callback(Action<TTopicType> callback, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var handler = new CallbackMessageHandler<TTopicType>(callback, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler);
            
            return this;
        }

        public ExpectationBuilder<TTopicType> Timeout(TimeSpan timeout, TimeoutBehaviour behaviour = TimeoutBehaviour.ThrowValidationError, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var handler = new TimeoutMessageHandler<TTopicType>(timeout, behaviour, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, true);
            
            return this;
        }

        public ExpectationBuilder<TTopicType> Occurrences(Times times, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (times == null) throw new ArgumentNullException(nameof(times));

            var handler = new OccurrenceMessageHandler<TTopicType>(times, CallerReference.Create(callerFilePath, lineNumber));
            _expectation.AddMessageHandler(handler, true);
            
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

    public enum TimeoutBehaviour
    {
        ThrowValidationError,
        DisableRule
    }
}