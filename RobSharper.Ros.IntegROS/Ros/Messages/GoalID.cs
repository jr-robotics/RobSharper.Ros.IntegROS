using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.Messages
{
    [RosMessage("actionlib_msgs/GoalID")]
    public class GoalID
    {
        [RosMessageField(1, "time", "stamp")]
        public DateTime Stamp { get; set; }
        
        [RosMessageField(3, "string", "id")]
        public string Id { get; set; }
    }
}