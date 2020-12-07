using System;
using IntegROS.ROS;

namespace IntegROS.Configuration
{
    public class IntegROSConfigurationOptions
    {
        public const string ConfigurationSection = "IntegROS";
        
        public RosCoreConfigurationOptions RosCore { get; set; }
    }

    public class RosCoreConfigurationOptions
    {
        public string StartupStrategy { get; set; }
        
        public string MASTER_URI { get; set; }
        
        public int Port { get; set; }
        
        public string StartupScriptPath { get; set; }
        
        public string TeardownStrategy { get; set; }
        
        public string TeardownScriptPath { get; set; }
    }
    
    public static class RosCoreConfigurationOptionsExtensions
    {
        public static IRosCoreConnectionAction GetTeardownAction(this RosCoreConfigurationOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            var strategyName = options.TeardownStrategy?.Trim();

            if (strategyName == null)
            {
                if (options.StartupStrategy != null && options.StartupStrategy.Equals("launch", StringComparison.InvariantCultureIgnoreCase))
                {
                    strategyName = "close";
                }
                else
                {
                    strategyName = "none";
                }
            }
            
            if (strategyName.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                return new NoAction();
            }

            if (strategyName.Equals("close", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ShutdownRosCoreAction();
            }

            if (strategyName.Equals("script", StringComparison.CurrentCultureIgnoreCase))
            {
                if (options.TeardownScriptPath == null)
                    throw new Exception("No script");    // TODO: Correct exception

                return ScriptAction.Create(options.TeardownScriptPath);
            }
            
            throw new InvalidOperationException($"The provided roscore teardown strategy {options.TeardownStrategy} is not supported.");
        }
    }
}