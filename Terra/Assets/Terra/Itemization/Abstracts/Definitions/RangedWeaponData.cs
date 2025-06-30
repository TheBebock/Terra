using Terra.Combat.Projectiles;
using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstract.Definitions;
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
            
            ValidateEffects();
            
           ValidateModifiers();

            if (bulletData.bulletDamage > 0)
            {
                Debug.LogError("Bullet damage on player based range weapons needs to be 0, " +
                               "as the damage is taken directly from player's Dexterity statistic");
                bulletData.bulletDamage = 0;
            }
           
        }

        private void ValidateModifiers()
        {
            for (int i = dexModifiers.Count-1; i >= 0; i--)
            {
                if (dexModifiers[i].type != StatModType.PercentMult) continue;
                dexModifiers.RemoveAt(i);
                Debug.LogError($"Dexterity Modifiers of type {StatModType.PercentMult} are not allowed on ranged weapons! " +
                               $"Removing dexterity modifier.");
            }
            
            if (dexModifiers.Count == 0)
            {
                Debug.LogError("No dexterity modifiers assigned. Creating new dexterity modifier with base dexterity 10.");
                dexModifiers.Add(new ValueModifier(10, StatModType.Flat));
            }
        }
        private void ValidateEffects()
        {
            for (int i = effects.statuses.Count-1; i >= effects.statuses.Count; i--)
            {
                var status = effects.statuses[i];
                if(status.containerType is ContainerType.RangedWeapon or ContainerType.AllWeapons) continue;
                
                Debug.LogError($"{this}: {status.effectName} has invalid container type {status.containerType}." +
                               $"Only {ContainerType.RangedWeapon} or {ContainerType.AllWeapons} can be added to Ranged Weapons." +
                               $"Removing the {status.effectName} from weapon");
                effects.statuses.RemoveAt(i);
            }
            
            for (int i = effects.actions.Count-1; i >= effects.actions.Count; i--)
            {
                var action = effects.actions[i];
                if(action.containerType is ContainerType.RangedWeapon or ContainerType.AllWeapons) continue;
                
                Debug.LogError($"{this}: {action.effectName} has invalid container type {action.containerType}." +
                               $"Only {ContainerType.RangedWeapon} or {ContainerType.AllWeapons} can be added to Ranged Weapons." +
                               $"Removing the {action.effectName} from weapon");
                effects.actions.RemoveAt(i);
            }
        }
    }
}