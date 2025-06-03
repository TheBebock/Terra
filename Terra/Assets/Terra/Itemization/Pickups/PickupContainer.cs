using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Itemization.Abstracts;
using UnityEngine;

namespace Terra.Itemization.Pickups
{

    /// <summary>
    /// Represents a container for a single Pickup item type
    /// </summary>
    public sealed class PickupContainer : InGameMonobehaviour, IPickupable
    {
        public bool CanBePickedUp { get; private set; } = false;

        [SerializeField, ReadOnly] private PickupBase _pickup;
        [SerializeField] private SpriteRenderer _pickupRenderer;

        Tween _tween;
        private void Awake()
        {
            _tween = _pickupRenderer.transform
                .DOLocalMoveY(0.25f, 0.75f)
                .SetRelative()        
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player") && CanBePickedUp && _pickup != null)
            {
                PickUp();
            }
        }

        public void Initialize(PickupBase pickup)
        {
            _pickup = pickup;
            _pickupRenderer.sprite = pickup.ItemIcon;
            CanBePickedUp = true;
        }

        public void PickUp()
        {
            if (!CanBePickedUp) return;
            _pickup.OnPickUp();
            Destroy(gameObject);
        }

        public void SetAvailability(bool isAvailable)
        {
            CanBePickedUp = isAvailable;
        }
        
        protected override void CleanUp()
        {
            _tween?.Kill();

            base.CleanUp();
        }
    }
}