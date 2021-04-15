using System;
using System.Reflection;
using RobSharper.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace RobSharper.Ros.IntegROS.Ros.MessageEssentials
{
    internal static class UmlRoboticsAdapterTypeLoader
    {
        private const string UmlRoboticsAssemblyName = "RobSharper.Ros.Adapters.UmlRobotics";
        private const string MessageFormatterTypeName = UmlRoboticsAssemblyName + ".UmlRoboticsRosMessageFormatter";
        private const string TypeInfoFactoryTypeName = UmlRoboticsAssemblyName + ".UmlRoboticsMessageTypeInfoFactory";

        public static bool AssemblyAvailable
        {
            get
            {
                try
                {
                    var assembly = Assembly;
                    return assembly != null;
                }
                catch
                {
                    // ignored
                }

                return false;
            }
        }

        public static Assembly Assembly
        {
            get
            {
                var assembly = Assembly.Load(UmlRoboticsAssemblyName);
                return assembly;
            }
        }

        public static Type MessageFormatterType => Assembly.GetType(MessageFormatterTypeName, true);
        public static Type TypeInfoFactoryType => Assembly.GetType(TypeInfoFactoryTypeName, true);

        
        public static IRosMessageTypeInfoFactory CreateTypeInfoFactory()
        {
            var type = TypeInfoFactoryType;
            var instance = Activator.CreateInstance(type);

            return (IRosMessageTypeInfoFactory) instance;
        }

        public static IRosMessageFormatter CreateFormatter()
        {
            var type = MessageFormatterType;
            var instance = Activator.CreateInstance(type);

            return (IRosMessageFormatter) instance;
        }
    }
}