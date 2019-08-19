namespace RosComponentTesting
{
    public static class TestExecutorExtensions
    {
        public static void Execute(this ITestExecutor target, TestExecutionOptions options = null)
        {
            target.ExecuteAsync(options)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}