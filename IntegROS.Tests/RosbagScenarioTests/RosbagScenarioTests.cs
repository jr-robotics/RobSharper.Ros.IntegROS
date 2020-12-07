using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace IntegROS.Tests.RosbagScenarioTests
{
    public class RosbagScenarioTests
    {
        [Fact]
        public void Accessing_messages_before_loading_them_causes_exception()
        {
            var target = new RosbagScenario();
            
            target.Invoking(s => s.Messages)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void If_no_bagfile_loaded_Bagname_is_null()
        {
            var target = new RosbagScenario();

            target.RosbagFile.Should().BeNull();
        }

        [Fact]
        public void Bagfile_name_is_set_on_load()
        {
            const string ExpectedBagFileName = "filepath.bag";
            RosbagReader.Instance = new Mock<IRosbagReader>().Object;
            
            var target = new RosbagScenario();

            target.Load(ExpectedBagFileName);
            
            target.RosbagFile.Should().Be(ExpectedBagFileName);
        }

        [Fact]
        public void Load_loads_messages()
        {
            const string ExpectedBagFileName = "filepath.bag";
            
            var rosbagReaderMock = new Mock<IRosbagReader>(MockBehavior.Strict);
            rosbagReaderMock
                .Setup(x => x.Read(ExpectedBagFileName))
                .Returns(Enumerable.Empty<RecordedMessage>());

            RosbagReader.Instance = rosbagReaderMock.Object;
            var target = new RosbagScenario();

            target.Load(ExpectedBagFileName);
            
            target.Messages.Should().NotBeNull();
        }
        
        [Fact]
        public void Load_is_only_called_once()
        {
            const string ExpectedBagFileName = "filepath.bag";
            
            var rosbagReaderMock = new Mock<IRosbagReader>(MockBehavior.Strict);
            rosbagReaderMock
                .Setup(x => x.Read(ExpectedBagFileName))
                .Returns(Enumerable.Empty<RecordedMessage>());

            RosbagReader.Instance = rosbagReaderMock.Object;
            var target = new RosbagScenario();

            target.Load(ExpectedBagFileName);
            target.Load(ExpectedBagFileName);
            
            rosbagReaderMock.Verify(x => x.Read(ExpectedBagFileName), Times.Once);
        }

        [Fact]
        public void Load_throws_exception_if_called_twice_for_different_bagfiles()
        {
            RosbagReader.Instance = new Mock<IRosbagReader>().Object;
            
            var target = new RosbagScenario();

            target.Load("rosbag1.bag");

            target.Invoking(t => t.Load("rosbagFAILED.bag"))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}