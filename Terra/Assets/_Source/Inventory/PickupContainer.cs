using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups;
using UnityEngine;

public class PickupContainer : MonoBehaviour, IPickupable
{
    public bool CanBePickedUp { get; private set; }

    [SerializeField] Pickup pickup;

    public void PickUp()
    {
        pickup.OnPickUp();
    }

    public void SetAvailability(bool isAvailable)
    {
        CanBePickedUp = isAvailable;
    }
    
}
