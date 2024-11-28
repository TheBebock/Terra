using System;
using Inventory.Abstracts;
using Inventory.Items.Definitions;
using UnityEngine;

namespace Inventory.Items
{
    public class PassiveItem : Item
    {
        public PassiveItemData passiveItemData;

        private void OnValidate()
        {
            itemType = ItemType.Passive;
        }
    }
    
}