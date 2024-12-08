using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Pickups.Definitions
{
    [CreateAssetMenu(fileName = "AmmoPickupData", menuName = "TheBebocks/Pickups/AmmoPickupData")]
    public class AmmoPickupData : PickupData
    {
        public int ammoAmount;
    }
}