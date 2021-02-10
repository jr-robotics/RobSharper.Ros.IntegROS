using System;
using System.Collections.Generic;
using FluentAssertions;
using IntegROS.Scenarios;
using Xunit;

namespace IntegROS
{
    public abstract class ForScenario<TScenario> : IClassFixture<TScenario> where TScenario : class, IScenario
    {
        public TScenario Scenario { get; }

        public IEnumerable<IRecordedMessage> Messages => Scenario?.Messages;

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

    public abstract class ForNewScenario
    {
        public IScenario Scenario { get; internal set; }

        public IEnumerable<IRecordedMessage> Messages => Scenario?.Messages;
    }
}