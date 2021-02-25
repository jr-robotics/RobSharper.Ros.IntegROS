using System;
using System.Collections.Generic;
using RobSharper.Ros.IntegROS.Ros.Actionlib;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.MessageEssentials
{
    public sealed class IntegrosActionTypeInfo : IRosMessageTypeInfo
    {
        public static readonly IDictionary<Type, IntegrosActionTypeInfo> Types =
            new Dictionary<Type, IntegrosActionTypeInfo>()
            {
                {typeof(ActionGoal), new IntegrosActionTypeInfo(typeof(ActionGoal))},
                {typeof(ActionFeedback), new IntegrosActionTypeInfo(typeof(ActionFeedback))},
                {typeof(ActionResult), new IntegrosActionTypeInfo(typeof(ActionResult))}
            };
        
        public RosType RosType { get; }
        public Type Type { get; }
        public string MD5Sum => throw new NotSupportedException("This type is not a real usable ROS type. It can only be used to deserialize basic Action messages.");
        public string MessageDefinition => throw new NotSupportedException("This type is not a real usable ROS type. It can only be used to deserialize basic Action messages.");

        private IntegrosActionTypeInfo(Type type)
        {
            Type = type;
            RosType = RosType.Parse("integros_msgs/" + type.Name);
        }
    }
}