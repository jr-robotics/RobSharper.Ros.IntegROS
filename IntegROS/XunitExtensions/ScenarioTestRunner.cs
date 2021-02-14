using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IntegROS.Scenarios;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestRunner : XunitTestRunner
    {
        protected ScenarioTest ScenarioTest
        {
            get { return (ScenarioTest) Test; }
        }
        
        protected IScenario Scenario { get; set; }
        
        public ScenarioTestRunner(ScenarioTest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterTestAttributes, ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            :base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterTestAttributes, exceptionAggregator, cancellationTokenSource)
        {
        }

        protected override void AfterTestStarting()
        {
            base.AfterTestStarting();
         
            //TODO: Load Scenario
        }

        protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            return new ScenarioTestInvoker(Test, MessageBus, TestClass, ConstructorArguments, Scenario, TestMethod, TestMethodArguments, BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
        }
    }
}