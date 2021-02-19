using RobSharper.Ros.Adapters.UmlRobotics;
using RobSharper.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace IntegROS.Rosbag
{
    public static class RosbagReader
    {
        // TODO: Avoid default hardcoded initialization
        public static IRosbagReader Instance { get; set; }

        static RosbagReader()
        {
            var serializer = new RosMessageSerializer(new MessageTypeRegistry());
            
            serializer.MessageFormatters.Add(new UmlRoboticsRosMessageFormatter());
            serializer.MessageTypeRegistry.RosMessageTypeInfoFactories.Add(new UmlRoboticsMessageTypeInfoFactory());
            
            Instance = new RobSharperRosbagReaderAdapter(serializer);
        }
    }
}