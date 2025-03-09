namespace Inventory.Items.Definitions
{
    public abstract class WeaponData : ItemData
    {
        public int damage;
        public float range;
        public float attackSpeed;
        public WeaponType WeaponType;
    }

    public enum WeaponType
    {
        Melee,
        Ranged
    }
}