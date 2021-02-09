using System;

namespace IntegROS
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RosbagScenarioAttribute : Attribute, IHaveAScenario
    {
        private object _lock = new object();
        private RosbagScenario _scenario;
        public string Bagfile { get; }
        
        public RosbagScenario Scenario
        {
            get
            {
                if (_scenario == null)
                {
                    lock (_lock)
                    {
                        if (_scenario == null)
                        {
                            _scenario = new RosbagScenario(Bagfile);
                        }
                    }
                }

                return _scenario;
            }
        }

        IScenario IHaveAScenario.Scenario => Scenario;

        public RosbagScenarioAttribute(string bagfile)
        {
            Bagfile = bagfile;
        }
    }
}