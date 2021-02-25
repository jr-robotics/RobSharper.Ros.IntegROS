using RobSharper.Ros.Adapters.UmlRobotics;
using RobSharper.Ros.IntegROS.Ros.Actionlib;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace RobSharper.Ros.IntegROS.Ros.MessageEssentials
{
    public static class RosMessageSerializerExtensions
    {
        public static void UseUmlRoboticsRos(this RosMessageSerializer serializer)
        {
            serializer.MessageFormatters.Add(new UmlRoboticsRosMessageFormatter());
            serializer.MessageTypeRegistry.RosMessageTypeInfoFactories.Add(new UmlRoboticsMessageTypeInfoFactory());
        }

        public static void UseIntegROSActions(this RosMessageSerializer serializer)
        {
            serializer.MessageTypeRegistry.RosMessageTypeInfoFactories.Add(new IntegrosActionTypeInfoFactory());
            serializer.MessageFormatters.Add(new ActionFormatter(serializer));
        }
    }
}