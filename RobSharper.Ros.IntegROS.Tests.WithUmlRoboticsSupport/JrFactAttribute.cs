using Xunit;

namespace RobSharper.Ros.IntegROS.Tests.WithUmlRoboticsSupport
{
    public class JrFactAttribute : FactAttribute
    {
        public JrFactAttribute() : base()
        {
#if !JRINTERNAL
            Skip = "JR infrastructure is not available. Compile project with property JRINTERNAL=True";
#endif
        }
    }
}