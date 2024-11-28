using System;
using Inventory.Abstracts;

namespace Inventory
{

//Note: ItemSlot cannot be a struct, because casting it to correct class, for example as ItemSlot<MeleeWeapon> is costly
//involving a wrapper and a heap. 
    
    [Serializable]
    public class ItemSlot<T> : ItemSlotBase
        where T : Item
    {
        public T EquippedItem { get; private set; }
        public override bool IsSlotTaken { get; set; }

        //NOTE: isSlotTaken == false means that EquippedItem is null, and it will throw a null reference.
        //That's why it just returns false
        public bool CanItemBeRemoved => IsSlotTaken ? EquippedItem.data.canBeRemoved : false;


        public override bool CanEquip()
        {
            return IsSlotTaken ? EquippedItem.data.canBeRemoved : true;
        }

        public override bool Swap(Item item)
        {
            if(!UnEquip()) return false;
            if(!Equip(item)) return false;
            return true;
        }

        public override bool Equip(Item newItem)
        {
            if(newItem == null) return false;
            if(newItem is not T item) return false;
            if (!CanEquip()) return false;
            IsSlotTaken = true;
            EquippedItem = item;
            EquippedItem.OnEquip();
            return true;
        }

        public override bool UnEquip()
        {
            if (!CanItemBeRemoved) return false;
            EquippedItem?.OnUnEquip();
            IsSlotTaken = false;
            EquippedItem = null;
            return true;
        }
    }
}