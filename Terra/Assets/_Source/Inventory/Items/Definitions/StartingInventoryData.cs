using System.Collections.Generic;
using UnityEngine;


namespace Inventory.Items.Definitions
{
    [CreateAssetMenu(fileName ="Inventory_", menuName = "TheBebocks/Inventory/StartingInventoryData")]
    public class StartingInventoryData : ScriptableObject
    {
        public MeleeWeapon startingMelee;
        public RangedWeapon startingRanged;
        public ActiveItem startingActive;
        public List<PassiveItem> startingPassiveItems;
    }

}
