using System;

namespace RosComponentTesting
{
    public class RosTestBuilder
    {
        public RosTestBuilder Publish(string advertiseTopic, object message)
        {
            return this;
        }

        public RosTestBuilder Expect<T>(string subscribeTopic, Func<T, bool> func)
        {
            return Expect(subscribeTopic, new Match<T>(func));
        }

        public RosTestBuilder Expect<T>(string subscriberTopic, Match<T> match)
        {
            // TODO Expect
            return this;
        }

        public void Execute()
        {
            // TODO Register Publishers
            // TODO Register Subscribers
            // TODO Publish Messages
            return;
        }

        public RosTestBuilder Expect<T>(Action<ExpectationBuilder<T>> builderAction)
        {
            var builder = new ExpectationBuilder<T>();
            builderAction(builder);
            
            // TODO: Apply builder

            return this;
        }
    }
}