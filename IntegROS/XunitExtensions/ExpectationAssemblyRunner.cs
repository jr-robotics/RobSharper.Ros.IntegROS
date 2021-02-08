using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationAssemblyRunner : TestAssemblyRunner<ExpectationTestCase>
    {
        public ExpectationAssemblyRunner(ITestAssembly testAssembly,
                                         IEnumerable<ExpectationTestCase> testCases,
                                         IMessageSink diagnosticMessageSink,
                                         IMessageSink executionMessageSink,
                                         ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        {
            TestCaseOrderer = new ExpectationTestCaseOrderer();
        }

        protected override string GetTestFrameworkDisplayName()
        {
            return "IntegROS Framework";
        }

        protected override string GetTestFrameworkEnvironment()
        {
            return String.Format("{0}-bit .NET {1}", IntPtr.Size * 8, Environment.Version);
        }

        protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
                                                                   ITestCollection testCollection,
                                                                   IEnumerable<ExpectationTestCase> testCases,
                                                                   CancellationTokenSource cancellationTokenSource)
        {
            return new ExpectationTestCollectionRunner(testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
        }
    }
}
