using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;

namespace RosComponentTesting.Util
{
    public class RosTestBuilder
    {
        public RosTestBuilder Publish(string advertiseTopic, object message)
        {
            return this;
        }

        public RosTestBuilder Expect<T>(string subscribeTopic, Func<T, bool> func)
        {
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