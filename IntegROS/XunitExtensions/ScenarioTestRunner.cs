using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestRunner : XunitTestRunner
    {
        public ScenarioTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterTestAttributes, ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            :base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterTestAttributes, exceptionAggregator, cancellationTokenSource)
        {
        }

        protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            return new ScenarioTestInvoker(Test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
        }
    }
}