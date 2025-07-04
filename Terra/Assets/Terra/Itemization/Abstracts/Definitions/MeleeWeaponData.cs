using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
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
        
        protected override void OnValidate()
        {
            base.OnValidate();

            ValidateEffects();

            ValidateModifiers();
        }

        private void ValidateModifiers()
        {
            for (int i = strengthModifiers.Count-1; i >= 0; i--)
            {
                if (strengthModifiers[i].type != StatModType.PercentMult) continue;
                strengthModifiers.RemoveAt(i);
                Debug.LogError($"Strength Modifiers of type {StatModType.PercentMult} are not allowed on melee weapons! " +
                               $"Removing strength modifier of type {strengthModifiers[i].type}");
            }
            
            if (strengthModifiers.Count == 0)
            {
                Debug.LogError("No strength modifiers assigned. Creating new strength modifier with base strength 10.");
                strengthModifiers.Add(new ValueModifier(10, StatModType.Flat));
            }
           
        }
        private void ValidateEffects()
        {
            for (int i = effects.statuses.Count-1; i >= effects.statuses.Count; i--)
            {
                var status = effects.statuses[i];
                if(status.containerType is ContainerType.MeleeWeapon or ContainerType.AllWeapons) continue;
                
                Debug.LogError($"{this}: {status.effectName} has invalid container type {status.containerType}." +
                               $"\nOnly {ContainerType.MeleeWeapon} or {ContainerType.AllWeapons} can be added to Melee Weapons." +
                               $"\nRemoving the {status.effectName} from weapon");
                effects.statuses.RemoveAt(i);
            }
            
            for (int i = effects.actions.Count-1; i >= effects.actions.Count; i--)
            {
                var action = effects.actions[i];
                if(action.containerType is ContainerType.MeleeWeapon or ContainerType.AllWeapons) continue;
                
                Debug.LogError($"{this}: {action.effectName} has invalid container type {action.containerType}." +
                               $"\nOnly {ContainerType.MeleeWeapon} or {ContainerType.AllWeapons} can be added to Melee Weapons." +
                               $"\nRemoving the {action.effectName} from weapon");
                effects.actions.RemoveAt(i);
            }
        }
    }
}