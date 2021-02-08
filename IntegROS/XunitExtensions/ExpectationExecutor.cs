using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationExecutor : TestFrameworkExecutor<ExpectationTestCase>
    {
        public ExpectationExecutor(AssemblyName assemblyName,
                                   ISourceInformationProvider sourceInformationProvider,
                                   IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink) { }

        protected override ITestFrameworkDiscoverer CreateDiscoverer()
        {
            return new ExpectationDiscoverer(AssemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }

        protected override async void RunTestCases(IEnumerable<ExpectationTestCase> testCases,
                                                   IMessageSink executionMessageSink,
                                                   ITestFrameworkExecutionOptions executionOptions)
        {
            string config = null;
#if NETFRAMEWORK
            config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif
            
            var testAssembly = new TestAssembly(AssemblyInfo, config);

            using (var assemblyRunner = new ExpectationAssemblyRunner(testAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
                await assemblyRunner.RunAsync();
        }
    }
}
