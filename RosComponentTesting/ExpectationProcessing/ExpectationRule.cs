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

        public abstract void OnActivateExpectation();
        public abstract void OnDeactivateExpectation();
        public abstract void OnHandleMessage(TTopic message, ExpectationRuleContext context);
    }
}