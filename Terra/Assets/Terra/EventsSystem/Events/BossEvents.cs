namespace Terra.EventsSystem.Events
{
    public struct OnBossSpawnedEvent : IEvent
    {
        public int bossHp;
    }
    public struct OnBossStartedMovingEvent : IEvent
    {
        
    }
    
    public struct OnBossDamagedEvent : IEvent
    {
        public int damage;
        public float normalizedDamage;
    }
    
    public struct OnBossPerformedNormalAttack : IEvent
    {
        
    }
    public struct OnBossDiedEvent : IEvent
    {
        
    }

    public struct OnBossCorpseInteractedEvent : IEvent
    {
        
    }
}

