using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Pickups.Definitions
{
    [CreateAssetMenu(fileName = "CrystalPickupData", menuName = "TheBebocks/Pickups/CrystalPickupData")]
    public class CrystalPickupData : PickupData
    {
        public int crystalAmount;
    }
}