using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestInvoker : XunitTestInvoker
    {
        public ScenarioTestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource)
        {
        }

        protected override object CreateTestClass()
        {
            var testClass = base.CreateTestClass();
            
            // TODO: Set scenario
            if (testClass is ForNewScenario scenario)
            {
                var val = Interlocked.Increment(ref NextValue);

                if (val % 2 == 0)
                {
                    val *= -1;
                }
                
                scenario.Value = val;
                
            }

            return testClass;
        }
        
        private static int NextValue = 1;
    }
}