using Terra.Core.ModifiableValue;
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

            if (strengthModifiers.Count == 0)
            {
                Debug.LogError("No strength modifiers assigned. Creating new strength modifier with base strength 10.");
                strengthModifiers.Add(new ValueModifier(10, StatModType.Flat));
            }
            for (int i = strengthModifiers.Count-1; i >= 0; i--)
            {
                if (strengthModifiers[i].type != StatModType.PercentMult) continue;
                strengthModifiers.RemoveAt(i);
                Debug.LogError($"Strength Modifiers of type {StatModType.PercentMult} are not allowed on melee weapons! " +
                               $"Removing strength modifier of type {strengthModifiers[i].type}");
            }
        }
    }
}