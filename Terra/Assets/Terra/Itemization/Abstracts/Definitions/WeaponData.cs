using Terra.EffectsSystem.Abstract;
using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
{
    /// <summary>
    ///     Represents data for all weapons
    /// </summary>
    public abstract class WeaponData : ItemData 
    {
        public EffectsContainer effects;
        public int damage;
        public float attackCooldown;
        public AudioClip attackSFX;
        public abstract WeaponType WeaponType { get; }
    }

    public enum WeaponType
    {
        Melee,
        Ranged
    }
}