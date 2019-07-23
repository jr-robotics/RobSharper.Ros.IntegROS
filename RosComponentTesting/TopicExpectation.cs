using System;
using System.Collections.Generic;
using System.Linq;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class TopicExpectation<TTopic> : ITopicExpectation
    {
        public enum StateValue
        {
            Inactive,
            Active
        };
        
        public string TopicName { get; set; }
        
        public Type TopicType { get; set; }
        
        public StateValue State { get; private set; }

        private List<ExpectationRule<TTopic>> _validators = new List<ExpectationRule<TTopic>>();

        public void ThrowIfNotInitialized()
        {
            if (string.IsNullOrEmpty(TopicName)) throw new InvalidOperationException("Topic name not set");
            if (TopicType == null) throw new InvalidOperationException("Topic type not set");
        }

        public virtual void Activate()
        {
            if (State == StateValue.Active) return;
            
            lock (_validators)
            {
                if (State == StateValue.Active) return;
                
                foreach (var validator in _validators)
                {
                    validator.OnActivateExpectation();
                }

                State = StateValue.Active;
            }
        }

        public virtual void Deactivate()
        {
            if (State == StateValue.Inactive) return;

            lock (_validators)
            {
                if (State == StateValue.Inactive) return;
                
                foreach (var validator in _validators)
                {
                    validator.OnDeactivateExpectation();
                }

                State = StateValue.Inactive;
            }
        }

        public void OnReceiveMessage(object message)
        {
            if (State != StateValue.Active) return;
            
            HandleMessage((TTopic) message);;
        }
        
        protected virtual void HandleMessage(TTopic message)
        {
            var context = new ExpectationRuleContext();
            
            lock (_validators)
            {
                foreach (var validator in _validators)
                {
                    validator.OnHandleMessage(message, context);

                    if (!context.Continue)
                        break;
                }
            }
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var context = new ValidationContext();
            lock (_validators)
            {
                foreach (var validationRule in _validators.OfType<IValidationRule>())
                {
                    validationRule.Validate(context);
                }
            }

            return context.Errors;
        }

        public void AddExpectationRule(ExpectationRule<TTopic> rule, bool isSingleton = false)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            lock (_validators)
            {
                if (isSingleton)
                {
                    _validators.RemoveAll(v => v.GetType() == rule.GetType());
                }

                _validators.Add(rule);
            }
        }
    }
}