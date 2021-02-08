using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationDiscoverer : TestFrameworkDiscoverer
    {
        readonly CollectionPerClassTestCollectionFactory testCollectionFactory;

        public ExpectationDiscoverer(IAssemblyInfo assemblyInfo,
                                     ISourceInformationProvider sourceProvider,
                                     IMessageSink diagnosticMessageSink)
            : base(assemblyInfo, sourceProvider, diagnosticMessageSink)
        {
            string config = null;
#if NETFRAMEWORK
            config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif
            var testAssembly = new TestAssembly(assemblyInfo, config);
            testCollectionFactory = new CollectionPerClassTestCollectionFactory(testAssembly, diagnosticMessageSink);
        }

        protected override ITestClass CreateTestClass(ITypeInfo @class)
        {
            return new TestClass(testCollectionFactory.Get(@class), @class);
        }

        bool FindTestsForMethod(ITestMethod testMethod,
                                TestMethodDisplay defaultMethodDisplay,
                                TestMethodDisplayOptions defaultMethodDisplayOptions,
                                bool includeSourceInformation,
                                IMessageBus messageBus)
        {
            var observationAttribute = testMethod.Method.GetCustomAttributes(typeof(ExpectThatAttribute)).FirstOrDefault();
            if (observationAttribute == null)
                return true;

            var testCase = new ExpectationTestCase(defaultMethodDisplay, defaultMethodDisplayOptions, testMethod);
            if (!ReportDiscoveredTestCase(testCase, includeSourceInformation, messageBus))
                return false;

            return true;
        }

        protected override bool FindTestsForType(ITestClass testClass,
                                                 bool includeSourceInformation,
                                                 IMessageBus messageBus,
                                                 ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var methodDisplay = discoveryOptions.MethodDisplayOrDefault();
            var methodDisplayOptions = discoveryOptions.MethodDisplayOptionsOrDefault();

            foreach (var method in testClass.Class.GetMethods(includePrivateMethods: true))
                if (!FindTestsForMethod(new TestMethod(testClass, method), methodDisplay, methodDisplayOptions, includeSourceInformation, messageBus))
                    return false;

            return true;
        }
    }
}
