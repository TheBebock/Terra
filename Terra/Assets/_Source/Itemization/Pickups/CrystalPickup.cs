using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using UnityEngine;
using Terra.Managers;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class CrystalPickup : Pickup<CrystalPickupData>
    {
        
        public override PickupType PickupType => PickupType.Crystal;

        public override void OnPickUp()
        {
            Debug.Log($"Picked up Crystal: +{Data.crystalAmount}");
            EconomyManager.Instance?.ModifyCurrentGoldAmount(Data.crystalAmount);
        }
    }
}