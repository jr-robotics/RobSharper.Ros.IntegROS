using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.IntegROS.Ros.Actionlib
{
    public class ActionCallCollection : IEnumerable<RosActionCall>
    {
        public ActionMessagesCollection ActionMessages { get; }

        public ActionCallCollection(ActionMessagesCollection actionMessages)
        {
            ActionMessages = actionMessages ?? throw new ArgumentNullException(nameof(actionMessages));
        }

        public IEnumerator<RosActionCall> GetEnumerator()
        {
            if (ActionMessages.IsFullQualifiedActionNamePattern)
                return GetEnumeratorForFullQualifiedActionNamePattern();
            else
                return GetEnumeratorForRelativeActionNamePattern();
        }

        private IEnumerator<RosActionCall> GetEnumeratorForFullQualifiedActionNamePattern()
        {
            var goals = ActionMessages.GoalMessages.ToList();

            foreach (var goal in goals)
            {
                yield return new RosActionCall(goal, ActionMessages);
            }
        }

        private IEnumerator<RosActionCall> GetEnumeratorForRelativeActionNamePattern()
        {
            var goals = ActionMessages.GoalMessages.ToList();
            var globalActionMessages = new ConcurrentDictionary<string, ActionMessagesCollection>();

            foreach (var goal in goals)
            {
                var globalActionName = goal.Topic.Substring(0, goal.Topic.LastIndexOf("/", StringComparison.InvariantCulture));
                var actionMessages = globalActionMessages.GetOrAdd(globalActionName,
                    actionName => ActionMessagesCollection.Create(actionName, ActionMessages));
                
                yield return new RosActionCall(goal, actionMessages);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}