namespace IntegROS.Ros.Messages
{
    public static class GoalStatusValueExtensions
    {
        public static bool IsPending(this GoalStatusValue status)
        {
            switch (status)
            {
                case GoalStatusValue.Pending:
                case GoalStatusValue.Recalling:
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool IsActive(this GoalStatusValue status)
        {
            switch (status)
            {
                case GoalStatusValue.Active:
                case GoalStatusValue.Preempting:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsDone(this GoalStatusValue status)
        {
            switch (status)
            {
                case GoalStatusValue.Succeeded:
                case GoalStatusValue.Preempted:
                case GoalStatusValue.Aborted:
                case GoalStatusValue.Rejected:
                case GoalStatusValue.Recalled:
                case GoalStatusValue.Lost:
                    return true;
                default:
                    return false;
            }
        }
    }
}