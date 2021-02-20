using System.IO;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace IntegROS.Ros.MessageEssentials
{
    public class PartialMessageDeserializer
    {
        private readonly MemoryStream _data;
        private readonly RosMessageSerializer _serializer;

        private PartialMessageDeserializer(MemoryStream data, RosMessageSerializer serializer)
        {
            _data = data;
            _serializer = serializer;
        }

        public T DeserializeAs<T>()
        {
            _data.Position = 0;
            return _serializer.Deserialize<T>(_data);
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