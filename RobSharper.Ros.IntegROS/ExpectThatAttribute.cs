using System;
using Xunit;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS
{
    [XunitTestCaseDiscoverer("RobSharper.Ros.IntegROS.XunitExtensions.ScenarioExpectationDiscoverer", "RobSharper.Ros.IntegROS")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExpectThatAttribute : FactAttribute
    {
        // In fact, this is a Fact. But it sounds nicer as test method attribute for a scenario test.
    }
}