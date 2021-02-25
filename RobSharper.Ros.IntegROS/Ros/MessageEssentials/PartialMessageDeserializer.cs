using System.IO;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace RobSharper.Ros.IntegROS.Ros.MessageEssentials
{
    public class PartialMessageDeserializer
    {
        private readonly MemoryStream _data;
        private readonly RosMessageSerializer _serializer;
        private object _value;

        private PartialMessageDeserializer(MemoryStream data, RosMessageSerializer serializer)
        {
            _data = data;
            _serializer = serializer;
        }

        public T DeserializeAs<T>()
        {
            if (_value != null && typeof(T) == _value.GetType())
                return (T) _value;
            
            _data.Position = 0;
            var value = _serializer.Deserialize<T>(_data);
            _value = value;

            return value;
        }

        public static PartialMessageDeserializer CreateFromStream(Stream stream, RosMessageSerializer serializer)
        {
            var goalMessage = new MemoryStream();
            stream.CopyTo(goalMessage);
            goalMessage.Position = 0;

            return new PartialMessageDeserializer(goalMessage, serializer);
        }
    }
}