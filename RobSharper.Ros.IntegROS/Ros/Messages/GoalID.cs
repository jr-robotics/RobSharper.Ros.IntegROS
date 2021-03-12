using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.Messages
{
    [RosMessage("actionlib_msgs/GoalID")]
    public class GoalID
    {
        [RosMessageField("time", "stamp", 1)]
        public DateTime Stamp { get; set; }
        
        [RosMessageField("string", "id", 3)]
        public string Id { get; set; }
    }
}