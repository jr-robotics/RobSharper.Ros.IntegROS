using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;

namespace RosComponentTesting.Util
{
    public class RosCommunicationBuilder
    {
        public RosCommunicationBuilder Publish(string advertiseTopic, object message)
        {
            return this;
        }

        public RosCommunicationBuilder Expect<T>(string subscribeTopic, Func<T, bool> func)
        {
            return this;
        }

        public void Execute()
        {
            // TODO
            return;
        }

        public RosCommunicationBuilder Expect<T>(Action<ExpectationBuilder<T>> builderAction)
        {
            var builder = new ExpectationBuilder<T>();
            builderAction(builder);
            
            // TODO: Apply builder

            return this;
        }
    }
}