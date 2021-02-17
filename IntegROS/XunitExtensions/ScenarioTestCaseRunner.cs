using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestCaseRunner : XunitTestCaseRunner
    {
        public IMessageSink DiagnosticMessageSink { get; }
        public IScenarioIdentifier ScenarioIdentifier { get; }
        
        public ScenarioTestCaseRunner(ScenarioTestCase scenarioTestCase, string displayName, string skipReason,
            object[] constructorArguments, IMessageSink diagnosticMessageSink, IMessageBus messageBus,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(scenarioTestCase, displayName, skipReason, constructorArguments, new object[0], messageBus,
                aggregator, cancellationTokenSource)
        {
            DiagnosticMessageSink = diagnosticMessageSink;
            ScenarioIdentifier = scenarioTestCase.ScenarioIdentifier;
        }

        protected override ITest CreateTest(IXunitTestCase testCase, string displayName)
        {
            return new ScenarioTest((ScenarioTestCase) testCase, displayName);
        }

        protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
            MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            return new ScenarioTestRunner((ScenarioTest) test, DiagnosticMessageSink, messageBus, testClass, constructorArguments, testMethod,
                testMethodArguments, skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator),
                cancellationTokenSource);
        }
    }
}