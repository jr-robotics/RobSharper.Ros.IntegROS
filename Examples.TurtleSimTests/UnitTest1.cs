using System;
using System.ComponentModel;
using FluentAssertions;
using IntegROS;
using IntegROS.Scenarios;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit;
using Xunit.Abstractions;

namespace Examples.TurtleSimTests
{
    [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
    [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
    public class ForClassScenariosExample : ForScenario
    {
        [ExpectThat]
        public void TestCase()
        {
            Scenario.Should().NotBeNull();
        }
    }
    
    public class ForScenarioExample : ForScenario
    {
        [ExpectThat(Skip = "Does not work without scenario attribute")]
        public void Expectation_without_scenario_should_not_run()
        {
            Scenario.Should().BeNull();
        }
        
        [ExpectThat(DisplayName = "Expect that")]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        public void Expectation_for_one_scenario()
        {
            Scenario.Should().NotBeNull();
        }
        
        [Theory(DisplayName = "Theory Name")]
        [InlineData(2)]
        public void My_single_data_theory(int input)
        {
            input.Should().BePositive();
        }
        
        [ExpectThat()]
        [RosbagScenario(TurtleSimBagFiles.MoveForwards)]
        [RosbagScenario(TurtleSimBagFiles.MoveBackwards)]
        public void Expectation_for_two_scenarios()
        {
            Scenario.Should().NotBeNull();
        }
    }

    public class NullScenarioTests : ForScenario
    {
        public class NullScenarioIdentifier : ScenarioIdentifierBase
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
            public NullScenarioIdentifier()
            {
            }
        
            public NullScenarioIdentifier(Type scenarioDiscovererType, string displayName = null)
            {
                ScenarioDiscovererType = scenarioDiscovererType;
                DisplayName = displayName;
            }
            
            protected override string GetUniqueId()
            {
                return NullScenario.NullId.ToString("D");
            }
        }
        
        public class NullScenarioDiscoverer : IScenarioDiscoverer
        {
            public IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute)
            {
                return new NullScenarioIdentifier(this.GetType());
            }

            public IScenario GetScenario(IScenarioIdentifier scenarioIdentifier)
            {
                return null;
            }
        }
        
        [ScenarioDiscoverer("Examples.TurtleSimTests.NullScenarioTests+NullScenarioDiscoverer", "Examples.TurtleSimTests")]
        public class NullScenario : ScenarioAttribute
        {
            public static readonly  Guid NullId = new Guid("535124A8-20E9-4706-877F-4C99F9513454");

            public NullScenario()
            {
            }

            public override int GetScenarioHashCode()
            {
                return HashCode.Combine(NullId);
            }
        }

        [ExpectThat]
        [NullScenario]
        public void Null_scenario_returns_no_scenario()
        {
            Scenario.Should().BeNull();
        }
    }
}