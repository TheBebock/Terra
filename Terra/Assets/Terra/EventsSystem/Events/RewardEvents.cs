using Terra.RewardSystem;

namespace Terra.EventsSystem.Events
{
    public struct OnRewardSelected : IEvent
    {
        public StatsDataComparison comparison;
    }

    public struct OnRewardUnselected : IEvent
    {
        
    }
}
