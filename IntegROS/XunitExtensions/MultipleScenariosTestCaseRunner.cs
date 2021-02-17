using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class MultipleScenariosTestCaseRunner : XunitTestCaseRunner
    {
        private readonly List<XunitTestRunner> _testRunners = new List<XunitTestRunner>();
        private Exception _scenarioDiscoveryException;

        /// <summary>
        /// Gets the message sink used to report <see cref="IDiagnosticMessage"/> messages.
        /// </summary>
        protected IMessageSink DiagnosticMessageSink { get; }
        
        public MultipleScenariosTestCaseRunner(MultipleScenariosTestCase testCase, string displayName, string skipReason,
            object[] constructorArguments, IMessageSink diagnosticMessageSink, IMessageBus messageBus,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(testCase,
            displayName, skipReason, constructorArguments, new object[0], messageBus, aggregator,
            cancellationTokenSource)
        {
            DiagnosticMessageSink = diagnosticMessageSink;
        }
        
        protected override async Task AfterTestCaseStartingAsync()
        {
            await base.AfterTestCaseStartingAsync();

            try
            {
                var scenarioAttributes = TestCase.TestMethod.GetScenarioAttributes(DiagnosticMessageSink);
                
                if (!scenarioAttributes.Any())
                {
                    throw new InvalidOperationException(
                        $"No scenario specified for {TestCase.TestMethod.TestClass.Class.Name}.{TestCase.TestMethod.Method.Name}. Make sure to add at least one ScenarioAttribute to the test method or class.");
                }
                
                foreach (var scenarioAttribute in scenarioAttributes)
                {
                    IScenarioIdentifier scenarioIdentifier = null;
                    var aggregator = new ExceptionAggregator(Aggregator);
                    var skipReason = scenarioAttribute.GetNamedArgument<string>("Skip");

                    try
                    {
                        var scenarioDiscoverer = ScenarioDiscovererFactory.GetDiscoverer(DiagnosticMessageSink, scenarioAttribute);
                        scenarioIdentifier = scenarioDiscoverer.GetScenarioIdentifier(scenarioAttribute);
                    }
                    catch (Exception exception)
                    {
                        scenarioIdentifier = new DummyScenarioDiscoverer().GetScenarioIdentifier(scenarioAttribute);
                        aggregator.Add(exception);
                    }

                    var test = CreateTest(TestCase, scenarioIdentifier);
                    var testRunner = new ScenarioTestRunner(test, DiagnosticMessageSink, MessageBus, TestClass, ConstructorArguments, TestMethod,
                        TestMethodArguments, skipReason, BeforeAfterAttributes, aggregator,
                        CancellationTokenSource);
                    
                    _testRunners.Add(testRunner);
                }
            }
            catch (Exception ex)
            {
                _scenarioDiscoveryException = ex;
            }
        }

        protected override ITest CreateTest(IXunitTestCase testCase, string displayName)
        {
            throw new NotSupportedException("Test can only be created with scenario");
        }
        
        protected virtual ScenarioTest CreateTest(IXunitTestCase testCase, IScenarioIdentifier scenarioIdentifier)
        {
            var displayName = $"{testCase.DisplayName} (Scenario: {scenarioIdentifier})";
            return new ScenarioTest((MultipleScenariosTestCase) testCase, scenarioIdentifier, displayName);
        }

        protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
            MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            throw new NotSupportedException("TestRunner collection is created in AfterTestCaseStartingAsync");
        }
        
        protected override async Task<RunSummary> RunTestAsync()
        {
            if (_scenarioDiscoveryException != null)
                return RunTestScenarioDiscoveryException();

            var runSummary = new RunSummary();
            foreach (var testRunner in _testRunners)
                runSummary.Aggregate(await testRunner.RunAsync());

            return runSummary;
        }

        private RunSummary RunTestScenarioDiscoveryException()
        {
            var test = new XunitTest(TestCase, DisplayName);

            if (!MessageBus.QueueMessage(new TestStarting(test)))
                CancellationTokenSource.Cancel();
            else if (!MessageBus.QueueMessage(new TestFailed(test, 0, null, _scenarioDiscoveryException.Unwrap())))
                CancellationTokenSource.Cancel();
            if (!MessageBus.QueueMessage(new TestFinished(test, 0, null)))
                CancellationTokenSource.Cancel();

            return new RunSummary { Total = 1, Failed = 1 };
        }
    }
}