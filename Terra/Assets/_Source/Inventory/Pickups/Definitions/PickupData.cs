using UnityEngine;

namespace Inventory.Pickups.Definitions
{
    public enum PickupType
    {
         Health = 0,
         Ammo = 1,
         Crystal = 2
         
    }
    public abstract class PickupData : ScriptableObject
    {
        public string pickupName;
        public Sprite pickupSprite;
    }
}

