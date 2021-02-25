using RobSharper.Ros.IntegROS.Ros.MessageEssentials;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionFeedback
    {
        private readonly PartialMessageDeserializer _goalMessageDeserializer;
        
        public Header Header { get;}
        
        public GoalStatus GoalStatus { get; }

        public TFeedback Feedback<TFeedback>()
        {
            return _goalMessageDeserializer.DeserializeAs<TFeedback>();
        }
        
        public ActionFeedback(Header header, GoalStatus goalStatus, PartialMessageDeserializer goalMessageDeserializer)
        {
            _goalMessageDeserializer = goalMessageDeserializer;
            
            Header = header;
            GoalStatus = goalStatus;
        }
    }
}