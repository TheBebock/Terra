using System;
using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Interfaces;
using UnityEngine;

namespace Terra.Itemization.Items
{

//Note: ItemSlot cannot be a struct, because casting it to correct class, for example as ItemSlot<MeleeWeapon> is costly
//involving a wrapper and a heap. 
    
    /// <summary>
    /// Represents a single slot in the inventory for an item.
    /// </summary>
    /// <remarks>Does not support item stacks</remarks>
    [Serializable]
    public class ItemSlot<TItem> : ItemSlotBase<TItem>
    where TItem : ItemBase
    {
        [SerializeField, ReadOnly] private string itemName;
        public TItem EquippedItem { get; private set; }
        public override bool IsSlotTaken { get; set; }

        //NOTE: isSlotTaken == false means that EquippedItem is null, and it will throw a null reference if checked.
        //That's why it just returns false
        public bool CanItemBeRemoved => IsSlotTaken ? EquippedItem.CanBeRemoved : false;


        public override bool CanEquip()
        {
            return IsSlotTaken ? EquippedItem.CanBeRemoved : true;
        }

        public override bool Swap(TItem item)
        {
            if(!UnEquip()) return false;
            if(!Equip(item)) return false;
            return true;
        }

        public override bool Equip(TItem newItem)
        {
            if(newItem == null) return false;
            if(newItem is not IEquipable equipable) return false;
            if (!CanEquip()) return false;
            IsSlotTaken = true;
            EquippedItem = newItem;
            itemName = newItem.ItemName;
            equipable.OnEquip();
            return true;
        }

        public override bool UnEquip()
        {
            // Check can item be removed
            if (!CanItemBeRemoved) return false;
            //  Check is item equipable
            if(EquippedItem is not IEquipable equipable) return false;
            // Perform UnEquip on the item
            equipable.OnUnEquip();
            // Invoke event, that item has been removed
            InvokeOnItemRemoved(EquippedItem);
            // Change slot status
            IsSlotTaken = false;
            // Clear cached item
            EquippedItem = null;
            itemName = String.Empty;
            return true;
        }
        
        
    }
}