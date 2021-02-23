using System;
using RobSharper.Ros.BagReader;
using RobSharper.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace IntegROS.Ros.Rosbag
{
    public class RobSharperRecordedMessage : IRecordedMessage
    {
        private readonly BagMessage _bagMessage;
        private readonly RosMessageSerializer _serializer;
        private object _message;

        public RobSharperRecordedMessage(BagMessage bagMessage, RosMessageSerializer serializer)
        {
            _bagMessage = bagMessage ?? throw new ArgumentNullException(nameof(bagMessage));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public string Topic => _bagMessage.Connection.HeaderTopic;

        public RosType Type => _bagMessage.Connection.Type;
        
        public object GetMessage(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            if (_message == null || _message.GetType() != type)
            {
                _message = _serializer.Deserialize(type, _bagMessage.Message.Data);
            }

            return _message;
        }

        public TType GetMessage<TType>()
        {
            return (TType) GetMessage(typeof(TType));
        }
    }
}