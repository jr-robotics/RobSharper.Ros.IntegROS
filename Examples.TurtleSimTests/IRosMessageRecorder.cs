using System.Collections.Generic;
using IntegROS;

namespace Examples.TurtleSimTests
{
    public interface IRosMessageRecorder
    {
        IEnumerable<RecordedMessage> Messages { get; }
        
        void Start();
        void Stop();
    }
}