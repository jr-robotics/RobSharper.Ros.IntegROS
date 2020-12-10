using System;
using Xunit;

namespace IntegROS
{
    public class ForScenario<TScenario> : IClassFixture<TScenario> where TScenario : class, IScenario
    {
        public TScenario Scenario { get; }

        public ForScenario(TScenario scenario)
        {
            Scenario = scenario ?? throw new ArgumentNullException(nameof(scenario), "No scenario provided.");
        }
    }
}