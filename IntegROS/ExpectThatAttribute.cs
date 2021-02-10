using System;
using Xunit;
using Xunit.Sdk;

namespace IntegROS
{
    [XunitTestCaseDiscoverer("IntegROS.XunitExtensions.ScenarioExpectationDiscoverer", "IntegROS")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExpectThatAttribute : FactAttribute
    {
        // In fact, this is a Fact. But it sounds nicer as test method attribute for a scenario test.
    }
}