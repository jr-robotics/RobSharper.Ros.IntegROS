using System;

namespace IntegROS.ROS
{
    public class RosCore
    {
        private readonly RosConfiguration _configuration;
        private readonly IRosCoreConnectionAction _connectAction;
        private readonly IRosCoreConnectionAction _disconnectAction;

        public RosCore(RosConfiguration configuration, IRosCoreConnectionAction connectAction, IRosCoreConnectionAction disconnectAction)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectAction = connectAction ?? throw new ArgumentNullException(nameof(connectAction));
            _disconnectAction = disconnectAction ?? throw new ArgumentNullException(nameof(disconnectAction));
        }

        public void Connect()
        {
            _connectAction.Execute(_configuration);
        }

        public void Disconnect()
        {
            _disconnectAction.Execute(_configuration);
        }
    }
}