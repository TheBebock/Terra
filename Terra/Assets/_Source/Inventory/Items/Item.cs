using System;
using Inventory.Items.Definitions;
using Player;

namespace Inventory.Items
{
    [Serializable]
    public abstract class Item : IEquipable<Item>
    {
        public ItemData data { get; private set; }
        public void Equip(Item item)
        {
            PlayerInventory.Instance.EquipItem(this);
        }
    }
}