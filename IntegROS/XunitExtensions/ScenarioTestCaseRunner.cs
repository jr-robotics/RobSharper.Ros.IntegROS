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
                var mstc = (MultipleScenariosTestCase)TestCase;
                var methodScenarioAttributes = TestCase.TestMethod.Method.GetCustomAttributes(typeof(ScenarioAttribute));
                var classScenarioAttributes = TestCase.TestMethod.TestClass.Class.GetCustomAttributes(typeof(ScenarioAttribute));

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
                            Aggregator.Add(new InvalidOperationException($"Scenario was null for {TestCase.TestMethod.TestClass.Class.Name}.{TestCase.TestMethod.Method.Name}."));
                        }
                    }

                    var scenarioTestCase = new ScenarioTestCase(DiagnosticMessageSink, mstc.DefaultMethodDisplay,
                        mstc.DefaultMethodDisplayOptions, TestCase.TestMethod, scenarioIdentifier);
                    
                    var test = CreateTest(scenarioTestCase, DisplayName);
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

    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Unwraps an exception to remove any wrappers, like <see cref="TargetInvocationException"/>.
        /// </summary>
        /// <param name="ex">The exception to unwrap.</param>
        /// <returns>The unwrapped exception.</returns>
        public static Exception Unwrap(this Exception ex)
        {
            while (true)
            {
                var tiex = ex as TargetInvocationException;
                if (tiex == null)
                    return ex;

                ex = tiex.InnerException;
            }
        }
    }
    
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