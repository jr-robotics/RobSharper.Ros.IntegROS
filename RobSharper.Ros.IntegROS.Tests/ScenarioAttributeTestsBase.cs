using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests
{
    public abstract class ScenarioAttributeTestsBase
    {
        protected abstract void CreateIdenticalScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b);
        
        private void GetIdenticalScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b)
        {
            CreateIdenticalScenarioAttributes(out a, out b);

            a.DisplayName = "DISPLAY";
            a.Skip = "SKIP";

            b.DisplayName = "DISPLAY"; 
            b.Skip = "SKIP";
        }

        protected abstract void CreateDifferentScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b);
        
        private void GetDifferentScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b)
        {
            CreateDifferentScenarioAttributes(out a, out b);
            
            a.DisplayName = "DISPLAY";
            a.Skip = "SKIP";

            b.DisplayName = "DISPLAY"; 
            b.Skip = "SKIP";
        }

        
        [Fact]
        public void Two_attributes_are_equal_if_scenario_displayName_and_skip_are_the_same()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.Should().BeEquivalentTo(b);
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void Two_attributes_are_not_equal_if_scenario_differs()
        {
            GetDifferentScenarioAttributes(out var a, out var b);

            a.Should().NotBeEquivalentTo(b);
            b.Should().NotBeEquivalentTo(a);
        }

        [Fact]
        public void Two_attributes_are_not_equal_if_displayName_differs()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.DisplayName = "DISPLAY A";
            b.DisplayName = "DISPLAY B"; 

            a.Should().NotBeEquivalentTo(b);
            b.Should().NotBeEquivalentTo(a);
        }

        [Fact]
        public void Two_attributes_are_not_equal_if_skip_differs()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.Skip = "SKIP A";
            b.Skip = "SKIP B";

            a.Should().NotBeEquivalentTo(b);
            b.Should().NotBeEquivalentTo(a);
        }

        [Fact]
        public void Two_attributes_have_the_same_hash_code_if_scenario_displayName_and_skip_are_the_same()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void Two_attributes_have_not_the_same_hash_code_if_scenario_differs()
        {
            GetDifferentScenarioAttributes(out var a, out var b);

            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }

        [Fact]
        public void Two_attributes_have_not_the_same_hash_code_if_displayName_differs()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.DisplayName = "DISPLAY A";
            b.DisplayName = "DISPLAY B";

            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }

        [Fact]
        public void Two_attributes_have_not_the_same_hash_code_if_skip_differs()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.Skip = "SKIP A";
            b.Skip = "SKIP B";

            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }
        
        [Fact]
        public void Two_attributes_have_the_same_scenario_hash_code_if_skip_differs()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.Skip = "SKIP A";
            b.Skip = "SKIP B";

            a.GetScenarioHashCode().Should().Be(b.GetScenarioHashCode());
        }
        
        [Fact]
        public void Two_attributes_have_the_same_scenario_hash_code_if_display_name_differs()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.DisplayName = "DISPLAY A";
            b.DisplayName = "DISPLAY B"; 

            a.GetScenarioHashCode().Should().Be(b.GetScenarioHashCode());
        }
        
        
        
        [Fact]
        public void Scenario_equals_is_true_for_same_scenario_with_different_display_name()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.DisplayName = "DISPLAY A";
            b.DisplayName = "DISPLAY B";

            a.ScenarioEquals(b).Should().BeTrue();
            b.ScenarioEquals(a).Should().BeTrue();
        }
        
        [Fact]
        public void Scenario_equals_is_true_for_same_scenario_with_different_skip_value()
        {
            GetIdenticalScenarioAttributes(out var a, out var b);

            a.Skip = "SKIP A";
            b.Skip = "SKIP B";

            a.ScenarioEquals(b).Should().BeTrue();
            b.ScenarioEquals(a).Should().BeTrue();
        }
    }
}