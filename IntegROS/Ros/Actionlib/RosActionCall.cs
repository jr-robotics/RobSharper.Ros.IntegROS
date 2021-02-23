﻿using System;
using System.Collections.Generic;
using System.Linq;
using IntegROS.Ros.Messages;

namespace IntegROS.Ros.Actionlib
{
    public class RosActionCall
    {
        private readonly IRecordedMessage<ActionGoal> _goal;
        private IRecordedMessage<ActionResult> _result;
        private readonly ActionCallCollection _actionCallCollection;
        private GoalStatusValue? _finalState;
        private List<Tuple<DateTime,GoalStatusValue>> _statusChanges;

        public string ActionName => _actionCallCollection.ActionName;
        public string GoalId => _goal.Value.GoalId.Id;

        public GoalStatusValue FinalState
        {
            get
            {
                if (_finalState == null)
                {
                    _finalState = StatusChanges
                        .Last()
                        .Item2;
                }
                
                return _finalState.Value;
            }
        }

        public IRecordedMessage<ActionGoal> GoalMessage => _goal;

        public IRecordedMessage<ActionResult> ResultMessage
        {
            get
            {
                if (_result == null)
                {
                    _result = _actionCallCollection
                        .ResultMessages
                        .FirstOrDefault(x => x.Value.GoalStatus.GoalId.Id == GoalId);
                }

                return _result;
            }
        }

        public IEnumerable<IRecordedMessage<ActionFeedback>> FeedbackMessages
        {
            get
            {
                return _actionCallCollection
                    .FeedbackMessages
                    .Where(x => x.Value.GoalStatus.GoalId.Id == GoalId);
            }
        }
        
        public IReadOnlyCollection<Tuple<DateTime, GoalStatusValue>> StatusChanges
        {
            get
            {
                if (_statusChanges == null)
                {
                    // Select all published statuses of the goal:
                    // 1) Select all timestamps and status of the goal (or null if the status message has no entry for the goal)
                    // 2) Select only those items where the status is not null (item belongs to the goal)
                    // 3) Group by Status
                    // 4) Select the first element of each group

                    // ReSharper disable once PossibleInvalidOperationException
                    _statusChanges = _actionCallCollection.StatusMessages
                        .Select(x => new
                        {
                            Stamp = x.Value.Header.Stamp,
                            Status = x.Value.StatusList.FirstOrDefault((y => y.GoalId.Id == GoalId))?.Status
                        })
                        .Where(x => x.Status.HasValue)
                        .GroupBy(x => x.Status)
                        .Select(g => g.First())
                        .Select(x => new Tuple<DateTime, GoalStatusValue>(x.Stamp, x.Status.Value))
                        .ToList();
                }
                
                return _statusChanges;
            }
        }

        public RosActionCall(IRecordedMessage<ActionGoal> goal, ActionCallCollection actionCallCollection)
        {
            _goal = goal ?? throw new ArgumentNullException(nameof(goal));
            _actionCallCollection = actionCallCollection ?? throw new ArgumentNullException(nameof(actionCallCollection));
        }
    }
}