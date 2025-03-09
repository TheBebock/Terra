using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups;
using JetBrains.Annotations;
using UnityEngine;

public class PickupContainer : MonoBehaviour, IPickupable
{
    public bool CanBePickedUp { get; private set; }
    
    [SerializeField] private Pickup pickup;
    [SerializeField] private List<CrystalPickup> crystalPickups = new List<CrystalPickup>();
    [SerializeField] private List<HealthPickup> healthPickups = new List<HealthPickup>();
    [SerializeField] private List<AmmoPickup> ammoPickups = new List<AmmoPickup>();
    
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
    
    public List<Pickup> GetAllPickups()
    {
        List<Pickup> allPickups = new List<Pickup>();

        allPickups.AddRange(crystalPickups);
        allPickups.AddRange(healthPickups);
        allPickups.AddRange(ammoPickups);

        return allPickups;
    }
    
}
