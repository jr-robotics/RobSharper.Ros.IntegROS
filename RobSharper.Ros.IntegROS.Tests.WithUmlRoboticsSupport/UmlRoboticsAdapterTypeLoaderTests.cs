using FluentAssertions;
using RobSharper.Ros.IntegROS.Ros.MessageEssentials;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests.WithUmlRoboticsSupport
{
    /// <summary>
    /// These tests will fail if RobSharper.Ros.Adapters.UmlRobotics
    /// is not installed in the project
    /// </summary>
    public class UmlRoboticsAdapterTypeLoaderTests
    {
        [Fact]
        public void UmlRobotics_Adapter_is_available()
        {
            var exists = UmlRoboticsAdapterTypeLoader.AssemblyAvailable;

            exists.Should().BeTrue();
        }

        [Fact]
        public void Can_create_formatter()
        {
            var item = UmlRoboticsAdapterTypeLoader.CreateFormatter();

            item.Should().NotBeNull();
        }

        [Fact]
        public void Can_create_type_info_factory()
        {
            var item = UmlRoboticsAdapterTypeLoader.CreateTypeInfoFactory();

            item.Should().NotBeNull();
        }
    }
}