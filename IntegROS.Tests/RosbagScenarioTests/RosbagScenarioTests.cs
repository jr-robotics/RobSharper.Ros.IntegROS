using System;
using System.Linq;
using FluentAssertions;
using IntegROS.Ros.Rosbag;
using IntegROS.Scenarios;
using Moq;
using Xunit;

namespace IntegROS.Tests.RosbagScenarioTests
{
    public class RosbagScenarioTests
    {
        [Fact]
        public void Initialize_Rosbag_Scenario_with_null_is_not_allowed()
        {
            Action target = () => new RosbagScenario(null);
            
            target
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Bagname_is_set_from_constructor()
        {
            const string ExpectedBagName = "My/File/Path.bag";
            
            var target = new RosbagScenario(ExpectedBagName);

            target.RosbagFile.Should().BeSameAs(ExpectedBagName);
        }

        [Fact]
        public void Load_loads_messages()
        {
            const string ExpectedBagFileName = "filepath.bag";
            
            var rosbagReaderMock = new Mock<IRosbagReader>(MockBehavior.Strict);
            rosbagReaderMock
                .Setup(x => x.Read(ExpectedBagFileName))
                .Returns(Enumerable.Empty<IRecordedMessage>());

            RosbagReader.Instance = rosbagReaderMock.Object;
            var target = new RosbagScenario(ExpectedBagFileName);
            
            target.Messages.Should().NotBeNull();
        }
        
        [Fact]
        public void Load_is_only_called_once()
        {
            const string ExpectedBagFileName = "filepath.bag";
            
            var rosbagReaderMock = new Mock<IRosbagReader>(MockBehavior.Strict);
            rosbagReaderMock
                .Setup(x => x.Read(ExpectedBagFileName))
                .Returns(Enumerable.Empty<IRecordedMessage>());

            RosbagReader.Instance = rosbagReaderMock.Object;
            var target = new RosbagScenario(ExpectedBagFileName);

            target.Messages.Should().NotBeNull();
            
            rosbagReaderMock.Verify(x => x.Read(ExpectedBagFileName), Times.Once);
        }
    }
}