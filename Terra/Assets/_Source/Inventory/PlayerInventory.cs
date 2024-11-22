using System.Collections;
using System.Collections.Generic;
using Generic;
using Inventory.Items;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviourSingleton<PlayerInventory>
    {
        private Item meleeWeaponSlot;
        private Item rangedWeaponSlot;
        private Item activeItemSlot;
        private List<Item> passiveItems = new List<Item>();
        
        public void EquipItem(Item newItem)
        {
            
        }

        public void UnEquipItem()
        {
            
        }
    }
    
}