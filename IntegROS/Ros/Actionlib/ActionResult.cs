using IntegROS.Ros.MessageEssentials;
using IntegROS.Ros.Messages;

namespace IntegROS.Ros.Actionlib
{
    public class ActionResult
    {
        private readonly PartialMessageDeserializer _goalMessageDeserializer;
        
        public Header Header { get;}
        
        public GoalStatus GoalStatus { get; }
        
        public ActionResult(Header header, GoalStatus goalStatus, PartialMessageDeserializer goalMessageDeserializer)
        {
            _goalMessageDeserializer = goalMessageDeserializer;
            
            Header = header;
            GoalStatus = goalStatus;
        }
    }
}