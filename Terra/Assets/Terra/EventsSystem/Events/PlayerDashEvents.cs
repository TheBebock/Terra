namespace Terra.EventsSystem.Events
{
    public struct OnPlayerDashStartedEvent : IEvent
    {
    
    }

    public struct OnPlayerDashEndedEvent : IEvent
    {
    
    }

    public struct OnPlayerDashTimerProgressedEvent : IEvent
    {
        public float progress;
    }
}