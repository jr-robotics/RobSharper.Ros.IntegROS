using System;
using FluentAssertions;
using RobSharper.Ros.IntegROS.Ros.MessageEssentials;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests.WithoutUmlRoboticsSupport
{
    /// <summary>
    /// These tests will fail if RobSharper.Ros.Adapters.UmlRobotics
    /// is installed in the project
    /// </summary>
    public class UmlRoboticsAdapterTypeLoaderTests
    {
        [Fact]
        public void UmlRobotics_Adapter_is_not_available()
        {
            var exists = UmlRoboticsAdapterTypeLoader.AssemblyAvailable;

            exists.Should().BeFalse();
        }

        [Fact]
        public void Cannot_create_formatter()
        {
            Action action = () => UmlRoboticsAdapterTypeLoader.CreateFormatter();
            
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Cannot_create_type_info_factory()
        {
            Action action = () => UmlRoboticsAdapterTypeLoader.CreateTypeInfoFactory();
            
            action.Should().Throw<Exception>();
        }
    }
}