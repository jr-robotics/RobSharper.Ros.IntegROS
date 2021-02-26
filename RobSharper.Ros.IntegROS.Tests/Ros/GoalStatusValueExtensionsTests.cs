using System;
using System.Collections.Generic;
using System.Linq;
using RobSharper.Ros.IntegROS.Ros.Messages;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests.Ros
{
    public class GoalStatusValueExtensionsTests
    {
        public static IEnumerable<object[]> GetGoalStatusValues()
        {
            var values = Enum.GetValues(typeof(GoalStatusValue)).Cast<GoalStatusValue>();

            foreach (var goalStatusValue in values)
            {
                yield return new object[] {goalStatusValue};
            }
        }
        
        [Theory]
        [MemberData(nameof(GetGoalStatusValues))]
        public void All_GoalStatusValues_are_either_pending_active_or_done(GoalStatusValue value)
        {

            var isPending = value.IsPending();
            var isActive = value.IsActive();
            var isDone = value.IsDone();
            
            Assert.True(isActive || isPending || isDone);
        }
    }
}