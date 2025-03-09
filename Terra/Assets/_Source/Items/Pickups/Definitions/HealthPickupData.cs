using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Pickups.Definitions
{
    [CreateAssetMenu(fileName = "HealthPickupData", menuName = "TheBebocks/Pickups/HealthPickupData")]
    public class HealthPickupData : PickupData
    {
        public int healthAmount;
    }
}