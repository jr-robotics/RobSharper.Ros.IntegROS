using RobSharper.Ros.MessageEssentials;

namespace Examples.TurtleSimTests.Messages
{
    [RosMessage("turtlesim/Pose")]
    public class Pose
    {
        /*
            float32 x
            float32 y
            float32 theta

            float32 linear_velocity
            float32 angular_velocity
        */
            
        [RosMessageField(1, "float32", "x" )]
        public float X { get; set; }
            
        [RosMessageField(2, "float32", "y" )]
        public float Y { get; set; }
            
        [RosMessageField(3, "float32", "theta" )]
        public float Theta { get; set; }
            
        [RosMessageField(4, "float32", "linear_velocity" )]
        public float LinearVelocity { get; set; }
            
        [RosMessageField(5, "float32", "angular_velocity" )]
        public float AngularVelocity { get; set; }
    }
}