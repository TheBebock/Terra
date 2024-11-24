using System.Collections.Generic;
using Generic;
using Inventory.Items;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviourSingleton<PlayerInventory>
    {
        private Item meleeWeaponSlot;
        private Item rangedWeaponSlot;
        private Item activeItemSlot;
        private List<Item> passiveItems = new List<Item>();
        
        public void EquipItem(Item newItem)
        {
            switch (newItem.data.itemType)
            {
                case "Melee":
                    EquipToSlot(ref meleeWeaponSlot, newItem);
                    break;
                case "Ranged":
                    EquipToSlot(ref rangedWeaponSlot, newItem);
                    break;
                case "Active":
                    EquipToSlot(ref activeItemSlot, newItem);
                    break;
                case "Passive":
                    passiveItems.Add(newItem);
                    break;
            }
            //TODO: PlayerStatsManager.Instance.AddStats(newItem.itemData.stats)   
        }

        public void UnEquipItem(Item newItem)
        {
            
            //TODO: PlayerStatsManager.Instance.Remove(newItem.itemData.stats)   
        }

        private void EquipToSlot(ref Item slot, Item newItem)
        {
            if (slot != null)
            {
                Debug.Log($"Slot already occupied. Unequipping {slot.data.itemName}");
                UnEquipItem(slot);
            }
            slot = newItem;
            Debug.Log($"Equipped {newItem.data.itemName}");
        }

        public Item GetMeleeWeapon() => meleeWeaponSlot;
        public Item GetRangedWeapon() => rangedWeaponSlot;
        public Item GetActiveItem() => activeItemSlot;
        public List<Item> GetPassiveItems() => passiveItems;

    }
    
}