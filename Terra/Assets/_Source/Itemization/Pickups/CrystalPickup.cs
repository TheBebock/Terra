using UnityEngine;
using Inventory.Pickups.Definitions;
using Terra.Managers;

namespace Inventory.Pickups
{
    public class CrystalPickup : Pickup
    {
        [SerializeField] private CrystalPickupData data;

        public override PickupType PickupType { get; protected set;} = PickupType.Crystal;

        public override void OnPickUp()
        {
            Debug.Log($"Picked up Crystal: +{data.crystalAmount}");
            EconomyManager.Instance.ModifyCurrentGoldAmount(data.crystalAmount);
        }
    }
}