using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;

namespace Terra.Itemization.Items
{
    [Serializable]
    public class PassiveItem : Item
    {
        public PassiveItemData passiveItemData;

        //TODO: Possible update of passive items
        public virtual void UpdateItem(){}
    }
}