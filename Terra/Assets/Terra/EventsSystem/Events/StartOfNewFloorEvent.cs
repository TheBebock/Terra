namespace Terra.EventsSystem.Events
{
    public struct StartOfNewFloorEvent : IEvent
    {
        
    }

    public struct LevelIncreasedEvent : IEvent
    {
        public int level;
    }
}
