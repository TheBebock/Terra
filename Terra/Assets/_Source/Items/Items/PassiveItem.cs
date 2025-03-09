using System;
using Inventory.Abstracts;
using Inventory.Items.Definitions;
using Player;
using UnityEngine;

namespace Inventory.Items
{
    [Serializable]
    public class PassiveItem : Item
    {
        public PassiveItemData passiveItemData;
        
    }
}