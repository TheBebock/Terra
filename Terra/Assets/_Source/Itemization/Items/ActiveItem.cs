using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.Itemization.Items
{
    [Serializable]
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