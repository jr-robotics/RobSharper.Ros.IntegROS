using System.Collections.Generic;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.Messages
{
    [RosMessage("actionlib_msgs/GoalStatusArray")]
    public class GoalStatusArray
    {
        [RosMessageField("Header", "header", 1)]
        public Header Header { get; set; }
        
        [RosMessageField("GoalStatus[]", "status_list", 2)]
        public ICollection<GoalStatus> StatusList { get; set; }
    }
}