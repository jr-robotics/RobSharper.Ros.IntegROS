using System.Linq;
using FluentAssertions;
using IntegROS.Tests.XunitExtensionsTests.Utility;
using RobSharper.Ros.IntegROS.XunitExtensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using TestFrameworkOptions = IntegROS.Tests.XunitExtensionsTests.Utility.TestFrameworkOptions;

namespace IntegROS.Tests.XunitExtensionsTests
{
    public class ScenarioExpectationDiscovererTests
    {
        public class TestableScenarioExpectationDiscoverer : ScenarioExpectationDiscoverer
        {
            public bool? PreEnumerateTestCases { get; set; }
            
            public TestableScenarioExpectationDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
            {
            }

            protected override bool IsPreEnumerationSupported(ITestFrameworkDiscoveryOptions testFrameworkDiscoveryOptions)
            {
                if (PreEnumerateTestCases.HasValue)
                    return PreEnumerateTestCases.Value;

                return base.IsPreEnumerationSupported(testFrameworkDiscoveryOptions);
            }
        }

        public IMessageSink DiagnosticsMessageSink { get; set; }
        
        public TestableScenarioExpectationDiscoverer Discoverer { get; set; }
        
        public IReflectionAttributeInfo ExpectThatAttribute { get; set; }
        
        public TestFrameworkOptions Options { get; set; }
        
        
        public ScenarioExpectationDiscovererTests()
        {
            DiagnosticsMessageSink = new NullMessageSink();
            Discoverer = new TestableScenarioExpectationDiscoverer(DiagnosticsMessageSink);

            ExpectThatAttribute = XunitMocks.ExpectThatAttribute();
            Options = new TestFrameworkOptions();
        }
    }

    public class ScenarioExpectationDiscovererTestsWithPreEnumerationOnOrOff : ScenarioExpectationDiscovererTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Method_without_scenarios(bool preEnumerateTestCases)
        {
            Discoverer.PreEnumerateTestCases = preEnumerateTestCases;
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_without_scenarios));

            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);

            testCases.First().Should().BeOfType<ExecutionErrorTestCase>();
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Method_with_skip_flag(bool preEnumerateTestCases)
        {
            Discoverer.PreEnumerateTestCases = preEnumerateTestCases;
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_without_scenarios));
            
            var skippedExpectThatAttribute = XunitMocks.ExpectThatAttribute(null, "Should be skipped");
            var testCases = Discoverer.Discover(Options, testMethod, skippedExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);

            testCases.First().Should().BeOfType<XunitTestCase>();
        }
    }

    public class ScenarioExpectationDiscovererTestsWithPreEnumerationOff : ScenarioExpectationDiscovererTests
    {
        public ScenarioExpectationDiscovererTestsWithPreEnumerationOff() : base()
        {
            Discoverer.PreEnumerateTestCases = false;
        }

        [Fact]
        public void Method_with_one_scenario()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_one_scenario));
            
            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);

            testCases.First().Should().BeOfType<MultipleScenariosTestCase>();
        }
    }
    
    public class ScenarioExpectationDiscovererTestsWithPreEnumerationOn : ScenarioExpectationDiscovererTests
    {
        public ScenarioExpectationDiscovererTestsWithPreEnumerationOn() : base()
        {
            Discoverer.PreEnumerateTestCases = true;
        }
        
        [Fact]
        public void Method_with_one_scenario()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_one_scenario));
            
            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
        
        [Fact]
        public void Method_with_skipped_scenario()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_one_skipped_scenario));
            
            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
        
        [Fact]
        public void Method_with_two_scenarios()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_two_different_scenarios));
            
            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(2);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
        
        [Fact]
        public void Method_with_two_equivalent_scenarios()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_two_same_scenarios));
            
            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(2);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
        
        [Fact]
        public void Method_with_scenario_no_discoverer()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_invalid_scenario_no_discoverer));

            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);

            testCases.First().Should().BeOfType<ExecutionErrorScenarioTestCase>();
        }
        
        [Fact]
        public void Method_with_scenario_wrong_discoverer()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_invalid_scenario_wrong_discoverer));

            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);

            testCases.First().Should().BeOfType<ExecutionErrorScenarioTestCase>();
        }
        
        [Fact]
        public void Method_with_null_scenario()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_null_scenario));

            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
        
        [Fact]
        public void Method_with_exception_scenario()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_exception_scenario));

            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(1);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
        
        [Fact]
        public void Method_with_successful_and_null_and_exception_scenarios()
        {
            ITestMethod testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases), nameof(XunitExtensionsTestCases.Method_with_successful_and_null_and_exception_scenarios));

            var testCases = Discoverer.Discover(Options, testMethod, ExpectThatAttribute);

            testCases.Should().NotBeNull();
            testCases.Count().Should().Be(3);
            testCases.Cast<ScenarioTestCase>().Should().NotContainNulls();
        }
    }
}