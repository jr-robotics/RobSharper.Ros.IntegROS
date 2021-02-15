using System.Collections.Generic;
using IntegROS.Scenarios;

namespace IntegROS
{
    public abstract class ForScenario
    {
        public IScenario Scenario { get; internal set; }

        public IEnumerable<IRecordedMessage> Messages => Scenario?.Messages;

        public ForScenario()
        {
        }
    }
}