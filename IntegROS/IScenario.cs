using System.Collections.Generic;
using System.Linq;

namespace IntegROS
{
    public interface IScenario
    {
        IQueryable<RecordedMessage> Messages { get; }
    }
}