namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    internal class XunitExtensionsTestCases : ForScenario
    {
        public void Method_without_scenarios() {}
        
        [TestScenario]
        public void Method_with_one_scenario() {}
        
        [TestScenario(Skip = "Should be skipped.")]
        public void Method_with_one_skipped_scenario() {}

        [TestScenario("A")]
        [TestScenario("B")]
        public void Method_with_two_different_scenarios() {}

        [TestScenario("A")]
        [TestScenario("A")]
        public void Method_with_two_same_scenarios() {}
        
        [TestScenarioWithoutScenarioDiscoverer]
        public void Method_with_invalid_scenario_no_discoverer() {}
        
        [TestScenarioWithInvalidScenarioDiscoverer]
        public void Method_with_invalid_scenario_wrong_discoverer() {}
        
        [NullScenario]
        public void Method_with_null_scenario() {}
    }
}