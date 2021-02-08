﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTestCollectionRunner : TestCollectionRunner<ExpectationTestCase>
    {
        readonly IMessageSink diagnosticMessageSink;

        public ExpectationTestCollectionRunner(ITestCollection testCollection,
                                               IEnumerable<ExpectationTestCase> testCases,
                                               IMessageSink diagnosticMessageSink,
                                               IMessageBus messageBus,
                                               ITestCaseOrderer testCaseOrderer,
                                               ExceptionAggregator aggregator,
                                               CancellationTokenSource cancellationTokenSource)
            : base(testCollection, testCases, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override async Task<RunSummary> RunTestClassAsync(ITestClass testClass,
                                                                    IReflectionTypeInfo @class,
                                                                    IEnumerable<ExpectationTestCase> testCases)
        {
            var timer = new ExecutionTimer();
            object testClassInstance = null;

            Aggregator.Run(() => testClassInstance = Activator.CreateInstance(testClass.Class.ToRuntimeType()));

            if (Aggregator.HasExceptions)
                return FailEntireClass(testCases, timer);

            var specification = testClassInstance as Specification;
            if (specification == null)
            {
                Aggregator.Add(new InvalidOperationException(String.Format("Test class {0} cannot be static, and must derive from Specification.", testClass.Class.Name)));
                return FailEntireClass(testCases, timer);
            }

            Aggregator.Run(specification.OnStart);
            if (Aggregator.HasExceptions)
                return FailEntireClass(testCases, timer);
            
            var result = await new ExpectationTestClassRunner(specification, testClass, @class, testCases, diagnosticMessageSink, MessageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), CancellationTokenSource).RunAsync();

            Aggregator.Run(specification.OnFinish);

            var disposable = specification as IDisposable;
            if (disposable != null)
                timer.Aggregate(disposable.Dispose);

            return result;
        }

        private RunSummary FailEntireClass(IEnumerable<ExpectationTestCase> testCases, ExecutionTimer timer)
        {
            foreach (var testCase in testCases)
            {
                MessageBus.QueueMessage(new TestFailed(new ExpectationTest(testCase, testCase.DisplayName), timer.Total,
                    "Exception was thrown in class constructor", Aggregator.ToException()));
            }
            int count = testCases.Count();
            return new RunSummary { Failed = count, Total = count };
        }
    }
}
