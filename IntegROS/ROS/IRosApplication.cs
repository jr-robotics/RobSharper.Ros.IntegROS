namespace IntegROS.ROS
{
    public interface IRosApplication
    {
        public void CallService(string serviceName, TeleportAbsolute.Request request);

        public TServiceResponse CallService<TServiceResponse>(string serviceName, TeleportAbsolute.Request request);
        
        void Advertise(string topic, Type messageType);

        public void Publish(string topic, RosMessage message);
    }
}