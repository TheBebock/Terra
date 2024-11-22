using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items.Definitions;
using UnityEngine;

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