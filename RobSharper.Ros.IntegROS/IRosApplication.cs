using System;

namespace RobSharper.Ros.IntegROS
{
    public interface IRosApplication
    {
        RosConfiguration Configuration { get; }
        
        RosCore RosCore { get; }
        
        void CallService(string serviceName, object request);

        TServiceResponse CallService<TServiceResponse>(string serviceName, object request);
        
        void Advertise(string topic, Type messageType);

        void Publish(string topic, object message);
    }
}