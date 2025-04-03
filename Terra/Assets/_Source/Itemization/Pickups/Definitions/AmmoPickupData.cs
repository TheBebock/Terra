using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra.Itemization.Pickups.Definitions
{
    [CreateAssetMenu(fileName = "AmmoPickupData", menuName = "TheBebocks/Pickups/AmmoPickupData")]
    public class AmmoPickupData : PickupData
    {
        public int ammoAmount;
    }
}