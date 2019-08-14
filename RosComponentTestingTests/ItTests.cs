using FluentAssertions;
using RosComponentTesting;
using Xunit;

namespace RosComponentTestingTests
{
    public class ItTests
    {
        private class TestObject
        {
            public int Value { get; set; }
        }
        
        [Fact]
        public void It_IsAny_matches_any_object_of_type()
        {
            var item = new TestObject {Value = 15};
            var match = It.IsAny<TestObject>();

            var actual = match.Evaluate(item);

            actual.Should().BeTrue();
        }
        
        [Fact]
        public void It_IsAny_does_not_match_null()
        {
            TestObject item = null;
            var match = It.IsAny<TestObject>();

            var actual = match.Evaluate(item);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Match_value()
        {
            var item = new TestObject {Value = 15};
            var match = It.Matches<TestObject>(o => o.Value == 15);

            var actual = match.Evaluate(item);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Mismatch_value()
        {
            var item = new TestObject {Value = 15};
            var match = It.Matches<TestObject>(o => o.Value == 0);
            
            var actual = match.Evaluate(item);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Match_derived_object()
        {
            var item = new TestObject {Value = 15};
            var match = It.Matches<object>(o => true);

            var actual = match.Evaluate(item);

            actual.Should().BeTrue();
        }
    }
}