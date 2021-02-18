namespace Examples.TurtleSimTests
{
    public class FibonacciActionServerBagFiles
    {
        private const string BasePath = "bags/fibonacci_action_server/";

        public static string Fibonacci5 = BasePath + "action_fibonacci_5_2021-02-18-10-01-52.bag";
        public static string Fibonacci20 = BasePath + "action_fibonacci_20_2021-02-18-10-02-13.bag";
        public static string Fibonaccis = BasePath + "action_fibonaccis_2021-02-18-10-02-51.bag";
        public static string FibonacciPreempted1 = BasePath + "action_fibonacci_parallel_2021-02-18-10-05-25.bag";
        public static string FibonacciPreempted2 = BasePath + "action_fibonacci_parallel_2021-02-18-10-05-59.bag";
        public static string FibonacciSuccessfulAndPreempted = BasePath + "action_fibonacci_success_and_preempted_2021-02-18-10-07-23.bag";
    }
}