using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    [CreateAssetMenu(fileName = "PassiveItemData_", menuName = "TheBebocks/Items/PassiveItemData")]
    public class PassiveItemData : ItemData
    {
        public EffectType effectType;
        public float effectValue;
        public float duration; //Set to 0 if the effect is permanent
    }

    public enum EffectType
    {
        HealthRegen,
        SpeedBoost,
        DamageBoost,
        DefenseBoost
    }
}