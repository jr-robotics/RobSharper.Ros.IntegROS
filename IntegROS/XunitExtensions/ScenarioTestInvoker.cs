using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using IntegROS.Scenarios;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestInvoker : XunitTestInvoker
    {
        protected IScenario Scenario { get; }

        public ScenarioTestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
            IScenario scenario, MethodInfo testMethod, object[] testMethodArguments,
            IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                beforeAfterAttributes, aggregator, cancellationTokenSource)
        {
            Scenario = scenario;
        }

        protected override object CreateTestClass()
        {
            var testClass = base.CreateTestClass();
            
            if (testClass is ForScenario scenarioTestClass)
            {
                scenarioTestClass.Scenario = Scenario;
            }

            return testClass;
        }
    }
}