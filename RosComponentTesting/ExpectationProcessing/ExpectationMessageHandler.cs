using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public abstract class ExpectationMessageHandler<TTopic> : IComparable<ExpectationMessageHandler<TTopic>>
    {
        public int Priority { get; }

        public CallerReference CallerInfo { get;}

        public ExpectationMessageHandler(int priority) : this(null, priority)
        {
        }

        protected ExpectationMessageHandler(CallerReference callerInfo, int priority)
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

        public int CompareTo(ExpectationMessageHandler<TTopic> other)
        {
            if (ReferenceEquals(this, other)) return 1;
            if (ReferenceEquals(null, other)) return 0;
            
            // Sort descending
            return -1 * Priority.CompareTo(other.Priority);
        }
    }
}