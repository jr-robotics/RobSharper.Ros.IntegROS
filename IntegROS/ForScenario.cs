using System.Collections.Generic;
using FluentAssertions;
using IntegROS.Scenarios;

namespace IntegROS
{
    public abstract class ForScenario
    {
        public IScenario Scenario { get; internal set; }

        public IEnumerable<IRecordedMessage> Messages => Scenario?.Messages;
        
        public bool ShouldContainMessages { get; set; } = true;

        public ForScenario()
        {
        }
        
        public ForScenario(bool shouldContainMessages)
        {
            ShouldContainMessages = shouldContainMessages;
        }

        [ExpectThat]
        public void Should_contain_messages()
        {
            Scenario.Should().NotBeNull("scenario should be available");
            Scenario.Messages.Should().NotBeNull("it should not be null");

            if (ShouldContainMessages)
                Scenario.Messages.Should().NotBeEmpty("it should contain messages");
        }
    }
}