using System.Collections.Generic;
using RobSharper.Ros.IntegROS.Scenarios;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS
{
    /// <summary>
    /// Base class for scenario dependent test classes.
    ///
    /// Test methods in the extending test class must be annotated with the <see cref="ExpectThatAttribute"/>.
    /// Scenarios are specified in the test class or test methods with one or more <see cref="ScenarioAttribute"/>
    /// annotations. 
    /// </summary>
    public abstract class ForScenario
    {
        /// <summary>
        /// Test output helper for capturing output in the test explorer.
        /// See <see href="https://xunit.net/docs/capturing-output">xUnit.net Documentation</see> for further information.
        /// </summary>
        public ITestOutputHelper OutputHelper { get; }

        /// <summary>
        /// The scenario to be used.
        /// The scenario can be configured via <see cref="ScenarioAttribute"/> attributes and is set by the
        /// testing framework.
        /// </summary>
        public IScenario Scenario { get; internal set; }

        /// <summary>
        /// Returns the recorded messages from the scenario.
        /// </summary>
        public IEnumerable<IRecordedMessage> Messages => Scenario?.Messages;

        public ForScenario()
        {
        }

        public ForScenario(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }
    }
}