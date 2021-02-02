using System;
using FluentAssertions;
using Xunit;

namespace IntegROS
{
    public abstract class ScenarioTests
    {
        public IScenario Scenario { get; set; }

        public bool ShouldContainMessages { get; set; } = true;

        [ExpectThat]
        public void Should_contain_messages()
        {
            Scenario.Should().NotBeNull("a scenario must be set");
            
            Scenario.Messages.Should().NotBeNull("it should not be null");

            if (ShouldContainMessages)
                Scenario.Messages.Should().NotBeEmpty("it should contain messages");
        }
    }
    
    public abstract class ForScenario<TScenario> : IClassFixture<TScenario> where TScenario : class, IScenario
    {
        public TScenario Scenario { get; }

        public bool ShouldContainMessages { get; set; } = true;

        public ForScenario(TScenario scenario)
        {
            Scenario = scenario ?? throw new ArgumentNullException(nameof(scenario), "No scenario provided.");
        }

        [ExpectThat]
        public void Should_contain_messages()
        {
            Scenario.Messages.Should().NotBeNull("it should not be null");

            if (ShouldContainMessages)
                Scenario.Messages.Should().NotBeEmpty("it should contain messages");
        }
    }
}