using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Represents a container for a single Pickup item type
/// </summary>
public class PickupContainer : MonoBehaviour, IPickupable
{
    public bool CanBePickedUp { get; private set; }
    
    [SerializeField] private Pickup pickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CanBePickedUp && pickup != null)
        {
            PickUp();
        }
    }
    public void PickUp()
    {
        if (!CanBePickedUp) return;
        pickup.OnPickUp();
        CanBePickedUp = false;
        Destroy(gameObject);
    }

    public void SetAvailability(bool isAvailable)
    {
        CanBePickedUp = isAvailable;
    }
}
