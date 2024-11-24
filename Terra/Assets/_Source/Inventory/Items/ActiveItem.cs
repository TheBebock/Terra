using Inventory.Items.Definitions;
using UnityEngine;

namespace Inventory.Items
{
    public class ActiveItem : Item
    {
        public ActiveItemData activeItemData;

        public void ActivateItem()
        {
            Debug.Log($"Activating {activeItemData.itemName}, Cooldown: {activeItemData.cooldown}s");
        }
    }
}