using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;

namespace Terra.Itemization.Items
{
    /// <summary>
    /// Represents a passive item
    /// </summary>
    [Serializable]
    public class PassiveItem : Item<PassiveItemData>
    {

        public override ItemType ItemType => ItemType.Passive;

        /// <summary>
        /// Called on Update() when item is in inventory.
        /// </summary>
        public virtual void UpdateItem(){}

        public PassiveItem(PassiveItemData itemData)
        {
            Data = itemData;
        }


    }
}