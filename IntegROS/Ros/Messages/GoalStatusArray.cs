using System.Collections.Generic;
using RobSharper.Ros.MessageEssentials;

namespace IntegROS.Ros.Messages
{
    [RosMessage("actionlib_msgs/GoalStatusArray")]
    public class GoalStatusArray
    {
        [RosMessageField(1, "Header", "header")]
        public Header Header { get; set; }
        
        [RosMessageField(2, "GoalStatus[]", "status_list")]
        public ICollection<GoalStatus> StatusList { get; set; }
    }
}