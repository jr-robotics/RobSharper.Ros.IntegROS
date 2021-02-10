using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestCaseRunner : XunitTestCaseRunner
    {
        public ScenarioTestCaseRunner(ScenarioTestCase scenarioTestCase, string displayName, string skipReason,
            object[] constructorArguments, IMessageSink diagnosticMessageSink, IMessageBus messageBus,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(scenarioTestCase, displayName, skipReason, constructorArguments, new object[0], messageBus,
                aggregator, cancellationTokenSource)
        {
        }


        protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
            MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            return new ScenarioTestRunner(test, messageBus, testClass, constructorArguments, testMethod,
                testMethodArguments, skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator),
                cancellationTokenSource);
        }
    }

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

            return testClass;
        }
    }
}