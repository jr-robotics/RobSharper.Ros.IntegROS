using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.Messages
{
    [RosMessage("actionlib_msgs/GoalStatus")]
    public class GoalStatus
    {
        [RosMessageField("GoalID", "goal_id", 1)]
        public GoalID GoalId { get; set; }
        
        [RosMessageField("uint8", "status", 2)]
        public GoalStatusValue Status { get; set; }
        
        [RosMessageField("string", "text", 13)]
        public string Text { get; set; }
        
        

        [RosMessageField("uint8", "PENDING", 3)]
        public const GoalStatusValue Pending = GoalStatusValue.Pending;
        
        [RosMessageField("uint8", "ACTIVE", 4)]
        public const GoalStatusValue Active = GoalStatusValue.Active;
        
        [RosMessageField("uint8", "PREEMPTED", 5)]
        public const GoalStatusValue Preempted = GoalStatusValue.Preempted;
        
        [RosMessageField("uint8", "SUCCEEDED", 6)]
        public const GoalStatusValue Succeeded = GoalStatusValue.Succeeded;
        
        [RosMessageField("uint8", "ABORTED", 7)]
        public const GoalStatusValue Aborted = GoalStatusValue.Aborted;
        
        [RosMessageField("uint8", "REJECTED", 8)]
        public const GoalStatusValue Rejected = GoalStatusValue.Rejected;
        
        [RosMessageField("uint8", "PREEMPTING", 9)]
        public const GoalStatusValue Preempting = GoalStatusValue.Preempting;
        
        [RosMessageField("uint8", "RECALLING", 10)]
        public const GoalStatusValue Recalling = GoalStatusValue.Recalling;
        
        [RosMessageField("uint8", "RECALLED", 11)]
        public const GoalStatusValue Recalled = GoalStatusValue.Recalled;
        
        [RosMessageField("uint8", "LOST", 12)]
        public const GoalStatusValue Lost = GoalStatusValue.Lost;
    }
}