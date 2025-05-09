using NaughtyAttributes;
using Terra.EffectsSystem;


namespace Terra.Itemization.Items.Definitions
{
    /// <summary>
    ///     Reperesents data for all weapons
    /// </summary>
    public abstract class WeaponData : ItemData
    {
        public EffectsContainer effects = new ();
        public int damage;
        public float range;
        public float attackSpeed;
        [ReadOnly] public WeaponType WeaponType;
    }

    public enum WeaponType
    {
        Melee,
        Ranged
    }
}