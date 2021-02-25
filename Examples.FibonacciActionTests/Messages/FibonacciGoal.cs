using RobSharper.Ros.MessageEssentials;

namespace Examples.FibonacciActionTests.Messages
{
    [RosMessage("actionlib_tutorials/FibonacciGoal")]
    public class FibonacciGoal
    {
        [RosMessageField("int32", "order", 1)]
        public int Order { get; set; }
    }
}