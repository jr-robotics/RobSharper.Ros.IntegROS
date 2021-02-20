using IntegROS.Ros.MessageEssentials;
using IntegROS.Ros.Messages;

namespace IntegROS.Ros.Actionlib
{
    public class ActionGoal
    {
        private readonly PartialMessageDeserializer _goalMessageDeserializer;
        
        public Header Header { get;}
        
        public GoalID GoalId { get; }
        
        public ActionGoal(Header header, GoalID goalId, PartialMessageDeserializer goalMessageDeserializer)
        {
            _goalMessageDeserializer = goalMessageDeserializer;
            
            Header = header;
            GoalId = goalId;
        }
    }
}