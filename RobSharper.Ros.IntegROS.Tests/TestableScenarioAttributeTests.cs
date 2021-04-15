using System;

namespace RobSharper.Ros.IntegROS.Tests
{
    public class TestableScenarioAttributeTests : ScenarioAttributeTestsBase
    {
        private class TestableScenarioAttribute : ScenarioAttribute
        {
            private readonly Guid _value;

            public TestableScenarioAttribute(Guid value)
            {
                _value = value;
            }
            
            public override int GetScenarioHashCode()
            {
                return HashCode.Combine(_value);
            }
        }

        protected override void CreateIdenticalScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b)
        {
            var g = new Guid("6A94DF01-FA7B-4ADD-9552-F886C07859F5");
            
            a = new TestableScenarioAttribute(g);
            b = new TestableScenarioAttribute(g);
        }

        protected override void CreateDifferentScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b)
        {
            a = new TestableScenarioAttribute(new Guid("37358A11-3CE3-47B6-8FB2-ECF51B161FC5"));
            b = new TestableScenarioAttribute(new Guid("035B35D8-6A5F-4418-AA3F-7B0377248ADC"));
        }
    }
}