
using Terra.Enums;

namespace Terra.EventsSystem.Events
{
    public struct OnPlayerMeleeAttackPerformedEvent : IEvent
    {
        public FacingDirection facingDirection;

        public OnPlayerMeleeAttackPerformedEvent(FacingDirection newFacingDirection)
        {
            facingDirection = newFacingDirection;
        }
    }
    
    public struct OnPlayerRangeAttackPerformedEvent : IEvent
    {

    }
}
