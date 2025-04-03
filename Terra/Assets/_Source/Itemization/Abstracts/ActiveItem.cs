using System;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.Itemization.Abstracts
{
    [Serializable]
    public class ActiveItem : Item<ActiveItemData>
    {
        public override ItemType ItemType => ItemType.Active;

        public ActiveItem(ActiveItemData itemData)
        {
            Data = itemData;
        }

        public void ActivateItem()
        {
            Debug.Log($"Activating {Data.itemName}, Cooldown: {Data.itemName}s");
        }
    }
}