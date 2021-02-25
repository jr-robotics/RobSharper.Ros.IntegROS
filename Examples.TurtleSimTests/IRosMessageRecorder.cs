using System.Collections.Generic;
using RobSharper.Ros.IntegROS;

namespace Examples.TurtleSimTests
{
    public interface IRosMessageRecorder
    {
        IEnumerable<IRecordedMessage> Messages { get; }
        
        void Start();
        void Stop();
    }
}