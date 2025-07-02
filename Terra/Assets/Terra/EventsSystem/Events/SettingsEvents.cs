using Terra.Enums;

namespace Terra.EventsSystem.Events
{
    public struct ItemsOpacityChangedEvent : IEvent
    {
        public float alfa;
    }

    public struct StatsOpacityChangedEvent : IEvent
    {
        public float alfa;
    }

    public struct GameDifficultyChangedEvent : IEvent
    {
        public GameDifficulty difficulty;
    }

    public struct SettingsClosedEvent : IEvent
    {
        
    }
}