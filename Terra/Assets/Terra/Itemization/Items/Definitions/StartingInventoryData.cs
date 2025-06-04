using System.Collections.Generic;
using UnityEngine;

namespace Terra.Itemization.Items.Definitions
{
    /// <summary>
    /// Represents player's starting inventory
    /// </summary>
    [CreateAssetMenu(fileName ="Inventory_", menuName = "TheBebocks/Inventory/StartingInventoryData")]
    public class StartingInventoryData : ScriptableObject
    {
        public int startingAmmo = 15;
        public MeleeWeapon startingMelee;
        public RangedWeapon startingRanged;
        public ActiveItem startingActive;
        public List<PassiveItem> startingPassiveItems;
        
    }

}
