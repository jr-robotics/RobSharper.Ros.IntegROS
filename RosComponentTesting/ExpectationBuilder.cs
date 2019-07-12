using System;

namespace RosComponentTesting
{
    public class ExpectationBuilder<TTopicType>
    {
        private string _topicName;
        private Type _topicType;
        private Match<TTopicType> _matchExpression;

        public ExpectationBuilder<TTopicType> Topic(string topicName)
        {
            _topicName = topicName;
            return this;
        }

        public ExpectationBuilder<TTopicType> Match(Func<TTopicType, bool> matchExpression)
        {
            return Match(new Match<TTopicType>(matchExpression));
        }

        public ExpectationBuilder<TTopicType> Match(Match<TTopicType> match)
        {
            _topicType = typeof(TTopicType);
            _matchExpression = match;

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
    }
}