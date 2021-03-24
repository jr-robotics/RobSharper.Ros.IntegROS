using Xunit;

namespace RobSharper.Ros.IntegROS.Tests.WithUmlRoboticsSupport
{
    public class JrFactAttribute : FactAttribute
    {
        /// <summary>
        /// Tests are skipped, if compiler property JRINTERNAL=True is not set.
        /// </summary>
        public JrFactAttribute() : base()
        {
#if !JRINTERNAL
            Skip = "JR infrastructure is not available. Compile project with property JRINTERNAL=True";
#endif
        }
    }
}