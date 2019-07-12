using System;

namespace RosComponentTestingTests.Util
{
    public class RosNodeSetup
    {
        public RosNodeSetup Launch(string launchfile)
        {
            // TODO set launchfile
            return this;
        }

        public RosNodeSetup Wait(TimeSpan duration)
        {
            // TODO
            return this;
        }

        public RosNodeSetup WaitFor<T>(Action<ExpectationBuilder<T>> builderAction)
        {
            var builder = new ExpectationBuilder<T>();
            builderAction(builder);
            
            // TODO: Apply builder

            return this;
        }

        public RosNodeContext StartNode()
        {
            // TODO
            return new RosNodeContext();
        }
    }
}