using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Interfaces;
using Terra.Itemization.Items.Definitions;

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
        public TItem EquippedItem { get; private set; }
        public override bool IsSlotTaken { get; set; }

        //NOTE: isSlotTaken == false means that EquippedItem is null, and it will throw a null reference.
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
            equipable.OnEquip();
            return true;
        }

        public override bool UnEquip()
        {
            if (!CanItemBeRemoved) return false;
            if(EquippedItem is not IEquipable equipable) return false;
            equipable.OnUnEquip();
            IsSlotTaken = false;
            EquippedItem = null;
            return true;
        }
    }
}