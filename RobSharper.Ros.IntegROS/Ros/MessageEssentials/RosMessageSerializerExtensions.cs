using RobSharper.Ros.IntegROS.Ros.Actionlib;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace RobSharper.Ros.IntegROS.Ros.MessageEssentials
{
    public static class RosMessageSerializerExtensions
    {
        public static void UseUmlRoboticsRos(this RosMessageSerializer serializer)
        {
            if (!UmlRoboticsAdapterTypeLoader.AssemblyAvailable)
                return;

            var typeInfoFactory = UmlRoboticsAdapterTypeLoader.CreateTypeInfoFactory();
            var messageFormatter = UmlRoboticsAdapterTypeLoader.CreateFormatter();
            
            serializer.MessageTypeRegistry.RosMessageTypeInfoFactories.Add(typeInfoFactory);
            serializer.MessageFormatters.Add(messageFormatter);
        }

        public static void UseIntegROSActions(this RosMessageSerializer serializer)
        {
            serializer.MessageTypeRegistry.RosMessageTypeInfoFactories.Add(new IntegrosActionTypeInfoFactory());
            serializer.MessageFormatters.Add(new ActionFormatter(serializer));
        }
    }
}