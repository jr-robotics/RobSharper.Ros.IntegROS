using System.Collections.Generic;

namespace IntegROS
{
    public interface IScenario
    {
        IEnumerable<RecordedMessage> Messages { get; }
    }
}