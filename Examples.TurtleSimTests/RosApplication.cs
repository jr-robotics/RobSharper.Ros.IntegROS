using System;
using Messages.turtlesim;
using Uml.Robotics.Ros;

namespace Examples.TurtleSimTests
{
    public interface IRosApplication
    {
        void CallService(string serviceName, TeleportAbsolute.Request request);

        TServiceResponse CallService<TServiceResponse>(string serviceName, TeleportAbsolute.Request request);
        
        void Advertise(string topic, Type messageType);

        void Publish(string topic, RosMessage message);
    }
}