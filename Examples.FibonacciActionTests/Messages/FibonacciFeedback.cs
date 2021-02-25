using System.Collections.Generic;
using RobSharper.Ros.MessageEssentials;

namespace Examples.FibonacciActionTests.Messages
{
    [RosMessage("actionlib_tutorials/FibonacciFeeback")]
    public class FibonacciFeedback
    {
        [RosMessageField("int32[]", "sequence", 1)]
        public IEnumerable<int> Sequence { get; set; }
    }
}