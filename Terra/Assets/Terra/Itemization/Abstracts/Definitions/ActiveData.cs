using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
{
    //NOTE: It should never have [CrateAssetMenu], creating new definitions is done through Resources -> ItemsDatabase
    public class ActiveItemData : ItemData
    {
        public float itemCooldown;
        
        public virtual void ActivateItem(){}
    }
}