using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IntegROS.Scenarios;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestRunner : XunitTestRunner
    {
        public IMessageSink DiagnosticMessageSink { get; }

        public ScenarioTest ScenarioTest
        {
            get { return (ScenarioTest) Test; }
        }
        
        public IScenario Scenario { get; protected set; }

        public ScenarioTestRunner(ScenarioTest test, IMessageSink diagnosticMessageSink, IMessageBus messageBus,
            Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments,
            string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterTestAttributes,
            ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason,
                beforeAfterTestAttributes, exceptionAggregator, cancellationTokenSource)
        {
            DiagnosticMessageSink = diagnosticMessageSink;
        }

        protected override void AfterTestStarting()
        {
            base.AfterTestStarting();

            if (Aggregator.HasExceptions || SkipReason != null)
                return;
            
            try
            {
                var scenarioDiscoverer = ScenarioDiscovererFactory.GetDiscoverer(DiagnosticMessageSink, ScenarioTest.ScenarioIdentifier);
                Scenario = scenarioDiscoverer.GetScenario(ScenarioTest.ScenarioIdentifier);

                if (Scenario == null)
                {
                    throw new InvalidOperationException("Scenario is null (ScenarioDiscoverer returned no scenario).");
                }

                if (Scenario is IExecutableScenario executableScenario)
                {
                    Aggregator
                        .RunAsync(() => executableScenario.ExecuteAsync())
                        .Wait();
                }
            }
            catch (Exception e)
            {
                Aggregator.Add(e);
            }
        }

        protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            return new ScenarioTestInvoker(Test, MessageBus, TestClass, ConstructorArguments, Scenario, TestMethod,
                TestMethodArguments, BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
        }
    }
}