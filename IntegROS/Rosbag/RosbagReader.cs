using IntegROS.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace IntegROS.Rosbag
{
    public static class RosbagReader
    {
        public static RosMessageSerializer MessageSerializer { get; set; }
        public static IRosbagReader Instance { get; set; }

        // TODO: Avoid default hardcoded initialization
        static RosbagReader()
        {
            var serializer = new RosMessageSerializer(new MessageTypeRegistry());
            
            serializer.UseUmlRoboticsRos();
            serializer.UseIntegROSActions();

            MessageSerializer = serializer;
            Instance = new RobSharperRosbagReaderAdapter(serializer);
        }
    }
}