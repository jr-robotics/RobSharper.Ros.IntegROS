using System;
using IntegROS.Ros.Actionlib;
using RobSharper.Ros.MessageEssentials;

namespace IntegROS.Ros.MessageEssentials
{
    public sealed class IntegrosActionTypeInfoFactory : IRosMessageTypeInfoFactory
    {
        public bool CanCreate(Type messageType)
        {
            return IntegrosActionTypeInfo.Types.Keys.Contains(messageType);
        }

        public IRosMessageTypeInfo Create(Type messageType)
        {
            return IntegrosActionTypeInfo.Types[messageType];
        }
    }
}