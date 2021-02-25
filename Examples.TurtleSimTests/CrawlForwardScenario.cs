using System;
using Messages.geometry_msgs;
using Messages.turtlesim;
using Twist = Uml.Robotics.Ros.Messages.geometry_msgs.Twist;

namespace Examples.TurtleSimTests
{
    public class CrawlForwardScenario : RosScenario
    {
        // Setup Scenario
            
        // 1. Start ROS
        // 2. Start TurtleSim (LaunchFile)
            
        // 3. Wait for ready
        //    What does this mean?
            
        // 4. Reset TurtleSim
        //    CallService("/turtle1/teleport_absolute", new TeleportAbsolute.Request { x = 5, y = 5, theta = 0 })
            
        // 5. Publish cmd_vel for 3 second at 10Hz
        /*
                Publish("/turtle1/cmd_vel", new Twist
                {
                    linear = new Vector3 { x = linearVelocity, y = 0, z = 0 },
                    angular = new Vector3 { x = 0, y = 0, z = 0 }
                })
            */
            
        // 6.a Wait for 3 seconds
        // 6.b Wait until Twist = 0
        
        protected override void Setup()
        {
            RosApp.Advertise("/turtle1/cmd_vel", typeof(Twist));
            RosApp.CallService("/turtle1/teleport_absolute", new TeleportAbsolute.Request {x = 5, y = 5, theta = 0});
        }
        
        protected override void Exercise()
        {
            RosApp.Publish("/turtle1/cmd_vel", new Twist
            {
                linear = new Vector3 {x = 2, y = 0, z = 0},
                angular = new Vector3 {x = 0, y = 0, z = 0}
            });
            
            Wait(TimeSpan.FromSeconds(3));
        }
        
        protected override void Teardown()
        {
        }
    }
}