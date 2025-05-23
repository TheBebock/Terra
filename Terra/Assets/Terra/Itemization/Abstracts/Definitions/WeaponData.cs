using NaughtyAttributes;
using Terra.EffectsSystem.Abstract;
using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
{
    /// <summary>
    ///     Represents data for all weapons
    /// </summary>
    public abstract class WeaponData : ItemData 
    {
        public GameObject attackPrefab;
        public EffectsContainer effects;
        public int damage;
        public float attackCooldown;
        public AudioClip attackSFX;
        [ReadOnly] public WeaponType WeaponType;
    }

    public enum WeaponType
    {
        Melee,
        Ranged
    }
}