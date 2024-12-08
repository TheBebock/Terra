using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PickupContainer : MonoBehaviour, IPickupable
{
    public bool CanBePickedUp { get; private set; }
    
    [SerializeField] private Pickup pickup;
    [SerializeField] private List<Pickup> crystalPickups = new List<Pickup>();
    [SerializeField] private List<Pickup> healthPickups = new List<Pickup>();
    [SerializeField] private List<Pickup> ammoPickups = new List<Pickup>();
    
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
