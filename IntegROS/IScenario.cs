using System.Collections.Generic;

namespace IntegROS
{
    public interface IScenario
    {
        IEnumerable<IRecordedMessage> Messages { get; }
    }
}