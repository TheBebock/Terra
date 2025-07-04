
using Terra.Enums;

namespace Terra.EventsSystem.Events
{
    public struct OnPlayerMeleeAttackPerformedEvent : IEvent
    {
        public FacingDirection facingDirection;
        
    }
    public struct OnPlayerMeleeAttackEndedEvent : IEvent
    {
            
    }
    
    public struct OnPlayerRangeAttackPerformedEvent : IEvent
    {

    }
    
    public struct OnPlayerRangeAttackEndedEvent : IEvent
    {
            
    }
}
