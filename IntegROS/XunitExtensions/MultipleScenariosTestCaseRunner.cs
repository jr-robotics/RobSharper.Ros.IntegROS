using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IntegROS.Scenarios;
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
                var methodScenarioAttributes = TestCase.TestMethod.Method.GetCustomAttributes(typeof(ScenarioAttribute));
                var classScenarioAttributes = TestCase.TestMethod.TestClass.Class.GetCustomAttributes(typeof(ScenarioAttribute));

                if (!methodScenarioAttributes.Any() && !classScenarioAttributes.Any())
                {
                    throw new InvalidOperationException(
                        $"No scenario specified for {TestCase.TestMethod.TestClass.Class.Name}.{TestCase.TestMethod.Method.Name}. Make sure to add at least one ScenarioAttribute to the test method or class.");
                }
                
                foreach (var methodScenarioAttribute in methodScenarioAttributes)
                {
                    var skipReason = methodScenarioAttribute.GetNamedArgument<string>("Skip");
                    var scenarioDiscoverer = ScenarioDiscovererFactory.GetDiscoverer(DiagnosticMessageSink, methodScenarioAttribute);
                    var scenarioIdentifier = scenarioDiscoverer.GetScenarioIdentifier(methodScenarioAttribute);
                    IScenario scenario = null;

                    if (skipReason != null)
                    {
                        scenario = scenarioDiscoverer.GetScenario(scenarioIdentifier);

                        if (scenario == null)
                        {
                            // Aggregator errors are ignored... maybe handle this issue in test runner?
                            Aggregator.Add(new InvalidOperationException($"Scenario was null for {TestCase.TestMethod.TestClass.Class.Name}.{TestCase.TestMethod.Method.Name}."));
                        }
                    }

                    var test = CreateTest(TestCase, scenarioIdentifier);
                    var testRunner = new ScenarioTestRunner(test, MessageBus, TestClass, ConstructorArguments, TestMethod,
                        TestMethodArguments, skipReason, BeforeAfterAttributes, new ExceptionAggregator(Aggregator),
                        CancellationTokenSource);
                    
                    _testRunners.Add(testRunner);
                }
                
                foreach (var classScenarioAttribute in classScenarioAttributes)
                {
                    // TODO: Do the same for class attributes
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
        
        protected virtual ITest CreateTest(IXunitTestCase testCase, IScenarioIdentifier scenarioIdentifier)
        {
            var displayName = testCase.DisplayName + $"(scenario: \"{scenarioIdentifier}\")";
            return new ScenarioTest(testCase, displayName, scenarioIdentifier);
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

        RunSummary RunTestScenarioDiscoveryException()
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