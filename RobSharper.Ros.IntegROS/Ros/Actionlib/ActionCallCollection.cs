using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionCallCollection : IEnumerable<RosActionCall>
    {
        public ActionMessagesCollection ActionMessages { get; }

        private bool PatternContainsPlaceholders { get; }

        public ActionCallCollection(ActionMessagesCollection actionMessages)
        {
            ActionMessages = actionMessages ?? throw new ArgumentNullException(nameof(actionMessages));
            PatternContainsPlaceholders = RosNameRegex.ContainsPlaceholders(actionMessages.ActionNamePattern);
        }

        public IEnumerator<RosActionCall> GetEnumerator()
        {
            var goals = ActionMessages.GoalMessages.ToList();

            foreach (var goal in goals)
            {
                ActionMessagesCollection actionMessages;
                
                if (PatternContainsPlaceholders)
                {
                    var globalActionName = goal.Topic.Substring(0, goal.Topic.LastIndexOf("/", StringComparison.InvariantCulture));
                    actionMessages = ActionMessagesCollection.Create(globalActionName, ActionMessages);
                }
                else
                {
                    actionMessages = ActionMessages;
                }

                yield return new RosActionCall(goal, actionMessages);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}