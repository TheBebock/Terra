using Terra.Combat.Projectiles;
using Terra.Core.ModifiableValue;
using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
{
    //NOTE: It should never have [CrateAssetMenu], creating new definitions is done through Resources -> ItemsDatabase
    public class RangedWeaponData : WeaponData
    {
        public int ammoCapacity;
        public BulletData bulletData;

        public override WeaponType WeaponType => WeaponType.Ranged;
        
        protected override void OnValidate()
        {
            base.OnValidate();

            if (dexModifiers.Count == 0)
            {
                Debug.LogError("No dexterity modifiers assigned. Creating new dexterity modifier with base dexterity 10.");
                dexModifiers.Add(new ValueModifier(10, StatModType.Flat));
            }
            for (int i = dexModifiers.Count-1; i >= 0; i--)
            {
                if (dexModifiers[i].type != StatModType.PercentMult) continue;
                dexModifiers.RemoveAt(i);
                Debug.LogError($"Dexterity Modifiers of type {StatModType.PercentMult} are not allowed on ranged weapons! " +
                               $"Removing dexterity modifier.");
            }
        }
    }
}