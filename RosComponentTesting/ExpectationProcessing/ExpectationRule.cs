using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public abstract class ExpectationRule<TTopic>
    {
        public int Priority { get; }

        public CallerReference CallerInfo { get;}

        public ExpectationRule(int priority) : this(null, priority)
        {
        }

        protected ExpectationRule(CallerReference callerInfo, int priority)
        {
            Priority = priority;
            CallerInfo = callerInfo;
        }

        public virtual void OnActivateExpectation()
        {
            // Nothing to do in the default implementation
        }

        public virtual void OnDeactivateExpectation()
        {
            // Nothing to do in the default implementation
        }
        
        public abstract void OnHandleMessage(TTopic message, ExpectationRuleContext context);
    }
}