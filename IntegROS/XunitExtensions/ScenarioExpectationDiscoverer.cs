using System;
using System.Collections.Generic;
using System.Linq;
using IntegROS.Scenarios;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioExpectationDiscoverer : IXunitTestCaseDiscoverer
    {
        /// <summary>
        /// Message sink to be used to send diagnostic messages.
        /// </summary>
        private IMessageSink DiagnosticMessageSink { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioExpectationDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        public ScenarioExpectationDiscoverer(IMessageSink diagnosticMessageSink)
        {
            DiagnosticMessageSink = diagnosticMessageSink;
        }
        
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod,
            IAttributeInfo expectThatAttribute)
        {
            // Check if expectation should be skipped
            if (expectThatAttribute.GetNamedArgument<string>("Skip") != null)
            {
                return CreateTestCasesForSkip(discoveryOptions, testMethod);
            }
            
            if (!discoveryOptions.PreEnumerateTheoriesOrDefault())
            {
                // TODO
                throw new NotSupportedException("Scenarios must be enumerable.");
            }
            
            try
            {
                // Create one test per scenario
                var methodScenarioAttributes = testMethod.Method.GetCustomAttributes(typeof(ScenarioAttribute));
                var classScenarioAttributes = testMethod.TestClass.Class.GetCustomAttributes(typeof(ScenarioAttribute));
                
                var testCases = new List<IXunitTestCase>();

                foreach (var methodScenarioAttribute in methodScenarioAttributes)
                {
                    var skipReason = methodScenarioAttribute.GetNamedArgument<string>("Skip");

                    if (skipReason != null)
                    {
                        var skippedScenarioTestCase = new SkippedScenarioTestCase(DiagnosticMessageSink,
                            discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(),
                            testMethod, skipReason, null);
                        
                        testCases.Add(skippedScenarioTestCase);
                        continue;
                    }

                    var scenarioDiscoverer = ScenarioDiscovererFactory.GetDiscoverer(DiagnosticMessageSink, methodScenarioAttribute);
                    var scenarioIdentifier = scenarioDiscoverer.GetScenarioIdentifier(methodScenarioAttribute);
                    var scenario = scenarioDiscoverer.GetScenario(scenarioIdentifier);

                    if (scenario == null)
                    {
                        testCases.Add(
                            new ExecutionErrorTestCase(
                                DiagnosticMessageSink,
                                discoveryOptions.MethodDisplayOrDefault(),
                                discoveryOptions.MethodDisplayOptionsOrDefault(),
                                testMethod,
                                $"Scenario was null for {testMethod.TestClass.Class.Name}.{testMethod.Method.Name}."
                            )
                        );
                        
                        continue;
                    }

                    var testCase = new ScenarioTestCase(DiagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(),
                        testMethod, scenarioIdentifier);

                    testCases.Add(testCase);
                }

                foreach (var classScenarioAttribute in classScenarioAttributes)
                {
                    // TODO: Do the same for class attributes
                }

                if (testCases.Count == 0)
                {
                    testCases.Add(new ExecutionErrorTestCase(DiagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(),
                        testMethod,
                        $"No scenario found for {testMethod.TestClass.Class.Name}.{testMethod.Method.Name}"));
                }

                return testCases;
            }
            catch (Exception e)
            {
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Exception thrown during scenario expectation discovery on '{testMethod.TestClass.Class.Name}.{testMethod.Method.Name}'.{Environment.NewLine}{e}"));
                throw;
            }
        }

        /// <summary>
        /// Returns a standard XunitTestCase which handles the Skip property.
        /// </summary>
        /// <param name="discoveryOptions"></param>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        private IEnumerable<IXunitTestCase> CreateTestCasesForSkip(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod)
        {
            return new[]
            {
                new XunitTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod)
            };
        }
    }
}