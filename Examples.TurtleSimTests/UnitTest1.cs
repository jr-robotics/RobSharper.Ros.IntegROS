using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Messages.geometry_msgs;
using Messages.turtlesim;
using Uml.Robotics.Ros;
using Xunit;
using Twist = Uml.Robotics.Ros.Messages.geometry_msgs.Twist;

namespace Examples.TurtleSimTests
{
    public class CrawlTests : IClassFixture<CrawlForwardScenario>
    {
        public CrawlForwardScenario Scenario { get; private set; }
        
        public CrawlTests(CrawlForwardScenario scenario)
        {
            Scenario = scenario;
        }

        [Fact]
        public void CrawlForwardTest()
        {
            Scenario.Messages
                .Where(m => m.Topic == "/turtle1/pose")
                .Count()
                .Should().Be(0, "no pose should be published");
        }
    }
    
        public class RosApplication
        {
            public void CallService(string serviceName, TeleportAbsolute.Request request)
            {
                throw new NotImplementedException();
            }
            
            public TServiceResponse CallService<TServiceResponse>(string serviceName, TeleportAbsolute.Request request)
            {
                throw new NotImplementedException();
            }

            public void Publish(string topic, RosMessage message)
            {
                throw new NotImplementedException();
            }

            public void Wait(TimeSpan duration)
            {
                throw new NotImplementedException();
            }
        }
        
        public class CrawlForwardScenario
        {
            public CrawlForwardScenario()
            {
            }
            
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
            
            public RosApplication RosApp { get; set; }
            public IEnumerable<RecordedMessage> Messages { get; set; }

            public void Setup()
            {
                RosApp.CallService("/turtle1/teleport_absolute", new TeleportAbsolute.Request {x = 5, y = 5, theta = 0});
            }
            
            public void Exercise()
            {
                RosApp.Publish("/turtle1/cmd_vel", new Twist
                {
                    linear = new Vector3 {x = 2, y = 0, z = 0},
                    angular = new Vector3 {x = 0, y = 0, z = 0}
                });
                RosApp.Wait(TimeSpan.FromSeconds(3));
            }

            public void Teardown()
            {
                
            }

            public void Execute()
            {
                Setup();
                Exercise();
                Teardown();
            }
        }

        public class RecordedMessage
        {
            public TimeSpan Timestamp { get; }
            public string Topic { get; }
            public Type Type { get; }
            public object Data { get; }
        }
}