using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra.Itemization.Items.Definitions
{
    //NOTE: It should never have [CrateAssetMenu], creating new definitions is done through Resources -> ItemsDatabase
    public class ActiveItemData : ItemData
    {
        public float itemCooldown;
        public AnimationClip activationAnimationClip;
        
        public virtual void ActivateItem(){}
    }
}