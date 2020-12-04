using System.Collections.Generic;

namespace Examples.TurtleSimTests
{
    public interface IRosMessageRecorder
    {
        IEnumerable<RecordedMessage> Messages { get; }
        
        void Start();
        void Stop();
    }
}