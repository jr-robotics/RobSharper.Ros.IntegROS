using System;

namespace RosComponentTesting
{
    public class TopicExpectation<TTopic> : ITopicExpectation
    { 
        public string TopicName { get; set; }
        public Type TopicType { get; set; }

        public Match<TTopic> Match { get; set; }
        
        
        public void OnReceiveMessage(object message)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ExpectationBuilder<TTopicType>
    {
        private TopicExpectation<TTopicType> _expectation = new TopicExpectation<TTopicType>();

        public ExpectationBuilder<TTopicType> Topic(string topicName)
        {
            _expectation.TopicName = topicName;
            return this;
        }

        public ExpectationBuilder<TTopicType> Match(Func<TTopicType, bool> matchExpression)
        {
            return Match(new Match<TTopicType>(matchExpression));
        }

        public ExpectationBuilder<TTopicType> Match(Match<TTopicType> match)
        {
            _expectation.TopicType = typeof(TTopicType);
            _expectation.Match = match;

            return this;
        }

        public ExpectationBuilder<TTopicType> Callback(Action<TTopicType> func)
        {
            // TODO
            return this;
        }

        public ExpectationBuilder<TTopicType> Timeout(TimeSpan fromSeconds)
        {
            // TODO
            return this;
        }

        public ExpectationBuilder<TTopicType> Occurrences(Times times)
        {
            // TODO
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

        public IExpectation ToExpectation()
        {
            return _expectation;
        }
    }
}