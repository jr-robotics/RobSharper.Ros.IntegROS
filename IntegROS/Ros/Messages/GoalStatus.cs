using RobSharper.Ros.MessageEssentials;

namespace IntegROS.Ros.Messages
{
    [RosMessage("actionlib_msgs/GoalStatus")]
    public class GoalStatus
    {
        [RosMessageField(1, "GoalID", "goal_id")]
        public GoalID GoalId { get; set; }
        
        [RosMessageField(2, "uint8", "status")]
        public GoalStatusValue Status { get; set; }
        
        [RosMessageField(13, "string", "text")]
        private string Text { get; set; }
        
        

        [RosMessageField(3, "uint8", "PENDING")]
        public const GoalStatusValue Pending = GoalStatusValue.Pending;
        
        [RosMessageField(4, "uint8", "ACTIVE")]
        public const GoalStatusValue Active = GoalStatusValue.Active;
        
        [RosMessageField(5, "uint8", "PREEMPTED")]
        public const GoalStatusValue Preempted = GoalStatusValue.Preempted;
        
        [RosMessageField(6, "uint8", "SUCCEEDED")]
        public const GoalStatusValue Succeeded = GoalStatusValue.Succeeded;
        
        [RosMessageField(7, "uint8", "ABORTED")]
        public const GoalStatusValue Aborted = GoalStatusValue.Aborted;
        
        [RosMessageField(8, "uint8", "REJECTED")]
        public const GoalStatusValue Rejected = GoalStatusValue.Rejected;
        
        [RosMessageField(9, "uint8", "PREEMPTING")]
        public const GoalStatusValue Preempting = GoalStatusValue.Preempting;
        
        [RosMessageField(10, "uint8", "RECALLING")]
        public const GoalStatusValue Recalling = GoalStatusValue.Recalling;
        
        [RosMessageField(11, "uint8", "RECALLED")]
        public const GoalStatusValue Recalled = GoalStatusValue.Recalled;
        
        [RosMessageField(12, "uint8", "LOST")]
        public const GoalStatusValue Lost = GoalStatusValue.Lost;
        
    }
}