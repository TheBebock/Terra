using Terra.Itemization.Abstracts.Definitions;
using UnityEngine;

namespace Terra.EventsSystem.Events
{
    public struct OnWeaponsChangedEvent : IEvent
    {
        public Sprite weaponSprite;
        public WeaponType itemType;
    }
}
