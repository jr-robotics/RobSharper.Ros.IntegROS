namespace RobSharper.Ros.IntegROS.Test.Expectations
{
    public class TurtleSimBagFiles
    {
        private const string BasePath = "bags/turtlesim/";
        
        public const string MoveForwards = BasePath + "move_forward_2020-12-11-11-27-40.bag";
        public const string MoveBackwards = BasePath + "move_backwards_2020-12-11-11-32-19.bag";
        public const string MoveRandom = BasePath + "move_random_2020-12-11-11-33-25.bag";
        public const string TurnClockwise = BasePath + "turn_clockwise_2020-12-11-11-28-42.bag";
        public const string TurnCounterClockwise = BasePath + "turn_counterclockwiwise_2020-12-11-11-29-11.bag";
        public const string HitWall = BasePath + "hit_wall_2020-12-11-11-34-39.bag";

        public const string RectangleShapeAction = BasePath + "action_turtle_shape_rectangle_2021-02-18-09-50-48.bag";
        public const string TriangleShapeAction = BasePath + "action_turtle_shape_triangle_2021-02-18-09-51-43.bag";
        public const string TurtleShapeClientAction = BasePath + "action_turtle_shape_client_test_2021-02-18-09-54-15.bag";
    }
}