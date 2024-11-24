using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    [CreateAssetMenu(fileName = "ActiveItemData_", menuName = "TheBebocks/Items/ActiveItemData")]
    public class ActiveItemData : ItemData
    {
        public float cooldown;
        public string effectDescription;
        public AnimationClip activationAnimationClip;
    }
}