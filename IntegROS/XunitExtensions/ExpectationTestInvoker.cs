using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTestInvoker : TestInvoker<ExpectationTestCase>
    {
        private readonly Specification specification;

        public ExpectationTestInvoker(Specification specification,
                                      ITest test,
                                      IMessageBus messageBus,
                                      Type testClass,
                                      MethodInfo testMethod,
                                      ExceptionAggregator aggregator,
                                      CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, null, testMethod, null, aggregator, cancellationTokenSource)
        {
            this.specification = specification;
        }

        public new Task<decimal> RunAsync()
        {
            return Aggregator.RunAsync(async () =>
            {
                if (!CancellationTokenSource.IsCancellationRequested)
                {
                    if (!CancellationTokenSource.IsCancellationRequested)
                    {
                        if (!Aggregator.HasExceptions)
                            await Timer.AggregateAsync(() => InvokeTestMethodAsync(specification));
                    }
                }

                return Timer.Total;
            });
        }
    }
}
