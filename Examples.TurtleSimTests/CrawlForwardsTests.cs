using System;
using System.Linq;
using FluentAssertions;
using IntegROS;
using Xunit;

namespace Examples.TurtleSimTests
{
    public class CrawlForwardsTests : ForScenario<RosbagScenario>
    {
        public CrawlForwardsTests(RosbagScenario scenario) : base(scenario)
        {
            scenario.Load(TurtleSimBagFiles.MoveForwards);
        }

        [ExpectThat]
        public void Turtle_always_moves_forwards()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .SetMessageType<Object>()
                .Select(x => x.Message)
                //.Should()
                //.BeInAscendingOrder(message => message.X)
                .ToList();

            throw new NotImplementedException();
        }

        [ExpectThat]
        public void Turtle_does_not_turn()
        {
            throw new NotImplementedException();
        }
    }
}