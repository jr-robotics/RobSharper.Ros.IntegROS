using IntegROS.Ros.MessageEssentials;
using IntegROS.Ros.Messages;

namespace IntegROS.Ros.Actionlib
{
    public class ActionResult
    {
        private readonly PartialMessageDeserializer _goalMessageDeserializer;
        
        public Header Header { get;}
        
        public GoalStatus GoalStatus { get; }

        public TResult Result<TResult>()
        {
            return _goalMessageDeserializer.DeserializeAs<TResult>();
        }
        
        public ActionResult(Header header, GoalStatus goalStatus, PartialMessageDeserializer goalMessageDeserializer)
        {
            _goalMessageDeserializer = goalMessageDeserializer;
            
            Header = header;
            GoalStatus = goalStatus;
        }
    }
}