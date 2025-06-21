using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
{
    
    /// <summary>
    /// Represents melee weapon definition
    /// </summary>
    //NOTE: It should never have [CrateAssetMenu], creating new definitions is done through Resources -> ItemsDatabase
    public class MeleeWeaponData : WeaponData
    {
        public GameObject attackPrefab;
        public override WeaponType WeaponType => WeaponType.Melee;
    }
}