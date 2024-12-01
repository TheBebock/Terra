using System;
using Inventory.Abstracts;
using Inventory.Items.Definitions;
using Player;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "PassiveItem_", menuName = "TheBebocks/Items/PassiveItem")]
    public class PassiveItem : Item
    {
        public PassiveItemData passiveItemData;

        private void OnValidate()
        {
            itemType = ItemType.Passive;
        }
    }
}