using FluentAssertions;
using RobSharper.Ros.IntegROS.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Tests.WithUmlRoboticsSupport
{
    /// <summary>
    /// These tests will fail if RobSharper.Ros.Adapters.UmlRobotics
    /// is not installed in the project
    /// </summary>
    public class UmlRoboticsAdapterTypeLoaderTests
    {
        [JrFact]
        public void UmlRobotics_Adapter_is_available()
        {
            var exists = UmlRoboticsAdapterTypeLoader.AssemblyAvailable;

            exists.Should().BeTrue();
        }

        [JrFact]
        public void Can_create_formatter()
        {
            var item = UmlRoboticsAdapterTypeLoader.CreateFormatter();

            item.Should().NotBeNull();
        }

        [JrFact]
        public void Can_create_type_info_factory()
        {
            var item = UmlRoboticsAdapterTypeLoader.CreateTypeInfoFactory();

            item.Should().NotBeNull();
        }
    }
}