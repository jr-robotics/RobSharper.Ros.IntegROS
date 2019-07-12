using System;

namespace RosComponentTesting
{
    public class ExpectationBuilder<TTopicType>
    {
        private string _topicName;
        private Type _topicType;
        private Func<TTopicType, bool> _matchExpression;

        public ExpectationBuilder<TTopicType> Topic(string topicName)
        {
            _topicName = topicName;
            return this;
        }

        public ExpectationBuilder<TTopicType> Match(Func<TTopicType, bool> matchExpression)
        {
            _topicType = typeof(TTopicType);
            _matchExpression = matchExpression;

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