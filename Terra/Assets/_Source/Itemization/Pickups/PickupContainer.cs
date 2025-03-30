using Inventory.Pickups;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Represents a container for a single Pickup item type
/// </summary>
public class PickupContainer : MonoBehaviour, IPickupable
{
    public bool CanBePickedUp { get; private set; } = false;
    
    [SerializeField, ReadOnly] private Pickup pickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CanBePickedUp && pickup != null)
        {
            PickUp();
        }
    }

    public void Initialize(Pickup pickup)
    {
        this.pickup = pickup;
        CanBePickedUp = true;
    }
    public void PickUp()
    {
        if (!CanBePickedUp) return;
        pickup.OnPickUp();
        Destroy(gameObject);
    }

    public void SetAvailability(bool isAvailable)
    {
        CanBePickedUp = isAvailable;
    }
}
