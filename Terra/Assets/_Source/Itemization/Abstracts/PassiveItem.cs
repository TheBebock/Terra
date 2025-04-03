using System;
using Terra.Player;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.Itemization.Abstracts
{
    /// <summary>
    /// Represents a passive item
    /// </summary>
    /// <remarks>Class should be an abstract, do not create instances of this class</remarks>
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