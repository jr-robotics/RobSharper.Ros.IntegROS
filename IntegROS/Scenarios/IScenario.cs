using System.Collections.Generic;

namespace IntegROS.Scenarios
{
    public interface IScenario
    {
        IEnumerable<IRecordedMessage> Messages { get; }
    }
}