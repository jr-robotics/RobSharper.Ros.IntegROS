using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    [Obsolete("Unused", true)]
    public class IntegrosTestFramework : TestFramework
    {
        public IntegrosTestFramework(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink) { }

        protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo)
        {
            return new ExpectationDiscoverer(assemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }

        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        {
            return new ExpectationExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
        }
    }
}
