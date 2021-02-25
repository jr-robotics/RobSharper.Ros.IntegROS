using RobSharper.Ros.IntegROS.Ros.MessageEssentials;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
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