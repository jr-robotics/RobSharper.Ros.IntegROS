using System.Collections.Generic;

namespace RobSharper.Ros.IntegROS.Scenarios
{
    public interface IScenario
    {
        IEnumerable<IRecordedMessage> Messages { get; }
    }
}