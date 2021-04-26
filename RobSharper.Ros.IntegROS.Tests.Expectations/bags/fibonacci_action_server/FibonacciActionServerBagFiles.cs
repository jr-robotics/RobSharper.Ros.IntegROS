namespace RobSharper.Ros.IntegROS.Tests.Expectations
{
    public class FibonacciActionServerBagFiles
    {
        private const string BasePath = "bags/fibonacci_action_server/";

        public const string Fibonacci5 = BasePath + "action_fibonacci_5_2021-02-18-10-01-52.bag";
        public const string Fibonacci20 = BasePath + "action_fibonacci_20_2021-02-18-10-02-13.bag";
        public const string Fibonaccis = BasePath + "action_fibonaccis_2021-02-18-10-02-51.bag";
        public const string FibonacciPreempted = BasePath + "action_fibonacci_parallel_2021-02-18-10-05-25.bag";
        public const string FibonacciSuccessfulAndPreempted = BasePath + "action_fibonacci_success_and_preempted_2021-02-18-10-07-23.bag";
        public const string FibonacciCancel = BasePath + "action_fibonacci_cancel_2021-02-22-17-24-33.bag";
        public const string FibonacciWithoutCalls = BasePath + "fibonacci_no_calls_2021-02-25-14-48-01.bag";
        public const string FibonacciIn2Namespaces = BasePath + "fibonacci_namespaces_2021-04-26-09-46-48.bag";
    }
}