using System;
using RosComponentTesting.Util;
using Xunit;

namespace RosComponentTesting
{
    public class UnitTest1 : IDisposable
    {
        private readonly IDisposable _executionContext;

        public UnitTest1()
        {
            _executionContext = new RosNodeSetup()
                .Launch("LaunchFile.launch")
                .Wait(TimeSpan.FromSeconds(5))
                .WaitFor<ExpectedType>(x => x
                    .Topic("GoToTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                    .Timeout(TimeSpan.FromSeconds(10))
                )
                .StartNode();
        }

        public void Dispose()
        {
            _executionContext.Dispose();
        }
        
        
        
        [Fact]
        public void SimpleTest()
        {
            object message = null;

            // When Publish message on AdvertiseTopic
            // Expect ExpectType on SubscriberTopic
            //   with value == "x"
            
            new RosTestBuilder()
                .Publish("AdvertiseTopic", message)
                .Expect<ExpectedType>("SubscribeTopic", m => m.Value == "x")
                .Execute();
        }
        
        [Fact]
        public void SimpleTest3()
        {
            object message = null;

            // Same as Simple Test 1
            
            // When Publish message on AdvertiseTopic
            // Expect ExpectType on SubscriberTopic
            //   with value == "x"
            
            new RosTestBuilder()
                .Publish("AdvertiseTopic", message)
                .Expect<ExpectedType>("SubscribeTopic", func: It.Matches<ExpectedType>(m => m.Value == "x"))
                .Execute();
        }

        [Fact]
        public void SimpleTest2()
        {
            
            // When Publish message on AdvertiseTopic
            // Expect any ExpectType on SubscriberTopic1
            // AND Expect any ExpectType on SubscriberTopic2
            
            new RosTestBuilder()
                .Publish("AdvertiseTopic", new object())
                .Expect<ExpectedType>("SubscriberTopic1", It.IsAny<ExpectedType>())
                .Expect<ExpectedType>("SubscriberTopic2", It.IsAny<ExpectedType>())
                .Execute();
        }

        [Fact]
        public void ComplexExpectation()
        {
            new RosTestBuilder()
                .Publish("StartMovement", new object())
                .Expect<ExpectedType>(x => x
                        .Topic("GoToTopic")
                        .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                        .Timeout(TimeSpan.FromSeconds(10))
                        .Occurrences(Times.Once())
                )
                .Execute();
        }

        [Fact]
        public void DependentExpectations()
        {
            string pos = null;
            
            // Dependent expectations
            new RosTestBuilder()
                .Publish("StartMovement", new object())
                .Expect<ExpectedType>(x => x
                    .Name("GotoMessage")
                    .Topic("GoToTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                    .Callback(m => pos = m.Value)
                    .Occurrences(Times.Once())
                )
                .Expect<ExpectedType>(x => x
                    .DependsOn("GotoMessage")
                    .Topic("DestinationReachedTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value == pos))
                    .Occurrences(Times.Once())
                )
                .Execute();
        }

        [Fact]
        public void WaitForEvent()
        {
            new RosTestBuilder()
                .Publish("StartMovement", new object())
                .Expect<ExpectedType>(x => x
                    .Name("GotoMessage")
                    .Topic("GoToTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                    .Timeout(TimeSpan.FromSeconds(10))
                    .Occurrences(Times.Once())
                )
                .Expect<ExpectedType>(x => x
                    .Name("PositionPrecondition")
                    .Topic("CurrentPosition")
                    .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                    .Timeout(TimeSpan.FromSeconds(10))
                )
                .Expect<ExpectedType>(x => x
                    .DependsOn("GotoMessage")
                    .DependsOn("PositionPrecondition")
                    .Topic("DestinationReachedTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                    .Occurrences(Times.Once())
                )
                .Execute();
        }

        [Fact]
        public void NoOtherPublications()
        {
            new RosTestBuilder()
                .Publish("StartMovement", new object())
                .Expect<ExpectedType>(x => x
                    .Topic("GoToTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value == "x"))
                    .Timeout(TimeSpan.FromSeconds(10))
                    .Occurrences(Times.Once())
                )
                .Expect<ExpectedType>(x => x        // Reject all Publications on Topic "GoToTopic" (except value == x).
                    .Topic("GoToTopic")
                    .Match(It.Matches<ExpectedType>(m => m.Value != "x"))
                    .Occurrences(Times.Never())
                )
                .Expect<ExpectedType>(x => x        // Reject all Publications on Topic "OtherTopic"
                    .Topic("OtherTopic")
                    .Match(It.IsAny<ExpectedType>())
                    .Occurrences(Times.Never())
                )
                .Execute();
        }
    }
}