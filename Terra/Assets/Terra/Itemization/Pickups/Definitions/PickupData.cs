using UnityEngine;

namespace Terra.Itemization.Pickups.Definitions
{
    
    public abstract class PickupData : ScriptableObject
    {
        [Range(1f, 100f)] public float dropRateChance = 20f;
        public AudioClip pickupSound;
        public string pickupName;
        public Sprite pickupSprite;
        public Material pickupMaterial;
    }
}

