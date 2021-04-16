using System;
using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests
{
    public class TopicRegexTests
    {
        [Fact]
        public void Null_topic_name_is_not_allowed()
        {
            const string pattern = null;

            this.Invoking(x => RosNameRegex.Create(pattern))
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Empty_topic_name_is_not_allowed()
        {
            this.Invoking(x => RosNameRegex.Create(String.Empty))
                .Should()
                .Throw<InvalidTopicPatternException>();
        }
        
        [Fact]
        public void Relative_topic_name_is_not_allowed()
        {
            const string pattern = "bar";

            this.Invoking(x => RosNameRegex.Create(pattern))
                .Should()
                .Throw<InvalidTopicPatternException>();
        }
        
        [Fact]
        public void Relative_namespace_topic_name_is_not_allowed()
        {
            const string pattern = "fooo/bar";

            this.Invoking(x => RosNameRegex.Create(pattern))
                .Should()
                .Throw<InvalidTopicPatternException>();
        }
        
        [Theory]
        [InlineData("*fooo")]
        [InlineData("*/fooo")]
        [InlineData("bar/")]
        [InlineData("/bar/")]
        [InlineData("/fooo/*/")]
        [InlineData("/fooo/bar/")]
        [InlineData("")]
        [InlineData(" ")]
        public void Invalid_patterns_throw_exception(string pattern)
        {
            this.Invoking(x => RosNameRegex.Create(pattern))
                .Should()
                .Throw<InvalidTopicPatternException>();
        }
        
        [Theory]
        [InlineData("/fooo/bar", false)]
        [InlineData("/fooo1/bar", false)]
        [InlineData("/fooo_2/bar", false)]
        [InlineData("/bar", true)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Match_global_topic(string topic, bool shouldMatch)
        {
            const string pattern = "/bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo1/bar", false)]
        [InlineData("/fooo_2/bar", false)]
        [InlineData("/bar", false)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Match_lobal_namespace_and_topic(string topic, bool shouldMatch)
        {
            const string pattern = "/fooo/bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo1/bar", true)]
        [InlineData("/fooo_2/bar", true)]
        [InlineData("/bar", false)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Topic_in_a_namespace(string topic, bool shouldMatch)
        {
            const string pattern = "/*/bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", false)]
        [InlineData("/fooo1/bar", false)]
        [InlineData("/fooo_2/bar", false)]
        [InlineData("/bar", false)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", true)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Match_topic_in_a_sub_namespace(string topic, bool shouldMatch)
        {
            const string pattern = "/*/*/bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", false)]
        [InlineData("/fooo1/bar", false)]
        [InlineData("/fooo_2/bar", false)]
        [InlineData("/bar", true)]
        [InlineData("/bar1", true)]
        [InlineData("/bar_2", true)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Match_partial_topic_placeholder(string topic, bool shouldMatch)
        {
            const string pattern = "/bar*";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", false)]
        [InlineData("/fooo1/bar", false)]
        [InlineData("/fooo_2/bar", false)]
        [InlineData("/bar", true)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", true)]
        [InlineData("/foo/nobar", false)]
        public void Match_partial_topic_prefix_placeholder(string topic, bool shouldMatch)
        {
            const string pattern = "/*bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo1/bar", true)]
        [InlineData("/fooo_2/bar", true)]
        [InlineData("/bar", false)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Match_partial_namespace_placeholder(string topic, bool shouldMatch)
        {
            const string pattern = "/fo*/bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo1/bar", true)]
        [InlineData("/fooo_2/bar", true)]
        [InlineData("/bar", true)]
        [InlineData("/bar1", true)]
        [InlineData("/bar_2", true)]
        [InlineData("/fo/foo/bar", true)]
        [InlineData("/nobar", true)]
        [InlineData("/foo/nobar", true)]
        public void Any_placeholder_matches_all(string topic, bool shouldMatch)
        {
            const string pattern = "**";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo1/bar", true)]
        [InlineData("/fooo_2/bar", true)]
        [InlineData("/bar", true)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", true)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Any_prefix_placeholder_matches_all_namespaces(string topic, bool shouldMatch)
        {
            const string pattern = "**/bar";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo1/bar", true)]
        [InlineData("/fooo_2/bar", true)]
        [InlineData("/bar", true)]
        [InlineData("/bar1", true)]
        [InlineData("/bar_2", true)]
        [InlineData("/fo/foo/bar", true)]
        [InlineData("/nobar", true)]
        [InlineData("/foo/nobar", true)]
        public void Any_postfix_placeholder_matches_all(string topic, bool shouldMatch)
        {
            const string pattern = "/**";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        [Theory]
        [InlineData("/fooo/bar", true)]
        [InlineData("/fooo/bar2", true)]
        [InlineData("/fooo1/bar", false)]
        [InlineData("/fooo_2/bar", false)]
        [InlineData("/bar", false)]
        [InlineData("/bar1", false)]
        [InlineData("/bar_2", false)]
        [InlineData("/fo/foo/bar", false)]
        [InlineData("/nobar", false)]
        [InlineData("/foo/nobar", false)]
        public void Any_postfix_placeholder_matches_all_topics_in_namespace(string topic, bool shouldMatch)
        {
            const string pattern = "/fooo/**";

            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
        
        [Theory]
        [InlineData("/fooo/bar", "/fooo/**", true)]
        [InlineData("/fooo/bar2", "/fooo/**", true)]
        [InlineData("/fooo1/bar", "/fooo/**", false)]
        [InlineData("/fooo_2/bar", "/fooo/**", false)]
        [InlineData("/bar", "/fooo/**", false)]
        [InlineData("/bar1", "/fooo/**", false)]
        [InlineData("/bar_2", "/fooo/**", false)]
        [InlineData("/fo/foo/bar", "/fooo/**", false)]
        [InlineData("/nobar", "/fooo/**", false)]
        [InlineData("/foo/nobar", "/fooo/**", false)]
        [InlineData("/foo/bar", "/f**", true)]
        [InlineData("/foo/bar", "**bar", true)]
        [InlineData("/foo1/a", "/f*/a", true)]
        [InlineData("/foo2/a", "/f**/a", true)]
        [InlineData("/foo3/a/b/c", "/f**/c", true)]
        [InlineData("/foo3/a/b/c", "/f**/x", false)]
        [InlineData("/foo/a/b", "/f**/*/b", true)]
        [InlineData("/foo/b", "/f**/*/b", false)]
        [InlineData("/foo/a/x/b", "/f**/*/b", true)]
        public void AnyPlaceholder_match_tests(string topic, string pattern, bool shouldMatch)
        {
            var regex = RosNameRegex.Create(pattern);

            regex.IsMatch(topic).Should().Be(shouldMatch);
        }
        
    }
}