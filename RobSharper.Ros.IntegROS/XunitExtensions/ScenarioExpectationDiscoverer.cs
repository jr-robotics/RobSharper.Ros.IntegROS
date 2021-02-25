using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.XunitExtensions
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
        
        /// <inheritdoc cref="IXunitTestCaseDiscoverer"/>
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod,
            IAttributeInfo expectThatAttribute)
        {
            // Check if expectation should be skipped
            if (expectThatAttribute.GetNamedArgument<string>(nameof(ExpectThatAttribute.Skip)) != null)
            {
                return CreateSkipTestCases(discoveryOptions, testMethod);
            }
            
            try
            {
                var scenarioAttributes = testMethod.GetScenarioAttributes(DiagnosticMessageSink);

                if (!scenarioAttributes.Any())
                {
                    return CreateNoScenarioTestCases(discoveryOptions, testMethod);
                }
                
                if (IsPreEnumerationSupported(discoveryOptions))
                {
                    return CreatePreEnumeratedTestCases(discoveryOptions, testMethod, scenarioAttributes);
                }
                else
                {
                    return CreateSingleTestCaseForAllScenarios(discoveryOptions, testMethod);
                }
            }
            catch (Exception e)
            {
                DiagnosticMessageSink.OnMessage(new PrintableDiagnosticMessage($"Exception thrown during scenario expectation discovery on '{testMethod.TestClass.Class.Name}.{testMethod.Method.Name}'.{Environment.NewLine}{e}"));
                throw;
            }
        }

        private ICollection<IXunitTestCase> CreatePreEnumeratedTestCases(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod, IEnumerable<IAttributeInfo> scenarioAttributes)
        {
            var testCases = new List<IXunitTestCase>();

            foreach (var scenarioAttribute in scenarioAttributes)
            {
                IScenarioIdentifier scenarioIdentifier;
                var skipReason = scenarioAttribute.GetNamedArgument<string>(nameof(ScenarioAttribute.Skip));

                try
                {
                    var scenarioDiscoverer =
                        ScenarioDiscovererFactory.GetDiscoverer(DiagnosticMessageSink, scenarioAttribute);
                    scenarioIdentifier = scenarioDiscoverer.GetScenarioIdentifier(scenarioAttribute);
                }
                catch (Exception e)
                {
                    scenarioIdentifier = new DummyScenarioDiscoverer().GetScenarioIdentifier(scenarioAttribute);

                    if (skipReason == null)
                    {
                        testCases.Add(
                            new ExecutionErrorScenarioTestCase(
                                DiagnosticMessageSink,
                                discoveryOptions.MethodDisplayOrDefault(),
                                discoveryOptions.MethodDisplayOptionsOrDefault(),
                                testMethod,
                                scenarioIdentifier,
                                $"{e.Message} for {testMethod.TestClass.Class.Name}.{testMethod.Method.Name}, Scenario {scenarioIdentifier}."
                            )
                        );

                        continue;
                    }
                }

                var testCase = new ScenarioTestCase(DiagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod, scenarioIdentifier, skipReason);

                testCases.Add(testCase);
            }

            return testCases;
        }

        private ICollection<IXunitTestCase> CreateSingleTestCaseForAllScenarios(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod)
        {
            return new[]
            {
                new MultipleScenariosTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod)
            };
        }

        /// <summary>
        /// Checks if test cases should be pre-enumerated during discovery.
        /// This is only necessary for UI test explorers.
        ///
        /// If pre-enumeration of theories is not desired, pre-enumeration of Expectations isn't also.
        /// Jetbrains Rider and Resharper Test explorers get messed up with pre-enumeration of expectations, so if the
        /// entry assembly is a Resharper test runner, pre-enumeration is also disabled.
        /// </summary>
        /// <param name="testFrameworkDiscoveryOptions"></param>
        /// <returns>true if pre-enumeration is supported, false otherwise</returns>
        protected virtual bool IsPreEnumerationSupported(ITestFrameworkDiscoveryOptions testFrameworkDiscoveryOptions)
        {
            var preEnumerateTheories = testFrameworkDiscoveryOptions.PreEnumerateTheories();
            if (preEnumerateTheories.HasValue && !preEnumerateTheories.Value)
                return false;
            
            // Jetbrains Resharper/Rider test explorer gets messed up with pre enumerated (Fact) testcases
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
                return true;
            
            var isJetBrainsExecutor = entryAssembly.FullName.StartsWith("resharper", StringComparison.OrdinalIgnoreCase);

            return !isJetBrainsExecutor;
        }

        /// <summary>
        /// Returns a standard <see cref="XunitTestCase"/> which handles the Skip property.
        /// </summary>
        /// <param name="discoveryOptions"></param>
        /// <param name="testMethod"></param>
        /// <returns>A standard <see cref="XunitTestCase"/> which handles the Skip property</returns>
        private IEnumerable<IXunitTestCase> CreateSkipTestCases(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod)
        {
            return new[]
            {
                new XunitTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod)
            };
        }
        
        /// <summary>
        /// Returns an <see cref="ExecutionErrorTestCase"/> with a notification that no scenario was specified.
        /// </summary>
        /// <param name="discoveryOptions"></param>
        /// <param name="testMethod"></param>
        /// <returns>An <see cref="ExecutionErrorTestCase"/> with a notification that no scenario was specified</returns>
        private ICollection<IXunitTestCase> CreateNoScenarioTestCases(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod)
        {
            return new[]
            {
                new ExecutionErrorTestCase(DiagnosticMessageSink,
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(),
                    testMethod,
                    $"No scenario specified for {testMethod.TestClass.Class.Name}.{testMethod.Method.Name}.{Environment.NewLine}Make sure to add at least one ScenarioAttribute to the test method or class.")
            };
        }
    }
}