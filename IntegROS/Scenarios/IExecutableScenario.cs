using System.Threading.Tasks;

namespace IntegROS.Scenarios
{
    public interface IExecutableScenario : IScenario
    {
        Task ExecuteAsync();
    }
}