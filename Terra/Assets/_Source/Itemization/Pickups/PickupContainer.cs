using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using UnityEngine;

namespace Terra.Itemization.Pickups
{

    /// <summary>
    /// Represents a container for a single Pickup item type
    /// </summary>
    public class PickupContainer : MonoBehaviour, IPickupable
    {
        public bool CanBePickedUp { get; private set; } = false;

        [SerializeField, ReadOnly] private PickupBase pickup;
        [SerializeField] private SpriteRenderer pickupRenderer;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && CanBePickedUp && pickup != null)
            {
                PickUp();
            }
        }

        public void Initialize(PickupBase pickup)
        {
            this.pickup = pickup;
            pickupRenderer.sprite = pickup.ItemIcon;
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
}