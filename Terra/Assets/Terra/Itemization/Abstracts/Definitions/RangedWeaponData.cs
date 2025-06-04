using Terra.Combat.Projectiles;

namespace Terra.Itemization.Abstracts.Definitions
{
    //NOTE: It should never have [CrateAssetMenu], creating new definitions is done through Resources -> ItemsDatabase
    public class RangedWeaponData : WeaponData
    {
        public int ammoCapacity;
        public BulletData bulletData;

        public override WeaponType WeaponType => WeaponType.Ranged;
    }
}