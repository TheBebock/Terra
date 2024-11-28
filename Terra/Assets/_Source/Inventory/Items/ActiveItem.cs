using Inventory.Abstracts;
using Inventory.Items.Definitions;
using UnityEngine;

namespace Inventory.Items
{
    public class ActiveItem : Item
    {
        public ActiveItemData activeItemData;
        private void OnValidate()
        {
            itemType = ItemType.Active;
        }
        public void ActivateItem()
        {
            Debug.Log($"Activating {activeItemData.itemName}, Cooldown: {activeItemData.cooldown}s");
        }
    }
}