using RobSharper.Ros.IntegROS.Ros.MessageEssentials;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionGoal
    {
        private readonly PartialMessageDeserializer _goalMessageDeserializer;
        
        public Header Header { get;}
        
        public GoalID GoalId { get; }

        public TGoal Goal<TGoal>()
        {
            return _goalMessageDeserializer.DeserializeAs<TGoal>();
        }

        public ActionGoal(Header header, GoalID goalId, PartialMessageDeserializer goalMessageDeserializer)
        {
            _goalMessageDeserializer = goalMessageDeserializer;
            
            Header = header;
            GoalId = goalId;
        }
    }
}