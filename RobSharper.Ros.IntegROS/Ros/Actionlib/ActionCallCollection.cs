using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IntegROS.Ros.Actionlib
{
    public class ActionCallCollection : IEnumerable<RosActionCall>
    {
        public ActionMessages ActionMessages { get; }

        public ActionCallCollection(ActionMessages actionMessages)
        {
            ActionMessages = actionMessages ?? throw new ArgumentNullException(nameof(actionMessages));
        }

        public IEnumerator<RosActionCall> GetEnumerator()
        {
            var goals = ActionMessages.GoalMessages.ToList();

            foreach (var goal in goals)
            {
                yield return new RosActionCall(goal, ActionMessages);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}