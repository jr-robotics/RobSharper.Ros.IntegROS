using System.Collections.Generic;
using IntegROS;

namespace Examples.TurtleSimTests
{
    public interface IRosMessageRecorder
    {
        IEnumerable<IRecordedMessage> Messages { get; }
        
        void Start();
        void Stop();
    }
}