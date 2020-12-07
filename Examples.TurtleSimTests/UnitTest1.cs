using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Examples.TurtleSimTests
{
    public class CrawlTests : IClassFixture<CrawlForwardScenario>
    {
        private CrawlForwardScenario Scenario { get; }
        
        public CrawlTests(CrawlForwardScenario scenario)
        {
            Scenario = scenario;
        }

        [Fact]
        public void CrawlForwardTest()
        {
            Scenario.Messages
                .Where(m => m.Topic == "/turtle1/pose")
                .Count()
                .Should().Be(0, "no pose should be published");
        }
    }
    
/*
    public class Crawl2Tests : ForScenario<CrawlForward>
    {
        [ExpectThat]
        public void No_pose_is_published()
        {
            
        }
    }

    public class Crawl3Tests : ForScenario
    {
        protected override void Setup()
        {
        }
        
        protected override void Exercise()
        {
        }
        
        protected override void Teardown()
        {
        }
        
        [ExpectThat]
        public void No_pose_is_published()
        {
            
        }
    }
*/
}