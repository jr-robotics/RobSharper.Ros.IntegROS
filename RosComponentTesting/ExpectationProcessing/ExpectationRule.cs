namespace RosComponentTesting.ExpectationProcessing
{
    public abstract class ExpectationRule<TTopic>
    {
        public int Priority { get; }
        
        public ExpectationRule(int priority)
        {
            Priority = priority;
        }
        
        public abstract void OnActivateExpectation();
        public abstract void OnDeactivateExpectation();
        public abstract void OnHandleMessage(TTopic message, ExpectationRuleContext context);
    }
}