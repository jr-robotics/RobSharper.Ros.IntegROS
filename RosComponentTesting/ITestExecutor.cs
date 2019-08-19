using System.Threading.Tasks;

namespace RosComponentTesting
{
    public interface ITestExecutor
    {
        TestExecutionState State { get; }
        Task ExecuteAsync(TestExecutionOptions options = null);
    }
}