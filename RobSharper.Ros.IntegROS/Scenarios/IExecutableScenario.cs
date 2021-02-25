using System.Threading.Tasks;

namespace RobSharper.Ros.IntegROS.Scenarios
{
    public interface IExecutableScenario : IScenario
    {
        Task ExecuteAsync();
    }
}