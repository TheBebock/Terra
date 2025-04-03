namespace Terra.Itemization.Items.Definitions
{
    public abstract class WeaponData : ItemData
    {
        public int damage;
        public float range;
        public float attackSpeed;
        public float knockback;
        public WeaponType WeaponType;
    }

    public enum WeaponType
    {
        Melee,
        Ranged
    }
}